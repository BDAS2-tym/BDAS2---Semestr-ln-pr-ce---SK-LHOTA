using System.ComponentModel;
using System.Runtime.CompilerServices;

public class DenTreninku : INotifyPropertyChanged
{
    private int _pocet;
    private double _procenta;
    private string _den;

    public string Den
    {
        get => _den;
        set => _den = value;
    }

    public int Pocet
    {
        get => _pocet;
        set
        {
            if (_pocet != value)
            {
                _pocet = value;
                OnPropertyChanged();
            }
        }
    }

    public double Procenta
    {
        get => _procenta;
        set
        {
            if (_procenta != value)
            {
                _procenta = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}