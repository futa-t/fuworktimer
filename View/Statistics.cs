using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

using fuworktimer.Model;

using static fuworktimer.Utility.TimeFormat;

namespace fuworktimer;

internal partial class Statistics : Form
{
    WindowList _windowList;

    int scroolDownRange;
    int scroolUpRange;
    int scroolOffset = 40;

    Dictionary<string, Label> procTotalTimeLabel = [];

    public Statistics(WindowList windowList)
    {
        _windowList = windowList;
        InitializeComponent();
        this.MouseWheel += Form2_MouseWheel;
        scroolUpRange = tableLayoutPanel1.Location.Y;
        UpdateScroolDownRange();
        UpdateView();
    }

    void UpdateScroolDownRange()
    {
        scroolDownRange = this.Size.Height - this.tableLayoutPanel1.Height - scroolOffset - this.Padding.Bottom;
    }

    private void Form2_MouseWheel(object? sender, MouseEventArgs e)
    {
        var y = tableLayoutPanel1.Location.Y;

        if (e.Delta > 0)
            y = Math.Min(y + scroolOffset, scroolUpRange);
        else
            y = Math.Max(y - scroolOffset, scroolDownRange);

        tableLayoutPanel1.Location = new Point(tableLayoutPanel1.Location.X, y);
    }

    void UpdateView()
    {
        foreach (var wd in _windowList.WindowDataList.Where(wd => wd.ActiveTimeTotal > 0).OrderByDescending(w => w.ActiveTimeTotal))
        {
            if (procTotalTimeLabel.TryGetValue(wd.ProcessName, out var ltotaltime))
                ltotaltime.Text = fmt_dhms(wd.ActiveTimeTotal);
            else
                AddControl(wd);
        }

        UpdateScroolDownRange();
    }

    void AddControl(WindowData wd)
    {
        var panel = new Panel { Dock = DockStyle.Fill };
        var colorLabel = new Panel { BackColor = wd.Color, Width = 8, Location = new(0, 0) };
        panel.Controls.Add(colorLabel);
        var lprocname = new Label { Text = wd.ProcessName, Location = new(10, 0), Font = new(new FontFamily("メイリオ"), 14), AutoSize = true, TextAlign = ContentAlignment.MiddleLeft };
        panel.Height = lprocname.Height + 8;
        panel.Controls.Add(lprocname);

        var ltotaltime = new Label { Text = fmt_dhms(wd.ActiveTimeTotal), Location = new(10, 0), Font = new(new FontFamily("メイリオ"), 14), AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill };
        procTotalTimeLabel[wd.ProcessName] = ltotaltime;

        tableLayoutPanel1.RowCount++;
        int row = tableLayoutPanel1.RowCount - 1;
        tableLayoutPanel1.RowStyles.Insert(0, new RowStyle(SizeType.AutoSize));
        tableLayoutPanel1.Controls.Add(panel, 0, row);
        tableLayoutPanel1.Controls.Add(ltotaltime, 1, row);
        Debug.WriteLine($"Added {wd.ProcessName} {panel.Location.ToString()}");
    }

    private void UpdateButton_Click(object sender, EventArgs e) => UpdateView();
}
