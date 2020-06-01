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
using System.Xml.Serialization;
using StructureCreator.Properties; 

namespace StructureCreator.UI_extensions.SolveUI
{
    public partial class solImportForm : Form
    {
        int diameterNumber = 0; 
        int currentPage = 0;

        bool diameterValuesInserted = false;

        private String solPath = "";
        private String directory = "";
        private String projectName = "";

        private String csv1Path = "";
        private String csv2Path = "";
        private String csv3Path = "";

        public solImportForm()
        {
            InitializeComponent();
            InitializeBackgroundWorker();

            panel1.BringToFront();

            Settings set = Settings.Default;

            solPath = set.solFilePath; 
            directory = set.ProjectPath; 
            projectName = set.ProjectName;

            csv2Path = set.csv2Path;

            label5.Text = set.ProjectName;
            label2.Text = set.solFilePath;
            label29.Text = set.ProjectName;
            label26.Text = "Create or select file.";
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

            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;

            backgroundWorker3.DoWork +=
                new DoWorkEventHandler(backgroundWorker3_DoWork);
            backgroundWorker3.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(
            backgroundWorker3_RunWorkerCompleted);
            backgroundWorker3.ProgressChanged +=
                new ProgressChangedEventHandler(
            backgroundWorker3_ProgressChanged);

            backgroundWorker3.WorkerReportsProgress = true;
            backgroundWorker3.WorkerSupportsCancellation = true;


            backgroundWorker4.DoWork +=
                new DoWorkEventHandler(backgroundWorker4_DoWork);
            backgroundWorker4.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(
            backgroundWorker4_RunWorkerCompleted);
            backgroundWorker4.ProgressChanged +=
                new ProgressChangedEventHandler(
            backgroundWorker4_ProgressChanged);

            backgroundWorker4.WorkerReportsProgress = true;
            backgroundWorker4.WorkerSupportsCancellation = true;
        }

        //Initialize backgrounsworkers
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
                label3.Text = "Completed";
                progressBar1.Value = 100;

                Settings set = Settings.Default;
                set.latexCommand = true;
                set.Save();
                SpaceClaim.Api.V19.Window.ActiveWindow.ZoomExtents(); // used to Force a window update, so the Ribbon Button is enabled/disabled
                
                button11.Enabled = true; 

                diameterValuesInserted = false;

                label26.Text = csv1Path;
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
                progressBar1.Value = 0;

                DateTime now = DateTime.Now;
                string datum = now.ToString("dd-MM-yyy");
                datum = datum.Replace("-", "");

                string csv = directory + "\\CSV\\" + projectName + "_csv1_" + datum + ".csv";
                csv1Path = csv; 

                Settings set = Settings.Default;
                set.csv1Path = csv;
                set.Save();

                // Parse chosen .sol File to csv File

                XmlRootAttribute root = new XmlRootAttribute();
                root.ElementName = "CPLEXSolution";

                VariableCollection2 col = null;

                XmlSerializer s = new XmlSerializer(typeof(VariableCollection2), root);
                StreamReader reader = new StreamReader(solPath);

                col = (VariableCollection2)s.Deserialize(reader);
                reader.Close();


                // Write deserialized data in csv file.
                int count = col.var.Count();

                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(csv))
                {
                    int i = 1; // counter for number of stabs

                    foreach (Variable2 va in col.var)
                    {
                        decimal dwert = 0;

                        if (!va.value.Contains("e"))
                        {
                            string temp = va.value.Replace(".", ",");
                            dwert = Convert.ToDecimal(temp);
                        }


                        if ((dwert > 0.85m) && (dwert < 1.15m)) // only stabs with value near 1 written in csv file
                        {
                            if (va.name.Contains("S("))
                            {
                                Stab2 stab = nameToStab(va.name);

                                String force = getForce(stab, col);

                                file.WriteLine(stab.node1 + ";" + stab.node2 + ";" + stab.diameter + ";" + force);

                                if(int.Parse(stab.diameter) > diameterNumber) // Get highest value of diameter
                                {
                                    diameterNumber = int.Parse(stab.diameter);
                                }

                                i++;
                            }
                        }

                        int percentComplete =
                          (int)((float)i / (float)count * 100);
                        worker.ReportProgress(percentComplete);

                        //int pro = (int)((float)i / (float)count);
                        //worker.ReportProgress(pro*100);
                    }

                    //file.WriteLine("Stabs: " + i); // To check if every Stab is represented 

                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show("The solution file has the wrong format!", "Info");
            }

        }

        // Returns a Stab Object as result of a name input S(diameter, node1, node2)
        public static Stab2 nameToStab(String name)
        {
            Stab2 st = new Stab2();

            int index1 = name.IndexOf("(");
            int index2 = name.IndexOf(")");

            String stab = name.Substring((index1) + 1, (index2) - 2);

            String[] werte = stab.Split(',');

            st.node1 = werte[1];
            st.node2 = werte[2];
            st.diameter = werte[0];

            return st;
        }

        // Returns the Force of a stab
        public static String getForce(Stab2 s, VariableCollection2 col)
        {
            String force = "";

            foreach (Variable2 va in col.var)
            {
                String name = "F(" + s.node1 + "," + s.node2 + ")";
                if (va.name.Contains(name))
                {
                    force = va.value;
                }
            }

            return force;
        }
        
        private void backgroundWorker3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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
                label3.Text = "Completed";
                progressBar1.Value = 100;

                button4.Enabled = true; 
            }

        }

        private void backgroundWorker3_DoWork(object sender,
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
                DateTime now = DateTime.Now;
                string datum = now.ToString("dd-MM-yyy");
                datum = datum.Replace("-", "");

                csv3Path = directory + "\\CSV\\" + projectName + "_csv3_" + datum + ".csv";
                Settings set = Settings.Default;
                set.csv3Path = csv3Path;
                set.Save();

                progressBar1.Value = 0;
                

                List<Stab2> stabs = new List<Stab2>();

                using (var reader = new StreamReader(csv1Path))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(';');

                        stabs.Add(new Stab2(values[0], values[1], values[2], values[3]));
                    }
                }

                // Write Points of cvs1 with coordinates from csv2 into csv3
                
                string csv3 = directory + "\\CSV\\" + projectName + "_csv3_" + datum + ".csv";

                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(csv3))
                {
                    for (int i = 0; i < stabs.Count(); i++)
                    {
                        String[] eins = getCoordinates(UInt64.Parse(stabs[i].node1), csv2Path);
                        String[] zwei = getCoordinates(UInt64.Parse(stabs[i].node2), csv2Path);

                        file.WriteLine(eins[1] + ";" + eins[2] + ";" + eins[3] + ";" + zwei[1] + ";" + zwei[2] + ";" + zwei[3] + ";" + stabs[i].diameter + ";" + stabs[i].force);

                        int percentComplete =
                          (int)((float)i / (float)stabs.Count() * 100);
                        worker.ReportProgress(percentComplete);
                    }
                }
                

            }
            catch (Exception exe)
            {
                MessageBox.Show("The csv1 file does not match the csv2 file. Choose the right files!", "Info");
            }

        }

        // Returns the coordinates of a given Point based on a cluster file
        public static String[] getCoordinates(UInt64 point, String pfad)
        {
            using (var reader = new StreamReader(pfad))
            {
                // Get coordinates of point i
                for (UInt64 i = 0; i < point - 1; i++)
                {
                    reader.ReadLine();
                }

                var line2 = reader.ReadLine();
                var coor1 = line2.Split(';');

                return coor1;
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            string path = "";
            string file = "";


            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Choose .sol file";
            ofd.Filter = "Solution File|*.sol";
            ofd.InitialDirectory = Settings.Default.ProjectPath + "\\Optimization";


            if (ofd.ShowDialog() == DialogResult.OK) // if user didn't cancel
            {
                path = ofd.FileName; // full File Path
                file = Path.GetFileName(path);
            }

            label2.Text = path;
            solPath = path;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Cancel Button
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Finish Button
            Settings set = Settings.Default;
            set.latexCommand = true;
            set.Save();
            SpaceClaim.Api.V19.Window.ActiveWindow.ZoomExtents(); // used to Force a window update, so the Ribbon Button is enabled/disabled

            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Back Button
            if (currentPage == 0)
            {

            }
            else if (currentPage == 1)
            {
                panel1.BringToFront();
                currentPage = 0;
                button2.Enabled = false;
                button3.Enabled = true;

                progressBar1.Value = 0;
                label3.Text = "Start process";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Next button 
            if (currentPage == 0)
            {
                panel4.BringToFront();
                currentPage = 1;
                button3.Enabled = false;
                button2.Enabled = true;

                if (!diameterValuesInserted)
                {
                    flowLayoutPanel1.Controls.Clear();
                    // Initialize diameter options
                    for (int i = 0; i < diameterNumber; i++)
                    {
                        DiameterOption dia = new DiameterOption();
                        dia.Name = "Stab " + (i + 1) + " Ø :";
                        dia.Index = i;
                        dia.Anchor = AnchorStyles.Left; 
                        dia.Anchor = AnchorStyles.Right; 
                        flowLayoutPanel1.Controls.Add(dia);
                    }

                    diameterValuesInserted = true; 
                }

            }
            else if(currentPage == 1)
            {
                
            }
        }

        // Generates csv1 file based on given sol file
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                label3.Text = "Please wait...";
                button11.Enabled = true; 
                backgroundWorker1.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured : "+ ex.ToString(), "Info");
            }
        }

        // Use Cluster Input to generate a csv File for the Cluster (cvs2) and a csv File with appropriate Coordinates 
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Settings.Default.csv2Path.Equals(""))
                {
                    label3.Text = "Please wait...";
                    backgroundWorker2.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show("You already created a working point file in this workflow. If you wish to create a new one, press the create button again.", "Info");
                    Settings.Default.csv2Path = "";
                    Settings.Default.Save();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured : "+ ex.ToString(), "Info");
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                button7.Enabled = true;
                label3.Text = "Please wait...";

                backgroundWorker4.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured: "+ ex.ToString(), "Info");
            }
        }

        private void backgroundWorker4_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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
                label3.Text = "Completed";
                progressBar1.Value = 100;
            }

        }

        private void backgroundWorker4_DoWork(object sender,
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
                progressBar1.Value = 0;

                List<Stab2> stabs = new List<Stab2>();

                // Read csv1 lines 
                using (var reader = new StreamReader(csv1Path))
                {
                    while (!reader.EndOfStream)
                    {
                       
                            var line = reader.ReadLine();
                            var values = line.Split(';');

                            stabs.Add(new Stab2(values[0], values[1], values[2], values[3]));
                        
                    }
                }

                // Exchange diameter values 
                foreach (Stab2 stab in stabs)
                {
                    foreach (DiameterOption opt in flowLayoutPanel1.Controls)
                    {
                        if (opt.Index == int.Parse(stab.diameter))
                        {
                            stab.diameter = "" + opt.Value;
                        }
                    }
                }


                // rewrite csv1 file
                System.IO.File.WriteAllText(csv1Path, string.Empty);

                int i = 1; // counter for number of stabs
                int count = stabs.Count();

                using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(csv1Path))
                {
                    foreach (Stab2 stab in stabs)
                    {
                        file.WriteLine(stab.node1 + ";" + stab.node2 + ";" + stab.diameter + ";" + stab.force);

                        int percentComplete =
                          (int)((float)i / (float)stabs.Count() * 100);
                        worker.ReportProgress(percentComplete);

                        i++;
                    }
                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show("An error occured: " + ex.ToString(), "Info");
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Choose csv1 file";
            ofd.Filter = "CSV |*.csv";
            ofd.InitialDirectory = Settings.Default.ProjectPath + "\\CSV";


            if (ofd.ShowDialog() == DialogResult.OK) // if user didn't cancel
            {
                csv1Path = ofd.FileName; // full File Path
                //file = Path.GetFileName(path);

                label26.Text = csv1Path;
                Settings.Default.csv1Path = csv1Path;
                Settings.Default.Save();

                // Renew diameterNumber
                diameterNumber = 0; 
                
                using (var reader = new StreamReader(csv1Path))
                {
                    while (!reader.EndOfStream)
                    {
                        if (!reader.ReadLine().Equals(""))
                        {
                            var line = reader.ReadLine();
                            var values = line.Split(';');

                            if (int.Parse(values[2]) > diameterNumber) // Get highest value of diameter
                            {
                                diameterNumber = int.Parse(values[2]);
                            }
                        }
                    }
                }

                diameterValuesInserted = false; 
                // Renew Diameter options
                if (!diameterValuesInserted)
                {
                    flowLayoutPanel1.Controls.Clear();
                    // Initialize diameter options
                    for (int i = 0; i <= diameterNumber; i++)
                    {
                        DiameterOption dia = new DiameterOption();
                        dia.Name = "Stab " + (i + 1) + " Ø :";
                        dia.Index = i;
                        flowLayoutPanel1.Controls.Add(dia);
                    }

                    diameterValuesInserted = true;
                }

                button11.Enabled = true; 
            }
        }


        // Creates the final csv File with stabs and their coordinates
        private void button7_Click_1(object sender, EventArgs e)
        {
            try
            {
                label3.Text = "Please wait...";

                backgroundWorker3.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured: " + ex.ToString(), "Info");
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Settings.Default.CPLEXSolutionFormOpened = false;
            Settings.Default.Save();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MessageBox.Show("", "Info");
        }
    }
}



// Represents a stab
public class Stab2
{
    public String node1 { get; set; }
    public String node2 { get; set; }
    public String diameter { get; set; }
    public String force { get; set; }

    public Stab2(String p1, String p2, String dia, String fo)
    {
        node1 = p1;
        node2 = p2;
        diameter = dia;
        force = fo;
    }

    public Stab2()
    {

    }
}


// Represents a variable xml Element 
[Serializable()]
public class Variable2
{
    [System.Xml.Serialization.XmlAttribute("name")]
    public String name { get; set; }

    [System.Xml.Serialization.XmlAttribute("index")]
    public String index { get; set; }

    [System.Xml.Serialization.XmlAttribute("value")]
    public String value { get; set; }
}


// Holds all Variable Elements in a List 
[Serializable()]
[System.Xml.Serialization.XmlRoot("variables")]
public class VariableCollection2
{
    [XmlArray("variables")]
    [XmlArrayItem("variable", typeof(Variable2))]
    public Variable2[] var { get; set; }
}