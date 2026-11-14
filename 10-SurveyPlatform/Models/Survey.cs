using System.ComponentModel.DataAnnotations;

namespace SurveyPlatform.Models;

public class Survey
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(120)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public List<Question> Questions { get; set; } = new();
}

public class Question
{
    public int Id { get; set; }
    public int SurveyId { get; set; }
    public string Text { get; set; } = string.Empty;
    public List<SurveyOption> Options { get; set; } = new();
}

public class SurveyOption
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public string Text { get; set; } = string.Empty;
}

// One completed response to a survey.
public class Submission
{
    public int Id { get; set; }
    public int SurveyId { get; set; }
    public DateTime SubmittedAt { get; set; } = DateTime.Now;
}

// The option chosen for one question inside a submission.
public class Answer
{
    public int Id { get; set; }
    public int SubmissionId { get; set; }
    public int QuestionId { get; set; }
    public int OptionId { get; set; }
}
