using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace foesmm.common
{
    public partial class CrashReporter : Form
    {
        private string Crashdump { get; set; }

        public CrashReporter(FoESMM app, string reason, string crashdump)
        {
            Crashdump = crashdump;

            InitializeComponent();

            errorImage.Image = SystemIcons.Error.ToBitmap();

            errorReason.Text = reason;

            crashdumpContents.Lines = File.ReadAllLines(crashdump);
        }

        private void CloseClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ShowDumpClick(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("explorer.exe", $"/select,\"{Crashdump}\"");
        }

        private void DetailsClick(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (crashdumpContents.Visible)
            {
                Clipboard.SetText(crashdumpContents.Text);
            }
            else
            {
                crashdumpContents.Visible = true;
                detailsButton.Text = "Copy Crushdump";
            }
        }
    }
}
