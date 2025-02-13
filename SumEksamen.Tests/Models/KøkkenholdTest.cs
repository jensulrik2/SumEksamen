using System;
using JetBrains.Annotations;
using SumEksamen.Models;
using Xunit;

namespace SumEksamen.Tests.Models;

[TestSubject(typeof(Køkkenhold))]
public class KøkkenholdTest
{
    private readonly Elev e1;
    private readonly Elev e2;
    private readonly Elev e3;
    private readonly Elev e4;
    private readonly Elev e5;

    // This method is called before each test method.
    public KøkkenholdTest()
    {
        e1 = new Elev("Mikkel", 13, Køn.dreng);
        e2 = new Elev("Mazza", 16, Køn.dreng);
        e3 = new Elev("Tully", 13, Køn.dreng);
        e4 = new Elev("Jens", 13, Køn.dreng);
        e5 = new Elev("Abukar", 15, Køn.dreng);
    }

    [Fact]
    public void TC1_KøkkenholdConstructor_withFourElever_shouldInitializeKøkkenholdCorrectly()
    {
        //Arrange & Act
        var køkkenhold = new Køkkenhold(e1, e2, e3, e4);

        //Assert
        Assert.NotNull(køkkenhold);
        Assert.Equal(33, køkkenhold.UgeNr);
        Assert.Equal(4, køkkenhold.GetElevListe().Count);
    }

    [Fact]
    public void TC2_KøkkenholdConstructor_withMoreThanFourElever_shouldThrowArgumentException()
    {
        //Act & Assert
        //Kaster fejl, hvis hold bliver lavet med mere end 4 elever
        Assert.Throws<ArgumentException>(() => new Køkkenhold(e1, e2, e3, e4, e5));
    }

    [Fact]
    public void TC3_KøkkenholdConstructor_withLessThanFourElever_shouldThrowArgumentException()
    {
        //Act & Assert
        //Kaster fejl, hvis hold bliver lavet med mindre end 4 elever
        Assert.Throws<ArgumentException>(() => new Køkkenhold(e1, e2, e3));
    }

    [Fact]
    public void TC1_TilfojElev_withFullKøkkenhold_shouldThrowInvalidOperationException()
    {
        //Arrange
        var køkkenhold = new Køkkenhold(e1, e2, e3, e4);

        //Act & Assert
        //Kaster fejl, hvis hold bliver over 4 elever
        Assert.Throws<InvalidOperationException>(() => køkkenhold.AddElev(e5));
    }
}