using System;
using NAudio.Wave;

namespace RockOut.Core;

public class AudioPlaybackService : IAudioPlayback
{
    private IWavePlayer? _waveOut;
    private BufferedWaveProvider? _bufferedProvider;
    private WaveFormat? _waveFormat;

    public void Initialize(int sampleRate, int channels)
    {
        _waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channels);
        _bufferedProvider = new BufferedWaveProvider(_waveFormat)
        {
            DiscardOnBufferOverflow = true
        };
        _waveOut = new WaveOutEvent();
        _waveOut.Init(_bufferedProvider);
        _waveOut.Play();
    }

    public void Write(float[] buffer, int offset, int count)
    {
        if (_bufferedProvider == null)
            throw new InvalidOperationException("Playback not initialized.");
        var byteBuffer = new byte[count * sizeof(float)];
        Buffer.BlockCopy(buffer, offset * sizeof(float), byteBuffer, 0, byteBuffer.Length);
        _bufferedProvider.AddSamples(byteBuffer, 0, byteBuffer.Length);
    }

    public void Dispose()
    {
        _waveOut?.Stop();
        _waveOut?.Dispose();
        _bufferedProvider = null;
        _waveFormat = null;
    }
}
