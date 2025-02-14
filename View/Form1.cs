using System.Diagnostics;

using fuworktimer.Model;

using static fuworktimer.Utility.TimeFormat;

namespace fuworktimer;

public partial class Form1 : Form
{
    WindowList windowList;
    AutoSaveTimer autoSaveTimer;

    WindowData lastActive;
    readonly string appProcName;

    bool? isFocusActive = null;
    bool CloseTaskTray => CloseToTaskTray.Checked;

    public Form1()
    {
        using (Process p = Process.GetCurrentProcess())
        {
            this.appProcName = p.ProcessName;
        }

        InitializeComponent();
        windowList = WindowList.FromSaveFile(GetDailySaveFileName());

        lastActive = windowList.GetActiveWindowData();

        autoSaveTimer = new(callback: () => Save());
        autoSaveTimer.Start();
#if DEBUG
        CloseToTaskTray.Checked = false;
#endif
        timer1.Start();

    }

    static string GetDailySaveFileName()
    {
        string today = DateTime.Now.ToString("yyMMdd");

        return Path.Combine(Program.AppDir, $"{today}.pack");
    }

    bool Save() => windowList.Save(GetDailySaveFileName());

    private void timer1_Tick(object sender, EventArgs e)
    {
        WindowData active = windowList.GetActiveWindowData();

        active.AddActiveTime();

        if (active.ProcessName != appProcName)
        {
            if (windowList.FocusWindow == null)
                focusMode.Text = $"{focusMode.Tag}[{active.ProcessName}]";
            colorChange.Text = $"{colorChange.Tag}[{active.ProcessName}]";
            lastActive = active;
        }

        UpdateView(active);
    }

    void UpdateView(WindowData? wd = null)
    {
        wd ??= windowList.GetActiveWindowData();

        if (windowList.FocusWindow != null)
            UpdateViewFocusWindow(wd);
        else if (wd.ProcessName != appProcName)
            UpdateViewActiveWindow(wd);
    }

    void UpdateViewFocusWindow(WindowData aw)
    {
        var fw = windowList.GetFocusWindowData();

        if (fw == null) return;

        this.Text = fw.DisplayName + "[focus]";

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
        UpdateActiveTimer(fw);
    }

    void UpdateViewActiveWindow(WindowData aw)
    {
        this.Text = aw.DisplayName;
        this.BackColor = aw.Color;
        UpdateActiveTimer(aw);
    }

    void UpdateActiveTimer(WindowData wd)
    {
        int time = 0;
        if (viewTotalTime.Checked)
            time = wd.TotalTime;
        else if (viewSessionTime.Checked)
            time = wd.SessionTime;

        ActiveTimeLabel.Text = fmt_hms(time);

        this.notifyIcon1.Text = $"{this.Text} {ActiveTimeLabel.Text}";
    }

    private void ResetEvent(object sender, EventArgs e)
        => windowList.ResetSessionTime();


    private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
    {

        if (CloseTaskTray)
        {
            e.Cancel = true;
            this.WindowState = FormWindowState.Minimized;
        }
        Save();
    }

    private void ViewTimeChecked(object sender, EventArgs e)
    {
        if (sender is not ToolStripMenuItem s)
            return;

        foreach (var item in contextMenuStrip1.Items)
            if (item is ToolStripMenuItem a && a.Tag?.ToString() == "TimeFmt")
                a.Checked = s == item;

        s.Checked = true;
        UpdateView(lastActive);
    }

    void SetFocusMode(bool state)
    {
        if (state)
        {
            windowList.FocusWindow = lastActive?.ProcessName;
            focusMode.Text = $"フォーカス終了";
        }
        else
        {
            windowList.FocusWindow = null;
            focusMode.Text = $"{focusMode.Tag}[{lastActive.ProcessName}]";
            isFocusActive = null;
            UpdateNotifyIcon(true);
        }
        UpdateView(lastActive);
    }

    private void FocusModeClick(object sender, EventArgs e)
        => SetFocusMode(focusMode.Checked);

    //  https://blog.ch3cooh.jp/entry/20080113/1311501361
    private void Form1_ClientSizeChanged(object sender, EventArgs e)
    {
        if (this.WindowState == FormWindowState.Minimized)
        {
            this.Hide();
            notifyIcon1.Visible = true;
        }
    }


    void UpdateNotifyIcon(bool? focus)
    {
        if (focus == isFocusActive) return;

        isFocusActive = focus;
        
        notifyIcon1.Icon?.Dispose();
        if (focus is bool f)
            notifyIcon1.Icon = f ? Resource1.focus : Resource1.focusout;
        else
            notifyIcon1.Icon = Resource1.focus;
    }

    private void NotifyIconCloseClick(object sender, EventArgs e)
    {
        this.FormClosing -= Form1_FormClosing;
        Save();
        this.Close();
    }

    private void NotifyIconClick(object sender, EventArgs e)
    {
        if (((MouseEventArgs)e).Button == MouseButtons.Left)
        {
            this.Visible = true;
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;
            this.Activate();
        }
    }

    private void ColorChange(object sender, EventArgs e)
    {
        var dialog = new ColorDialog();
        dialog.ShowDialog();
        lastActive.Color = dialog.Color;
        UpdateView(lastActive);
        Save();
    }

    private void OpenStatistics_Click(object sender, EventArgs e)
    {
        var f = new Statistics(windowList);
        f.Show();
    }

    private void AppInfo_Click(object sender, EventArgs e)
    {
        (new Form3()).Show();
    }
}

