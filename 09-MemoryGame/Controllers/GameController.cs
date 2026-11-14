using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MemoryGame.Data;
using MemoryGame.Models;

namespace MemoryGame.Controllers;

public class GameController : Controller
{
    private static readonly string[] Symbols =
    {
        "🐶", "🐱", "🦊", "🐼", "🐸", "🦁", "🐵", "🐙", "🦄", "🐢", "🦋", "🐳"
    };

    private readonly AppDbContext _db;

    public GameController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var scores = await _db.Scores
            .OrderByDescending(s => s.Points)
            .Take(10)
            .ToListAsync();
        return View(new GameIndexViewModel { TopScores = scores });
    }

    public IActionResult Play(Difficulty difficulty = Difficulty.Easy)
    {
        var symbols = Symbols.Take(difficulty.PairCount()).ToList();

        // Two cards per symbol, then shuffle (Fisher-Yates).
        var cards = symbols.Concat(symbols).Select(s => new Card { Symbol = s }).ToList();
        for (var i = cards.Count - 1; i > 0; i--)
        {
            var j = Random.Shared.Next(i + 1);
            (cards[i], cards[j]) = (cards[j], cards[i]);
        }
        for (var i = 0; i < cards.Count; i++)
        {
            cards[i].Id = i;
        }

        return View(new PlayViewModel { Difficulty = difficulty, Cards = cards });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveScore(ScoreSubmission submission)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Index));
        }

        var pairs = submission.Difficulty.PairCount();
        var points = Math.Max(10, pairs * 300 - submission.Moves * 10 - submission.Seconds * 2);

        _db.Scores.Add(new GameScore
        {
            PlayerName = submission.PlayerName.Trim(),
            Difficulty = submission.Difficulty,
            Moves = submission.Moves,
            Seconds = submission.Seconds,
            Points = points,
            PlayedAt = DateTime.Now
        });
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Scores), new { difficulty = submission.Difficulty });
    }

    public async Task<IActionResult> Scores(Difficulty difficulty = Difficulty.Easy)
    {
        ViewBag.Difficulty = difficulty;
        var scores = await _db.Scores
            .Where(s => s.Difficulty == difficulty)
            .OrderByDescending(s => s.Points)
            .Take(20)
            .ToListAsync();
        return View(scores);
    }

    public IActionResult Error() => View();
}
