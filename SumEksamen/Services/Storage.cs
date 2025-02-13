using SumEksamen.Models;

namespace SumEksamen.Services;

public static class Storage
{
    private static readonly List<Venteliste> ventelister = new();
    private static readonly List<Bord> borde = new();
    private static readonly List<Værelse> værelser = new();
    private static readonly List<Køkkenhold> køkkenholdListe = new();
    private static List<Elev> elevListe = new();
    private static readonly List<Linjehold> linjeholdListe = new();
    private static readonly List<Klassehold> klasseholdListe = new();

    public static void TilføjKlassehold(Klassehold klassehold)
    {
        klasseholdListe.Add(klassehold);
    }

    public static List<Klassehold> HentKlassehold()
    {
        return klasseholdListe;
    }

    public static void sletKlassehold(Klassehold klassehold)
    {
        klasseholdListe.Remove(klassehold);
    }

    public static Klassehold FindKlassehold(string s)
    {
        return klasseholdListe.Find(k => s.Equals(k.Fag));
    }

    public static void TilføjVenteliste(Venteliste venteliste)
    {
        ventelister.Add(venteliste);
    }

    public static void TilføjBord(Bord bord)
    {
        borde.Add(bord);
    }

    public static void TilføjVærelse(Værelse værelse)
    {
        værelser.Add(værelse);
    }

    public static void TilføjKøkkenhold(Køkkenhold køkkenhold)
    {
        køkkenholdListe.Add(køkkenhold);
    }

    public static void TilføjElev(Elev elev)
    {
        elevListe.Add(elev);
    }

    public static void TilføjLinjehold(Linjehold linjehold)
    {
        linjeholdListe.Add(linjehold);
    }

    public static List<Venteliste> HentVentelister()
    {
        return ventelister;
    }

    public static List<Linjehold> HentLinjehold()
    {
        return linjeholdListe;
    }

    public static List<Bord> HentBorde()
    {
        return borde;
    }

    public static List<Værelse> HentVærelser()
    {
        return værelser;
    }

    public static List<Køkkenhold> HentKøkkenhold()
    {
        return køkkenholdListe;
    }

    public static List<Elev> HentElevListe()
    {
        return elevListe;
    }

    public static void SletVenteliste(Venteliste venteliste)
    {
        ventelister.Remove(venteliste);
    }

    public static void SletLinjehold(Linjehold linjehold)
    {
        linjeholdListe.Remove(linjehold);
    }

    public static void SletBord(Bord bord)
    {
        borde.Remove(bord);
    }

    public static void SletVærelse(Værelse værelse)
    {
        værelser.Remove(værelse);
    }

    public static void SletKøkkenhold(Køkkenhold køkkenhold)
    {
        køkkenholdListe.Remove(køkkenhold);
    }

    public static Venteliste FindVenteliste(string aargang)
    {
        if (ventelister.Find(v => v.Aargang == aargang) == null) throw new ArgumentException("Venteliste findes ikke.");
        return ventelister.Find(v => v.Aargang == aargang);
    }

    public static Linjehold FindLinjehold(Guid id)
    {
        if (linjeholdListe.Find(l => l.Id == id) == null) throw new ArgumentException("Linjehold findes ikke.");
        return linjeholdListe.Find(l => l.Id == id);
    }

    public static Bord FindBord(int bordNr)
    {
        return borde.Find(b => b.bordNr == bordNr);
    }

    public static Værelse FindVærelse(int værelseId)
    {
        return værelser.Find(v => v.VærelelsesId == værelseId);
    }


    // Skal potentielt rykkes et andet sted hen
    public static void VentelisteTilElevliste(string aargang)
    {
        var venteliste = ventelister.FirstOrDefault(v => v.Aargang == aargang);
        if (venteliste == null) throw new ArgumentException($"VenteListe for årgang {aargang} ikke fundet.");

        elevListe = venteliste.hentElever()
            .Take(136)
            .ToList();

        foreach (var elev in elevListe) elev.Status = Status.Aktiv;
    }
}