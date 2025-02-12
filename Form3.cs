using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fuworktimer
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            LVersion.Text = Program.GetFileVersion();
            Lgithub.Links.Add(1, Lgithub.Text.Length - 2);
        }

        private void LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (sender is LinkLabel l && l.Tag is string url)
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
    }
}
