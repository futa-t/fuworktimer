using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

using MemoryPack;

using static fuworktimer.Win32;
using static fuworktimer.ColorUtil;
using static fuworktimer.TimeFormat;

using System.Xml.Serialization;

namespace fuworktimer;

public partial class Form1 : Form
{
    private WindowData? currentActive;
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
        windowList = WindowList.FromDailySaveFile();

        autoSaveTimer = new(callback: () => windowList.Save());
        autoSaveTimer.Start();

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
        UpdateActiveWindow(windowList.GetFocusWindow() ?? currentActive);
    }

    WindowData? GetActioveWindow()
    {
        WindowData wd = windowList.GetActiveWindw();

        if (wd.ProcessName == appProcName) return null;

        return wd;
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
        }
        catch (Exception ex)
        {
            Program.ErrorLog(ex);
        }


        int time = 0;
        if (viewTotalTime.Checked)
            time = window.ActiveTimeTotal;
        else if (viewSessionTime.Checked)
            time = window.ActiveTimeSession;

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
        windowList.Save();
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
        windowList.Save();
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

