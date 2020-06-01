using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StructureCreator.Properties;

namespace StructureCreator.UI_extensions
{
    public partial class OptionsControl : UserControl
    {
        String stylePath = "";
        String stylePath2 = "";
        String exePath = "";

        String stabwerkerzeugerPath = "";

        public OptionsControl()
        {
            InitializeComponent();
            textBox1.Text = Settings.Default.ProjectPath;

            if (Settings.Default.cmdActive)
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }
            
        }

        public void LoadOptions()
        {
            Settings settings = Settings.Default;
            textBox1.Text = settings.ProjectPath;
            textBox2.Text = settings.StyPath;
            textBox3.Text = settings.exePath;
            textBox4.Text = settings.stabwerkerzeugerexePath;
        }

        public void SaveOptions()
        {
            Settings settings = Settings.Default;
            settings.ProjectPath = textBox1.Text;
            settings.StyPath = textBox2.Text;
            settings.exePath = textBox3.Text;
            settings.stabwerkerzeugerexePath = textBox4.Text;
            settings.Save();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveOptions();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog ofd = new FolderBrowserDialog();
            ofd.Description = "Choose project path";

            if (ofd.ShowDialog() == DialogResult.OK) // if user didn't cancel
            {
                Settings set = Settings.Default; 
                set.ProjectPath = ofd.SelectedPath;
                set.Save();

                textBox1.Text = ofd.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Settings set = Settings.Default;
            set.DevelopMode = true;

            set.Save();

            button3.Enabled = false;
            button4.Enabled = true;
            button3.FlatAppearance.BorderColor = Color.Black;

            
            label1.Text = "Developers mode is enabled";

            SpaceClaim.Api.V19.Window.ActiveWindow.ZoomExtents(); // used to Force a window update, so the Ribbon Button is enabled/disabled
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Settings set = Settings.Default;
            set.DevelopMode = false;

            set.Save();

            button4.Enabled = false;
            button3.Enabled = true;
            button4.FlatAppearance.BorderColor = Color.Black;

            label1.Text = "Developers mode is disabled";

            SpaceClaim.Api.V19.Window.ActiveWindow.ZoomExtents(); // used to Force a window update, so the Ribbon Button is enabled/disabled
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string path = "";

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Choose sty file";
            ofd.Filter = "Style File|*.sty";

            if (ofd.ShowDialog() == DialogResult.OK) // if user didn't cancel
            {
                path = ofd.FileName; // full File Path
            }

            textBox2.Text = path;
            stylePath = path;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Settings set = Settings.Default;
            set.StyPath = stylePath;

            set.Save();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string path = "";

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Choose CreateTex.exe";
            ofd.Filter = "Executable|*.exe";

            if (ofd.ShowDialog() == DialogResult.OK) // if user didn't cancel
            {
                path = ofd.FileName; // full File Path
            }

            textBox3.Text = path;
            exePath = path;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Settings set = Settings.Default;
            set.exePath = exePath;

            set.Save();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Settings.Default.cmdActive = true;
                Settings.Default.Save();
            }
            else
            {
                Settings.Default.cmdActive = false;
                Settings.Default.Save();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string path = "";

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Choose Stabwerkserzeuger.exe";
            ofd.Filter = "Executable|*.exe";

            if (ofd.ShowDialog() == DialogResult.OK) // if user didn't cancel
            {
                path = ofd.FileName; // full File Path
            }

            textBox4.Text = path;
            stabwerkerzeugerPath = path;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Settings set = Settings.Default;
            set.stabwerkerzeugerexePath = stabwerkerzeugerPath;

            set.Save();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            UserGuideForm form = new UserGuideForm();
            form.Show();
        }
    }
}
