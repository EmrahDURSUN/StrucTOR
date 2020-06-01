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

namespace StructureCreator.UI_extensions
{
    public partial class DevelopEditStabsForm : Form
    {
        DialogBox form2;

        String csvPath = "";

        DataTable table = new DataTable(); // DataTable that shows the data of the selected csv file
        private object txtResult;

        public DevelopEditStabsForm()
        {
            InitializeComponent();
        }

        // Choose a csv file. Reads the first line of the file and calls initializeColumns to generate the correct 
        // table format
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                table = new DataTable(); //Remove old Columns and Data

                string path = "";
                string file = "";
                

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Choose csv file";
                ofd.Filter = "csv File|*.csv";

                if (ofd.ShowDialog() == DialogResult.OK) // if user didn't cancel
                {
                    path = ofd.FileName; // full File Path
                    file = Path.GetFileName(path);
                }

                csvPath = path;
                label4.Text = file;


                string line1 = File.ReadLines(path).First(); // gets the first line from file.


                initializeColumns(line1, path);

            }catch(Exception ex)
            {
                MessageBox.Show("CSV file has the wrong format" + ex.ToString(), "Info");
            }
        }

        // Makes Diameter Column writeable or not 
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                dataGridView1.Columns["D"].ReadOnly = false;
            }else if (!checkBox1.Checked)
            {
                dataGridView1.Columns["D"].ReadOnly = true;
            }
        }

        // Opens the DialogBox Form and calls a method based on the users decision in the 
        // DialogBox
        private void button2_Click(object sender, EventArgs e)
        {
            
            form2 = new DialogBox();
            form2.StartPosition = FormStartPosition.CenterScreen;

            DialogResult dr = form2.ShowDialog(this);
            // Show testDialog as a modal dialog and determine if DialogResult = OK.

            if (dr == DialogResult.OK)
            {
                // Read the contents of testDialog's TextBox.
                writeOK();
            }
            else if(dr == DialogResult.Cancel)
            {
                writeC();
            }
        }

        // Sample Method
        public void writeOK()
        {
            MessageBox.Show("modify", "Info"); 
        }

        // Sample Method
        public void writeC()
        {
            MessageBox.Show("save", "Info"); 
        }

        // Imports the data of the selected file based on the csv1 file (P1, P2, D, F) 
        // or the csv3 file (x1, y1, z1, x2, y2, z2, D, F)
        public void initializeColumns(String line1, String path)
        {
            List<Reihe1> rows1 = new List<Reihe1>();
            List<Reihe2> rows2 = new List<Reihe2>();

            String[] ar = line1.Split(';');
            int count = 1;

            if (ar.Length == 4)
            {
                DataColumn c = new DataColumn();
                c.DataType = typeof(int);
                c.ColumnName = "Index";
                table.Columns.Add(c);
                table.Columns.Add("P1");
                table.Columns.Add("P2");
                table.Columns.Add("D");
                table.Columns.Add("F");

                dataGridView1.DataSource = table;

                dataGridView1.Columns["Index"].ReadOnly = true;
                dataGridView1.Columns["P1"].ReadOnly = true;
                dataGridView1.Columns["P2"].ReadOnly = true;
                dataGridView1.Columns["D"].ReadOnly = true;
                dataGridView1.Columns["F"].ReadOnly = true;

                using (var reader = new StreamReader(path))
                {

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(';');

                        rows1.Add(new Reihe1(count, values[0], values[1], values[2], values[3]));

                        count++;
                    }
                }

                foreach (Reihe1 r in rows1)
                {
                    DataRow rei = table.NewRow();

                    rei["Index"] = r.index;
                    rei["P1"] = r.p1;
                    rei["P2"] = r.p2;
                    rei["D"] = r.diameter;
                    rei["F"] = r.force;

                    table.Rows.Add(rei);
                }

            }
            else if (ar.Length == 8)
            {
                DataColumn c = new DataColumn();
                c.DataType = typeof(int);
                c.ColumnName = "Index";
                table.Columns.Add(c);
                table.Columns.Add("x1");
                table.Columns.Add("y1");
                table.Columns.Add("z1");
                table.Columns.Add("x2");
                table.Columns.Add("y2");
                table.Columns.Add("z2");
                table.Columns.Add("D");
                table.Columns.Add("F");

                dataGridView1.DataSource = table;

                dataGridView1.Columns["Index"].ReadOnly = true;
                dataGridView1.Columns["x1"].ReadOnly = true;
                dataGridView1.Columns["y1"].ReadOnly = true;
                dataGridView1.Columns["z1"].ReadOnly = true;
                dataGridView1.Columns["x2"].ReadOnly = true;
                dataGridView1.Columns["y2"].ReadOnly = true;
                dataGridView1.Columns["z2"].ReadOnly = true;
                dataGridView1.Columns["D"].ReadOnly = true;
                dataGridView1.Columns["F"].ReadOnly = true;

                using (var reader = new StreamReader(path))
                {

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(';');

                        rows2.Add(new Reihe2(count, values[0], values[1], values[2], values[3], values[4], values[5], values[6], values[7]));

                        count++;
                    }
                }

                foreach (Reihe2 r in rows2)
                {
                    DataRow rei = table.NewRow();

                    rei["Index"] = r.index;
                    rei["x1"] = r.x1;
                    rei["y1"] = r.y1;
                    rei["z1"] = r.z1;
                    rei["x2"] = r.x2;
                    rei["y2"] = r.y2;
                    rei["z2"] = r.z2;
                    rei["D"] = r.diameter;
                    rei["F"] = r.force;

                    table.Rows.Add(rei);
                }

                dataGridView1.Sort(dataGridView1.Columns["Index"], ListSortDirection.Ascending);
            }
        }


        // Represents a row of csv1 file
        public class Reihe1
        {
            public int index; 
            public string p1;
            public string p2;
            public string diameter;
            public string force;

            public Reihe1(int _index, String P1, String P2, String Dia, String For)
            {
                index = _index; 
                p1 = P1;
                p2 = P2;
                diameter = Dia;
                force = For;
            }
        }

        // Represents a row of csv3 file
        public class Reihe2
        {
            public int index;
            public string x1;
            public string y1;
            public string z1;
            public string x2;
            public string y2;
            public string z2;
            public string diameter;
            public string force;

            public Reihe2(int _index, String X1 , String Y1, String Z1, String X2, String Y2, String Z2, String Dia, String For)
            {
                index = _index;
                x1 = X1;
                y1 = Y1;
                z1 = Z1;
                x2 = X2;
                y2 = Y2;
                z2 = Z2;
                diameter = Dia;
                force = For;
            }
        }
    }
}
