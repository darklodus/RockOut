using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace RockOut.Core;

public static class PresetSerializer
{
    public static void Save(string path, EqualizerPreset preset)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(preset, options);
        File.WriteAllText(path, json);
    }

    public static EqualizerPreset Load(string path)
    {
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<EqualizerPreset>(json) ?? new EqualizerPreset();
    }
}

public class PresetManager
{
    private readonly List<EqualizerPreset> _presets = new();
    public IReadOnlyList<EqualizerPreset> Presets => _presets;

    public void LoadFromDirectory(string directory)
    {
        _presets.Clear();
        if (!Directory.Exists(directory)) return;
        foreach (var file in Directory.GetFiles(directory, "*.json"))
        {
            try
            {
                var preset = PresetSerializer.Load(file);
                if (ValidatePreset(preset))
                    _presets.Add(preset);
            }
            catch { /* Optionally log or ignore invalid files */ }
        }
    }

    public static bool ValidatePreset(EqualizerPreset preset)
    {
        if (string.IsNullOrWhiteSpace(preset.Name)) return false;
        if (preset.Bands == null || preset.Bands.Count == 0) return false;
        foreach (var band in preset.Bands)
        {
            if (band.F0 <= 0 || band.Q <= 0) return false;
        }
        return true;
    }
}
