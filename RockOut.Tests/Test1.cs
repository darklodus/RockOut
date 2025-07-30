using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockOut.Core;
using System;

namespace RockOut.Tests;

[TestClass]
public sealed class Test1
{
    [TestMethod]
    public void TestMethod1()
    {
    }
}

// Placeholder for UI automation tests using FlaUI or similar framework.
// To be implemented: Launch RockOut.UI, simulate user actions, and verify UI state.
// Example: [TestMethod] public void MainWindow_OpensAndDisplaysBands() { /* ... */ }

[TestClass]
public class BiquadFilterTests
{
    [TestMethod]
    public void BiquadFilter_Peaking_ZeroGain_IsTransparent()
    {
        var filter = new BiquadFilter();
        filter.SetParameters(1000, 1f, 0, FilterType.Peaking);
        float input = 0.5f;
        float output = filter.ProcessSample(input);
        Assert.AreEqual(input, output, 1e-4f, "Zero gain peaking filter should be transparent");
    }

    [TestMethod]
    public void BiquadFilter_LowPass_FrequencyResponse()
    {
        var filter = new BiquadFilter();
        filter.SetParameters(500, 0.707f, 0, FilterType.LowPass);
        float[] input = new float[100];
        input[0] = 1.0f; // Impulse
        float[] output = new float[100];
        for (int i = 0; i < input.Length; i++)
            output[i] = filter.ProcessSample(input[i]);
        // Check that at least one output sample is nonzero (impulse response)
        bool nonZero = false;
        for (int i = 0; i < output.Length; i++)
        {
            if (Math.Abs(output[i]) > 1e-6f)
            {
                nonZero = true;
                break;
            }
        }
        Assert.IsTrue(nonZero, "Impulse response should not be zero");
    }

    [TestMethod]
    public void BiquadFilter_ParameterChange_UpdatesResponse()
    {
        var filter = new BiquadFilter();
        filter.SetParameters(1000, 1f, 6, FilterType.Peaking);
        float out1 = filter.ProcessSample(1.0f);
        filter.SetParameters(1000, 1f, -6, FilterType.Peaking);
        float out2 = filter.ProcessSample(1.0f);
        Assert.AreNotEqual(out1, out2, "Changing gain should change output");
    }
}
