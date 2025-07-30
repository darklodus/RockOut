using System;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace RockOut.Core;

public class AudioCaptureService : IAudioCapture, IDisposable
{
    private WasapiLoopbackCapture? _capture;
    private WaveInEvent? _waveIn;
    private bool _useWasapi;

    public event EventHandler<AudioDataAvailableEventArgs>? DataAvailable;

    public AudioCaptureService(bool useWasapi = true)
    {
        _useWasapi = useWasapi;
    }

    public void Start()
    {
        if (_useWasapi)
        {
            _capture = new WasapiLoopbackCapture();
            _capture.DataAvailable += OnWasapiDataAvailable;
            _capture.StartRecording();
        }
        else
        {
            _waveIn = new WaveInEvent();
            _waveIn.DataAvailable += OnWaveInDataAvailable;
            _waveIn.StartRecording();
        }
    }

    public void Stop()
    {
        if (_useWasapi && _capture != null)
        {
            _capture.StopRecording();
            _capture.DataAvailable -= OnWasapiDataAvailable;
            _capture.Dispose();
            _capture = null;
        }
        else if (_waveIn != null)
        {
            _waveIn.StopRecording();
            _waveIn.DataAvailable -= OnWaveInDataAvailable;
            _waveIn.Dispose();
            _waveIn = null;
        }
    }

    private void OnWasapiDataAvailable(object? sender, WaveInEventArgs e)
    {
        DataAvailable?.Invoke(this, new AudioDataAvailableEventArgs(e.Buffer, e.BytesRecorded));
    }

    private void OnWaveInDataAvailable(object? sender, WaveInEventArgs e)
    {
        DataAvailable?.Invoke(this, new AudioDataAvailableEventArgs(e.Buffer, e.BytesRecorded));
    }

    public void Dispose()
    {
        Stop();
    }
}
