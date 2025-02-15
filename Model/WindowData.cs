using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

using static fuworktimer.Utility.ColorUtil;
using static fuworktimer.Utility.JsonExtentions;
namespace fuworktimer.Model;


public class ActiveTimer(int time=0)
{
    public int Time { get => _time; set => _time = value; }
    private int _time = time;

    public int Add(int count = 1) => _time += count;
    public void Reset() => _time = 0;
}

public class WindowData
{
    public string ProcessName { get; init; }
    public string DisplayName { get; set; }

    [JsonIgnore]
    public Color Color { get; set; }
    public int IntColor { get => Color.ToArgb(); set => Color = Color.FromArgb(value); }

    private ActiveTimer _totalTimer = new();
    public int TotalTime { get => _totalTimer.Time; set => _totalTimer.Time = value; }

    private ActiveTimer _dailyTimer = new();
    public int DailyTime { get => _dailyTimer.Time; set => _dailyTimer.Time = value;}
    //public int DailyTime { get; set; }

    private ActiveTimer _sessionTimer = new();
    [JsonIgnore]
    public int SessionTime { get => _sessionTimer.Time; set => _sessionTimer.Time = value; }

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
        => new(process.ProcessName, process.Description, GetColorFromText(process.Description));

    public string ToJson()
    {
        string j = _ToJson();
        Debug.WriteLine (j);
        return j;
    }

    public string _ToJson()
        => JsonSerializer.Serialize(this, JsonDefaultOption);

    public static WindowData? FromJson(string jsonString)
        => JsonSerializer.Deserialize<WindowData>(jsonString, JsonDefaultOption);

    public void AddActiveTime(int sec = 1)
    {
        _totalTimer.Add(sec);
        _dailyTimer.Add(sec);
        _sessionTimer.Add(sec);
    }
}


