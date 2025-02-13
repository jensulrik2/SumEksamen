using Microsoft.AspNetCore.Mvc;
using SumEksamen.Models;
using SumEksamen.Services;

namespace SumEksamen.Controllers;

public class BordController : Controller
{
    private static List<Bord> borde = new();
    private static List<Elev> elevListe = new();
    private static bool eleverFordelt;
    private readonly VentelisteController _ventelisteController;

    public BordController(VentelisteController ventelisteController)
    {
        _ventelisteController = ventelisteController;
    }

    public IActionResult Bordopsætning()
    {
        // Check if the list of tables is already populated
        if (Storage.HentBorde().Count == 0)
        {
            for (var i = 0; i <= 5; i++) Storage.TilføjBord(new Bord(12));

            for (var i = 0; i <= 7; i++) Storage.TilføjBord(new Bord(8));
        }

        // Fetch the list of years from VentelisteController
        var aargangList = Storage.HentVentelister().Select(v => v.Aargang).ToList();
        ViewBag.AargangList = aargangList;

        // Pass the list of tables to the view
        return View("/Views/Spisesal/Bordopsætning.cshtml", Storage.HentBorde());
    }

    [HttpPost]
    public IActionResult UpdateBord(int bordNr, int antalPladser)
    {
        var bord = Storage.FindBord(bordNr);
        if (bord != null) bord.antalPladser = antalPladser;

        return RedirectToAction("Bordopsætning");
    }

    [HttpPost]
    public IActionResult OpretBord(int bordNr, int antalPladser)
    {
        try
        {
            if (antalPladser > 12) throw new ArgumentException("Et bord kan maksimalt have 12 pladser.");

            foreach (var bord in Storage.HentBorde())
                if (bord.bordNr == bordNr)
                    throw new ArgumentException("Et bord med dette ID eksisterer allerede.");

            // Find the smallest available bordNr if the provided one already exists
            var newBordNr = bordNr;
            while (Storage.HentBorde().Any(b => b.bordNr == newBordNr)) newBordNr++;

            Storage.TilføjBord(new Bord { bordNr = newBordNr, antalPladser = antalPladser });

            return Json(new { success = true });
        }
        catch (ArgumentException ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    public IActionResult SletBord(int bordNr)
    {
        var bord = Storage.FindBord(bordNr);
        if (bord != null)
            Storage.SletBord(bord);
        else
            throw new ArgumentException("Et bord med dette ID findes ikke.");

        return RedirectToAction("Bordopsætning");
    }

    [HttpPost]
    public IActionResult TilfojElevTilBordFraVenteliste(string aargang)
    {
        if (eleverFordelt) return Json(new { success = false, message = "Students have already been distributed." });

        // Retrieve students from the waiting list for the specified year
        Storage.VentelisteTilElevliste(aargang);

        var piger = Storage.HentElevListe().Where(e => e.Køn == Køn.pige).OrderBy(e => Guid.NewGuid()).ToList();
        var drenge = Storage.HentElevListe().Where(e => e.Køn == Køn.dreng).OrderBy(e => Guid.NewGuid()).ToList();

        foreach (var bord in Storage.HentBorde())
        {
            if (piger.Count < 2) throw new InvalidOperationException("Not enough girls to fill the tables.");

            if (bord.elever == null) bord.elever = new List<Elev>();

            // Ensure at least 2 girls at each table
            for (var i = 0; i < 2 && piger.Count > 0; i++)
            {
                bord.elever.Add(piger[0]);
                piger.RemoveAt(0);
            }

            // Distribute remaining seats randomly between boys and girls
            var remainingStudents = piger.Concat(drenge).OrderBy(e => Guid.NewGuid()).ToList();
            while (bord.elever.Count < bord.antalPladser && remainingStudents.Count > 0)
            {
                bord.elever.Add(remainingStudents[0]);
                remainingStudents.RemoveAt(0);
            }
        }

        eleverFordelt = true; // Set the flag to true after distribution
        return Json(new { success = true });
    }
}