using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Lan_State_PC_CLIENT
{
    public partial class About_app : Form
    {
        public About_app()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var url = new ProcessStartInfo("https://github.com/perforator-coder/Lan-State-PC-CLIENT");
            url.UseShellExecute = true;
            Process.Start(url);
        }
    }
}
