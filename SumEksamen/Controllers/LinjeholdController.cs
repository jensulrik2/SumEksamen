using Microsoft.AspNetCore.Mvc;
using SumEksamen.Models;
using SumEksamen.Services;

namespace SumEksamen.Controllers;

public class LinjeholdController : Controller
{
    // GET: LinjeholdController
    public ActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Route("linjehold")]
    public IActionResult Opretlinjehold()
    {
        return View();
    }

    [HttpGet]
    [Route("linjehold/opret")]
    public IActionResult OpretLinjehold(string type, int kapacitet, string køn)
    {
        if (string.IsNullOrWhiteSpace(type) || kapacitet == 0) return BadRequest("Udfyld venligst alle felter.");

        var kønEnum = (Køn)Enum.Parse(typeof(Køn), køn, true);
        var linjehold = new Linjehold(type, kapacitet, kønEnum);


        Storage.TilføjLinjehold(linjehold);

        return RedirectToAction("LinjeholdOversigt");
    }

    //linjehold oversigt
    //
    [HttpGet]
    [Route("linjehold/oversigt")]
    public IActionResult LinjeholdOversigt()
    {
        var aargangList = Storage.HentVentelister().Select(v => v.Aargang).ToList();
        ViewBag.AargangList = aargangList;
        ViewData["Elever"] = Storage.HentElevListe();
        return View(Storage.HentLinjehold());
    }

    [HttpGet]
    [Route("linjehold/tilføj")]
    public IActionResult FordelElevPaaLinjehold(string elevNavn, Guid LinjeholdId)
    {
        var elev = Storage.HentElevListe()
            .FirstOrDefault(e => e.Navn.Equals(elevNavn, StringComparison.OrdinalIgnoreCase));
        if (elev == null)
            return NotFound("Elev ikke fundet.");


        var linjehold = Storage.FindLinjehold(LinjeholdId);
        if (linjehold == null)
            return NotFound("Linjehold ikke fundet.");


        if (linjehold.hentElever().Count >= linjehold.kapacitet)
            return BadRequest("Linjeholdets kapacitet er nået.");


        if (linjehold.køn != Køn.blandet && linjehold.køn != elev.Køn)
            return BadRequest("Elevens køn passer ikke til linjeholdets køn.");

        try
        {
            linjehold.tilfojElev(elev);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }

        return RedirectToAction("LinjeholdOversigt");
    }


    [HttpGet]
    [Route("linjehold/elevliste")]
    public IActionResult OpretElevlisteFraVenteliste(string aargang)
    {
        Storage.VentelisteTilElevliste(aargang);


        var aargangList = Storage.HentVentelister().Select(v => v.Aargang).ToList();
        ViewBag.AargangList = aargangList;

        //return View("OpretElevlisteFraVenteliste");
        return RedirectToAction("LinjeholdOversigt");
    }
}