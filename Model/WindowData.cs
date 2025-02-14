using MemoryPack;

namespace fuworktimer.Model;

internal class WindowData(string processName, Color color, int activeTime = 0)
{
    public string ProcessName { get; } = processName;
    public Color Color { get; set; } = color;
    public int ActiveTimeTotal { get; set; } = activeTime;
    public int ActiveTimeSession { get; set; } = 0;
}

[MemoryPackable]
public partial class WindowDataPack
{
    public string ProcessName { get; set; }
    public int Color { get; set; }
    public int ActiveTime { get; set; }

    public WindowDataPack(string processName, int color, int activeTime)
    {
        ProcessName = processName;
        Color = color;
        ActiveTime = activeTime;
    }
}

