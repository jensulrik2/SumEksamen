namespace SumEksamen.Models;

public class Køkkenhold
{
    private static int _currentUgeNr = 33;
    private List<Elev> _holdListe = new(4);

    public Køkkenhold(params Elev[] elever)
    {
        if (elever.Length != 4) throw new ArgumentException("Der skal være 4 elever");

        if (_currentUgeNr == 53) _currentUgeNr = 1;
        UgeNr = _currentUgeNr++;
        _holdListe.AddRange(elever);
    }

    public List<Elev> HoldListe
    {
        get => _holdListe;
        set => _holdListe = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int UgeNr { get; }

    public void AddElev(Elev elev)
    {
        if (_holdListe.Count >= 4) throw new InvalidOperationException("Kan ikke tilføje flere end 4 elever");
        _holdListe.Add(elev);
    }

    public void RemoveElev(Elev elev)
    {
        _holdListe.Remove(elev);
    }

    public List<Elev> GetElevListe()
    {
        return _holdListe;
    }
}