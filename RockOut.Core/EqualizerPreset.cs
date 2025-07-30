// Preset schema for RockOut
// {
//   "name": "Rock",
//   "bands": [
//     { "f0": 100, "gain": 5, "Q": 1 },
//     ...
//   ]
// }

using System.Collections.Generic;

namespace RockOut.Core;

public class PresetBand
{
    public float F0 { get; set; }
    public float Gain { get; set; }
    public float Q { get; set; }
}

public class EqualizerPreset
{
    public string Name { get; set; } = string.Empty;
    public List<PresetBand> Bands { get; set; } = new();
}
