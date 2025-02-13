namespace SumEksamen.Models;

public class Bord
{
    public Bord()
    {
        bordNr = currentBordNr++;
        antalPladser = 8;
    }

    public Bord(int antalPladser)
    {
        bordNr = currentBordNr++;
        this.antalPladser = antalPladser;
    }

    public static int currentBordNr { get; set; } = 1;
    public int bordNr { get; set; }
    public int antalPladser { get; set; }
    public List<Elev> elever { get; set; } = new();

    public void TilføjElevTilBord(Elev elev)
    {
        elever.Add(elev);
    }
}