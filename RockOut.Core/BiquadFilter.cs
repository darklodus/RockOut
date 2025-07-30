using System;

namespace RockOut.Core;

public class BiquadFilter : IMonoFilter
{
    private float a0, a1, a2, b0, b1, b2;
    private float z1, z2;
    private FilterType type;
    private float sampleRate = 44100f;

    public void SetParameters(float f0, float q, float gainDb, FilterType type)
    {
        this.type = type;
        float A = (float)Math.Pow(10, gainDb / 40);
        float omega = 2 * (float)Math.PI * f0 / sampleRate;
        float sn = (float)Math.Sin(omega);
        float cs = (float)Math.Cos(omega);
        float alpha = sn / (2 * q);
        float beta = (float)Math.Sqrt(A) / q;

        switch (type)
        {
            case FilterType.LowPass:
                b0 = (1 - cs) / 2;
                b1 = 1 - cs;
                b2 = (1 - cs) / 2;
                a0 = 1 + alpha;
                a1 = -2 * cs;
                a2 = 1 - alpha;
                break;
            case FilterType.HighPass:
                b0 = (1 + cs) / 2;
                b1 = -(1 + cs);
                b2 = (1 + cs) / 2;
                a0 = 1 + alpha;
                a1 = -2 * cs;
                a2 = 1 - alpha;
                break;
            case FilterType.BandPass:
                b0 = alpha;
                b1 = 0;
                b2 = -alpha;
                a0 = 1 + alpha;
                a1 = -2 * cs;
                a2 = 1 - alpha;
                break;
            case FilterType.Notch:
                b0 = 1;
                b1 = -2 * cs;
                b2 = 1;
                a0 = 1 + alpha;
                a1 = -2 * cs;
                a2 = 1 - alpha;
                break;
            case FilterType.LowShelf:
                {
                    float sqrtA = (float)Math.Sqrt(A);
                    b0 = A * ((A + 1) - (A - 1) * cs + 2 * sqrtA * alpha);
                    b1 = 2 * A * ((A - 1) - (A + 1) * cs);
                    b2 = A * ((A + 1) - (A - 1) * cs - 2 * sqrtA * alpha);
                    a0 = (A + 1) + (A - 1) * cs + 2 * sqrtA * alpha;
                    a1 = -2 * ((A - 1) + (A + 1) * cs);
                    a2 = (A + 1) + (A - 1) * cs - 2 * sqrtA * alpha;
                }
                break;
            case FilterType.HighShelf:
                {
                    float sqrtA = (float)Math.Sqrt(A);
                    b0 = A * ((A + 1) + (A - 1) * cs + 2 * sqrtA * alpha);
                    b1 = -2 * A * ((A - 1) + (A + 1) * cs);
                    b2 = A * ((A + 1) + (A - 1) * cs - 2 * sqrtA * alpha);
                    a0 = (A + 1) - (A - 1) * cs + 2 * sqrtA * alpha;
                    a1 = 2 * ((A - 1) - (A + 1) * cs);
                    a2 = (A + 1) - (A - 1) * cs - 2 * sqrtA * alpha;
                }
                break;
            case FilterType.Peaking:
                b0 = 1 + alpha * A;
                b1 = -2 * cs;
                b2 = 1 - alpha * A;
                a0 = 1 + alpha / A;
                a1 = -2 * cs;
                a2 = 1 - alpha / A;
                break;
        }
        // Normalize
        b0 /= a0;
        b1 /= a0;
        b2 /= a0;
        a1 /= a0;
        a2 /= a0;
        // Reset filter state to avoid stale values after parameter change
        z1 = 0;
        z2 = 0;
    }

    public float ProcessSample(float input)
    {
        float output = b0 * input + z1;
        z1 = b1 * input + z2 - a1 * output;
        z2 = b2 * input - a2 * output;
        return output;
    }
}
