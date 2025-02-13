using Microsoft.AspNetCore.Mvc;
using SumEksamen.Models;
using SumEksamen.Services;

namespace SumEksamen.Controllers;

public class KlasseholdController : Controller
{
    [HttpGet]
    [Route("klassehold")]
    public ActionResult Index()
    {
        return View();
    }


    [HttpGet]
    [Route("klassehold/opret")]
    public IActionResult OpretKlassehold(string fag, string lokale)
    {
        if (string.IsNullOrWhiteSpace(fag))
        {
            ModelState.AddModelError("Fag", "Angivelse af fag er påkrævet.");
            return View();
        }

        var klassehold = new Klassehold(fag, lokale);

        Storage.TilføjKlassehold(klassehold);

        return RedirectToAction("KlasseholdDetaljer", new { fag });
    }

    [HttpGet]
    [Route("klassehold/detaljer/{fag}")]
    public IActionResult KlasseholdDetaljer(string fag)
    {
        var klassehold = Storage.HentKlassehold().FirstOrDefault(k => k.Fag == fag);

        if (klassehold == null) return NotFound("Klasseholdet blev ikke fundet.");

        ViewData["Fag"] = klassehold.Fag;
        ViewData["Lokale"] = klassehold.Lokale;

        return View(klassehold);
    }

    [HttpGet]
    [Route("klassehold/oversigt")]
    public IActionResult KlasseholdOversigt()
    {
        var klasseholdListe = Storage.HentKlassehold();

        return View(klasseholdListe);
    }
}