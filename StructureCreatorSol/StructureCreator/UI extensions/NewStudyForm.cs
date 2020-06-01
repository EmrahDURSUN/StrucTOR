using SpaceClaim.Api.V19;
using StructureCreator.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StructureCreator.UI_extensions
{
    public partial class NewStudyForm : Form
    {
        string proPath = "";
        string proFolder = "";

        public NewStudyForm()
        {
            InitializeComponent();
            panel4.BringToFront();

            panel12.BringToFront(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Choose project folder";

            if (fbd.ShowDialog() == DialogResult.OK) // if user didn't cancel
            {
                proPath = fbd.SelectedPath;
                proFolder = Path.GetFileName(proPath);


                Settings set = Settings.Default;
                set.ProjectPath = proPath;
                set.ProjectName = proFolder;
                set.glpsolCommand = true;
                set.Save();

                SpaceClaim.Api.V19.Window.ActiveWindow.ZoomExtents(); // used to Force a window update, so the Ribbon Button is enabled/disabled

                Directory.CreateDirectory(proPath + "/Latex");
                Directory.CreateDirectory(proPath + "/CSV");
                Directory.CreateDirectory(proPath + "/CAD");
                Directory.CreateDirectory(proPath + "/Optimization");

                try
                {
                    Command.Execute(CreateSCDOCCapsule.CommandName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Info");
                }

                // Reset Settings
                set.csv1Path = "";
                set.csv2Path = "";
                set.csv3Path = "";
                set.modPath = "";
                set.lpPath = "";
                set.lplogPath = "";
                set.cplexlogPath = "";
                set.solFilePath = "";
                set.texFilePath = "";
                set.pdfPath = "";
                set.distance = 1;
                set.forces = "";
                set.pointForceX = 0;
                set.pointForceY = 0;
                set.pointForceZ = 0;
                set.activePoint = "";
                set.xLength = 0;
                set.yLength = 0;
                set.zLength = 0;
                set.forceCount = 0;
                set.activePointCoord = "";
                set.sphereMultiplier = 1;
                set.activePointName = "";
                set.bearingLoads = "";
                set.bearingLoadCount = 0;
                set.deleteArrows = "";

                this.Close();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel2.BringToFront();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel4.BringToFront();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            panel13.BringToFront();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            panel12.BringToFront();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel5.BringToFront();            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            panel10.BringToFront();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            panel9.BringToFront();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            panel8.BringToFront();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panel7.BringToFront();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel6.BringToFront();
        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label28_Click(object sender, EventArgs e)
        {

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Settings.Default.NewStudyFormOpened = false;
            Settings.Default.Save();
        }

        private void label26_Click(object sender, EventArgs e)
        {

        }
    }
}
