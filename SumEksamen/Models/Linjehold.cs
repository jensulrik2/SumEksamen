namespace SumEksamen.Models;

public class Linjehold
{
    private readonly List<Elev> elever;

    public Linjehold(string type, int kapacitet, Køn køn)
    {
        Id = Guid.NewGuid();
        this.type = type;
        this.kapacitet = kapacitet;
        this.køn = køn;
        elever = new List<Elev>();
    }

    public string type { get; set; }
    public Guid Id { get; set; }
    public int kapacitet { get; set; }

    public Køn køn { get; set; }

    public void tilfojElev(Elev elev)
    {
        if (!elever.Contains(elev) && equalsKøn(elev.Køn, køn) && elever.Count < kapacitet &&
            elev.Status.Equals(Status.Aktiv))
        {
            elever.Add(elev);
        }
        else
        {
            var errorMessage = elev.Køn.Equals(køn)
                ? elever.Count < kapacitet
                    ? elev.Status.Equals(Status.Aktiv) ? "Linjeholdets kapacitet er nået." : "Elev findes allerede."
                    : "Elevens køn passer ikke til linjeholdets køn."
                : "Eleven er ikke en aktiv elev på skolen";
            throw new ArgumentException(errorMessage);
        }
    }

    private bool equalsKøn(Køn elevKøn, Køn linjeKøn)
    {
        if (linjeKøn == Køn.blandet) return true;

        return elevKøn == linjeKøn;
    }

    public List<Elev> hentElever()
    {
        return elever;
    }
}