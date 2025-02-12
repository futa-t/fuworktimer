namespace fuworktimer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            timer1 = new System.Windows.Forms.Timer(components);
            ActiveTimeLabel = new Label();
            contextMenuStrip1 = new ContextMenuStrip(components);
            focusMode = new ToolStripMenuItem();
            colorChange = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            viewSessionTime = new ToolStripMenuItem();
            viewTotalTime = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            リセットToolStripMenuItem = new ToolStripMenuItem();
            CloseToTaskTray = new ToolStripMenuItem();
            閉じるToolStripMenuItem1 = new ToolStripMenuItem();
            notifyIcon1 = new NotifyIcon(components);
            taskiconMenu = new ContextMenuStrip(components);
            閉じるToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            taskiconMenu.SuspendLayout();
            SuspendLayout();
            // 
            // timer1
            // 
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            // 
            // ActiveTimeLabel
            // 
            ActiveTimeLabel.BackColor = Color.Transparent;
            ActiveTimeLabel.Dock = DockStyle.Fill;
            ActiveTimeLabel.Font = new Font("Yu Gothic UI", 20F);
            ActiveTimeLabel.ForeColor = SystemColors.ControlText;
            ActiveTimeLabel.Location = new Point(0, 0);
            ActiveTimeLabel.Margin = new Padding(0);
            ActiveTimeLabel.Name = "ActiveTimeLabel";
            ActiveTimeLabel.Size = new Size(144, 41);
            ActiveTimeLabel.TabIndex = 98;
            ActiveTimeLabel.Text = "00:00:00";
            ActiveTimeLabel.TextAlign = ContentAlignment.TopCenter;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.AllowMerge = false;
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { focusMode, colorChange, toolStripSeparator2, viewSessionTime, viewTotalTime, toolStripSeparator1, リセットToolStripMenuItem, CloseToTaskTray, 閉じるToolStripMenuItem1 });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.ShowCheckMargin = true;
            contextMenuStrip1.ShowImageMargin = false;
            contextMenuStrip1.ShowItemToolTips = false;
            contextMenuStrip1.Size = new Size(181, 192);
            // 
            // focusMode
            // 
            focusMode.CheckOnClick = true;
            focusMode.Name = "focusMode";
            focusMode.Size = new Size(180, 22);
            focusMode.Tag = "フォーカス";
            focusMode.Text = "フォーカス";
            focusMode.Click += FocusModeClick;
            // 
            // colorChange
            // 
            colorChange.Name = "colorChange";
            colorChange.Size = new Size(180, 22);
            colorChange.Tag = "色の変更";
            colorChange.Text = "色の変更";
            colorChange.Click += ColorChange;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(177, 6);
            // 
            // viewSessionTime
            // 
            viewSessionTime.Checked = true;
            viewSessionTime.CheckState = CheckState.Checked;
            viewSessionTime.Name = "viewSessionTime";
            viewSessionTime.Size = new Size(180, 22);
            viewSessionTime.Tag = "TimeFmt";
            viewSessionTime.Text = "セッション";
            viewSessionTime.Click += ViewTimeChecked;
            // 
            // viewTotalTime
            // 
            viewTotalTime.Name = "viewTotalTime";
            viewTotalTime.Size = new Size(180, 22);
            viewTotalTime.Tag = "TimeFmt";
            viewTotalTime.Text = "トータル";
            viewTotalTime.Click += ViewTimeChecked;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(177, 6);
            // 
            // リセットToolStripMenuItem
            // 
            リセットToolStripMenuItem.Name = "リセットToolStripMenuItem";
            リセットToolStripMenuItem.Size = new Size(180, 22);
            リセットToolStripMenuItem.Text = "リセット";
            リセットToolStripMenuItem.Click += ResetEvent;
            // 
            // CloseToTaskTray
            // 
            CloseToTaskTray.Checked = true;
            CloseToTaskTray.CheckOnClick = true;
            CloseToTaskTray.CheckState = CheckState.Checked;
            CloseToTaskTray.Name = "CloseToTaskTray";
            CloseToTaskTray.ShowShortcutKeys = false;
            CloseToTaskTray.Size = new Size(180, 22);
            CloseToTaskTray.Text = "タスクトレイに閉じる";
            // 
            // 閉じるToolStripMenuItem1
            // 
            閉じるToolStripMenuItem1.Name = "閉じるToolStripMenuItem1";
            閉じるToolStripMenuItem1.Size = new Size(180, 22);
            閉じるToolStripMenuItem1.Text = "閉じる";
            閉じるToolStripMenuItem1.Click += NotifyIconCloseClick;
            // 
            // notifyIcon1
            // 
            notifyIcon1.ContextMenuStrip = taskiconMenu;
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "fuworktimer";
            notifyIcon1.Visible = true;
            notifyIcon1.Click += NotifyIconClick;
            // 
            // taskiconMenu
            // 
            taskiconMenu.Items.AddRange(new ToolStripItem[] { 閉じるToolStripMenuItem });
            taskiconMenu.Name = "taskiconMenu";
            taskiconMenu.Size = new Size(105, 26);
            // 
            // 閉じるToolStripMenuItem
            // 
            閉じるToolStripMenuItem.Name = "閉じるToolStripMenuItem";
            閉じるToolStripMenuItem.Size = new Size(104, 22);
            閉じるToolStripMenuItem.Text = "閉じる";
            閉じるToolStripMenuItem.Click += NotifyIconCloseClick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(144, 41);
            ContextMenuStrip = contextMenuStrip1;
            Controls.Add(ActiveTimeLabel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            Name = "Form1";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "fulogger";
            TopMost = true;
            FormClosing += Form1_FormClosing;
            ClientSizeChanged += Form1_ClientSizeChanged;
            contextMenuStrip1.ResumeLayout(false);
            taskiconMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private Label ActiveTimeLabel;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem リセットToolStripMenuItem;
        private ToolStripMenuItem viewTotalTime;
        private ToolStripMenuItem viewSessionTime;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem focusMode;
        private ToolStripSeparator toolStripSeparator2;
        private NotifyIcon notifyIcon1;
        private ContextMenuStrip taskiconMenu;
        private ToolStripMenuItem 閉じるToolStripMenuItem;
        private ToolStripMenuItem 閉じるToolStripMenuItem1;
        private ToolStripMenuItem CloseToTaskTray;
        private ToolStripMenuItem colorChange;
    }
}
