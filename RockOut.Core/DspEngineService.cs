namespace RockOut.Core;

public class DspEngineService
{
    private readonly FilterChain _filterChain = new();

    public FilterChain FilterChain => _filterChain;

    public void ProcessBuffer(float[] buffer)
    {
        _filterChain.ProcessBuffer(buffer);
    }
}
