using System.Text.Json;
using System.Text.Json.Serialization;

using static fuworktimer.Utility.ColorUtil;
using static fuworktimer.Utility.JsonExtentions;
namespace fuworktimer.Model;

 
public class WindowData
{
    public string ProcessName { get; init; } 
    public string DisplayName { get; set; }
    [JsonIgnore]
    public Color Color { get; set; }
    public int IntColor { get => Color.ToArgb(); set => Color = Color.FromArgb(value); }

    public int TotalTime { get; set; }

    [JsonIgnore]
    public int SessionTime { get; set; } = 0;

    public WindowData(string processName, string displayName, Color color, int totalTime = 0)
    {
        this.ProcessName = processName;
        this.DisplayName = displayName;
        this.Color = color;
        this.TotalTime = totalTime;
    }

    [JsonConstructor]
    public WindowData(string processName, string displayName, int totalTime)
    {
        this.ProcessName = processName;
        this.DisplayName = displayName;
        // ColorはIntColorで設定される
        this.TotalTime = totalTime;
    }

    public static WindowData FromProcessInfo(ProcessInfo process)
        => new(process.ProcessName,process.Description, GetColorFromText(process.Description));

    public string ToJson()
        => JsonSerializer.Serialize(this, JsonDefaultOption);

    public static WindowData? FromJson(string jsonString)
        => JsonSerializer.Deserialize<WindowData>(jsonString, JsonDefaultOption);

    public void AddActiveTime(int sec = 1)
    {
        SessionTime += sec;
        TotalTime += sec;
    }
}


