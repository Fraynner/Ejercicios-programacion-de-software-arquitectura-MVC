using Microsoft.AspNetCore.Mvc;
using TipCalculator.Models;

namespace TipCalculator.Controllers;

public class TipController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View(new TipViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(TipViewModel vm)
    {
        if (ModelState.IsValid && vm.Input.BillAmount.HasValue)
        {
            vm.Result = TipResult.Calculate(vm.Input.BillAmount.Value, vm.Input.TipPercentage, vm.Input.People);
        }
        return View(vm);
    }

    public IActionResult Error() => View();
}
