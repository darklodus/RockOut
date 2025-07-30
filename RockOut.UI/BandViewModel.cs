using System.ComponentModel;
using System.Runtime.CompilerServices;
using RockOut.Core;

namespace RockOut.UI;

public class BandViewModel : INotifyPropertyChanged
{
    private double _gain;
    public float Frequency { get; }
    public float Q { get; }
    public double Gain
    {
        get => _gain;
        set
        {
            if (_gain != value)
            {
                try
                {
                    _gain = value;
                    OnPropertyChanged();
                }
                catch (System.Exception ex)
                {
                    Log.Error($"Error setting Gain for {Frequency}Hz band", ex);
                }
            }
        }
    }
    public BandViewModel(float frequency, float q, double gain = 0)
    {
        Frequency = frequency;
        Q = q;
        Gain = gain;
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
