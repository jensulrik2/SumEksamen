using System;
using SumEksamen.Models;
using Xunit;

namespace SumEksamen.Tests.Models;

public class LinjeholdTest
{
    private readonly Elev e1;
    private readonly Elev e2;
    private readonly Elev e3;
    private readonly Elev e4;
    private readonly Elev e5;
    private readonly Elev e6;

    private readonly Linjehold lh;
    private readonly Linjehold lh2;

    public LinjeholdTest()
    {
        //TC1 og TC2
        e1 = new Elev("Rasmus", Køn.dreng);
        e2 = new Elev("Mikkel", Køn.dreng);
        e3 = new Elev("Hanne", Køn.pige);

        //TC3
        e4 = new Elev("Oliver", Køn.dreng);

        //TC4
        e5 = new Elev("Rasmus", 15, Status.Aktiv, Køn.dreng);
        e6 = new Elev("Jens", 16, Status.Inaktiv, Køn.dreng);

        lh = new Linjehold("Fodbold", 20, Køn.dreng);
        lh2 = new Linjehold("Fodbold", 2, Køn.dreng);
    }

    [Fact]
    public void TC1_TilfojElev_withValidElever_shouldAddEleverToLinjehold()
    {
        //Act
        lh.tilfojElev(e1);
        lh.tilfojElev(e2);

        //Assert
        Assert.True(lh.hentElever().Contains(e1));
        Assert.True(lh.hentElever().Contains(e2));
    }


    //Metoden tester for at den rigtige exception bliver kastet, når der tilføjes en pige til et drenge linjehold
    [Fact]
    public void TC2_TilfojElev_withIncorrectGender_shouldThrowArgumentException()
    {
        //Act
        lh.tilfojElev(e1);
        lh.tilfojElev(e2);

        //Assert
        Assert.True(lh.hentElever().Contains(e1));
        Assert.True(lh.hentElever().Contains(e2));
        Assert.Throws<ArgumentException>(() => lh.tilfojElev(e3));
    }

    //Metoden tester for at den rigtige exception bliver kastet, når der tilføjes en elev efter linjeholdets kapacitet er nået
    [Fact]
    public void TC3_TilfojElev_withExceedingCapacity_shouldThrowArgumentException()
    {
        //Act
        lh2.tilfojElev(e1);
        lh2.tilfojElev(e2);

        Assert.True(lh2.hentElever().Contains(e1));
        Assert.True(lh2.hentElever().Contains(e2));
        Assert.Throws<ArgumentException>(() => lh2.tilfojElev(e4));
    }

    //Metoden tester for om den rigtige exception bliver kastet, når der tilføjes en inaktiv elev til et linjehold
    [Fact]
    public void TC4_TilfojElev_withInactiveElev_shouldThrowArgumentException()
    {
        lh.tilfojElev(e5);

        Assert.True(lh.hentElever().Contains(e5));
        Assert.Throws<ArgumentException>(() => lh.tilfojElev(e6));
    }
}