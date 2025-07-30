using System;
using RockOut.Core;

namespace RockOut.UI;

public class AudioEngineBridge : IDisposable
{
    private readonly IAudioCapture _capture;
    private readonly DspEngineService _dsp;
    private readonly IAudioPlayback _playback;
    private readonly float[] _floatBuffer = new float[512]; // Reused buffer
    private readonly object _lock = new();

    public AudioEngineBridge(IAudioCapture capture, DspEngineService dsp, IAudioPlayback playback)
    {
        _capture = capture;
        _dsp = dsp;
        _playback = playback;
        _capture.DataAvailable += OnDataAvailable;
    }

    public void Start()
    {
        _capture.Start();
    }

    public void Stop()
    {
        _capture.Stop();
    }

    private void OnDataAvailable(object? sender, AudioDataAvailableEventArgs e)
    {
        try
        {
            int floatCount = e.BytesRecorded / sizeof(float);
            int offset = 0;
            while (offset < floatCount)
            {
                int block = Math.Min(512, floatCount - offset);
                Buffer.BlockCopy(e.Buffer, offset * sizeof(float), _floatBuffer, 0, block * sizeof(float));
                _dsp.ProcessBuffer(_floatBuffer);
                _playback.Write(_floatBuffer, 0, block);
                offset += block;
            }
        }
        catch (Exception ex)
        {
            Diagnostics.LogFilterChainError(ex);
        }
    }

    public void Dispose()
    {
        _capture.DataAvailable -= OnDataAvailable;
        _playback.Dispose();
    }
}
