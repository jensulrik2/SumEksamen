namespace SumEksamen.Models;

public class Elev
{
    public Elev(int elevNr, string navn, Køn køn, Status status)
    {
        Navn = navn;
        Status = status;
        Køn = køn;
        ElevNr = elevNr;
    }

    public Elev(string navn, int alder, Status status, Køn køn)
    {
        Navn = navn;
        Alder = alder;
        Status = status;
        Køn = køn;
    }

    public Elev(string navn, int alder, Køn køn)
    {
        Navn = navn;
        Alder = alder;
        Køn = køn;
    }

    public Elev(string navn, Køn køn)
    {
        Navn = navn;
        Køn = køn;
    }


    public string Navn { get; set; }

    public int Alder { get; set; }

    public Status Status { get; set; }

    public Køn Køn { get; set; }

    public int ElevNr { get; set; }

    public List<Bemærkning> Bemærkninger { get; set; } = new();

    public void tilfojBemærkning(Bemærkning bemærkning)
    {
        Bemærkninger.Add(bemærkning);
    }


    public string hentBemærkninger()
    {
        return Bemærkninger.Any()
            ? string.Join(", ", Bemærkninger.Select(b => b.ToString()))
            : "Ingen bemærkninger.";
    }


    public override string ToString()
    {
        return
            $"ElevNr: {ElevNr}, Navn: {Navn}, Alder: {Alder}, Køn: {Køn}, Status: {Status}, Bemærkninger: {Bemærkninger}";
    }
}