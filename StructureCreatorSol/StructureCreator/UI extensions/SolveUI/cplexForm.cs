using ILOG.CPLEX;
using ILOG.Concert;
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
using StructureCreator.Properties;
using System.Diagnostics;

namespace StructureCreator.UI_extensions.SolveUI
{
    /// <summary>
    /// Represents Cplex form. User can execute an optimization
    /// </summary>
    public partial class cplexOptForm : Form
    {
        CplexOption1[] it = new CplexOption1[30]; //stores Options

        private String lpFilePath = "";
        private String directory = "";
        private String projectName = "";

        string datumt = DateTime.Now.ToString("dd-MM-yyy");
        string datum; 

        Cplex cplex = null;
        string log = "Log started (V";
        string projekt = "";

        StreamWriter w = null; // Stream writer for log file

        Int64 tmlimit = 0; // time limit for optimization
        String end = "";

        string logfile = ""; // log file path

        int currentPage = 0;


        public cplexOptForm()
        {
            try
            {
                InitializeComponent();

                initializeOptions(); // Initialize useable options

                InitializeBackgroundWorker();

                datumt = datumt.Replace("-", "");
                datum = datumt;

                cplex = new Cplex(); // Instance of CPLEX

                log += cplex.Version;
                log += ") " + " " + DateTime.Now.DayOfWeek.ToString() + " " + DateTime.Now.ToString();

                Settings set = Settings.Default;
                lpFilePath = set.lpPath;
                directory = set.ProjectPath;
                projectName = set.ProjectName;

                label10.Text = set.lpPath;
                label3.Text = projectName;
                label4.Text = projectName;

                panel1.BringToFront();


                // Get CPU usage value
                PerformanceCounter cpuCounter;
                PerformanceCounter ramCounter;

                cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                ramCounter = new PerformanceCounter("Memory", "Available MBytes");

                label7.Text = ramCounter.NextValue().ToString() + " available MBytes";

                ToolTip tip = new ToolTip();
                tip.Show("You can only add the options once.", button5);
            }catch(System.Exception ex)
            {
                MessageBox.Show(this, "CPLEX Optimization studio needs to be installed on the machine first. Also the path variable needs to be set.");
            }
            
        }

        private void InitializeBackgroundWorker()
        {
            backgroundWorker1.DoWork +=
                new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(
            backgroundWorker1_RunWorkerCompleted);

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

        // Back button
        private void button2_Click(object sender, EventArgs e)
        {
            if (currentPage == 0)
            {

            }
            else if (currentPage == 1)
            {
                panel1.BringToFront();
                currentPage = 0;
                button2.Enabled = false;
                button3.Enabled = true;
            }
        }

        // Next button
        private void button3_Click(object sender, EventArgs e)
        {
            if (currentPage == 0)
            {
                panel2.BringToFront();
                currentPage = 1;
                button3.Enabled = false;
                button2.Enabled = true;

                // Show the chosen options in a window

                flowLayoutPanel2.Controls.Clear();

                for (int i = 0; i < it.Length; i++)
                {
                    if (it[i] != null)
                    {
                        if (it[i].Use == true)
                        {
                            CplexOption2 option = new CplexOption2();
                            option.Name = it[i].Name;
                            option.Value = it[i].Value;

                            flowLayoutPanel2.Controls.Add(option);
                        }
                    }
                }
            }
            else if (currentPage == 1)
            {

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Change lp file 
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Choose lp file";
            ofd.Filter = "Problem File|*.lp";
            ofd.InitialDirectory = Settings.Default.ProjectPath + "\\Optimization"; 

            if (ofd.ShowDialog() == DialogResult.OK) // if user didn't cancel
            {
                lpFilePath = ofd.FileName; // full File Path
                //file = Path.GetFileName(path);
                label10.Text = lpFilePath;
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


            it[2] = new CplexOption1();
            it[2].Use = false;
            it[2].Name = "Time Limit";
            it[2].Value = "120";
            it[2].DataType = "Int";
            it[2].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_TimeLimit.htm";
            it[2].Anchor = AnchorStyles.Left;
            it[2].Anchor = AnchorStyles.Right;
            flowLayoutPanel1.Controls.Add(it[2]);

            it[3] = new CplexOption1();
            it[3].Use = false;
            it[3].Name = "Threads";
            it[3].Value = "1";
            it[3].DataType = "Int";
            it[3].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_Threads.htm";
            flowLayoutPanel1.Controls.Add(it[3]);

            it[4] = new CplexOption1();
            it[4].Use = false;
            it[4].Name = "WorkMem";
            it[4].Value = "25000";
            it[4].DataType = "Int";
            it[4].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_WorkMem.htm";
            flowLayoutPanel1.Controls.Add(it[4]);

            it[5] = new CplexOption1();
            it[5].Use = false;
            it[5].Name = "MIP Limits TreeMemory";
            it[5].Value = "25000";
            it[5].DataType = "Int";
            it[5].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_MIP_Limits_TreeMemory.htm";
            flowLayoutPanel1.Controls.Add(it[5]);

            it[6] = new CplexOption1();
            it[6].Use = false;
            it[6].Name = "MIP Strategy File";
            it[6].Value = "0";
            it[6].DataType = "Int";
            it[6].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_MIP_Strategy_File.htm";
            flowLayoutPanel1.Controls.Add(it[6]);

            it[7] = new CplexOption1();
            it[7].Use = false;
            it[7].Name = "Advance";
            it[7].Value = "0";
            it[7].DataType = "Int";
            it[7].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_Advance.htm";
            flowLayoutPanel1.Controls.Add(it[7]);

            it[8] = new CplexOption1();
            it[8].Use = false;
            it[8].Name = "ClockType";
            it[8].Value = "0";
            it[8].DataType = "Int";
            it[8].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_ClockType.htm";
            flowLayoutPanel1.Controls.Add(it[8]);

            //it[9] = new CplexOption1();
            //it[9].Use = false;
            //it[9].Name = "CPUmask";
            //it[9].Value = "";
            //it[9].DataType = "String";
            //it[9].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_CPUmask.htm";
            //flowLayoutPanel1.Controls.Add(it[9]);

            it[10] = new CplexOption1();
            it[10].Use = false;
            it[10].Name = "DetTimeLimit";
            it[10].Value = "0";
            it[10].DataType = "Double";
            it[10].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_DetTimeLimit.htm";
            flowLayoutPanel1.Controls.Add(it[10]);

            it[11] = new CplexOption1();
            it[11].Use = false;
            it[11].Name = "NodeAlgorithm";
            it[11].Value = "0";
            it[11].DataType = "Int";
            it[11].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_NodeAlgorithm.htm";
            flowLayoutPanel1.Controls.Add(it[11]);

            it[12] = new CplexOption1();
            it[12].Use = false;
            it[12].Name = "OptimalityTarget";
            it[12].Value = "0";
            it[12].DataType = "Int";
            it[12].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_OptimalityTarget.htm";
            flowLayoutPanel1.Controls.Add(it[12]);

            it[13] = new CplexOption1();
            it[13].Use = false;
            it[13].Name = "Parallel";
            it[13].Value = "0";
            it[13].DataType = "Int";
            it[13].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_Parallel.htm";
            flowLayoutPanel1.Controls.Add(it[13]);

            it[14] = new CplexOption1();
            it[14].Use = false;
            it[14].Name = "RandomSeed";
            it[14].Value = "0";
            it[14].DataType = "Int";
            it[14].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_RandomSeed.htm";
            flowLayoutPanel1.Controls.Add(it[14]);

            it[15] = new CplexOption1();
            it[15].Use = false;
            it[15].Name = "RootAlgorithm";
            it[15].Value = "0";
            it[15].DataType = "Int";
            it[15].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_RootAlgorithm.htm";
            flowLayoutPanel1.Controls.Add(it[15]);

            it[16] = new CplexOption1();
            it[16].Use = false;
            it[16].Name = "SolutionType";
            it[16].Value = "0";
            it[16].DataType = "Int";
            it[16].Link = "https://www.ibm.com/support/knowledgecenter/SSSA5P_12.7.1/ilog.odms.cplex.help/refdotnetcplex/html/F_ILOG_CPLEX_Cplex_Param_SolutionType.htm";
            flowLayoutPanel1.Controls.Add(it[16]);

        }

        // Add selected options to the CPLEX instance
        private void button5_Click(object sender, EventArgs e)
        {
            cplex.ImportModel(lpFilePath); // Import lp file into instance


            // Write output to log file
            StreamWriter t = File.CreateText(directory + "\\Optimization\\" + "cplexLog_" + projectName + "_" + datum + ".log");
            w = t;
            logfile = directory + "\\Optimization\\" + "cplexLog_" + projectName + "_" + datum + ".log";

            Settings set = Settings.Default;
            set.cplexlogPath = logfile;
            set.Save();

            String file = Path.GetFileName(lpFilePath);

            t.WriteLine(log);

            cplex.SetOut(t);

            w.WriteLine();
            w.WriteLine("Problem '" + file + "' read");
            w.WriteLine();

            // Add options to model 
            for (int i = 0; i < it.Length; i++)
            {
                if(it[i] != null)
                {
                    if (it[i].Use == true)
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
                            //case 9:
                            //    cplex.SetParam(Cplex.Param.CPUmask, it[9].Value); // String input
                            //    break;
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
                
            }

            button5.Enabled = false; 
            button6.Enabled = true; 
        }

        // Start CPLEX optimization
        private void button6_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            now = now.AddSeconds((double)tmlimit);
            end = now.ToString();

            try
            {
                label11.Text = end;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;

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




        // Initialize backgrounsworkers

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
                label11.Text = "Optimization finished!";
                button4.Enabled = true;

                Settings set = Settings.Default;
                set.solImportCommand = true;
                set.solFilePath = directory + "\\Optimization\\" + projectName + "_" + datum + ".sol";
                set.Save();

                SpaceClaim.Api.V19.Window.ActiveWindow.ZoomExtents(); // used to Force a window update, so the Ribbon Button is enabled/disabled

                w.Close(); // Close Stream writer

                //MessageBox.Show("Optimization successful!");

                if (File.Exists(directory + "\\Optimization\\" + projectName + "_" + datum + ".sol"))
                {
                    MessageBox.Show("The optimization is finished and the window can be closed now", "Info");
                }
                else
                {
                    MessageBox.Show("There is no solution and the window can be closed now", "Info");
                }
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

                if (cplex.Solve())
                {
                    System.Console.WriteLine("Solution status = " + cplex.GetStatus());
                    System.Console.WriteLine("Solution value  = " + cplex.ObjValue);
                }

                cplex.WriteSolution(directory + "\\Optimization\\" + projectName + "_" + datum + ".sol");
                
                cplex.End();

                

                
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Info");
            }

        }

        // Progress bar
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

            // Is used to update progress bar based on time limit of optimization
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

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            w.Close(); // Close Stream writer
            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Settings.Default.CPLEXFormOpened = false;
            Settings.Default.Save();
        }

    }
}
