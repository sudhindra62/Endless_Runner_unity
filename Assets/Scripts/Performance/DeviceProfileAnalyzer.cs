using UnityEngine;

public static class DeviceProfileAnalyzer
{
    public enum PerformanceTier
    {
        Low,
        Medium,
        High,
        Ultra
    }

    public static PerformanceTier AnalyzeDevice()
    {
        // More sophisticated analysis can be done here. For example, checking screen resolution,
        // specific GPU/CPU models, etc.
        int cpuCores = SystemInfo.processorCount;
        int memory = SystemInfo.systemMemorySize; // in MB

        if (cpuCores >= 8 && memory >= 8000)
        {
            return PerformanceTier.Ultra;
        }
        if (cpuCores >= 6 && memory >= 4000)
        {
            return PerformanceTier.High;
        }
        if (cpuCores >= 4 && memory >= 2000)
        {
            return PerformanceTier.Medium;
        }

        return PerformanceTier.Low;
    }

    public static DynamicQualityProfile GetProfileForTier(PerformanceTier tier, DynamicQualityProfile[] profiles)
    {
        // Assuming profiles are ordered from Low to Ultra
        int index = (int)tier;
        if (profiles != null && profiles.Length > index)
        { 
            return profiles[index];
        }

        // Fallback to the lowest quality if not enough profiles are provided
        return profiles != null && profiles.Length > 0 ? profiles[0] : null;
    }
}
