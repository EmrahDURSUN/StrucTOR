using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StructureCreator.Properties;
using System;
using System.Threading;

namespace StructureCreator.UI_extensions.SolveUI
{
    /// <summary>
    /// Represents lp creator form. User can create a lp Problem with chosen parameters
    /// </summary>
    public partial class CreateLPForm : Form
    {
        string datumt = DateTime.Now.ToString("dd-MM-yyy");
        string datum;

        // Save stab information
        List<double> stabCapacity = new List<double>();
        List<double> stabCost = new List<double>();

        // Save forces
        List<double> forcesX = new List<double>();
        List<double> forcesY = new List<double>();
        List<double> forcesZ = new List<double>();
        List<double> forcePositionsList = new List<double>();

        // Save supports
        int supportCount = 0; 
        List<double> supportsX = new List<double>();
        List<double> supportsY = new List<double>();
        List<double> supportsZ = new List<double>();
        List<double> supportsPositionsList = new List<double>();

        public CreateLPForm()
        {
            try
            {
                InitializeComponent();

                datumt = datumt.Replace("-", "");
                datum = datumt;

                Settings set = Settings.Default;

                // Node number
                textBox1.Text = ((set.xLength / set.distance) + 1).ToString();
                textBox2.Text = ((set.yLength / set.distance) + 1).ToString();
                textBox3.Text = ((set.zLength / set.distance) + 1).ToString();

                // Stab information
                textBox4.Text = "" + 1;

                // Force information
                textBox8.Text = set.forceCount.ToString();

                backgroundWorker1.DoWork +=
                   new DoWorkEventHandler(backgroundWorker1_DoWork);
                backgroundWorker1.RunWorkerCompleted +=
                    new RunWorkerCompletedEventHandler(
                backgroundWorker1_RunWorkerCompleted);

                backgroundWorker1.WorkerSupportsCancellation = true;

                label13.Text = "Start process";


                if(set.Dimension == 0) // 3D
                {
                    // Print force values into textboxes

                    String forceValues = "";
                    String forcePositions = "";

                    //MessageBox.Show("Forces: " + set.forces);

                    String temp = set.forces;

                    String[] values = temp.Split(';');

                    // get all forces
                    for (int i = 0; i < values.Count(); i++)
                    {
                        if (values[i] != "")
                        {
                            String[] force = values[i].Split(',');

                            if (force[0] != "")
                            {
                                String nodeName = force[0].Substring(2, force[0].Length - 2); // Node name
                                String x = force[1]; // x coordinate
                                String y = force[2]; // y coordinate
                                String z = force[3]; // z coordinate

                                String forceX = force[4]; // force in x direction
                                String forceY = force[5]; // force in y direction
                                String forceZ = force[6].Remove(force[6].Length - 1); // force in z direction

                                forceValues += "(" + forceX + "," + forceY + "," + forceZ + "),";
                                forcePositions += nodeName + ",";

                                forcesX.Add(Double.Parse(forceX, new CultureInfo("de-DE")));
                                forcesY.Add(Double.Parse(forceY, new CultureInfo("de-DE")));
                                forcesZ.Add(Double.Parse(forceZ, new CultureInfo("de-DE")));
                                forcePositionsList.Add(Double.Parse(nodeName, new CultureInfo("de-DE")));
                            }

                        }

                    }



                    // Remove last comma

                    if (forceValues.Length != 0)
                    {
                        forceValues = forceValues.Remove(forceValues.Length - 1);
                        forcePositions = forcePositions.Remove(forcePositions.Length - 1);
                    }


                    textBox10.Text = forceValues;
                    textBox11.Text = forcePositions;

                }else if(set.Dimension == 1) // 2D
                {
                    // Print force values into textboxes

                    String forceValues = "";
                    String forcePositions = "";

                    //MessageBox.Show("Forces: " + set.forces);

                    String temp = set.forces;

                    String[] values = temp.Split(';');

                    // get all forces
                    for (int i = 0; i < values.Count(); i++)
                    {
                        if (values[i] != "")
                        {
                            String[] force = values[i].Split(',');

                            if (force[0] != "")
                            {
                                String nodeName = force[0].Substring(2, force[0].Length - 2); // Node name
                                String x = force[1]; // x coordinate
                                String y = force[2]; // y coordinate
                                String z = force[3]; // z coordinate

                                String forceX = force[4]; // force in x direction
                                String forceY = force[5]; // force in y direction
                                String forceZ = force[6].Remove(force[6].Length - 1); // force in z direction

                                forceValues += "(" + forceX + "," + forceY + "," + forceZ + "),";
                                forcePositions += nodeName + ",";

                                forcesX.Add(Double.Parse(forceX, new CultureInfo("de-DE")));
                                forcesY.Add(Double.Parse(forceY, new CultureInfo("de-DE")));
                                forcesZ.Add(Double.Parse(forceZ, new CultureInfo("de-DE")));
                                forcePositionsList.Add(Double.Parse(nodeName, new CultureInfo("de-DE")));
                            }

                        }

                    }



                    // Remove last comma

                    if (forceValues.Length != 0)
                    {
                        forceValues = forceValues.Remove(forceValues.Length - 1);
                        forcePositions = forcePositions.Remove(forcePositions.Length - 1);
                    }


                    textBox10.Text = forceValues;
                    textBox11.Text = forcePositions;
                }



                if (set.Dimension == 0) // 3D
                {
                    // Print support values into textboxes

                    String supportValues = "";
                    String supportPositions = "";

                    //MessageBox.Show("Forces: " + set.forces);

                    String temp2 = set.bearingLoads;

                    String[] values2 = temp2.Split(';');

                    // get all supports
                    for (int i = 0; i < values2.Count(); i++)
                    {
                        if (values2[i] != "")
                        {
                            String[] support = values2[i].Split(',');

                            if (support[0] != "")
                            {
                                String nodeName = support[0].Substring(2, support[0].Length - 2); // Node name
                                String x = support[1]; // x coordinate
                                String y = support[2]; // y coordinate
                                String z = support[3]; // z coordinate

                                String supX = support[4]; // force in x direction
                                String supY = support[5]; // force in y direction
                                String supZ = support[6].Remove(support[6].Length - 1); // force in z direction

                                supportValues += "(" + supX + "," + supY + "," + supZ + "),";
                                supportPositions += nodeName + ",";

                                supportsX.Add(Double.Parse(supX, new CultureInfo("de-DE")));
                                supportsY.Add(Double.Parse(supY, new CultureInfo("de-DE")));
                                supportsZ.Add(Double.Parse(supZ, new CultureInfo("de-DE")));
                                supportsPositionsList.Add(Double.Parse(nodeName, new CultureInfo("de-DE")));
                            }

                        }

                    }

                    // Remove last comma

                    if (supportValues.Length != 0)
                    {
                        supportValues = supportValues.Remove(supportValues.Length - 1);
                        supportPositions = supportPositions.Remove(supportPositions.Length - 1);
                    }

                    textBox13.Text = supportValues;
                    textBox12.Text = supportPositions;
                    textBox14.Text = "" + Settings.Default.bearingLoadCount;
                    supportCount =  Settings.Default.bearingLoadCount;

                }else if(set.Dimension == 1) // 2D
                {
                    // Print support values into textboxes

                    String supportValues = "";
                    String supportPositions = "";

                    //MessageBox.Show("Forces: " + set.forces);

                    String temp2 = set.bearingLoads;

                    String[] values2 = temp2.Split(';');

                    // get all supports
                    for (int i = 0; i < values2.Count(); i++)
                    {
                        if (values2[i] != "")
                        {
                            String[] support = values2[i].Split(',');

                            if (support[0] != "")
                            {
                                String nodeName = support[0].Substring(2, support[0].Length - 2); // Node name
                                String x = support[1]; // x coordinate
                                String y = support[2]; // y coordinate
                                String z = support[3]; // z coordinate

                                String supX = support[4]; // force in x direction
                                String supY = support[5]; // force in y direction
                                String supZ = support[6].Remove(support[6].Length - 1); // force in z direction

                                supportValues += "(" + supX + "," + supY + "," + supZ + "),";
                                supportPositions += nodeName + ",";

                                supportsX.Add(Double.Parse(supX, new CultureInfo("de-DE")));
                                supportsY.Add(Double.Parse(supY, new CultureInfo("de-DE")));
                                supportsZ.Add(Double.Parse(supZ, new CultureInfo("de-DE")));
                                supportsPositionsList.Add(Double.Parse(nodeName, new CultureInfo("de-DE")));
                            }

                        }

                    }

                    // Remove last comma

                    if (supportValues.Length != 0)
                    {
                        supportValues = supportValues.Remove(supportValues.Length - 1);
                        supportPositions = supportPositions.Remove(supportPositions.Length - 1);
                    }

                    textBox13.Text = supportValues;
                    textBox12.Text = supportPositions;
                    textBox14.Text = "" + Settings.Default.bearingLoadCount;
                    supportCount = Settings.Default.bearingLoadCount;
                }

                    
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Info");
            }
            
            
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
                label13.Text = "Start process";
                button4.Enabled = true;
                button6.Enabled = true;
                button1.Enabled = true;

                MessageBox.Show(this, "Optimization canceled!", "Info");
            }
            else
            {
                // Finally, handle the case where the operation 
                // succeeded.
                //label5.Visible = true;
                
                    label13.Text = "Lp file created.";
                    button4.Enabled = true;
                
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

            bool readyToStart = true;

            // Check if values are correct
            try
            {
                for (int i = 0; i < 9; i++)
                {
                    switch (i)
                    {
                        case 0:
                            if (int.Parse(textBox1.Text) > 0)
                            {
                                textBox1.BackColor = System.Drawing.Color.White;
                            }
                            else
                            {
                                textBox1.BackColor = System.Drawing.Color.Red;
                                readyToStart = false;
                                backgroundWorker1.CancelAsync();
                                e.Cancel = true;
                            }
                            break;

                        case 1:
                            if (int.Parse(textBox2.Text) > 0)
                            {
                                textBox2.BackColor = System.Drawing.Color.White;
                            }
                            else
                            {
                                textBox2.BackColor = System.Drawing.Color.Red;
                                readyToStart = false;
                                backgroundWorker1.CancelAsync();
                                e.Cancel = true;
                            }
                            break;

                        case 2:
                            if (int.Parse(textBox3.Text) > 0)
                            {
                                textBox3.BackColor = System.Drawing.Color.White;
                            }
                            else
                            {
                                textBox3.BackColor = System.Drawing.Color.Red;
                                readyToStart = false;
                                backgroundWorker1.CancelAsync();
                                e.Cancel = true;
                            }
                            break;

                        case 3:
                            if (int.Parse(textBox4.Text) > 0)
                            {
                                textBox4.BackColor = System.Drawing.Color.White;
                            }
                            else
                            {
                                textBox4.BackColor = System.Drawing.Color.Red;
                                readyToStart = false;
                                backgroundWorker1.CancelAsync();
                                e.Cancel = true;
                            }
                            break;

                        case 4:
                            break;

                        case 5:
                            
                            break;

                        case 6:
                            
                            break;

                        case 7:
                            if (int.Parse(textBox8.Text) > 0)
                            {
                                textBox8.BackColor = System.Drawing.Color.White;
                            }
                            else
                            {
                                textBox8.BackColor = System.Drawing.Color.Red;
                                readyToStart = false;
                                backgroundWorker1.CancelAsync();
                                e.Cancel = true;
                            }
                            break;

                        case 8:
                            if (double.Parse(textBox9.Text) > 0)
                            {
                                textBox9.BackColor = System.Drawing.Color.White;
                            }
                            else
                            {
                                textBox9.BackColor = System.Drawing.Color.Red;
                                readyToStart = false;
                                backgroundWorker1.CancelAsync();
                                e.Cancel = true;
                            }
                            break;
                    }
                }

                try
                {

                    // Copy .exe in project folder 
                    Settings set = Settings.Default;

                    String filename = Path.GetFileName(set.stabwerkerzeugerexePath);

                    if (File.Exists(set.ProjectPath + "\\Optimization\\" + filename))
                    {

                    }
                    else
                    {
                        File.SetAttributes(set.stabwerkerzeugerexePath, FileAttributes.Normal);
                        File.Copy(set.stabwerkerzeugerexePath, set.ProjectPath + "\\Optimization\\" + filename);
                    }


                    // Get parameters for exe off textboxes
                    String xAxis = textBox1.Text;
                    String yAxis = textBox2.Text;
                    String zAxis = textBox3.Text;

                    String stabLength = textBox4.Text;
                    String stabCount = textBox5.Text;

                    String forceCount = textBox8.Text; 
                    String bigM = textBox9.Text;

                    // Get stab capacities
                    try
                    {
                        String[] Cap = textBox6.Text.Split(',');
                        for(int i=0; i<Cap.Length; i++)
                        {
                            stabCapacity.Add(Double.Parse(Cap[i]));
                        }

                    }catch(Exception ex)
                    {
                        MessageBox.Show(this, "Not allowed value at stab capacity!",  "Info");
                    }

                    // Get stab costs
                    try
                    {
                        String[] Costs = textBox7.Text.Split(',');
                        for (int i = 0; i < Costs.Length; i++)
                        {
                            stabCost.Add(Double.Parse(Costs[i]));
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "Not allowed value at stab costs!" , "Info");
                    }

                    if(stabCost.Count != stabCost.Count)
                    {
                        MessageBox.Show(this, "Not the same number of values in costs and capacity!", "Info");
                    }

                    if(stabCost.Count != int.Parse(stabCount) || stabCost.Count != int.Parse(stabCount))
                    {
                        MessageBox.Show(this, "Not the same number of values in costs or capacity and the number of stabs!", "Info");
                    }

                    // Create Parameter info for exe
                    String parameters = "";
                    parameters += xAxis + " " + yAxis + " " + zAxis + " " + stabLength + " " + stabCount + " ";

                    for(int i=0; i<stabCapacity.Count; i++) // Stab cpacities
                    {
                        parameters += stabCapacity[i] + " ";
                    }

                    for(int i=0; i<stabCost.Count; i++) // stab costs
                    {
                        parameters += stabCost[i] + " ";
                    }

                    // Add forces 

                    parameters += forceCount + " ";

                    for (int i = 0; i < forcesX.Count; i++) // forces in x direction
                    {
                        parameters += forcesX[i] + " ";
                    }

                    for (int i = 0; i < forcesY.Count; i++) // forces in y direction
                    {
                        parameters += forcesY[i] + " ";
                    }

                    for (int i = 0; i < forcesZ.Count; i++) // forces in z direction
                    {
                        parameters += forcesZ[i] + " ";
                    }

                    for (int i = 0; i < forcePositionsList.Count; i++) // force positions
                    {
                        parameters += forcePositionsList[i] + " ";
                    }

                    // Add supports

                    parameters += supportCount + " ";

                    for (int i = 0; i < supportsX.Count; i++) // forces in x direction
                    {
                        parameters += supportsX[i] + " ";
                    }

                    for (int i = 0; i < supportsY.Count; i++) // forces in y direction
                    {
                        parameters += supportsY[i] + " ";
                    }

                    for (int i = 0; i < supportsZ.Count; i++) // forces in z direction
                    {
                        parameters += supportsZ[i] + " ";
                    }

                    for (int i = 0; i < supportsPositionsList.Count; i++) // support positions
                    {
                        parameters += supportsPositionsList[i] + " ";
                    }

                    parameters += bigM; // big M

                    //MessageBox.Show(parameters);
                    
                    
                    
                    if (File.Exists(set.ProjectPath + "\\Optimization\\" + filename) && readyToStart)
                    {
                        // Create lp file
                        ProcessStartInfo pro = new ProcessStartInfo();
                        pro.WorkingDirectory = set.ProjectPath + "\\Optimization\\";
                        //Setting the FileName to be Started like in our
                        //Project we are just going to start a CMD Window.
                        pro.FileName = "cmd.exe";
                        pro.Arguments = "/C " + "Stabwerkerzeuger3.exe " + parameters; // call exe 


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
                        
                    }
                    else
                    {
                        MessageBox.Show(this, "Add Stabwerkerzeuger.exe path in options!" , "Info");
                        button6.Enabled = true; 
                    }

                    Settings.Default.lpPath = set.ProjectPath + "\\Optimization\\" + "stabwerk.lp";
                    Settings.Default.Save();

    
                }
                catch (Exception exep)
                {
                    MessageBox.Show(this, "Add Stabwerkerzeuger.exe path in options!", "Info");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Unallowed value!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Close button
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            label13.Text = "Process running...";
            button1.Enabled = false;
            button4.Enabled = false;
            button6.Enabled = false;

            backgroundWorker1.RunWorkerAsync();
        }



        // Information boxes
        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Set the number of nodes in positive direction of the x-axis of the global coordinate system.\r\rConsider the node distance!", "Info");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Set the number of nodes in positive direction of the y-axis of the global coordinate system.\r\rConsider the node distance!", "Info");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Set the number of nodes in positive direction of the z-axis of the global coordinate system.\r\rConsider the node distance!", "Info");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Set the length of a beam element between two nodes in the assembly space that are positioned purely horizontally or verticall to each other.", "Info");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Set the number of miscellaneous beam types", "Info");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Set the beam load capacity as a comma-separated list.", "Info");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Set the cost of a beam type as comma-seperated list", "Info");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Set the number of external point loads.", "Info");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Set the signed forces as comma-separated list.\r\rConsider that a positive coordinate direction with respect to the global coordinate system corresponds to a positive sign. A non-existent sign is interpreted as a positive sign.", "Info");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Set the force application points as comma-separated list.\r\rNote the indexing of the working points.", "Info");
        }

        private void button14_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Set Big-M", "Info");
        }

        private void CreateLPForm_Load(object sender, EventArgs e)
        {

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Settings.Default.CreateLPFormOpened = false;
            Settings.Default.Save();
        }
    }
}
