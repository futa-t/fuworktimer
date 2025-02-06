using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

using MemoryPack;

using static fuworktimer.Win32;
using static fuworktimer.ColorUtil;

namespace fuworktimer;

public partial class Form1 : Form
{
    readonly Dictionary<string, WindowData> windowList = [];

    private WindowData? currentActive;

    private readonly string appProcName;
    private string? focusProcName;

    private string SaveFilePath = "fl.pack";

    public Form1()
    {
        using (Process p = Process.GetCurrentProcess())
        {
            this.appProcName = p.ProcessName;
        }

        InitializeComponent();

        LoadWindowList();
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

        if (focusProcName != null && focusProcName != currentActive?.ProcessName)
            this.BackColor =Color.LightGray;
        else
            this.BackColor = window.Color;

        int time = 0;
        if (viewTotalTime.Checked)
            time = window.ActiveTimeTotal;
        else if (viewSessionTime.Checked)
            time = window.ActiveTimeSession;

        ActiveTimeLabel.Text = (new TimeSpan(0, 0, time)).ToString(@"hh\:mm\:ss");

        this.notifyIcon1.Text = $"{this.Text} {ActiveTimeLabel.Text}";
    }

    private void ResetEvent(object sender, EventArgs e)
    {
        foreach (var window in windowList.Values)
            window.ActiveTimeSession = 0;
    }

    public void SaveWindowList()
    {
        List<WindowDataPack> list = [];

        foreach (var item in windowList.Values)
            list.Add(new WindowDataPack(item.ProcessName, item.Color.ToArgb(), item.ActiveTimeTotal));

        File.WriteAllBytes(SaveFilePath, MemoryPackSerializer.Serialize(list));
    }

    public void LoadWindowList()
    {
        try
        {
            if (!File.Exists(SaveFilePath)) return;

            var data = MemoryPackSerializer.Deserialize<List<WindowDataPack>>(File.ReadAllBytes(SaveFilePath));
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
        e.Cancel = true;
        this.WindowState = FormWindowState.Minimized;
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
