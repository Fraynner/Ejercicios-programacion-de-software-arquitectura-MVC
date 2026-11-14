using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace RecipePlatform.Models;

public static class UiHelpers
{
    // Spanish culture used only to display dates (parsing keeps the default en-US culture).
    public static readonly CultureInfo Es = new CultureInfo("es-CO");

    // Returns the [Display(Name = "...")] of an enum value, or its name if it has none.
    public static string GetDisplayName(this Enum value)
    {
        var member = value.GetType().GetMember(value.ToString()).FirstOrDefault();
        var attribute = member?.GetCustomAttribute<DisplayAttribute>();
        return attribute?.GetName() ?? value.ToString();
    }
}
