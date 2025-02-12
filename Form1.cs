using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

using MemoryPack;

using static fuworktimer.Win32;
using static fuworktimer.ColorUtil;
using static fuworktimer.TimeFormat;

using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace fuworktimer;

public partial class Form1 : Form
{
    private string currentActive;
    WindowData lastActive;
    private AutoSaveTimer autoSaveTimer;

    private readonly string appProcName;
    private string? focusProcName;

    WindowList windowList;

    public Form1()
    {
        using (Process p = Process.GetCurrentProcess())
        {
            this.appProcName = p.ProcessName;
        }

        InitializeComponent();
        windowList = WindowList.FromSaveFile(GetDailySaveFileName());

        autoSaveTimer = new(callback: () => Save());
        autoSaveTimer.Start();

        timer1.Start();
    }

    string GetDailySaveFileName()
    {
        string today = DateTime.Now.ToString("yyMMdd");

        return Path.Combine(Program.AppDir, $"{today}.pack");
    }

    bool Save() => windowList.Save(GetDailySaveFileName());

    private void timer1_Tick(object sender, EventArgs e)
    {
        WindowData active = windowList.GetActiveWindowData();

        active.ActiveTimeTotal++;
        active.ActiveTimeSession++;

        if (active.ProcessName != appProcName)
        {
            focusMode.Text = $"フォーカス[{active.ProcessName}]";
            lastActive = active;
        }

        UpdateView(active);
    }

    void UpdateView(WindowData? wd =null) {
        wd ??= windowList.GetActiveWindowData();

        if (windowList.FocusWindow != null)
            UpdateViewFocusWindow(wd);
        else if (wd.ProcessName != appProcName)    
            UpdateViewActiveWindow(wd);
    }

    void UpdateNotifyIcon(bool focus)
    {
        // どうやらリソースからのアイコンの取得で例外吐いて落ちることがあるっぽいので経過観察
        // Disposeちゃんとしたら落ちなくなったっぽい？ 様子見 1.0.4.4
        notifyIcon1.Icon?.Dispose();
        notifyIcon1.Icon = focus ? Resource1.focus : Resource1.focusout;
    }

    void UpdateViewFocusWindow(WindowData aw)
    {
        var fw = windowList.GetFocusWindowData();

        if (fw == null) return;

        this.Text = fw.ProcessName + "[focus]";

        if (fw.ProcessName == aw.ProcessName)
        {
            this.BackColor = fw.Color;
            UpdateNotifyIcon(true);
        }
        else
        {
            this.BackColor = Color.LightGray;
            UpdateNotifyIcon(false);
        }
        UpdateTimer(fw);
    }

    void UpdateViewActiveWindow(WindowData aw)
    {
        this.Text = aw.ProcessName;
        this.BackColor = aw.Color;
        UpdateTimer(aw);
    }

    void UpdateTimer(WindowData wd)
    {
        int time = 0;
        if (viewTotalTime.Checked)
            time = wd.ActiveTimeTotal;
        else if (viewSessionTime.Checked)
            time = wd.ActiveTimeSession;

        ActiveTimeLabel.Text = fmt_hms(time);

        this.notifyIcon1.Text = $"{this.Text} {ActiveTimeLabel.Text}";
    }

    private void ResetEvent(object sender, EventArgs e)
        => windowList.ResetSessionTime();


    private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
    {
#if !DEBUG
        e.Cancel = true;
        this.WindowState = FormWindowState.Minimized;
#endif
        Save();
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
        {
            windowList.FocusWindow = lastActive?.ProcessName;
            UpdateView();
            focusMode.Text = $"フォーカス終了";
        }
        else
            windowList.FocusWindow = null;
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
        Save();
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

