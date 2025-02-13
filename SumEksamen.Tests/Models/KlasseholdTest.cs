using System;
using SumEksamen.Models;
using Xunit;

namespace SumEksamen.Tests.Models;

public class KlasseholdTest
{
    private readonly Elev e1 = new("Rasmus", Køn.dreng);
    private readonly Klassehold kh = new("Dansk", "Lokale 1");
    private Elev e2 = new("Mikkel", Køn.dreng);
    private Elev e3 = new("Hanne", Køn.pige);

    [Fact]
    public void TC1_tilfojElev_shouldAddElevToKlassehold()
    {
        kh.tilfojElev(e1);

        Assert.Contains(e1, kh.hentElever());
    }

    [Fact]
    public void TC2_tilfojElev_withStudentAlreadyInKlassehold_shouldThrowArgumentException()
    {
        kh.tilfojElev(e1);

        Assert.Throws<ArgumentException>(() => kh.tilfojElev(e1));
    }

    [Fact]
    public void TC1_fjernElev_shouldRemoveElevFromKlassehold()
    {
        kh.tilfojElev(e1);
        kh.fjernElev(e1);

        Assert.DoesNotContain(e1, kh.hentElever());
    }

    [Fact]
    public void TC2_fjernElev_withStudentNotInKlassehold_shouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => kh.fjernElev(e1));
    }
}