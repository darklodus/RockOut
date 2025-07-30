using System;

namespace RockOut.Core;

public class AudioDataAvailableEventArgs : EventArgs
{
    public byte[] Buffer { get; }
    public int BytesRecorded { get; }
    public AudioDataAvailableEventArgs(byte[] buffer, int bytesRecorded)
    {
        Buffer = buffer;
        BytesRecorded = bytesRecorded;
    }
}
