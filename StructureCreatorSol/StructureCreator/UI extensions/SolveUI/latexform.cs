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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using StructureCreator.Properties;

namespace StructureCreator.UI_extensions.SolveUI
{
    /// <summary>
    /// Represents pdf creator form. User can create pdf of lattice, forces and assembly space
    /// </summary>
    public partial class latexform : Form
    {
        String stylePath = "";

        String texPath = "";
        String pdfFilePath = "";

        String csv1 = "";
        String csv2 = "";

        double X = 0;
        double Y = 0;
        double Z = 0;

        double distance = 0;

        List<double> forces = new List<double>(); // forces
        List<String> forcePositionsList = new List<String>(); // force positions

        List<String> supportPositionsList = new List<String>(); // support positions
        int supportCount = 0; 

        public latexform()
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-EN", false);

            InitializeComponent();

            try
            {
                Settings set = Settings.Default;
                label5.Text = set.ProjectName;
                label7.Text = set.StyPath;
                stylePath = set.StyPath;
                csv1 = set.csv1Path;
                csv2 = set.csv2Path;

                label9.Text = csv1;
                label10.Text = csv2;

                InitializeBackgroundWorker();

                // Get node number information 
                distance = set.distance;
                X = int.Parse("" + (set.xLength));
                X /= distance;
                X += 1;
                Y = int.Parse(("" + (set.yLength)));
                Y /= distance;
                Y += 1;
                Z = int.Parse(("" + (set.zLength)));
                Z /= distance;
                Z += 1;

                // write node numbers in form
                numericUpDown1.Value = Decimal.Parse("" + X);
                numericUpDown2.Value = Decimal.Parse("" + Y);
                numericUpDown3.Value = Decimal.Parse("" + Z);
                numericUpDown4.Value = Decimal.Parse("" + distance);


                if (set.Dimension == 0) // 3D
                {
                    // Print force values into textboxes

                    textBox8.Text = "" + set.forceCount;

                    String forceValues = "";
                    String forcePositions = "";

                    String temp = set.forces;

                    String[] values = temp.Split(';');

                    for (int i = 0; i < values.Count(); i++)
                    {
                        if (values[i] != "")
                        {
                            String[] force = values[i].Split(',');

                            if (force[0] != "")
                            {
                                String nodeName = force[0].Substring(1, force[0].Length - 1); // Node name
                                String x = force[1]; // x coordinate
                                String y = force[2]; // y coordinate
                                String z = force[3]; // z coordinate

                                String forceX = force[4]; // force in x direction
                                String forceY = force[5]; // force in y direction
                                String forceZ = force[6]; // force in z direction

                                forceValues += forceY + ",";
                                forcePositions += nodeName + ",";

                                forces.Add(Double.Parse(forceY, new CultureInfo("en-EN")));
                                forcePositionsList.Add(nodeName);
                            }

                        }

                    }

                    // Eliminate last comma
                    if (forceValues.Length != 0)
                    {
                        forceValues = forceValues.Remove(forceValues.Length - 1);
                        forcePositions = forcePositions.Remove(forcePositions.Length - 1);
                    }



                    textBox10.Text = forceValues;
                    textBox11.Text = forcePositions;

                }
                else if(set.Dimension == 1)
                {
                    // Print force values into textboxes

                    textBox8.Text = "" + set.forceCount;

                    String forceValues = "";
                    String forcePositions = "";

                    String temp = set.forces;

                    String[] values = temp.Split(';');

                    for (int i = 0; i < values.Count(); i++)
                    {
                        if (values[i] != "")
                        {
                            String[] force = values[i].Split(',');

                            if (force[0] != "")
                            {
                                String nodeName = force[0].Substring(1, force[0].Length - 1); // Node name
                                String x = force[1]; // x coordinate
                                String y = force[2]; // y coordinate
                                String z = force[3]; // z coordinate

                                String forceX = force[4]; // force in x direction
                                String forceY = force[5]; // force in y direction
                                String forceZ = force[6].Remove(force[6].Length - 1); // force in z direction

                                forceValues += forceZ + ",";
                                forcePositions += nodeName + ",";

                                forces.Add(Double.Parse(forceZ, new CultureInfo("en-EN")));
                                forcePositionsList.Add(nodeName);
                            }

                        }

                    }

                    // Eliminate last comma
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
                    // Print supports into textboxes

                    supportCount = set.bearingLoadCount;
                    textBox3.Text = "" + supportCount;

                    String supportPositions = "";

                    String temp2 = set.bearingLoads;

                    String[] supportvalues = temp2.Split(';');

                    for (int i = 0; i < supportvalues.Count(); i++)
                    {
                        if (supportvalues[i] != "")
                        {
                            String[] support = supportvalues[i].Split(',');

                            if (support[0] != "")
                            {
                                String nodeName = support[0].Substring(1, support[0].Length - 1); // Node name

                                supportPositions += nodeName + ",";

                                supportPositionsList.Add(nodeName);
                            }

                        }

                    }

                    if (supportPositions.Length != 0)
                    {
                        supportPositions = supportPositions.Remove(supportPositions.Length - 1);  // Eliminate last comma
                    }


                    textBox1.Text = supportPositions;

                }
                else if(set.Dimension == 1)
                {
                    // Print supports into textboxes

                    supportCount = set.bearingLoadCount;
                    textBox3.Text = "" + supportCount;

                    String supportPositions = "";

                    String temp2 = set.bearingLoads;

                    String[] supportvalues = temp2.Split(';');

                    for (int i = 0; i < supportvalues.Count(); i++)
                    {
                        if (supportvalues[i] != "")
                        {
                            String[] support = supportvalues[i].Split(',');

                            if (support[0] != "")
                            {
                                String nodeName = support[0].Substring(1, support[0].Length - 1); // Node name

                                supportPositions += nodeName + ",";

                                supportPositionsList.Add(nodeName);
                            }

                        }

                    }

                    if (supportPositions.Length != 0)
                    {
                        supportPositions = supportPositions.Remove(supportPositions.Length - 1);  // Eliminate last comma
                    }


                    textBox1.Text = supportPositions;
                }
                    

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.ToString(), "Info");
            }
            
        }

        private void InitializeBackgroundWorker()
        {
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
                // Note that due to a race condition in 
                // the DoWork event handler, the Cancelled
                // flag may not have been set, even though
                // CancelAsync was called.
                MessageBox.Show("Canceled!", "Info");
            }
            else
            {
                // Finally, handle the case where the operation 
                // succeeded.
                //label7.Visible = false;
                //label4.Visible = true;
                Thread.Sleep(3);
                button7.Enabled = true;
                label8.Text = "Operation completed.";
            }

        }

        private void backgroundWorker1_DoWork(object sender,
          DoWorkEventArgs e)
        {
            // Get the BackgroundWorker that raised this event.
            BackgroundWorker worker = sender as BackgroundWorker;

            try
            {
                //label7.Visible = true; 
                // Get Paths of Settings 
                Settings set = Settings.Default;


                // Copy .sty files in project folder 
                String filename = Path.GetFileName(stylePath);

                if (File.Exists(set.ProjectPath + "\\Latex\\" + filename))
                {

                }
                else
                {
                    File.SetAttributes(stylePath, FileAttributes.Normal);
                    File.Copy(stylePath, set.ProjectPath + "\\Latex\\" + filename);
                }

                //String filename2 = Path.GetFileName(stylePath2);
                //File.Copy(stylePath2, set.ProjectPath + "\\Latex\\" + filename2);



                // Copy .exe in project folder 
                String exe = Path.GetFileName(set.exePath);

                if (File.Exists(set.ProjectPath + "\\Latex\\" + exe))
                {

                }
                else
                {
                    File.SetAttributes(set.exePath, FileAttributes.Normal);
                    File.Copy(set.exePath, set.ProjectPath + "\\Latex\\" + exe);
                }


                // Get parameters out of interface
                //String parameters = "3 3 3 1 2 P3 P8 4 P1 P6 P7 P8 -4 5 6 -7 2 50 50 C:\\Users\\hendr\\Desktop\\NeuerTest\\Latex\\testttt.tex C:\\Users\\hendr\\Desktop\\NeuerTest\\Latex\\Tex\\Firstprojekt_csv_16122019.csv C:\\Users\\hendr\\Desktop\\NeuerTest\\Latex\\Tex\\Firstprojekt_csv2_16122019.csv C:\\Users\\hendr\\Desktop\\NeuerTest\\Latex\\";

                String parameters = "";

                try
                {
                    
                    parameters += numericUpDown1.Value + " " + numericUpDown2.Value + " " + numericUpDown3.Value + " " + numericUpDown4.Value + " "; // Node numbers and distance

                    // add all supports written in textbox
                    String[] temp = textBox1.Text.Split(',');
                    if(int.Parse(textBox3.Text) == 0 || textBox3.Text.Equals(""))
                    {
                        parameters += 0 + " ";
                    }
                    else
                    {
                        parameters += textBox3.Text + " ";
                        for (int i = 0; i < int.Parse(textBox3.Text); i++)
                        {
                            if (!temp[i].Equals(""))
                            {
                                parameters += temp[i] + " ";
                            }
                            else
                            {

                            }
                        }
                    }

                    // add all forces written in text box and their position
                    if(int.Parse(textBox8.Text) == 0 || textBox8.Text.Equals(""))
                    {
                        parameters += 0 + " ";
                    }
                    else
                    {
                        parameters += int.Parse(textBox8.Text) + " ";

                        String[] temp2 = textBox10.Text.Split(',');
                        
                        String[] temp3 = textBox11.Text.Split(',');

                        for (int i = 0; i < int.Parse(textBox8.Text); i++)
                        {
                            if (!temp3[i].Equals(""))
                            {
                                parameters += temp3[i] + " ";
                            }
                        }

                        for (int i = 0; i < int.Parse(textBox8.Text); i++)
                        {
                            if (!temp2[i].Equals(""))
                            {
                                parameters += temp2[i] + " ";
                            }
                        }
                    }
                    
                    parameters += numericUpDown7.Value + " "; // Scaling 1
                    parameters += numericUpDown8.Value + " "; // Scaling 2
                    parameters += numericUpDown6.Value + " "; // Scaling 3

                    String texPath = Settings.Default.ProjectPath + "\\Latex\\" + Settings.Default.ProjectName + ".tex"; // set tex path
                    parameters += texPath + " ";

                    parameters += csv1 + " ";
                    parameters += csv2 + " ";

                    String folder = Settings.Default.ProjectPath + "\\Latex\\";
                    parameters += folder;
                    
                    
                }catch(Exception ex)
                {
                    MessageBox.Show("Add CreateTex.exe path in options!", "Info");
                }

                
                // Start CreateTex.exe
                if (File.Exists(set.ProjectPath + "\\Latex\\" + exe))
                {
                    // Create lp file
                    ProcessStartInfo pro = new ProcessStartInfo();
                    pro.WorkingDirectory = Settings.Default.ProjectPath + "\\Latex\\";
                    //Setting the FileName to be Started like in our
                    //Project we are just going to start a CMD Window.
                    pro.FileName = "cmd.exe";
                    pro.Arguments = "/C " + "CreateTex.exe " + parameters; // Call exe with parameters


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
                    MessageBox.Show("Add CreateTex.exe path in options!", "Info");
                }


                // Create PDF file out of tex-file created above

                //try
                //{
                //    String tex = Path.GetFileName(texPath);
                //    //Setting an instance of ProcessStartInfo class
                //    // under System.Diagnostic Assembly Reference
                //    ProcessStartInfo pro2 = new ProcessStartInfo();
                //    pro2.WorkingDirectory = Settings.Default.ProjectPath + "\\Latex\\";
                //    //Setting the FileName to be Started like in our
                //    //Project we are just going to start a CMD Window.
                //    pro2.FileName = "cmd.exe";
                //    pro2.Arguments = "/C " + "pdflatex " + tex;
                //    // pro.Arguments = "/C " + "pdflatex " + set.ProjectName + "_tikzlattice.tex";

                //    pro2.RedirectStandardOutput = true;
                //    pro2.UseShellExecute = false;

                //    Process proStart2 = new Process();
                //    //Setting up the Process Name here which we are 
                //    // going to start from ProcessStartInfo
                //    proStart2.StartInfo = pro2;
                //    proStart2.StartInfo.CreateNoWindow = true;
                //    proStart2.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                //    //Calling the Start Method of Process class to
                //    // Invoke our Process viz 'cmd.exe'

                //    proStart2.Start();

                //    set.pdfPath = Settings.Default.ProjectPath + "\\Latex\\" + Settings.Default.ProjectName + ".pdf";
                //    pdfFilePath = Settings.Default.ProjectPath + "\\Latex\\" + Settings.Default.ProjectName + ".pdf";
                //    set.Save();

                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show("Error: " + ex.ToString(), "Info");
                //}

            }
            catch (Exception exp)
            {
                MessageBox.Show("Add CreateTex.exe path in options!", "Info");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Close Button
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Finish Button
            Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Change style file
            string path = "";

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Choose sty file";
            ofd.Filter = "Style File|*.sty";

            if (ofd.ShowDialog() == DialogResult.OK) // if user didn't cancel
            {
                path = ofd.FileName; // full File Path
                label7.Text = path;
                stylePath = path;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Change csv file
            string path = "";

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Choose csv1 file";
            ofd.Filter = "CSV |*.csv";
            ofd.InitialDirectory = Settings.Default.ProjectPath + "\\CSV";


            if (ofd.ShowDialog() == DialogResult.OK) // if user didn't cancel
            {
                path = ofd.FileName; // full File Path
                label9.Text = path;
                csv1 = path;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Change csv file
            string path = "";

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Choose csv2 file";
            ofd.Filter = "CSV |*.csv";
            ofd.InitialDirectory = Settings.Default.ProjectPath + "\\CSV";


            if (ofd.ShowDialog() == DialogResult.OK) // if user didn't cancel
            {
                path = ofd.FileName; // full File Path
                label10.Text = path;
                csv2 = path;
            }
        }


        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (Settings.Default.ProjectPath.Equals(""))
                {
                    MessageBox.Show("Create new project first!", "Info");
                }
                else
                {
                    label8.Text = "PDFLatex is working...";
                    backgroundWorker1.RunWorkerAsync(); // start worker
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured : "+ ex.ToString(), "Info");
            }
        }

        // Open pdf
        private void button7_Click(object sender, EventArgs e)
        {
            Settings set = Settings.Default;

            if (File.Exists(set.ProjectPath + "\\Latex\\" + set.ProjectName + ".pdf"))
            {
                // Open PDF file
                ProcessStartInfo pro2 = new ProcessStartInfo();
                pro2.WorkingDirectory = set.ProjectPath + "\\Latex\\";
                //Setting the FileName to be Started like in our
                //Project we are just going to start a CMD Window.
                pro2.FileName = "cmd.exe";
                pro2.Arguments = "/C " + "start /max " + set.ProjectName + ".pdf"; // Open pdf

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
                MessageBox.Show("No pdf file found. Please install pdflatex!", "Info");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Set the number of nodes in positive direction of the x-axis of the global coordinate system.\r\rConsider the node distance!", "Info");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Set the number of nodes in positive direction of the y-axis of the global coordinate system.\r\rConsider the node distance!", "Info");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Set the number of nodes in positive direction of the z-axis of the global coordinate system.\r\rConsider the node distance!", "Info");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Set the distance between two nodes in the assembly space that are positioned purely horizontally or verticall to each other.", "Info");
        }

        private void button14_Click(object sender, EventArgs e)
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

        private void button17_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Set the number of supports", "Info");
        }

        private void button15_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Set the support points as comma-separated list.\r\rNote the indexing of the working points.", "Info");
        }

        private void button20_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Set a scaling factor for the force height");
        }

        private void button19_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Set a scaling factor for the force width", "Info");
        }

        private void button18_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Set a scaling factor for beams", "Info");
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Settings.Default.CreatePDFFormOpened = false;
            Settings.Default.Save();
        }
    }
}


class PointClass
{
    public string point { get; set; }
    public string x { get; set; }
    public string y { get; set; }
    public string z { get; set; }
}

class BeamClass
{
    public string p1 { get; set; }
    public string p2 { get; set; }
    public string d { get; set; }
    public string f { get; set; }
}