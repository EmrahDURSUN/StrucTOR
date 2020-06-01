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
    public partial class DevelopLatexForm : Form
    {
        String stylePath = "";
        String projectPath = "";
        String texPath = "";
        String pdfPath = "";

        String csv1 = "";
        String csv2 = "";

        public DevelopLatexForm()
        {
            InitializeComponent();

            Settings set = Settings.Default;
            stylePath = set.StyPath;

            label8.Text = set.StyPath;

            InitializeBackgroundWorker();
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
                button3.Enabled = true;
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
                File.Copy(stylePath, set.ProjectPath + "\\Latex\\" + filename);

                //String filename2 = Path.GetFileName(stylePath2);
                //File.Copy(stylePath2, set.ProjectPath + "\\Latex\\" + filename2);


                FileStream stream = null;
                string fileName = set.ProjectPath + "\\Latex\\" + set.ProjectName + "tikzlattice.tex";   // tex file Path
                stream = new FileStream(fileName, FileMode.Create);

                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    writer.WriteLine("\\documentclass{minimal}");
                    writer.WriteLine("\\usepackage{3dp}");
                    writer.WriteLine("\\usetikzlibrary{intersections}");
                    writer.WriteLine("");
                    writer.WriteLine("\\begin{document}");
                    writer.WriteLine("%");
                    writer.WriteLine("\t\\showpoint");
                    writer.WriteLine("\t\\begin{tikzpicture}[coords]");
                    writer.WriteLine("\t\t%");
                    writer.WriteLine("\t\t%Settings");
                    writer.WriteLine("\t\t%");
                    writer.WriteLine("\t\t\\dscaling{1}{0.1};\t%scaling of point distances");
                    writer.WriteLine("\t\t\\dscaling{2}{1.0};\t%scaling of the bearings");
                    writer.WriteLine("\t\t\\dscaling{3}{1.0};\t%scaling of the axes");
                    writer.WriteLine("\t\t\\dscaling{4}{1.0};\t%scaling of the angle loads and the line loads");
                    writer.WriteLine("\t\t\\dscaling{5}{0.6};\t%scaling of the dimensioning");
                    writer.WriteLine("\t\t\\dscaling{6}{1.0};\t%scaling of the additional symbols");
                    writer.WriteLine("\t\t%");
                    writer.WriteLine("\t\t%Points");
                    writer.WriteLine("\t\t%\\dpoint{name}{x-coordiante}{y-coordiante}{z-coordinate};");
                    writer.WriteLine("\t\t%");

                    writer.WriteLine("\t\t\\dpoint{coordGlobal}{-10}{-10}{0};");
                    writer.WriteLine("\t\t\\dpoint{coordLocal}{-10}{0}{0};");
                    // Get points of csv2 file
                    List<PointClass> points = new List<PointClass>();

                    using (var reader = new StreamReader(csv2)) // csv2 path
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var values = line.Split(';');

                            PointClass p = new PointClass();
                            p.point = values[0];
                            p.x = values[1];
                            p.y = values[2];
                            p.z = values[3];

                            points.Add(p);
                        }
                    }

                    // Write all points into tex file
                    foreach (PointClass p in points)
                    {
                        writer.WriteLine("\t\t\\dpoint{" + p.point + "}{" + p.x + "}{" + p.z + "}{" + p.y + "};");
                    }

                    writer.WriteLine("\t\t%");
                    writer.WriteLine("\t\t%Beams and bars");
                    writer.WriteLine("\t\t%bending beam: \\dbeam{1}{initial point}{end point}[rounded initial point][rounded end point];");
                    writer.WriteLine("\t\t%truss rod: \\dbeam{2}{initial point}{end point}[rounded initial point][rounded end point];");
                    writer.WriteLine("\t\t%invisible/dashed beam: \\dbeam{3}{initial point}{end point};");
                    writer.WriteLine("\t\t%");

                    // Get points of csv2 file
                    List<BeamClass> beams = new List<BeamClass>();

                    using (var reader = new StreamReader(csv1)) // csv1 path
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var values = line.Split(';');

                            BeamClass b = new BeamClass();
                            b.p1 = values[0];
                            b.p2 = values[1];
                            b.d = values[2];
                            b.f = values[3];

                            beams.Add(b);
                        }
                    }

                    // Write all points into tex file
                    foreach (BeamClass b in beams)
                    {
                        writer.WriteLine("\t\t\\dbeam{" + b.d + "mm}{P" + b.p1 + "}{P" + b.p2 + "}[1][1];");
                    }

                    writer.WriteLine("\t\t%");
                    writer.WriteLine("\t\t% Axis");
                    writer.WriteLine("\t\t%global: \\daxis{1}{insertion point}[X-orientation][Y-orientation][Z-orientation];");
                    writer.WriteLine("\t\t%local: \\daxis{2}{plane}[insertion point][end point][position][x-orientation][y-orientation][z-orientation][change y with z];");
                    writer.WriteLine("\t\t%local in space: \\daxis{3}{rotation A}[insertion point][end point][position][rotation 1][rotation 2][rotation 3][rotation B];");
                    writer.WriteLine("\t\t%");
                    writer.WriteLine("\t\t\\setaxis{1};"); // global coordinate system
                    writer.WriteLine("\t\t\\daxis{1}{coordGlobal}[][][];");
                    writer.WriteLine("\t\t\\daxis{3}{0}[coordGlobal][coordLocal][0.2][0][0][0];");
                    writer.WriteLine("\t\t%");
                    writer.WriteLine("\t\t%Supports and bearings");
                    writer.WriteLine("\t\t%fixed support, which can absorb forces in all directions, but no moments: \\dsupport{1}{insertion point}[x-direction][y-direction][z-direction];");
                    writer.WriteLine("\t\t%fixed support, which can absorb all forces and moments: \\dsupport{2}{insertion point}[plane];");
                    writer.WriteLine("\t\t%fixed support, base for a forked support: \\dsupport{3}{insertion point}[x-direction][y-direction][z-direction];");
                    writer.WriteLine("\t\t%fixed support, pendulum rods are springs: \\dsupport{4}{insertion point}[x-direction][y-direction][z-direction];");
                    writer.WriteLine("\t\t%fixed support, springs for a forked support: \\dsupport{5}{insertion point}[x-direction][y-direction][z-direction];");
                    writer.WriteLine("\t\t%");
                    writer.WriteLine("\t\t%Joints and Hinges");
                    writer.WriteLine("\t\t%full joint: \\dhinge{1}{insertion point};");
                    writer.WriteLine("\t\t%half joint: \\dhinge{2}{insertion point}[initial point][end point][orientation];");
                    writer.WriteLine("\t\t%forged hinge: \\dhinge{3}{insertion point}[rotation];");
                    writer.WriteLine("\t\t%stiffening of a corner: \\dhinge{4}{insertion point}[initial point][end point];");
                    writer.WriteLine("\t\t%");

                    //// write all points in file
                    //foreach (Point p in points)
                    //{
                    //    {
                    //        writer.WriteLine("\t\t\\dhinge{1}{" + p.point + "};");
                    //    }
                    //}

                    // only write used points in file
                    foreach (PointClass p in points)
                    {
                        string pName = p.point.Substring(1);

                        bool used = false;

                        foreach (BeamClass b in beams)
                        {
                            if (pName.Equals(b.p1) || pName.Equals(b.p1))
                            {
                                used = true;
                            }
                        }

                        if (used == true)
                        {
                            writer.WriteLine("\t\t\\dhinge{1}{" + p.point + "};");
                        }
                    }

                    writer.WriteLine("\t\t%");
                    writer.WriteLine("\t\t%Single loads");
                    writer.WriteLine("\t\t%single force: \\dload{1}{insertion point}[rotation A][rotation B][load length][load distance];");
                    writer.WriteLine("\t\t%force pointing away from point: \\dload{2}{insertion point}[rotation A][rotation B][load length][load distance];");
                    writer.WriteLine("\t\t%single moment: \\dload{2}{insertion point}[rotation A][rotation B][load length][load distance];");
                    writer.WriteLine("\t\t%single moment away from point: \\dload{2}{insertion point}[rotation A][rotation B][load length][load distance];");
                    writer.WriteLine("\t\t%");
                    writer.WriteLine("\t\t%Line loads");
                    writer.WriteLine("\t\t%linear load perpendicular to the beam axis: \\dlineload{1}{plane}[plane distance]{initial point}{end point}[initial force value] [end force value][force interval];");
                    writer.WriteLine("\t\t%linear load parallel to the coressponding global axis: \\dlineload{2}{plane}[plane distance]{initial point}{end point}[initial force value] [end force value][force interval];");
                    writer.WriteLine("\t\t%a projection of the forces on the beam:  \\dlineload{3}{plane}[plane distance]{initial point}{end point}[initial force value] [end force value][lineload distance from inital point][force interval];");
                    writer.WriteLine("\t\t%a line load along the bar axis: \\dlineload{4}{plane}[plane distance]{initial point}{end point}[force interval][force length];");
                    writer.WriteLine("\t\t%");
                    writer.WriteLine("\t\t%internal forces");
                    writer.WriteLine("\t\t%\\dinternalforces{plane}[plane distance]{initial point}{end point}{initial value}{end value}[parabola height][color][bend position]; ");
                    writer.WriteLine("\t\t%");
                    writer.WriteLine("\t\t%...");
                    writer.WriteLine("\t\\end{tikzpicture}");
                    writer.WriteLine("\\end{document}");
                }


                // Create PDF file out of tex-file created above

                //Setting an instance of ProcessStartInfo class
                // under System.Diagnostic Assembly Reference
                ProcessStartInfo pro = new ProcessStartInfo();
                pro.WorkingDirectory = set.ProjectPath + "\\Latex";
                //Setting the FileName to be Started like in our
                //Project we are just going to start a CMD Window.
                pro.FileName = "cmd.exe";
                pro.Arguments = "/C " + "pdflatex " + set.ProjectName + "tikzlattice.tex";

                //pro.RedirectStandardOutput = true;
                //pro.UseShellExecute = false;

                Process proStart = new Process();
                //Setting up the Process Name here which we are 
                // going to start from ProcessStartInfo
                proStart.StartInfo = pro;
                proStart.StartInfo.CreateNoWindow = true;
                //proStart.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                //Calling the Start Method of Process class to
                // Invoke our Process viz 'cmd.exe'

                proStart.Start();

            }
            catch (Exception exp)
            {
                Console.Write(exp.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = "";

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Choose sty file";
            ofd.Filter = "Style File|*.sty";

            if (ofd.ShowDialog() == DialogResult.OK) // if user didn't cancel
            {
                path = ofd.FileName; // full File Path
            }

            label3.Text = path;
            stylePath = path;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                backgroundWorker1.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured : " + ex.ToString(), "Info");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string path = "";

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Choose csv file";
            ofd.Filter = "Style File|*.csv";

            if (ofd.ShowDialog() == DialogResult.OK) // if user didn't cancel
            {
                path = ofd.FileName; // full File Path
            }

            csv1 = path;
            label5.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string path = "";

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Choose csv file";
            ofd.Filter = "Style File|*.csv";

            if (ofd.ShowDialog() == DialogResult.OK) // if user didn't cancel
            {
                path = ofd.FileName; // full File Path
            }

            csv2 = path;
            label6.Visible = true;

            button2.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Settings set = Settings.Default;

            if (File.Exists(set.ProjectPath + "\\Latex\\" + set.ProjectName + "tikzlattice.pdf"))
            {
                // Open PDF file
                ProcessStartInfo pro2 = new ProcessStartInfo();

                pro2.WorkingDirectory = set.ProjectPath + "\\Latex";

                //Setting the FileName to be Started like in our
                //Project we are just going to start a CMD Window.
                pro2.FileName = "cmd.exe";
                pro2.Arguments = "/C " + "start /max tikzlattice.pdf";

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
                MessageBox.Show("No pdf file created. Please install pdflatex!", "Info");
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
}