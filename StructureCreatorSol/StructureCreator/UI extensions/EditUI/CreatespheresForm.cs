using SpaceClaim.Api.V19;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StructureCreator.Properties;

namespace StructureCreator.UI_extensions.EditUI
{
    /// <summary>
    /// User can choose a csv3 file and a sphere multiplier for sphere creation.
    /// </summary>
    public partial class CreatespheresForm : Form
    {
        String csvPath = "";
        public CreatespheresForm()
        {
            InitializeComponent();

            Settings set = Settings.Default;

            label3.Text = set.csv3Path;
            csvPath = set.csv3Path;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Save csv3 path
            Settings set = Settings.Default;
            set.sphereMultiplier = (double)numericUpDown1.Value;
            set.csv3Path = csvPath;
            set.Save();

            try
            {
                Command.Execute(PostprocessingCapsule.CommandName); // Call command for sphere creation

            }
            catch (Exception ex)
            {
                MessageBox.Show("Choose an appropriate csv3 file!", "Info");
            }

            Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Postprocess the lattice structure by editing the connections.\r\rA solid sphere is created at all angled joints.The diameter of the solid sphere is a multiple of the largest bar diameter, which is adjacent to a specific connection joint.", "Info");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // Choose a new csv file
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Choose csv file";
            ofd.Filter = "CSV |*.csv";
            ofd.InitialDirectory = Settings.Default.ProjectPath + "\\CSV";


            if (ofd.ShowDialog() == DialogResult.OK) // if user didn't cancel
            {
                label3.Text = ofd.FileName; // full File Path
                //file = Path.GetFileName(path);
                csvPath = ofd.FileName;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Settings.Default.EditFormOpened = false;
            Settings.Default.Save();
        }
    }
}
