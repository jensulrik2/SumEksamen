namespace SumEksamen.Models;

public class Klassehold
{
    private readonly List<Elev> _elever = new();

    public Klassehold(string fag, string lokale)
    {
        this.Fag = fag;
        this.Lokale = lokale;
    }

    public string Fag { get; }

    public string Lokale { get; }

    public void tilfojElev(Elev elev)
    {
        if (!_elever.Contains(elev))
            _elever.Add(elev);
        else
            throw new ArgumentException("Eleven er allerede på holdet.");
    }

    public void fjernElev(Elev elev)
    {
        if (findElev(elev) != null)
            _elever.Remove(findElev(elev));
        else
            throw new ArgumentException("Elev findes ikke på klasseholdet.");
        ;
    }

    public Elev findElev(Elev elev)
    {
        var returElev = _elever.Find(e => e == elev);
        if (returElev != null) return returElev;

        throw new ArgumentException("Elev findes ikke.");
    }

    public List<Elev> hentElever()
    {
        return _elever;
    }
}