namespace SumEksamen.Models;

public class Bemærkning
{
    public Bemærkning(DateTime dato, string tekst)
    {
        this.Dato = dato;
        this.Tekst = tekst;
    }

    public DateTime Dato { get; set; }

    public string Tekst { get; set; }

    public override string ToString()
    {
        return $"{Dato}: {Tekst}";
    }
}