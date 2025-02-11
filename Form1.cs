using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

using MemoryPack;

using static fuworktimer.Win32;
using static fuworktimer.ColorUtil;
using System.Xml.Serialization;

namespace fuworktimer;

public partial class Form1 : Form
{
    readonly Dictionary<string, WindowData> windowList = [];

    private WindowData? currentActive;
    private AutoSaveTimer autoSaveTimer;

    private readonly string appProcName;
    private string? focusProcName;

    public Form1()
    {
        using (Process p = Process.GetCurrentProcess())
        {
            this.appProcName = p.ProcessName;
        }

        InitializeComponent();

        LoadWindowList();
        autoSaveTimer = new(callback: SaveWindowList);
        autoSaveTimer.Start();

        SaveWindowList();
        timer1.Start();
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
        if (GetActioveWindow() is WindowData active)
        {
            currentActive = active;
            currentActive.ActiveTimeTotal++;
            currentActive.ActiveTimeSession++;
        }
        UpdateActiveWindow(GetFocusWindow() ?? currentActive);
    }

    WindowData? GetActioveWindow()
    {
        string activeWindowProcName = GetActiveWindowProcessName();

        if (activeWindowProcName == appProcName) return null;

        if (!windowList.TryGetValue(activeWindowProcName, out var activeWindow))
        {
            activeWindow = new WindowData(activeWindowProcName, GetColorFromText(activeWindowProcName));
            windowList[activeWindowProcName] = activeWindow;
        }
        return activeWindow;
    }

    WindowData? GetFocusWindow()
    {
        if (focusProcName == null) return null;

        windowList.TryGetValue(focusProcName, out var focusWindow);
        return focusWindow;
    }

    void UpdateActiveWindow(WindowData? window)
    {
        if (window == null) return;

        this.Text = window.ProcessName;

        if (focusProcName != null)
            this.Text += "[focus]";

        try
        {
            // どうやらリソースからのアイコンの取得で例外吐いて落ちることがあるっぽいので経過観察

            Icon? currentIcon = notifyIcon1.Icon; 
            if (focusProcName != null && focusProcName != currentActive?.ProcessName)
            {
                this.BackColor = Color.LightGray;
                notifyIcon1.Icon = Resource1.focusout;
            }
            else
            {
                this.BackColor = window.Color;
                notifyIcon1.Icon = Resource1.focus;
            }
            currentIcon?.Dispose();
        } catch (Exception ex){
            Program.ErrorLog(ex);
        }


        int time = 0;
        if (viewTotalTime.Checked)
            time = window.ActiveTimeTotal;
        else if (viewSessionTime.Checked)
            time = window.ActiveTimeSession;

        //ActiveTimeLabel.Text = (new TimeSpan(0, 0, time)).ToString(@"hh\:mm\:ss");
        ActiveTimeLabel.Text = fmt_hms(time);

        this.notifyIcon1.Text = $"{this.Text} {ActiveTimeLabel.Text}";
    }

    static string fmt_hms(int sec)
    {
        int h, m, s;
        (h, s) = divmod(sec, 3600);
        (m, s) = divmod(s, 60);

        return $"{h:D2}:{m:D2}:{s:D2}";
    }

    static (int, int) divmod(int a, int b)
    {
        int c = a / b;
        int d = a % b;
        return (c, d);
    }

    

    private void ResetEvent(object sender, EventArgs e)
    {
        foreach (var window in windowList.Values)
            window.ActiveTimeSession = 0;
    }


    private static string DailySaveFile()
    {
        string today = DateTime.Now.ToString("yyMMdd");

        return Path.Combine(Program.AppDir, $"{today}.pack");
    }

    public void SaveWindowList()
    {
        string fname = DailySaveFile();
        Debug.WriteLine($"Save as {fname}");
        List<WindowDataPack> list = [];

        foreach (var item in windowList.Values)
            list.Add(new WindowDataPack(item.ProcessName, item.Color.ToArgb(), item.ActiveTimeTotal));

        File.WriteAllBytes(fname, MemoryPackSerializer.Serialize(list));
    }

    public void LoadWindowList()
    {
        try
        {
            string fname = DailySaveFile();
            if (!File.Exists(fname)) return;

            var data = MemoryPackSerializer.Deserialize<List<WindowDataPack>>(File.ReadAllBytes(fname));
            if (data == null) return;

            foreach (var d in data)
                windowList[d.ProcessName] = new WindowData(d.ProcessName, Color.FromArgb(d.Color), d.ActiveTime);
        }
        catch
        {
            return;
        }
    }

    private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
    {
#if !DEBUG
        e.Cancel = true;
        this.WindowState = FormWindowState.Minimized;
#endif
        SaveWindowList();
    }

    private void ViewTimeChecked(object sender, EventArgs e)
    {
        if (sender is not ToolStripMenuItem s)
            return;

        foreach (var item in contextMenuStrip1.Items)
            if (item is ToolStripMenuItem a)
                a.Checked = s == item;

        s.Checked = true;
    }

    private void focusMode_Click(object sender, EventArgs e)
    {
        if (focusMode.Checked)
            focusProcName = currentActive?.ProcessName;
        else
            focusProcName = null;
    }

    //  https://blog.ch3cooh.jp/entry/20080113/1311501361
    private void Form1_ClientSizeChanged(object sender, EventArgs e)
    {
        if (this.WindowState == System.Windows.Forms.FormWindowState.Minimized)
        {
            this.Hide();
            notifyIcon1.Visible = true;
        }
    }

    private void NotifyIconCloseClick(object sender, EventArgs e)
    {
        this.FormClosing -= Form1_FormClosing;
        SaveWindowList();
        this.Close();
    }

    private void notifyIcon1_Click(object sender, EventArgs e)
    {
        if (((MouseEventArgs)e).Button == MouseButtons.Left)
        {
            this.Visible = true;
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;
            this.Activate();
        }
    }
}


class WindowData(string processName, Color color, int activeTime = 0)
{
    public string ProcessName { get; } = processName;
    public Color Color { get; } = color;
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

public class AutoSaveTimer(Action callback)
{
    private System.Threading.Timer? _timer;
    private event Action _callback = callback;

    public void Start()
    {
        DateTime now = DateTime.Now;
        DateTime addHour = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0).AddHours(1);
        TimeSpan next = addHour - now;
        this._timer = new(Callback, null, next, TimeSpan.FromHours(1));
    }

    public void Stop() => this._timer?.Dispose();

    private void Callback(object? state) => Task.Run(this._callback.Invoke);

}

