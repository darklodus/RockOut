using System;

namespace RockOut.Core;

public interface IAudioCapture
{
    void Start();
    void Stop();
    event EventHandler<AudioDataAvailableEventArgs> DataAvailable;
}
