using System.Windows;
using RockOut.Core;

namespace RockOut.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private AudioEngineBridge? _engine;
    private EqualizerViewModel? _viewModel;
    private AudioFilePlayer? _filePlayer;
    private DspEngineService? _dsp;

    public MainWindow()
    {
        InitializeComponent();
        _dsp = new DspEngineService();
        _viewModel = new EqualizerViewModel(_dsp);
        DataContext = _viewModel;
        Loaded += MainWindow_Loaded;
        Closed += MainWindow_Closed;
        _viewModel.PlayRequested += OnPlayRequested;
        _viewModel.StopRequested += OnStopRequested;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        var capture = new AudioCaptureService();
        var playback = new AudioPlaybackService();
        playback.Initialize(44100, 2);
        _engine = new AudioEngineBridge(capture, _dsp!, playback);
        _filePlayer = new AudioFilePlayer(_dsp!);
    }

    private void OnPlayRequested()
    {
        if (!string.IsNullOrEmpty(_viewModel?.SelectedFilePath))
        {
            try
            {
                _filePlayer?.Open(_viewModel.SelectedFilePath);
                _filePlayer?.Play();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error playing file: {ex.Message}", "Playback Error");
            }
        }
        else
        {
            _engine?.Start();
        }
    }

    private void OnStopRequested()
    {
        _filePlayer?.Stop();
        _engine?.Stop();
    }

    private void MainWindow_Closed(object? sender, System.EventArgs e)
    {
        _engine?.Dispose();
        _filePlayer?.Dispose();
    }
}