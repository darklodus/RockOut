using System;
using NAudio.Wave;
using RockOut.Core;

namespace RockOut.UI;

public class DspWaveProvider : IWaveProvider
{
    private readonly IWaveProvider _source;
    private readonly DspEngineService _dsp;
    private readonly WaveFormat _waveFormat;
    private readonly float[] _floatBuffer;
    private readonly byte[] _byteBuffer;

    public DspWaveProvider(IWaveProvider source, DspEngineService dsp)
    {
        _source = source;
        _dsp = dsp;
        _waveFormat = source.WaveFormat;
        _floatBuffer = new float[4096];
        _byteBuffer = new byte[4096 * 4]; // 4 bytes per float
    }

    public WaveFormat WaveFormat => _waveFormat;

    public int Read(byte[] buffer, int offset, int count)
    {
        int totalBytesWritten = 0;
        int bytesPerChunk = _byteBuffer.Length;

        while (totalBytesWritten < count)
        {
            int bytesToRead = Math.Min(count - totalBytesWritten, bytesPerChunk);
            int bytesRead = _source.Read(_byteBuffer, 0, bytesToRead);
            if (bytesRead == 0)
                break; // End of stream
            int samples = bytesRead / 4;
            Buffer.BlockCopy(_byteBuffer, 0, _floatBuffer, 0, bytesRead);
            // Only process the valid samples in-place
            if (samples > 0)
            {
                float[] processBuffer = new float[samples];
                Array.Copy(_floatBuffer, processBuffer, samples);   
                _dsp.ProcessBuffer(processBuffer);
                Array.Copy(processBuffer, _floatBuffer, samples);
            }
            Buffer.BlockCopy(_floatBuffer, 0, buffer, offset + totalBytesWritten, bytesRead);
            totalBytesWritten += bytesRead;
            if (bytesRead < bytesToRead)
                break; // No more data available
        }
        return totalBytesWritten;
    }
}
