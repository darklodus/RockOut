using System;

namespace RockOut.Core;

public interface IAudioPlayback : IDisposable
{
    void Initialize(int sampleRate, int channels);
    void Write(float[] buffer, int offset, int count);
}
