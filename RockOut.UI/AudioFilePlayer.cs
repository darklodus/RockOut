using System;
using System.IO;
using NAudio.Wave;
using RockOut.Core;

namespace RockOut.UI;

public class AudioFilePlayer : IDisposable
{
    private IWavePlayer? _waveOut;
    private WaveStream? _reader;
    private string? _currentFile;
    private readonly DspEngineService _dsp;

    public AudioFilePlayer(DspEngineService dsp)
    {
        _dsp = dsp;
    }

    public void Open(string filePath)
    {
        Dispose();
        _currentFile = filePath;
        if (filePath.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
            _reader = new Mp3FileReader(filePath);
        else if (filePath.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
            _reader = new WaveFileReader(filePath);
        else
            throw new NotSupportedException("Only WAV and MP3 supported in this demo");
        _waveOut = new WaveOutEvent();
        var dspProvider = new DspWaveProvider(_reader.ToSampleProvider().ToWaveProvider(), _dsp);
        _waveOut.Init(dspProvider);
    }

    public void Play()
    {
        _waveOut?.Play();
    }

    public void Stop()
    {
        _waveOut?.Stop();
    }

    public void Dispose()
    {
        _waveOut?.Dispose();
        _reader?.Dispose();
        _waveOut = null;
        _reader = null;
    }
}
