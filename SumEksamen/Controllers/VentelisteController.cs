using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using SumEksamen.Models;
using SumEksamen.Services;

namespace SumEksamen.Controllers;

public class VentelisteController : Controller
{
    //private static List<Elev> elevListe = new List<Elev>(135);


    // GET: Venteliste
    public ActionResult Ventelister()
    {
        return View(Storage.HentVentelister()); // Ændret til 'Ventelister'
    }

    [HttpGet]
    [Route("venteliste/opret")]
    public IActionResult OpretVenteliste()
    {
        return View();
    }


    [HttpPost]
    [Route("venteliste/opret")]
    public IActionResult Opretventeliste(string aargang)
    {
        if (string.IsNullOrWhiteSpace(aargang))
        {
            ModelState.AddModelError("Aargang", "Årgang er påkrævet."); // Abu forklar hvad dette gør
            return View();
        }

        if (Storage.HentVentelister().Any(v => v.Aargang == aargang))
        {
            ModelState.AddModelError("Aargang", "En venteliste med denne årgang eksisterer allerede.");
            return View();
        }


        var venteliste = new Venteliste(aargang)
        {
            Aargang = aargang
        };

        Storage.TilføjVenteliste(venteliste);


        return RedirectToAction("VentelisteDetaljer", new { aargang });
    }

    [HttpPost]
    [Route("venteliste/findElev")]
    public IActionResult FindElev(string elevNavn)
    {
        if (string.IsNullOrEmpty(elevNavn)) return BadRequest("Elevens navn er påkrævet.");

        // Find ventelister, hvor eleven findes
        var ventelisterMedElev = Storage.HentVentelister()
            .Where(v => v.hentElever().Any(e => e.Navn.Equals(elevNavn, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        if (!ventelisterMedElev.Any()) return NotFound($"Ingen ventelister fundet for elev med navn: {elevNavn}");

        ViewBag.ElevNavn = elevNavn;
        return View("Ventelister", ventelisterMedElev); // Returner en view med ventelister
    }

    [HttpPost]
    [Route("venteliste/sletElev")]
    public IActionResult SletElev(string elevNavn, string aargang)
    {
        var venteliste = Storage.HentVentelister().FirstOrDefault(v => v.Aargang == aargang);
        if (venteliste == null) return NotFound($"Venteliste for årgang {aargang} ikke fundet.");


        var elev = venteliste.hentElever()
            .FirstOrDefault(e => e.Navn.Equals(elevNavn, StringComparison.OrdinalIgnoreCase));
        if (elev == null) return NotFound($"Elev med navn {elevNavn} ikke fundet på ventelisten for årgang {aargang}.");

        venteliste.hentElever().Remove(elev);

        return RedirectToAction("VentelisteDetaljer", new { aargang });
    }

    [HttpPost]
    [Route("venteliste/tilfoejBemærkning")]
    public IActionResult TilfojBemaerkning(string elevNavn, string bemærkning)
    {
        var elev = Storage.HentVentelister()
            .SelectMany(v => v.hentElever())
            .FirstOrDefault(e => e.Navn.Equals(elevNavn, StringComparison.OrdinalIgnoreCase));

        if (elev == null) return NotFound($"Elev med navn {elevNavn} blev ikke fundet.");

        var nyBemærkning = new Bemærkning(DateTime.Now, bemærkning);
        elev.tilfojBemærkning(nyBemærkning);


        return RedirectToAction("Ventelister");
    }


    public IActionResult VentelisteDetaljer(string aargang)
    {
        var venteliste = Storage.FindVenteliste(aargang);
        if (venteliste == null) return NotFound($"Venteliste for årgang {aargang} ikke fundet.");

        return View(venteliste);
    }


    [HttpGet]
    [Route("venteliste/AktiveElever")]
    public IActionResult AktiveElever()
    {
        var year = DateTime.Now.Year;
        var year2 = year + 1;
        var s = year + "/" + year2;
        var venteliste = Storage.HentVentelister().FirstOrDefault(v => v.Aargang == s);
        if (venteliste == null) return NotFound($"Elevliste for årgang {s} ikke fundet.");

        var elevKøn = venteliste.hentElever()
            .Where(e => e.Køn == Køn.dreng)
            .ToList();

        ViewBag.Aargang = s;

        return View(elevKøn);
    }


    [HttpGet]
    [Route("venteliste/tilfojElev")]
    public IActionResult TilfoejElev(string aargang)
    {
        ViewBag.Ventelister = Storage.HentVentelister();
        return View();
    }


    [HttpPost]
    [Route("venteliste/tilfojElev")]
    public IActionResult TilfoejElev(string aargang, string navn, string køn)
    {
        if (string.IsNullOrEmpty(navn) || string.IsNullOrEmpty(køn)) return BadRequest("Alle felter skal udfyldes.");


        var venteliste = Storage.FindVenteliste(aargang);
        if (venteliste == null) return NotFound($"Venteliste for årgang {aargang} ikke fundet.");

        try
        {
            var elevKøn = (Køn)Enum.Parse(typeof(Køn), køn, true);
            var elev = new Elev(navn, elevKøn)
            {
                Navn = navn,
                Køn = elevKøn
            };

            venteliste.tilfojElev(elev);

            return RedirectToAction("VentelisteDetaljer", new { aargang });
        }
        catch (Exception ex)
        {
            return BadRequest($"Fejl ved tilføjelse af elev: {ex.Message}");
        }
    }


    [HttpGet]
    [Route("venteliste/upload")]
    public IActionResult UploadElever()
    {
        ViewBag.Ventelister = Storage.HentVentelister();
        return View();
    }

    [HttpPost]
    [Route("venteliste/upload")]
    public IActionResult UploadExcel(IFormFile file, string aargang)
    {
        if (file == null || file.Length == 0) return BadRequest("Upload a valid file.");

        if (string.IsNullOrEmpty(aargang)) return BadRequest("Årgang skal angives.");

        var elevListe = new List<Elev>();
        using (var stream = new MemoryStream())
        {
            file.CopyTo(stream);
            using (var package = new ExcelPackage(stream))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;
                for (var row = 2; row <= rowCount; row++)
                {
                    var navn = worksheet.Cells[row, 1].Text;
                    var køn = (Køn)Enum.Parse(typeof(Køn), worksheet.Cells[row, 2].Text);


                    var elev = new Elev(navn, køn);
                    elevListe.Add(elev);
                }
            }
        }

        var venteliste = Storage.FindVenteliste(aargang);
        if (venteliste == null) return NotFound($"Venteliste for årgang {aargang} ikke fundet.");

        foreach (var elev in elevListe)
            try
            {
                venteliste.tilfojElev(elev);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Fejl ved tilføjelse af elev {elev.Navn}: {ex.Message}");
            }

        return RedirectToAction("VentelisteDetaljer", new { aargang });
    }

    //Skal måske fjernes?
    public Venteliste HentVenteliste(string aargang)
    {
        if (!Storage.HentVentelister().Any(v => v.Aargang == aargang))
            throw new ArgumentException("Venteliste findes ikke.");

        return Storage.FindVenteliste(aargang);
    }

    public static void ResetVenteliste()
    {
        Storage.HentVentelister().Clear();
    }
}