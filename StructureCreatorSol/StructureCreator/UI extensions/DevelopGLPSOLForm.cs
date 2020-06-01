using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StructureCreator.Properties;

namespace StructureCreator.UI_extensions
{
    // Uses GLPSOlver to create a lp file based on mod file using selected parameter
    public partial class DevelopGLPSOLForm : Form
    {
        String datumG = "";

        String command = "";
        String modPath = "";

        String projectName = "";
        String projectFolder = "";

        String logPath = "";

        public DevelopGLPSOLForm()
        {
            InitializeComponent();

            backgroundWorker1.DoWork +=
               new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(
            backgroundWorker1_RunWorkerCompleted);

            backgroundWorker1.WorkerSupportsCancellation = true;
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
                label5.Visible = true;
                label8.Text = "Completed";
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
                //Setting an instance of ProcessStartInfo class
                // under System.Diagnostic Assembly Reference
                ProcessStartInfo pro = new ProcessStartInfo();
                //pro.WorkingDirectory = projectFolder;
                //Setting the FileName to be Started like in our
                //Project we are just going to start a CMD Window.
                pro.FileName = "cmd.exe";
                pro.Arguments = "/C " + textBox2.Text;

                pro.RedirectStandardOutput = true;
                pro.UseShellExecute = false;

                Process proStart = new Process();
                //Setting up the Process Name here which we are 
                // going to start from ProcessStartInfo
                proStart.StartInfo = pro;

                // Show cmd, when checkbox in options is checked
                if (Settings.Default.cmdActive)
                {
                    proStart.StartInfo.CreateNoWindow = true;
                    proStart.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                }
                else
                {
                    proStart.StartInfo.CreateNoWindow = true;
                    proStart.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                }


                //Calling the Start Method of Process class to
                // Invoke our Process viz 'cmd.exe'

                proStart.Start();

                //using (System.IO.StreamWriter file =
                //  new System.IO.StreamWriter(logPath))
                //{
                //    file.WriteLine(proStart.StandardOutput.ReadToEnd());
                //}

                // Show cmd, when checkbox in options is checked

                StreamReader reader = proStart.StandardOutput;
                string output = reader.ReadToEnd();

                Console.WriteLine(output);

                using (System.IO.StreamWriter file =
                 new System.IO.StreamWriter(logPath))
                {
                    file.WriteLine(output);
                }

                //MessageBox.Show(logPath);

                // open log file, when show log is activated
                if (Settings.Default.cmdActive)
                {
                    // Open log file
                    ProcessStartInfo pro2 = new ProcessStartInfo();
                    //Setting the FileName to be Started like in our
                    //Project we are just going to start a CMD Window.
                    pro2.FileName = "cmd.exe";
                    pro2.Arguments = "/C " + "start /max " + logPath;

                    //pro.RedirectStandardOutput = true;
                    //pro.UseShellExecute = false;

                    Process proStart2 = new Process();
                    //Setting up the Process Name here which we are 
                    // going to start from ProcessStartInfo
                    proStart2.StartInfo = pro2;
                    proStart2.StartInfo.CreateNoWindow = true;
                    //proStart.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                    //Calling the Start Method of Process class to
                    // Invoke our Process viz 'cmd.exe'

                    proStart2.Start();
                }
                else
                {

                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show("An error occured: " + ex.ToString(), "Info");
            }

        }



        // Calls documentation of glpsol 
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://en.wikibooks.org/wiki/GLPK/Using_GLPSOL");
        }

        // Inserts all selected parameter to glpsol command
        private void button4_Click(object sender, EventArgs e)
        {
            foreach (object itemChecked in checkedListBox1.CheckedItems)
            {
                int pos = itemChecked.ToString().IndexOf(' ');
                String temp = itemChecked.ToString().Substring(0, pos);

                // Put Options after glpsol
                String tempS = command;
                String sub1 = tempS.Substring(0, 6);
                String sub2 = tempS.Substring(6, tempS.Length - 6);

                sub1 += " ";
                sub1 += temp;
                sub1 += " " + sub2;

                command = sub1;
            }

            textBox1.Text = command;
            textBox2.Text = command;
        }

        // Menu bar click
        private void glpsolOptimizerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.BringToFront();
        }

        // Menu bar click
        private void glpsoloptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel2.BringToFront();

            // Check frequently used options
            checkedListBox1.SetItemChecked(16, true);
            checkedListBox1.SetItemChecked(59, true);
            checkedListBox1.SetItemChecked(60, true);
            checkedListBox1.SetItemChecked(61, true);
            checkedListBox1.SetItemChecked(58, true);
        }

        // Choose a project folder 
        private void button2_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            string datum = now.ToString("dd-MM-yyy");
            datum = datum.Replace("-", "");
            datumG = datum;

            string path = "";
            string folder = "";


            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK) // if user didn't cancel
            {
                path = fbd.SelectedPath;
                folder = Path.GetFileName(path);
            }

            label4.Text = folder;
            label4.Font = new Font(label4.Font, FontStyle.Regular);

            projectFolder = path;
            projectName = folder;

            command = "glpsol -m < mod file > --wlp < lp file >";
            textBox2.Text = command;

            logPath += path;
            logPath += "\\glpsolLOG_" + datum + ".log";
        }

        // Chooses a mod file and inserts the file path into the command 
        private void button1_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            string datum = now.ToString("dd-MM-yyy");
            datum = datum.Replace("-", "");

            string file = "";

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Chose mod file";
            ofd.Filter = "Solution File|*.mod";

            if (ofd.ShowDialog() == DialogResult.OK) // if user didn't cancel
            {
                file = ofd.FileName; // full File Path
            }
            modPath = file;

            // Include modPath and lpPath into command 

            command = command.Replace("< mod file >", modPath);
            command = command.Replace("< lp file >", projectFolder += "\\CSV\\" + projectName + "_" + datum + ".lp");

            textBox1.Text = command;
            textBox2.Text = command;
        }

        // Executes generated command on the command line
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                label8.Text = "Process running...";
                backgroundWorker1.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString(), "Info");
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
