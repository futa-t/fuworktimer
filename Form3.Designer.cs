namespace fuworktimer
{
    partial class Form3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            pictureBox1 = new PictureBox();
            label1 = new Label();
            LVersion = new Label();
            Lgithub = new LinkLabel();
            Lcopyright = new LinkLabel();
            label2 = new Label();
            label3 = new Label();
            linkLabel2 = new LinkLabel();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Resource1.focus_png;
            pictureBox1.Location = new Point(12, 10);
            pictureBox1.Margin = new Padding(4, 8, 4, 8);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(120, 120);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("メイリオ", 9.75F);
            label1.Location = new Point(214, 10);
            label1.Margin = new Padding(4);
            label1.Name = "label1";
            label1.Size = new Size(85, 20);
            label1.TabIndex = 1;
            label1.Text = "fuworktimer";
            // 
            // LVersion
            // 
            LVersion.AutoSize = true;
            LVersion.Font = new Font("メイリオ", 9.75F);
            LVersion.Location = new Point(214, 38);
            LVersion.Margin = new Padding(4);
            LVersion.Name = "LVersion";
            LVersion.Size = new Size(47, 20);
            LVersion.TabIndex = 2;
            LVersion.Text = "0.0.0 ";
            // 
            // Lgithub
            // 
            Lgithub.AutoSize = true;
            Lgithub.Font = new Font("メイリオ", 9.75F);
            Lgithub.LinkBehavior = LinkBehavior.NeverUnderline;
            Lgithub.Location = new Point(294, 10);
            Lgithub.Name = "Lgithub";
            Lgithub.Size = new Size(64, 20);
            Lgithub.TabIndex = 3;
            Lgithub.TabStop = true;
            Lgithub.Tag = "https://github.com/futa-t/fuworktimer";
            Lgithub.Text = "(GitHub)";
            Lgithub.LinkClicked += LinkClicked;
            // 
            // Lcopyright
            // 
            Lcopyright.AutoSize = true;
            Lcopyright.Font = new Font("メイリオ", 9.75F);
            Lcopyright.LinkBehavior = LinkBehavior.NeverUnderline;
            Lcopyright.Location = new Point(140, 110);
            Lcopyright.Name = "Lcopyright";
            Lcopyright.Size = new Size(169, 20);
            Lcopyright.TabIndex = 4;
            Lcopyright.TabStop = true;
            Lcopyright.Tag = "https://github.com/futa-t";
            Lcopyright.Text = "Copyright (c) 2025 futa-t";
            Lcopyright.LinkClicked += LinkClicked;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("メイリオ", 9.75F);
            label2.Location = new Point(140, 38);
            label2.Margin = new Padding(4);
            label2.Name = "label2";
            label2.Size = new Size(74, 20);
            label2.TabIndex = 2;
            label2.Text = "バージョン";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("メイリオ", 9.75F);
            label3.Location = new Point(140, 66);
            label3.Margin = new Padding(4);
            label3.Name = "label3";
            label3.Size = new Size(74, 20);
            label3.TabIndex = 2;
            label3.Text = "ライセンス";
            // 
            // linkLabel2
            // 
            linkLabel2.AutoSize = true;
            linkLabel2.Font = new Font("メイリオ", 9.75F);
            linkLabel2.LinkBehavior = LinkBehavior.NeverUnderline;
            linkLabel2.Location = new Point(214, 66);
            linkLabel2.Name = "linkLabel2";
            linkLabel2.Size = new Size(84, 20);
            linkLabel2.TabIndex = 3;
            linkLabel2.TabStop = true;
            linkLabel2.Tag = "https://github.com/futa-t/fuworktimer/blob/main/LICENSE";
            linkLabel2.Text = "MIT License";
            linkLabel2.LinkClicked += LinkClicked;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("メイリオ", 9.75F);
            label4.Location = new Point(140, 10);
            label4.Margin = new Padding(4);
            label4.Name = "label4";
            label4.Size = new Size(61, 20);
            label4.TabIndex = 2;
            label4.Text = "アプリ名";
            // 
            // Form3
            // 
            AutoScaleDimensions = new SizeF(10F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(364, 141);
            Controls.Add(Lcopyright);
            Controls.Add(linkLabel2);
            Controls.Add(Lgithub);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(label3);
            Controls.Add(LVersion);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Font = new Font("メイリオ", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form3";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "アプリ情報";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label label1;
        private Label LVersion;
        private LinkLabel Lgithub;
        private LinkLabel Lcopyright;
        private Label label2;
        private Label label3;
        private LinkLabel linkLabel2;
        private Label label4;
    }
}