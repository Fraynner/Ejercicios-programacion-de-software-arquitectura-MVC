using Microsoft.AspNetCore.Mvc;
using PasswordGenerator.Models;
using PasswordGenerator.Services;

namespace PasswordGenerator.Controllers;

public class PasswordController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View(new PasswordViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(PasswordViewModel vm)
    {
        if (ModelState.IsValid)
        {
            vm.Password = PasswordGeneratorService.Generate(vm.Options);
            vm.Strength = PasswordGeneratorService.GetStrength(vm.Options);
        }
        return View(vm);
    }

    public IActionResult Error() => View();
}
