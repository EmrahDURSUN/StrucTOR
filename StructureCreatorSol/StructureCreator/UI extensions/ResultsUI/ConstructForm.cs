using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpaceClaim.Api.V19;
using StructureCreator.Properties;

namespace StructureCreator.UI_extensions.ResultsUI
{
    /// <summary>
    /// Represents construct form. User can choose a csv3 file that will be created
    /// </summary>
    public partial class ConstructForm : Form
    {
        String csvPath = "";
        public ConstructForm()
        {
            InitializeComponent();

            Settings set = Settings.Default;
            csvPath = set.csv3Path;

            label3.Text = csvPath;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Settings set = Settings.Default;
            set.csv3Path = csvPath;
            set.Save();

            try
            {
                Command.Execute(ConstructCapsule.CommandName); // Call command that does construction
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Info");
            }

            Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // change csv file
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
            Settings.Default.ConstructFormOpened = false;
            Settings.Default.Save();
        }
    }
}
