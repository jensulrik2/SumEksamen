using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using SumEksamen.Models;
using SumEksamen.Services;

namespace SumEksamen.Controllers;

public class KøkkenholdController : Controller
{
    // GET: KøkkenholdController
    public ActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Route("upload")]
    public IActionResult Upload()
    {
        var aargangList = Storage.HentVentelister().Select(v => v.Aargang).ToList();
        ViewBag.AargangList = aargangList;
        return View(Storage.HentElevListe());
    }

    [HttpPost]
    [Route("upload")]
    public IActionResult UploadExcel(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Upload a valid file.");

        Storage.HentElevListe().Clear();

        using (var stream = new MemoryStream())
        {
            file.CopyTo(stream);
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (var row = 2; row <= rowCount; row++)
                {
                    var navn = worksheet.Cells[row, 1].Text;
                    if (Enum.TryParse(worksheet.Cells[row, 2].Text, out Køn køn))
                    {
                        var elev = new Elev(navn, køn);
                        Storage.TilføjElev(elev);
                    }
                    else
                    {
                        // Fortæller hvis en enum værdi er ugyldig og hvor man kan finde den i excel arket
                        return BadRequest($"Invalid value for Køn at row {row}");
                    }
                }
            }
        }

        return View("Upload", Storage.HentElevListe());
    }

    [HttpPost]
    [Route("createKøkkenhold")]
    public IActionResult CreateKøkkenhold()
    {
        Storage.HentKøkkenhold().Clear();
        var drenge = Storage.HentElevListe().Where(e => e.Køn == Køn.dreng).ToList();
        var piger = Storage.HentElevListe().Where(e => e.Køn == Køn.pige).ToList();

        while (drenge.Count >= 2 && piger.Count >= 2) // Laver køkkenhold med 2 drenge og 2 piger
        {
            var køkkenhold = new Køkkenhold(drenge.Take(2).Concat(piger.Take(2)).ToArray());
            Storage.TilføjKøkkenhold(køkkenhold);
            drenge.RemoveRange(0, 2);
            piger.RemoveRange(0, 2);
        }

        // Hvis der er elever tilbage, laves et køkkenhold med de resterende elever
        // Der er et flertal af drenge -> Så der bliver lavet et køkkenhold med 4 drenge
        var remaining = drenge.Concat(piger).ToList();
        for (var i = 0; i < remaining.Count; i += 4)
            if (i + 4 <= remaining.Count)
            {
                var køkkenhold = new Køkkenhold(remaining.GetRange(i, 4).ToArray());
                Storage.TilføjKøkkenhold(køkkenhold);
            }

        return View("Køkkenhold", Storage.HentKøkkenhold());
    }


    [HttpPost]
    [Route("exportToExcel")]
    public IActionResult ExportToExcel()
    {
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Køkkenhold");

            // Add headers
            worksheet.Cells[1, 1].Value = "Køkkenhold";
            worksheet.Cells[1, 2].Value = "Navn";
            worksheet.Cells[1, 3].Value = "Køn";

            var row = 2;
            foreach (var køkkenhold in Storage.HentKøkkenhold())
            {
                foreach (var elev in køkkenhold.GetElevListe())
                {
                    worksheet.Cells[row, 1].Value = køkkenhold.UgeNr;
                    worksheet.Cells[row, 2].Value = elev.Navn;
                    worksheet.Cells[row, 3].Value = elev.Køn.ToString();
                    row++;
                }

                row++;
            }

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            var excelName = $"Køkkenhold-{DateTime.Now.ToString("yyyyMM")}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
    }


    public IActionResult OpretKøkkenhold(string aargang)
    {
        var aargangList = Storage.HentVentelister().Select(v => v.Aargang).ToList();
        ViewBag.AargangList = new SelectList(aargangList);
        return View();
    }

    [HttpPost]
    [Route("opretKøkkenholdFraVenteliste")]
    public IActionResult OpretKøkkenholdFraVenteliste(string aargang)
    {
        Storage.VentelisteTilElevliste(aargang);
        //elevListe = Storage.HentElevListe();
        return CreateKøkkenhold();
    }
}