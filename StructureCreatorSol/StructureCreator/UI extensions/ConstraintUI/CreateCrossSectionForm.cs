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

namespace StructureCreator.UI_extensions.ConstraintUI
{
    /// <summary>
    /// Represents create cross section form. User can choose point distance which is saved and used for working point creation.
    /// </summary>
    public partial class CreateCrossSectionForm : Form
    {
        double distance = 0.0;

        public CreateCrossSectionForm()
        {
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(CreateCrossSectionForm_FormClosed);
            InitializeComponent();

          

            backgroundWorker1.DoWork +=
                new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(
            backgroundWorker1_RunWorkerCompleted);
        }


        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown.
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Info");
            }
            else if (e.Cancelled)
            {
                // Next, handle the case where the user canceled 
                // the operation.
                // the operation.
                // the operation.
                // Note that due to a race condition in 
                // the DoWork event handler, the Cancelled
                // flag may not have been set, even though
                // CancelAsync was called.
                MessageBox.Show("Optimization canceled!", "Info");
            }
            else
            {
                // Finally, handle the case where the operation 
                // succeeded.
                Close();
            }

        }

        private void backgroundWorker1_DoWork(object sender,
          DoWorkEventArgs e)
        {
            // Get the BackgroundWorker that raised this event.
            BackgroundWorker worker = sender as BackgroundWorker;

            //bar.limit = tmlim;
            //bar.backgroundWorker1.RunWorkerAsync();
            //bar.StartPosition = FormStartPosition.CenterScreen;
            //bar.Show();

            // Assign the result of the computation
            // to the Result property of the DoWorkEventArgs
            // object. This is will be available to the 
            // RunWorkerCompleted eventhandler.

            try
            {
                
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("An error occured: " + ex.ToString(), "Info");
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // Save csv2 path in settings and start worker.

                Settings set = Settings.Default;
                DateTime now = DateTime.Now;
                string datum = now.ToString("dd-MM-yyy");
                datum = datum.Replace("-", "");

                // create csv2 file

                String csv2 = Settings.Default.ProjectPath + "\\CSV\\" + Settings.Default.ProjectName + "_csv2_" + datum + ".csv";
                set.csv2Path = csv2;

                set.distance = (double)numericUpDown1.Value;
                set.Save();

                backgroundWorker1.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured : "+ ex.ToString(), "Info");
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            // Save new value
            distance = (double)numericUpDown1.Value;
        }

        private void CreateCrossSectionForm_FormClosed(Object sender, FormClosedEventArgs e)
        {
            Settings.Default.CrossSecFormOpened = false;
            Settings.Default.Save();
        }

        private void CreateCrossSectionForm_FormClosing(Object sender, FormClosingEventArgs e)
        {
            Settings.Default.CrossSecFormOpened = false;
            Settings.Default.Save();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Settings.Default.CrossSecFormOpened = false;
            Settings.Default.Save();
        }
    }
}
