using System;

namespace Cuemon.Assets;
public class ClampOptions
{
    private int _maxConcurrentJobs;

    public ClampOptions()
    {
        MaxConcurrentJobs = 10;
    }

    public int MaxConcurrentJobs
    {
        get => _maxConcurrentJobs;
        set
        {
#if NET9_0_OR_GREATER
            _maxConcurrentJobs = Math.Clamp(value, 1, byte.MaxValue);
#else
            _maxConcurrentJobs = Clamp(value, 1, byte.MaxValue);
#endif
        }
    }

    private static int Clamp(int value, int min, int max)
    {
        if (value < min)
        {
            return min;
        }
        else if (value > max)
        {
            return max;
        }

        return value;
    }
}
