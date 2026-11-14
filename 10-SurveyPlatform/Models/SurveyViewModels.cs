using System.ComponentModel.DataAnnotations;

namespace SurveyPlatform.Models;

public class SurveyListItem
{
    public Survey Survey { get; set; } = new();
    public int ResponseCount { get; set; }
}

public class AddQuestionInput
{
    public int SurveyId { get; set; }

    [Required(ErrorMessage = "Escribe la pregunta.")]
    [StringLength(200)]
    [Display(Name = "Pregunta")]
    public string Text { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresa al menos dos opciones.")]
    [Display(Name = "Opciones (una por línea)")]
    public string OptionsText { get; set; } = string.Empty;
}

public class ManageSurveyViewModel
{
    public Survey Survey { get; set; } = new();
    public AddQuestionInput NewQuestion { get; set; } = new();
}

public class OptionResult
{
    public string Text { get; set; } = string.Empty;
    public int Count { get; set; }
    public double Percent { get; set; }
}

public class QuestionResult
{
    public string Text { get; set; } = string.Empty;
    public int TotalAnswers { get; set; }
    public List<OptionResult> Options { get; set; } = new();
}

public class SurveyResultsViewModel
{
    public Survey Survey { get; set; } = new();
    public int TotalResponses { get; set; }
    public List<QuestionResult> Questions { get; set; } = new();
}
