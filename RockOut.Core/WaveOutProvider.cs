using NAudio.Wave;

namespace RockOut.Core;

public class WaveOutProvider : IWaveProvider
{
    private readonly BufferedWaveProvider _bufferedProvider;
    public WaveOutProvider(WaveFormat waveFormat)
    {
        _bufferedProvider = new BufferedWaveProvider(waveFormat)
        {
            DiscardOnBufferOverflow = true
        };
    }
    public int Read(byte[] buffer, int offset, int count)
    {
        return _bufferedProvider.Read(buffer, offset, count);
    }
    public WaveFormat WaveFormat => _bufferedProvider.WaveFormat;
    public void AddSamples(byte[] buffer, int offset, int count)
    {
        _bufferedProvider.AddSamples(buffer, offset, count);
    }
}
