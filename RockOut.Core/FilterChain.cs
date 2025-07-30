using System.Collections.Generic;

namespace RockOut.Core;

public class FilterChain
{
    private readonly List<IMonoFilter> _filters = new();

    public void AddFilter(IMonoFilter filter)
    {
        _filters.Add(filter);
    }

    public void RemoveFilter(IMonoFilter filter)
    {
        _filters.Remove(filter);
    }

    public void ProcessBuffer(float[] buffer)
    {
        for (int i = 0; i < buffer.Length; i++)
        {
            float sample = buffer[i];
            foreach (var filter in _filters)
            {
                sample = filter.ProcessSample(sample);
            }
            buffer[i] = sample;
        }
    }
}
