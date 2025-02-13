namespace SumEksamen.Models;

public class Venteliste
{
    private readonly List<Elev> elever;

    public Venteliste(string aargang)
    {
        Aargang = aargang;
        elever = new List<Elev>();
    }

    public string Aargang { get; set; }

    public void tilfojElev(Elev elev)
    {
        if (!elever.Contains(elev) && elev.Alder < 18)
            elever.Add(elev);
        else
            throw new ArgumentException("Elev skal være under 18 år.");
    }

    public void sletElev(Elev elev)
    {
        if (findElev(elev) != null)
            elever.Remove(findElev(elev));
        else
            throw new ArgumentException("Elev findes ikke.");
        ;
    }

    public Elev findElev(Elev elev)
    {
        var returElev = elever.Find(e => e == elev);
        if (returElev != null) return returElev;

        throw new ArgumentException("Elev findes ikke.");
    }

    public void updateElev(Elev elev, string str)
    {
        var returElev = findElev(elev);
        if (returElev != null)
        {
            var bemærkning = new Bemærkning(DateTime.Now, str);
            elev.tilfojBemærkning(bemærkning);
        }
        else
        {
            throw new ArgumentException("Elev findes ikke.");
        }
    }

    public List<Elev> hentElever()
    {
        return elever;
    }
}