using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockOut.Core;
using System;

namespace RockOut.Tests;

[TestClass]
public class IntegrationTests
{
    [TestMethod]
    public void DspEngine_ProcessWavBuffer_ProducesOutput()
    {
        var dsp = new DspEngineService();
        var filter = new BiquadFilter();
        filter.SetParameters(1000, 1f, 6, FilterType.Peaking);
        dsp.FilterChain.AddFilter(filter);
        float[] buffer = new float[512];
        buffer[0] = 1.0f; // Impulse
        dsp.ProcessBuffer(buffer);
        Assert.IsTrue(Math.Abs(buffer[0]) > 0.1f, "DSP should process buffer and produce output");
    }

    // Placeholder for UI automation tests using FlaUI or similar framework.
    // To be implemented: Launch RockOut.UI, simulate user actions, and verify UI state.
    // Example: [TestMethod] public void MainWindow_OpensAndDisplaysBands() { /* ... */ }
}
