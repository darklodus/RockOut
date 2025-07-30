using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Win32;
using RockOut.Core;
using System.Collections.Generic;

namespace RockOut.UI;

public class EqualizerViewModel : INotifyPropertyChanged
{
    public ObservableCollection<BandViewModel> Bands { get; } = new();
    public ICommand OpenFileCommand { get; }
    public ICommand PlayCommand { get; }
    public ICommand StopCommand { get; }

    private bool _isPlaying;
    public bool IsPlaying
    {
        get => _isPlaying;
        set
        {
            if (_isPlaying != value)
            {
                _isPlaying = value;
                OnPropertyChanged(nameof(IsPlaying));
            }
        }
    }

    private string? _selectedFilePath;
    public string? SelectedFilePath
    {
        get => _selectedFilePath;
        set
        {
            if (_selectedFilePath != value)
            {
                _selectedFilePath = value;
                OnPropertyChanged(nameof(SelectedFilePath));
            }
        }
    }

    public event Action? PlayRequested;
    public event Action? StopRequested;

    private readonly DspEngineService? _dspEngine;
    private readonly List<BiquadFilter> _filters = new();

    public EqualizerViewModel(DspEngineService? dspEngine = null)
    {
        _dspEngine = dspEngine;
        float[] freqs = { 60, 170, 310, 600, 1000, 3000, 6000, 12000, 14000, 16000 };
        for (int i = 0; i < freqs.Length; i++)
        {
            var band = new BandViewModel(freqs[i], 1f);
            band.PropertyChanged += Band_PropertyChanged;
            Bands.Add(band);
            if (_dspEngine != null)
            {
                var filter = new BiquadFilter();
                filter.SetParameters(band.Frequency, band.Q, (float)band.Gain, FilterType.Peaking);
                _dspEngine.FilterChain.AddFilter(filter);
                _filters.Add(filter);
            }
        }
        OpenFileCommand = new RelayCommand(_ => ExecuteOpenFile());
        PlayCommand = new RelayCommand(_ => ExecutePlay());
        StopCommand = new RelayCommand(_ => ExecuteStop());
    }

    private void Band_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(BandViewModel.Gain) && _dspEngine != null)
        {
            try
            {
                for (int i = 0; i < Bands.Count && i < _filters.Count; i++)
                {
                    _filters[i].SetParameters(Bands[i].Frequency, Bands[i].Q, (float)Bands[i].Gain, FilterType.Peaking);
                }
            }
            catch (System.Exception ex)
            {
                Log.Error("Error updating filter parameters in Band_PropertyChanged", ex);
            }
        }
    }

    private void ExecuteOpenFile()
    {
        try
        {
            var dlg = new OpenFileDialog { Filter = "Audio Files|*.wav;*.mp3;*.flac;*.aac;*.ogg|All Files|*.*" };
            if (dlg.ShowDialog() == true)
            {
                SelectedFilePath = dlg.FileName;
            }
        }
        catch (System.Exception ex)
        {
            Log.Error("Error in ExecuteOpenFile", ex);
        }
    }

    private void ExecutePlay()
    {
        try
        {
            IsPlaying = true;
            PlayRequested?.Invoke();
        }
        catch (System.Exception ex)
        {
            Log.Error("Error in ExecutePlay", ex);
        }
    }

    private void ExecuteStop()
    {
        try
        {
            IsPlaying = false;
            StopRequested?.Invoke();
        }
        catch (System.Exception ex)
        {
            Log.Error("Error in ExecuteStop", ex);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}