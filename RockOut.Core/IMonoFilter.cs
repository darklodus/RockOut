namespace RockOut.Core;

public interface IMonoFilter
{
    float ProcessSample(float input);
    void SetParameters(float f0, float q, float gainDb, FilterType type);
}
