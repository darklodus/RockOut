// Example default preset for RockOut
using System.Collections.Generic;

namespace RockOut.Core;

public static class DefaultPresets
{
    public static EqualizerPreset Rock => new EqualizerPreset
    {
        Name = "Rock",
        Bands = new List<PresetBand>
        {
            new() { F0 = 60, Gain = 5, Q = 1 },
            new() { F0 = 170, Gain = 4, Q = 1 },
            new() { F0 = 310, Gain = 3, Q = 1 },
            new() { F0 = 600, Gain = 2, Q = 1 },
            new() { F0 = 1000, Gain = 0, Q = 1 },
            new() { F0 = 3000, Gain = 2, Q = 1 },
            new() { F0 = 6000, Gain = 4, Q = 1 },
            new() { F0 = 12000, Gain = 5, Q = 1 },
            new() { F0 = 14000, Gain = 4, Q = 1 },
            new() { F0 = 16000, Gain = 3, Q = 1 }
        }
    };
}
