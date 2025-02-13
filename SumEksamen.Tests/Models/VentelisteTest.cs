using System;
using SumEksamen.Models;
using Xunit;

namespace SumEksamen.Tests;

public class VentelisteTest
{
    private readonly Elev e1;
    private readonly Elev e2;
    private readonly Elev e3;
    private readonly Elev e4;
    private readonly Elev e5;
    private readonly Elev e6;
    private readonly Venteliste vl;

    public VentelisteTest()
    {
        e1 = new Elev("Rasmus", 14, Køn.dreng);
        e2 = new Elev("Abukar", 12, Køn.dreng);
        e3 = new Elev("Mikkel", 18, Køn.dreng);

        e4 = new Elev(06, "Rasmus", Køn.dreng, Status.Aktiv);
        e5 = new Elev(12, "Abukar", Køn.dreng, Status.Aktiv);
        e6 = new Elev(27, "Mikkel", Køn.dreng, Status.Inaktiv);

        vl = new Venteliste("25/26");
    }

    [Fact]
    public void TC1_TilfojElever_withValidElever_shouldAddEleverToVenteliste()
    {
        //Act
        vl.tilfojElev(e1);
        vl.tilfojElev(e2);

        //Assert
        Assert.True(vl.hentElever().Contains(e1));
        Assert.True(vl.hentElever().Contains(e2));
    }

    [Fact]
    public void TC2_TilfojElev_withDuplicateElev_shouldThrowArgumentException()
    {
        //Act
        vl.tilfojElev(e1);

        //Assert
        Assert.True(vl.hentElever().Contains(e1));
        Assert.Throws<ArgumentException>(() => vl.tilfojElev(e3));
    }

    [Fact]
    public void TC1_SletElev_withExistingElev_shouldRemoveElevFromVenteliste()
    {
        //Act
        vl.tilfojElev(e4);
        vl.tilfojElev(e5);
        vl.tilfojElev(e6);

        vl.sletElev(e5);

        //Assert
        Assert.True(vl.hentElever().Contains(e4));
        Assert.False(vl.hentElever().Contains(e5));
        Assert.True(vl.hentElever().Contains(e6));
    }

    [Fact]
    public void TC2_SletElev_withNonExistentElev_shouldThrowArgumentException()
    {
        //Act
        vl.tilfojElev(e4);
        vl.tilfojElev(e5);
        vl.tilfojElev(e6);

        vl.sletElev(e6);

        //Assert
        Assert.Throws<ArgumentException>(() => vl.sletElev(e6));
        Assert.True(vl.hentElever().Contains(e4));
        Assert.True(vl.hentElever().Contains(e5));
        Assert.False(vl.hentElever().Contains(e6));
    }

    [Fact]
    public void TC1_FindElev_withExistingElev_shouldReturnCorrectElev()
    {
        //Act
        vl.tilfojElev(e4);
        vl.tilfojElev(e5);
        vl.tilfojElev(e6);

        //Assert
        Assert.Equal(e4, vl.findElev(e4));
        
    }

    [Fact]
    public void TC2_FindElev_withNonExistentElev_shouldThrowArgumentException()
    {
        //Act
        vl.tilfojElev(e4);
        vl.tilfojElev(e5);
        vl.tilfojElev(e6);
        var elev = new Elev("Anders", Køn.dreng);

        //Assert
        Assert.Equal(e4, vl.findElev(e4));
        Assert.Equal(e5, vl.findElev(e5));
        Assert.Equal(e6, vl.findElev(e6));
        Assert.Throws<ArgumentException>(() => vl.findElev(elev));
    }

    [Fact]
    public void TC1_UpdateElev_withValidData_shouldUpdateElevNotesCorrectly()
    {
        //Act
        vl.tilfojElev(e4);
        vl.tilfojElev(e5);
        vl.tilfojElev(e6);

        vl.updateElev(e4, "Rasmus er en god elev");
        vl.updateElev(e4, "Rasmus er en dårlig elev");
        vl.updateElev(e5, "Abukar er en god elev");

        //Assert
        Assert.Equal("Rasmus er en god elev", e4.Bemærkninger[0].Tekst);
        Assert.Equal("Rasmus er en dårlig elev", e4.Bemærkninger[1].Tekst);
    }

    [Fact]
    public void TC2_UpdateElev_withNonExistentElev_shouldThrowArgumentException()
    {
        //Act
        vl.tilfojElev(e4);
        vl.tilfojElev(e5);
        vl.tilfojElev(e6);
        var elev = new Elev("Anders", Køn.dreng);

        //Assert
        Assert.Throws<ArgumentException>(() => vl.updateElev(elev, "Anders er en god elev"));
    }
}