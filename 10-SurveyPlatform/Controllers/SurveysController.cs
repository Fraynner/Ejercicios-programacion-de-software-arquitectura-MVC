using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyPlatform.Data;
using SurveyPlatform.Models;

namespace SurveyPlatform.Controllers;

public class SurveysController : Controller
{
    private readonly AppDbContext _db;

    public SurveysController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var surveys = await _db.Surveys.OrderByDescending(s => s.Id).ToListAsync();
        var counts = await _db.Submissions
            .GroupBy(s => s.SurveyId)
            .Select(g => new { SurveyId = g.Key, Count = g.Count() })
            .ToListAsync();

        var items = surveys.Select(s => new SurveyListItem
        {
            Survey = s,
            ResponseCount = counts.FirstOrDefault(c => c.SurveyId == s.Id)?.Count ?? 0
        }).ToList();

        return View(items);
    }

    // ---------- Create & manage surveys ----------

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Survey());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,Description")] Survey survey)
    {
        if (!ModelState.IsValid)
        {
            return View(survey);
        }

        survey.CreatedAt = DateTime.Now;
        _db.Surveys.Add(survey);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Manage), new { id = survey.Id });
    }

    public async Task<IActionResult> Manage(int id)
    {
        var survey = await LoadSurveyAsync(id);
        if (survey == null) return NotFound();

        return View(new ManageSurveyViewModel
        {
            Survey = survey,
            NewQuestion = new AddQuestionInput { SurveyId = id }
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddQuestion([Bind(Prefix = "NewQuestion")] AddQuestionInput input)
    {
        var survey = await LoadSurveyAsync(input.SurveyId);
        if (survey == null) return NotFound();

        var options = input.OptionsText
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Distinct()
            .ToList();

        if (options.Count < 2)
        {
            ModelState.AddModelError("NewQuestion.OptionsText", "Ingresa al menos dos opciones diferentes.");
        }

        if (!ModelState.IsValid)
        {
            return View("Manage", new ManageSurveyViewModel { Survey = survey, NewQuestion = input });
        }

        var question = new Question
        {
            SurveyId = survey.Id,
            Text = input.Text.Trim(),
            Options = options.Select(o => new SurveyOption { Text = o }).ToList()
        };
        _db.Questions.Add(question);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Manage), new { id = survey.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteQuestion(int id)
    {
        var question = await _db.Questions.FindAsync(id);
        if (question == null) return RedirectToAction(nameof(Index));

        var surveyId = question.SurveyId;
        _db.Answers.RemoveRange(_db.Answers.Where(a => a.QuestionId == id));
        _db.Questions.Remove(question); // its options are removed by cascade
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Manage), new { id = surveyId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var survey = await _db.Surveys.Include(s => s.Questions).FirstOrDefaultAsync(s => s.Id == id);
        if (survey != null)
        {
            var questionIds = survey.Questions.Select(q => q.Id).ToList();
            var submissionIds = await _db.Submissions.Where(s => s.SurveyId == id).Select(s => s.Id).ToListAsync();

            _db.Answers.RemoveRange(_db.Answers.Where(a => submissionIds.Contains(a.SubmissionId) || questionIds.Contains(a.QuestionId)));
            _db.Submissions.RemoveRange(_db.Submissions.Where(s => s.SurveyId == id));
            _db.Surveys.Remove(survey);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    // ---------- Answer a survey ----------

    [HttpGet]
    public async Task<IActionResult> Respond(int id)
    {
        var survey = await LoadSurveyAsync(id);
        if (survey == null) return NotFound();
        return View(survey);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Respond(int id, Dictionary<int, int> answers)
    {
        var survey = await LoadSurveyAsync(id);
        if (survey == null) return NotFound();

        // Every question must be answered with one of its own options.
        var valid = survey.Questions.Count > 0 && survey.Questions.All(q =>
            answers.TryGetValue(q.Id, out var optionId) && q.Options.Any(o => o.Id == optionId));

        if (!valid)
        {
            ViewBag.Error = "Por favor responde todas las preguntas.";
            return View(survey);
        }

        var submission = new Submission { SurveyId = id, SubmittedAt = DateTime.Now };
        _db.Submissions.Add(submission);
        await _db.SaveChangesAsync();

        foreach (var q in survey.Questions)
        {
            _db.Answers.Add(new Answer { SubmissionId = submission.Id, QuestionId = q.Id, OptionId = answers[q.Id] });
        }
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(ThankYou), new { id });
    }

    public async Task<IActionResult> ThankYou(int id)
    {
        var survey = await _db.Surveys.FindAsync(id);
        if (survey == null) return NotFound();
        return View(survey);
    }

    // ---------- Results ----------

    public async Task<IActionResult> Results(int id)
    {
        var survey = await LoadSurveyAsync(id);
        if (survey == null) return NotFound();

        var questionIds = survey.Questions.Select(q => q.Id).ToList();
        var answers = await _db.Answers.Where(a => questionIds.Contains(a.QuestionId)).ToListAsync();
        var totalResponses = await _db.Submissions.CountAsync(s => s.SurveyId == id);

        var vm = new SurveyResultsViewModel { Survey = survey, TotalResponses = totalResponses };
        foreach (var q in survey.Questions)
        {
            var questionAnswers = answers.Where(a => a.QuestionId == q.Id).ToList();
            var total = questionAnswers.Count;
            vm.Questions.Add(new QuestionResult
            {
                Text = q.Text,
                TotalAnswers = total,
                Options = q.Options.Select(o =>
                {
                    var count = questionAnswers.Count(a => a.OptionId == o.Id);
                    return new OptionResult
                    {
                        Text = o.Text,
                        Count = count,
                        Percent = total == 0 ? 0 : Math.Round(count * 100.0 / total, 1)
                    };
                }).ToList()
            });
        }
        return View(vm);
    }

    public IActionResult Error() => View();

    private Task<Survey?> LoadSurveyAsync(int id)
    {
        return _db.Surveys
            .Include(s => s.Questions)
            .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}
