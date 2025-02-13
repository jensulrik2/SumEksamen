using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SumEksamen.Controllers;
using SumEksamen.Models;
using SumEksamen.Services;
using Xunit;

namespace SumEksamen.Tests;

public class KøkkenholdControllerTest
{
    private readonly KøkkenholdController _controller;

    [Fact]
    public void TC1_CreateKøkkenhold_withElevList_shouldReturnKøkkenholdWithExpectedElevCount()
    {
        // Arrange
        var ventelisteController = new VentelisteController();
        var controller = new KøkkenholdController();
        ventelisteController.Opretventeliste("2025/2026");
        ventelisteController.TilfoejElev("2025/2026", "mikkel", "dreng");
        ventelisteController.TilfoejElev("2025/2026", "mikkel2", "dreng");
        ventelisteController.TilfoejElev("2025/2026", "mikkel3", "dreng");
        ventelisteController.TilfoejElev("2025/2026", "mikkel4", "dreng");
        Storage.VentelisteTilElevliste("2025/2026");

        // Act
        var elevliste = Storage.HentElevListe();
        var result = controller.CreateKøkkenhold() as ViewResult;

        // Assert
        Assert.NotNull(result);
        var køkkenholdListe = result.Model as List<Køkkenhold>;
        Assert.NotNull(køkkenholdListe);
        Assert.Equal(4, køkkenholdListe[0].GetElevListe().Count());
    }
}