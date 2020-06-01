using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ILOG.Concert;
using ILOG.CPLEX;
using System.Diagnostics;

namespace StructureCreator.UI_extensions
{
    public partial class DevelopCPLEXForm : Form
    {
        DevelopOptionsItem[] it = new DevelopOptionsItem[20]; //stores Options

        private String lpFilePath = "";
        private String directory = "";
        private String projectName = "";

        private String datum = "";


        Cplex cplex = null;
        string log = "Log started (V";
        string projekt = "";

        string now = DateTime.Now.ToString();

        StreamWriter w = null; // Stream writer for log file

        Int64 tmlimit = 0;
        String end = "";

        string logfile = "";

        public DevelopCPLEXForm()
        {
            InitializeComponent();

            initializeOptions(); // Initialize useable options

            InitializeBackgroundWorker();


            cplex = new Cplex(); // Instance of CPLEX

            log += cplex.Version;
            log += ") " + " " + DateTime.Now.DayOfWeek.ToString() + " " + now;
            
        }


        private void InitializeBackgroundWorker()
        {
            backgroundWorker1.DoWork +=
                new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(
            backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.ProgressChanged +=
                new ProgressChangedEventHandler(
            backgroundWorker1_ProgressChanged);

            backgroundWorker2.DoWork +=
               new DoWorkEventHandler(backgroundWorker2_DoWork);
            backgroundWorker2.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(
            backgroundWorker2_RunWorkerCompleted);
            backgroundWorker2.ProgressChanged +=
                new ProgressChangedEventHandler(
            backgroundWorker2_ProgressChanged);

            backgroundWorker2.WorkerReportsProgress = true;
        }

        // Choose a project folder from Dialog
        private void button1_Click_1(object sender, EventArgs e)
        {
            string path = "";
            string folder = "";


            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK) // if user didn't cancel
            {
                path = fbd.SelectedPath;
                folder = Path.GetFileName(path);
            }


            label3.Text = folder;
            label3.Font = new Font(label5.Font, FontStyle.Bold);

            directory = path;
            projectName = folder;
        }

        // Show the chosen options in a window
        private void button5_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();

            for (int i = 0; i < it.Length; i++)
            {
                if (it[i] != null)
                {
                    if (it[i].Use == true)
                    {
                        Label l = new Label();
                        l.AutoSize = false;
                        l.Size = new System.Drawing.Size(550, 35);
                        l.Font = new System.Drawing.Font("Microsoft Sans Serif", 11);
                        l.Text = " - " + it[i].Name + ", value: " + it[i].Value;
                        flowLayoutPanel1.Controls.Add(l);
                    }
                }
            }
        }

        // Choose a .lp file to optimize and load it into CPLEX instance
        private void button2_Click_1(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            string datumt = now.ToString("dd-MM-yyy");
            datumt = datumt.Replace("-", "");
            datum = datumt;

            string path = "";
            string file = "";

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Choose lp file";
            ofd.Filter = "Solution File|*.lp";

            if (ofd.ShowDialog() == DialogResult.OK) // if user didn't cancel
            {
                path = ofd.FileName; // full File Path
                file = Path.GetFileName(path);
            }

            label5.Text = file;

            cplex.ImportModel(path); // Import file into instance


            // Write output to log file
            StreamWriter t = File.CreateText(directory + "\\" + projectName + "_" + datum + ".log");
            w = t;
            logfile = directory + "\\" + projectName + "_" + datum + ".log";

            t.WriteLine(log);

            cplex.SetOut(t);

            w.WriteLine();
            w.WriteLine("Problem '" + file + "' read");
            w.WriteLine();
        }

        // Start CPLEX optimization
        private void button3_Click_1(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            now = now.AddSeconds((double)tmlimit);
            end = now.ToString();

            try
            {
                label12.Text = end;
                
                MessageBox.Show("Optimization started!", "Info");

                System.Threading.Thread.Sleep(2);

                backgroundWorker1.RunWorkerAsync();
                backgroundWorker2.RunWorkerAsync();
            }
            catch (ILOG.Concert.Exception ex)
            {
                System.Console.WriteLine("Concert exception caught: " + ex);
            }
        }

        // Add selected options to the CPLEX instance
        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < it.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        //String val = "Cplex.MIPSearch." + it[0].Value;
                        cplex.SetParam(Cplex.Param.MIP.Strategy.Search, 0);
                        break;
                    case 2:
                        cplex.SetParam(Cplex.BooleanParam.TimeLimit, Convert.ToInt64(it[2].Value));
                        tmlimit = Convert.ToInt64(it[2].Value);
                        break;
                    case 3:
                        cplex.SetParam(Cplex.Param.Threads, Convert.ToInt16(it[3].Value));
                        break;
                    case 4:
                        cplex.SetParam(Cplex.Param.WorkMem, Convert.ToInt64(it[4].Value));
                        break;
                    case 5:
                        cplex.SetParam(Cplex.Param.MIP.Limits.TreeMemory, Convert.ToInt64(it[5].Value));
                        break;
                    case 6:
                        cplex.SetParam(Cplex.Param.MIP.Strategy.File, Convert.ToInt16(it[6].Value));
                        break;
                    case 7:
                        cplex.SetParam(Cplex.Param.Advance, Convert.ToInt16(it[7].Value));
                        break;
                    case 8:
                        cplex.SetParam(Cplex.Param.ClockType, Convert.ToInt16(it[8].Value));
                        break;
                    case 9:
                        cplex.SetParam(Cplex.Param.CPUmask,it[9].Value); // String input
                        break;
                    case 10:
                        cplex.SetParam(Cplex.Param.DetTimeLimit, Convert.ToDouble(it[10].Value)); // Double input
                        break;
                    case 11:
                        cplex.SetParam(Cplex.Param.NodeAlgorithm, Convert.ToInt16(it[11].Value)); 
                        break;
                    case 12:
                        cplex.SetParam(Cplex.Param.OptimalityTarget, Convert.ToInt16(it[12].Value));
                        break;
                    case 13:
                        cplex.SetParam(Cplex.Param.Parallel, Convert.ToInt16(it[13].Value));
                        break;
                    case 14:
                        cplex.SetParam(Cplex.Param.RandomSeed, Convert.ToInt16(it[14].Value));
                        break;
                    case 15:
                        cplex.SetParam(Cplex.Param.RootAlgorithm, Convert.ToInt16(it[15].Value));
                        break;
                    case 16:
                        cplex.SetParam(Cplex.Param.SolutionType, Convert.ToInt16(it[16].Value));
                        break;
                }

            }
        }

        // Create an Instance of OptionsItem for every cplex param
        public void initializeOptions()
        {
            //it[0] = new OptionsItem();
            //it[0].Use = false;
            //it[0].Name = "MIP Startegy";
            //it[0].Value = "Traditional";
            //it[0].DataType = "Int";
            //it[0].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/AllMembers_T_ILOG_CPLEX_Cplex_Param_MIP_Strategy.htm";
            //flowLayoutPanel2.Controls.Add(it[0]);
            

            it[2] = new DevelopOptionsItem();
            it[2].Use = false;
            it[2].Name = "Time Limit";
            it[2].Value = "120";
            it[2].DataType = "Int";
            it[2].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/AllMembers_T_ILOG_CPLEX_Cplex_Param_MIP_Strategy.htm";
            flowLayoutPanel2.Controls.Add(it[2]);

            it[3] = new DevelopOptionsItem();
            it[3].Use = false;
            it[3].Name = "Threads";
            it[3].Value = "1";
            it[3].DataType = "Int";
            it[3].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_Threads.htm";
            flowLayoutPanel2.Controls.Add(it[3]);

            it[4] = new DevelopOptionsItem();
            it[4].Use = false;
            it[4].Name = "WorkMem";
            it[4].Value = "25000";
            it[4].DataType = "Int";
            it[4].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_WorkMem.htm";
            flowLayoutPanel2.Controls.Add(it[4]);

            it[5] = new DevelopOptionsItem();
            it[5].Use = false;
            it[5].Name = "MIP Limits TreeMemory";
            it[5].Value = "25000";
            it[5].DataType = "Int";
            it[5].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_MIP_Limits_TreeMemory.htm";
            flowLayoutPanel2.Controls.Add(it[5]);

            it[6] = new DevelopOptionsItem();
            it[6].Use = false;
            it[6].Name = "MIP Strategy File";
            it[6].Value = "0";
            it[6].DataType = "Int";
            it[6].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_MIP_Strategy_File.htm";
            flowLayoutPanel2.Controls.Add(it[6]);

            it[7] = new DevelopOptionsItem();
            it[7].Use = false;
            it[7].Name = "Advance";
            it[7].Value = "0";
            it[7].DataType = "Int";
            it[7].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_Advance.htm";
            flowLayoutPanel2.Controls.Add(it[7]);

            it[8] = new DevelopOptionsItem();
            it[8].Use = false;
            it[8].Name = "ClockType";
            it[8].Value = "0";
            it[8].DataType = "Int";
            it[8].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_ClockType.htm";
            flowLayoutPanel2.Controls.Add(it[8]);

            it[9] = new DevelopOptionsItem();
            it[9].Use = false;
            it[9].Name = "CPUmask";
            it[9].Value = "";
            it[9].DataType = "String";
            it[9].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_CPUmask.htm";
            flowLayoutPanel2.Controls.Add(it[9]);

            it[10] = new DevelopOptionsItem();
            it[10].Use = false;
            it[10].Name = "DetTimeLimit";
            it[10].Value = "0.0";
            it[10].DataType = "Double";
            it[10].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_DetTimeLimit.htm";
            flowLayoutPanel2.Controls.Add(it[10]);

            it[11] = new DevelopOptionsItem();
            it[11].Use = false;
            it[11].Name = "NodeAlgorithm";
            it[11].Value = "0";
            it[11].DataType = "Int";
            it[11].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_NodeAlgorithm.htm";
            flowLayoutPanel2.Controls.Add(it[11]);

            it[12] = new DevelopOptionsItem();
            it[12].Use = false;
            it[12].Name = "OptimalityTarget";
            it[12].Value = "0";
            it[12].DataType = "Int";
            it[12].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_OptimalityTarget.htm";
            flowLayoutPanel2.Controls.Add(it[12]);

            it[13] = new DevelopOptionsItem();
            it[13].Use = false;
            it[13].Name = "Parallel";
            it[13].Value = "0";
            it[13].DataType = "Int";
            it[13].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_Parallel.htm";
            flowLayoutPanel2.Controls.Add(it[13]);

            it[14] = new DevelopOptionsItem();
            it[14].Use = false;
            it[14].Name = "RandomSeed";
            it[14].Value = "0";
            it[14].DataType = "Int";
            it[14].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_RandomSeed.htm";
            flowLayoutPanel2.Controls.Add(it[14]);

            it[15] = new DevelopOptionsItem();
            it[15].Use = false;
            it[15].Name = "RootAlgorithm";
            it[15].Value = "0";
            it[15].DataType = "Int";
            it[15].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_RootAlgorithm.htm";
            flowLayoutPanel2.Controls.Add(it[15]);

            it[16] = new DevelopOptionsItem();
            it[16].Use = false;
            it[16].Name = "SolutionType";
            it[16].Value = "0";
            it[16].DataType = "Int";
            it[16].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_SolutionType.htm";
            flowLayoutPanel2.Controls.Add(it[16]);

            //for (int i = 0; i < it.Length; i++)
            //{
            //    if (it[i] != null)
            //    {
            //        if (it[i].Use == true)
            //        {
            //            Label l = new Label();
            //            l.Text = it[i].Name;
            //            flowLayoutPanel1.Controls.Add(l);
            //        }
            //    }
            //}
        }
        
        // Menu bar click
        private void cToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.BringToFront();
        }

        // Menu bar click
        private void cplexOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel5.BringToFront();

            // Get CPU usage value
            PerformanceCounter cpuCounter;
            PerformanceCounter ramCounter;

            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");

            label15.Text = ramCounter.NextValue().ToString() + " available MBytes";
        }

        // Starts the CPLEX optimization and writes output to solution file
        private void startCPLEX()
        {
            if (cplex.Solve())
            {
                System.Console.WriteLine("Solution status = " + cplex.GetStatus()); // log file output
                System.Console.WriteLine("Solution value  = " + cplex.ObjValue);
            }

            cplex.WriteSolution(directory + "\\" + projectName + "_" + datum + ".sol"); // sol file output


            cplex.End();

            label12.Text = "-";
        }


        // Initialize backgrounsworkers
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
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
                label12.Text = "Optimization finished!";
                button6.Enabled = true; 

                MessageBox.Show("Optimization successful!", "Info");
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
                button1.Enabled = false; 
                button2.Enabled = false; 
                button3.Enabled = false; 
                button4.Enabled = false; 

                if (cplex.Solve())
                {
                    System.Console.WriteLine("Solution status = " + cplex.GetStatus());
                    System.Console.WriteLine("Solution value  = " + cplex.ObjValue);
                }

                cplex.WriteSolution(directory + "\\" + projectName + "_" + datum + ".sol");

                cplex.End();
            }
            catch (System.Exception ex)
            {
            }

        }


        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {

            }
            else if (e.Error != null)
            {

            }
            else
            {
                progressBar1.Value = 100;
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            for (int i = 1; i <= tmlimit; i++)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    // Perform a time consuming operation and report progress.
                    System.Threading.Thread.Sleep(1000);
                    int pro = (int)((float)i / (float)tmlimit * 100);
                    worker.ReportProgress(pro);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Process.Start(logfile);
        }
    }
}
