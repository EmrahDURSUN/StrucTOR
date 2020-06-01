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
using System.Xml.Serialization;
using System.Collections;
//using System.Windows.Shapes; 
using System.Diagnostics;
using StructureCreator.Properties;
using StructureCreator;

namespace StructureCreator.UI_extensions
{
    /// <summary>
    /// Transform a sol file into different csv files. 
    /// Format : csv1 : (P1, P2, D, F), csv2: (x1, y1, z1,), csv3: (x1, y1, z1, x2, y2, z2, D, F)
    /// </summary>
    public partial class DevelopSolImportForm : Form
    {
        private String solFilePath = "";
        private String directory = "";
        private String projectName = "";

        private String csvStabsFilePath = "";
        private String csvClusterFilePath = "";

        public DevelopSolImportForm()
        {
            InitializeComponent();

            InitializeBackgroundWorker();
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

            backgroundWorker2.DoWork +=
               new DoWorkEventHandler(backgroundWorker2_DoWork);
            backgroundWorker2.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(
            backgroundWorker2_RunWorkerCompleted);
            backgroundWorker2.ProgressChanged +=
                new ProgressChangedEventHandler(
            backgroundWorker2_ProgressChanged);

            backgroundWorker2.WorkerReportsProgress = true;
            backgroundWorker2.WorkerSupportsCancellation = true;

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
                label8.Visible = true;
                label20.Text = "Completed";
                progressBar1.Value = 100;
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
                label20.Text = "Please wait..."; 
                progressBar1.Value = 0;

                DateTime now = DateTime.Now;
                string datum = now.ToString("dd-MM-yyy");
                datum = datum.Replace("-", "");

                string csv = directory + "\\" + projectName + "_csv_" + datum + ".csv";

                // Parse chosen .sol File to csv File

                XmlRootAttribute root = new XmlRootAttribute();
                root.ElementName = "CPLEXSolution";

                VariableCollection col = null;

                XmlSerializer s = new XmlSerializer(typeof(VariableCollection), root);
                StreamReader reader = new StreamReader(solFilePath);

                col = (VariableCollection)s.Deserialize(reader);
                reader.Close();


                // Write deserialized data in csv file.
                int count = col.var.Count();

                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(csv))
                {
                    int i = 1; // counter for number of stabs

                    foreach (Variable va in col.var)
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
                                Stab stab = nameToStab(va.name);

                                String force = getForce(stab, col);

                                file.WriteLine(stab.node1 + ";" + stab.node2 + ";" + stab.diameter + ";" + force);
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
                MessageBox.Show("An error occured: " + ex.ToString(), "Info");
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
                label15.Visible = true;
                label20.Text = "Completed";
                progressBar1.Value = 100;
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            try
            {
                label20.Text = "Please wait...";
                progressBar1.Value = 0;

                DateTime now = DateTime.Now;
                string datum = now.ToString("dd-MM-yyy");
                datum = datum.Replace("-", "");

                decimal width = numericUpDown1.Value;
                decimal heigth = numericUpDown2.Value;
                decimal depth = numericUpDown3.Value;
                decimal distance = numericUpDown4.Value;

                String csv2path = directory + "\\" + projectName + "_csv2_" + datum + ".csv";

                createCoordinateSystem(width, heigth, depth, distance, csv2path, worker);
            }catch(Exception ex)
            {
                MessageBox.Show("An error occured: " + ex.ToString(), "Info");
            }
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
                label19.Visible = true;
                label20.Text = "Completed";
                progressBar1.Value = 100;
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
                progressBar1.Value = 0; 
                label20.Text = "Please wait...";

                DateTime now = DateTime.Now;
                string datum = now.ToString("dd-MM-yyy");
                datum = datum.Replace("-", "");

                List<Stab> stabs = new List<Stab>();

                using (var reader = new StreamReader(csvStabsFilePath))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(';');

                        stabs.Add(new Stab(values[0], values[1], values[2], values[3]));
                    }
                }

                // Write Points of cvs1 with coordinates from csv2 into csv3

                string csv3 = directory + "\\" + projectName + "_csv3_" + datum + ".csv";

                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(csv3))
                {
                    for (int i = 0; i < stabs.Count(); i++)
                    {
                        String[] eins = getCoordinates(UInt64.Parse(stabs[i].node1), csvClusterFilePath);
                        String[] zwei = getCoordinates(UInt64.Parse(stabs[i].node2), csvClusterFilePath);

                        file.WriteLine(eins[1] + ";" + eins[2] + ";" + eins[3] + ";" + zwei[1] + ";" + zwei[2] + ";" + zwei[3] + ";" + stabs[i].diameter + ";" + stabs[i].force);

                        int percentComplete =
                          (int)((float)i / (float)stabs.Count() * 100);
                        worker.ReportProgress(percentComplete);
                    }
                }

                
            }
            catch (Exception exe)
            {
                MessageBox.Show("An error occured : " + exe.ToString(), "Info");
            }

        }





        // Chooses a project folder 
        private void Button1_Click(object sender, EventArgs e)
        {
            string path = "";
            string folder = "";


            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK) // if user didn't cancel
            {
                path = fbd.SelectedPath;
                folder = Path.GetFileName(path);
            }


            label5.Text = folder;
            label5.Font = new Font(label5.Font, FontStyle.Bold);

            directory = path;
            projectName = folder;
            
            
        }

        // Chooses a sol file 
        private void Button2_Click(object sender, EventArgs e)
        {
            string path = "";
            string file = "";


            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Choose sol file";
            ofd.Filter = "Solution File|*.sol";

            if (ofd.ShowDialog() == DialogResult.OK) // if user didn't cancel
            {
                path = ofd.FileName; // full File Path
                file = Path.GetFileName(path);
            }

            label7.Text = file;
            solFilePath = path;

            // Enable convert button
            if (!solFilePath.Equals("") && !directory.Equals("") && !projectName.Equals(""))
            {
                button3.Enabled = true;
            }
        }

        // Generates csv1 file based on given sol file
        private void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                backgroundWorker1.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured : "+ ex.ToString(), "Info");
            }
        }


        // Returns a Stab Object as result of a name input S(diameter, node1, node2)
        public static Stab nameToStab(String name)
        {
            Stab st = new Stab();

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
        public static String getForce(Stab s, VariableCollection col)
        {
            String force = "";

            foreach (Variable va in col.var)
            {
                String name = "F(" + s.node1 + "," + s.node2 + ")";
                if (va.name.Contains(name))
                {
                    force = va.value;
                }
            }

            return force;
        }


        // Use Cluster Input to generate a csv File for the Cluster (cvs2) and a csv File with appropriate Coordinates 
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                backgroundWorker2.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured : "+ ex.ToString(), "Info");
            }
        }

        // Creates a coordinate System in csv format
        public static void createCoordinateSystem(decimal width, decimal heigth, decimal depth, decimal distance, String path, BackgroundWorker worker)
        {
            DateTime now = DateTime.Now;
            string datum = now.ToString("dd-MM-yyy");
            datum = datum.Replace("-", "");

            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(path))
            {
                int point = 1;

                for (decimal z = 0; z < depth; z+=distance) //int z = 0; z < depth; z+=distance
                {
                    for (decimal y = 0; y < heigth; y+=distance)
                    {
                        for (decimal x = 0; x < width; x+=distance)
                        {
                            file.WriteLine("P" + point + ";" + x + ";" + y + ";" + z);
                            point++;
                        }
                    }

                    int percentComplete =
                          (int)((float)z / (float)depth * 100);
                    worker.ReportProgress(percentComplete);
                }
            }
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {
            // Create the ToolTip and associate with the Form container.
            ToolTip toolTip1 = new ToolTip();

            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 1;
            toolTip1.InitialDelay = 1;
            toolTip1.ReshowDelay = 1;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;

            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(this.button6, "Select a csv1 file that was generated above");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string path = "";

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "CSV Datei auswählen";
            ofd.Filter = "Solution File|*.csv";
            ofd.InitialDirectory = directory;

            if (ofd.ShowDialog() == DialogResult.OK) // if user didn't cancel
            {
                path = ofd.FileName; // full File Path
            }

            label17.Text = "File chosen";
            csvStabsFilePath = path;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string path = "";

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "CSV Datei auswählen";
            ofd.Filter = "Solution File|*.csv";
            ofd.InitialDirectory = directory;

            if (ofd.ShowDialog() == DialogResult.OK) // if user didn't cancel
            {
                path = ofd.FileName; // full File Path
            }

            label18.Text = "File chosen";
            csvClusterFilePath = path;
        }

        // Creates the final csv File with stabs and their coordinates
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                backgroundWorker3.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured : "+ ex.ToString(), "Info");
            }
        }

        // Returns the coordinates of a given Point based on a cluster file
        public static String[] getCoordinates(UInt64 point, String pfad)
        {
            using (var reader = new StreamReader(pfad))
            {
                // Get coordinates of point 1
                for (UInt64 i = 0; i < point - 1; i++)
                {
                    reader.ReadLine();
                }

                var line2 = reader.ReadLine();
                var coor1 = line2.Split(';');

                return coor1;
            }
        }

        private void SolImportForm_Load(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Settings set = Settings.Default;
            set.Active = true;
            set.Save();

            MessageBox.Show("Now enabled!", "Info");
            //SpaceClaim.Api.V19.Window.ActiveWindow.RefreshRendering();

            SpaceClaim.Api.V19.Window.ActiveWindow.ZoomExtents(); // used to Force a window update, so the Ribbon Button is enabled/disabled

        }

        private void button9_Click(object sender, EventArgs e)
        {
            Settings set = Settings.Default;
            set.Active = false;
            set.Save();

            MessageBox.Show("Now disabled!", "Info");
            //SpaceClaim.Api.V19.Window.ActiveWindow.RefreshRendering();

            SpaceClaim.Api.V19.Window.ActiveWindow.ZoomExtents(); // used to Force a window update, so the Ribbon Button is enabled/disabled
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }





        //private void button5_Click(object sender, EventArgs e)
        //{
        //    String test = "Lorem ipsum" + Environment.NewLine + "dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue duis dolore te feugait nulla facilisi.Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat.Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue duis dolore te feugait nulla facilisi. Nam liber tempor cum soluta nobis eleifend option congue nihil imperdiet doming id quod mazim placerat facer possim assum.Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat.Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis. At vero eos et accusam et justo duo dolores et ea rebum.Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua.At vero eos et accusam et justo duo dolores et ea rebum.Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.Lorem ipsum dolor sit amet, consetetur sadipscing elitr, At accusam aliquyam diam diam dolore dolores duo eirmod eos erat, et nonumy sed tempor et et invidunt justo labore Stet clita ea et gubergren, kasd magna no rebum.sanctus sea sed takimata ut vero voluptua.est Lorem ipsum dolor sit amet. " + "/n" + "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat. Consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua.At vero eos et accusam et justo duo dolores et ea rebum.Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua.At vero eos et accusam et justo duo dolores et ea rebum.Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua.At vero eos et accusam et justo duo dolores et ea rebum.Stet clita kasd gubergren, no sea takimata sanctus. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua.At vero eos et accusam et justo duo dolores et ea rebum.Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua.At vero eos et accusam et justo duo dolores et ea rebum.Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua.At vero eos et accusam et justo duo dolores et ea rebum.Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.";
        //    MessageBox.Show(test);
        //}


    }
}

// Represents a stab
public class Stab
{
    public String node1 { get; set; }
    public String node2 { get; set; }
    public String diameter { get; set; }
    public String force { get; set; }

    public Stab(String p1, String p2, String dia, String fo)
    {
        node1 = p1;
        node2 = p2;
        diameter = dia;
        force = fo;
    }

    public Stab()
    {

    }
}


// Represents a variable xml Element 
[Serializable()]
public class Variable
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
public class VariableCollection
{
    [XmlArray("variables")]
    [XmlArrayItem("variable", typeof(Variable))]
    public Variable[] var { get; set; }
}