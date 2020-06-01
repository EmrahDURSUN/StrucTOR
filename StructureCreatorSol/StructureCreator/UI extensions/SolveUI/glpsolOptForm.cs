using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StructureCreator.Properties; 

namespace StructureCreator.UI_extensions.SolveUI
{
    /// Uses GLPSOlver to create a lp file based on mod file using selected parameter
    public partial class glpsolOptForm : Form
    {

        String command = ""; // glpsol command
        String modFilePath = "";
        String lpFilePath = "";

        String projectFilePath = "";
        String projectName = "";
        String projectFolder = "";

        String logFilePath = "";

        int currentPage = 0;
        bool fileChosen = false;

        public glpsolOptForm()
        {
            InitializeComponent();

            DateTime now = DateTime.Now;
            string datum = now.ToString("dd-MM-yyy");
            datum = datum.Replace("-", "");

            Settings set = Settings.Default;
            label3.Text = set.ProjectName;
            label5.Text = set.ProjectName;


            projectFilePath = set.ProjectPath;
            projectName = set.ProjectName;
            panel1.BringToFront();


            command = "glpsol -m < mod file > --wlp < lp file >"; // Dummy command
            textBox1.Text = command;

            logFilePath += set.ProjectPath;
            logFilePath += "\\Optimization\\" + projectName + "glpk_" + datum + ".log";

            set.lplogPath = logFilePath;
            set.Save();

            // Check frequently used options
            checkedListBox1.SetItemChecked(16, true);
            checkedListBox1.SetItemChecked(59, true);
            checkedListBox1.SetItemChecked(60, true);
            checkedListBox1.SetItemChecked(61, true);
            checkedListBox1.SetItemChecked(58, true);

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
                //label5.Visible = true;
                label8.Text = "Optimization completed!";
                button4.Enabled = true;

                DateTime now = DateTime.Now;
                string datum = now.ToString("dd-MM-yyy");
                datum = datum.Replace("-", "");

                // Save lp path in settings
                Settings set = Settings.Default;
                set.lpPath = projectFilePath + "\\Optimization\\" + projectName + "_" + datum + ".lp";
                set.Save();
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
                pro.Arguments = "/C " + textBox1.Text;

                pro.RedirectStandardOutput = true;
                pro.UseShellExecute = false;

                Process proStart = new Process();
                //Setting up the Process Name here which we are 
                // going to start from ProcessStartInfo
                proStart.StartInfo = pro;
                proStart.StartInfo.CreateNoWindow = true;
                proStart.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                //Calling the Start Method of Process class to
                // Invoke our Process viz 'cmd.exe'

                proStart.Start();

                //using (System.IO.StreamWriter file =
                //  new System.IO.StreamWriter(logPath))
                //{
                //    file.WriteLine(proStart.StandardOutput.ReadToEnd());
                //}
                StreamReader reader = proStart.StandardOutput;
                string output = reader.ReadToEnd();

                Console.WriteLine(output);

                using (System.IO.StreamWriter file =
                  new System.IO.StreamWriter(logFilePath))
                {
                    file.WriteLine(output);
                }
                

                // Open logfile in Window

            }
            catch (System.Exception ex)
            {
                MessageBox.Show("An error occured: " + ex.ToString(), "Info");
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            // Next button 
            if (currentPage == 0)
            {
                panel2.BringToFront();
                currentPage = 1;
                button3.Enabled = false;
                button2.Enabled = true;

                // Delete prior chosen options 
                if (fileChosen)
                {
                    String tempS = command;
                    String sub1 = tempS.Substring(0, 6); // glpsol

                    int pos2 = tempS.IndexOf('C');
                    pos2 -= 3;
                    String sub2 = tempS.Substring(6, pos2); // used options of prior selection
                    int pos3 = tempS.Length - (sub1.Length + sub2.Length);
                    pos3 += 5;
                    String sub3 = tempS.Substring(pos2, pos3); // files

                    sub1 += " ";
                    sub1 += sub3;
                    sub1 += "p";

                    command = sub1;

                    textBox1.Text = command;
                }
                else if (!fileChosen)
                {
                    String tempS = command;
                    String sub1 = tempS.Substring(0, 6); // glpsol

                    int pos2 = command.IndexOf('<');
                    pos2 -= 3; 
                    String sub2 = tempS.Substring(6, pos2); // used options of prior selection
                    int pos3 = tempS.Length - (sub1.Length + sub2.Length);
                    pos3 += 5;
                    String sub3 = tempS.Substring(pos2, pos3); // files

                    sub1 += " ";
                    sub1 += sub3;
                    sub1 += ">";

                    command = sub1;

                    textBox1.Text = command;
                }


                // Inserts all selected parameter to glpsol command
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

                    textBox1.Text = command;
                }
            }
            else if (currentPage == 1)
            {
                
            }
        }

        // Calls documentation of glpsol 
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://en.wikibooks.org/wiki/GLPK/Using_GLPSOL");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Settings set = Settings.Default;
            set.modPath = modFilePath;
            set.lpPath = lpFilePath;
            set.lplogPath = logFilePath;
            set.cplexCommand = true; // Activate next workflow button in the ribbon
            set.Save();

            SpaceClaim.Api.V19.Window.ActiveWindow.ZoomExtents(); // used to Force a window update, so the Ribbon Button is enabled/disabled

            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Back button 
            if(currentPage == 0)
            {

            }else if(currentPage == 1)
            {
                panel1.BringToFront();
                currentPage = 0;
                button2.Enabled = false;
                button3.Enabled = true; 
            }
        }

        // Chooses a mod file and inserts the file path into the command 
        private void button5_Click(object sender, EventArgs e)
        {
            if (modFilePath.Equals(""))
            {
                DateTime now = DateTime.Now;
                string datum = now.ToString("dd-MM-yyy");
                datum = datum.Replace("-", "");

                string file = "";

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Chose mod file";
                ofd.Filter = "Problem File|*.mod";

                if (ofd.ShowDialog() == DialogResult.OK) // if user didn't cancel
                {
                    file = ofd.FileName; // full File Path

                    modFilePath = file;

                    // Include modPath and lpPath into command 

                    command = command.Replace("< mod file >", modFilePath);
                    command = command.Replace("< lp file >", projectFilePath + "\\Optimization\\" + projectName + "_" + datum + ".lp");

                    Settings set = Settings.Default;
                    set.lpPath = projectFilePath + "\\Optimization\\" + projectName + "_" + datum + ".lp";
                    set.Save();


                    textBox1.Text = command;
                    button6.Enabled = true;

                    label7.Text = modFilePath;

                    fileChosen = true;

                    // Copy .mod files in project folder 
                    String filename = Path.GetFileName(file);

                    if (File.Exists(set.ProjectPath + "\\Optimization\\" + filename))
                    {

                    }
                    else
                    {
                        File.SetAttributes(modFilePath, FileAttributes.Normal);
                        File.Copy(modFilePath, set.ProjectPath + "\\Optimization\\" + filename);
                    }
                }
            }
            else
            {
                // changes mod file in file path

                DateTime now = DateTime.Now;
                string datum = now.ToString("dd-MM-yyy");
                datum = datum.Replace("-", "");

                string file = "";

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Chose mod file";
                ofd.Filter = "Problem File|*.mod";

                if (ofd.ShowDialog() == DialogResult.OK) // if user didn't cancel
                {
                    file = ofd.FileName; // full File Path
                    
                    // Include modPath and lpPath into command 

                    command = command.Replace(modFilePath, file);

                    modFilePath = file; 
                    
                    textBox1.Text = command;
                    button6.Enabled = true;
                    label7.Text = modFilePath;

                    fileChosen = true;

                    // Copy .mod files in project folder 
                    String filename = Path.GetFileName(file);

                    if (File.Exists(Settings.Default.ProjectPath + "\\Optimization\\" + filename))
                    {

                    }
                    else
                    {
                        File.SetAttributes(modFilePath, FileAttributes.Normal);
                        File.Copy(modFilePath, Settings.Default.ProjectPath + "\\Optimization\\" + filename);
                    }
                }
            }
            
            
        }


        // Executes generated command on the command line
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                //button1.Enabled = false;
                //button2.Enabled = false;
                //button3.Enabled = false;
                //button4.Enabled = false;
                //button5.Enabled = false;
                //button6.Enabled = false;

                label8.Text = "Process running...";
                backgroundWorker1.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString(), "Info");
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Settings.Default.GLPKFormOpened = false;
            Settings.Default.Save();
        }
    }
}
