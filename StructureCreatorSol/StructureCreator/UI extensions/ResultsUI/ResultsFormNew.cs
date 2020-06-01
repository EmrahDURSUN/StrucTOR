using StructureCreator.Properties;
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

namespace StructureCreator.UI_extensions.ResultsUI
{
    /// <summary>
    /// Represents results form. User can read csv files and log files
    /// </summary>
    public partial class ResultsFormNew : Form
    {
        DataTable table = new DataTable(); // DataTable that shows the data of the selected csv file
        DataTable table2 = new DataTable(); // DataTable that shows the data of the selected csv2 file
        DataTable table3 = new DataTable(); // DataTable that shows the data of the selected csv3 file

        String csv1 = "";
        String csv2 = "";
        String csv3 = "";

      
        public ResultsFormNew()
        {
            InitializeComponent();

            try
            {
                Settings set = Settings.Default;

                // Print log file in text box
                if (!set.lplogPath.Equals(""))
                {
                    string[] lines = System.IO.File.ReadAllLines(set.lplogPath);

                    // Display the file contents by using a foreach loop.

                    foreach (string line in lines)
                    {
                        // Use a tab to indent each line of the file.
                        textBox1.AppendText(line);
                        textBox1.AppendText(Environment.NewLine);
                    }

                    label2.Text = set.lplogPath;
                }
                else
                {
                    label2.Text = "No log file created.";
                }
                
                label4.Text = set.csv1Path;

                csv1 = set.csv1Path;
                csv2 = set.csv2Path;
                csv3 = set.csv3Path;

                dataGridView1.DataSource = null;
                dataGridView2.DataSource = null;
                dataGridView3.DataSource = null;

                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;

                initializeColumns();

                panel1.BringToFront();
                dataGridView1.BringToFront();
            }
            catch (Exception ex)
            {
                //label2.Text = "No log file created";
                MessageBox.Show(ex.ToString());
            }
        }

        // Imports the data of the selected file based on the csv1 file (P1, P2, D, F) 
        // or the csv3 file (x1, y1, z1, x2, y2, z2, D, F) or csv2 file (P, x, y, z)
        public void initializeColumns()
        {

            if (!Settings.Default.csv1Path.Equals(""))
            {
                // CSV1 file
                List<Row1> rows1 = new List<Row1>();

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

                using (var reader = new StreamReader(csv1))
                {
                    int count = 1;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(';');

                        rows1.Add(new Row1(count, values[0], values[1], values[2], values[3]));

                        count++;
                    }
                }

                foreach (Row1 r in rows1)
                {
                    DataRow rei1 = table.NewRow();

                    rei1["Index"] = r.index;
                    rei1["P1"] = r.p1;
                    rei1["P2"] = r.p2;
                    rei1["D"] = r.diameter;
                    rei1["F"] = r.force;

                    table.Rows.Add(rei1);
                }

                dataGridView1.Sort(dataGridView1.Columns["Index"], ListSortDirection.Ascending);


            }


            if (!Settings.Default.csv2Path.Equals(""))
            {
                // Csv2 file
                List<Row2> rows2 = new List<Row2>();

                DataColumn d = new DataColumn();
                d.DataType = typeof(int);
                d.ColumnName = "P";
                table2.Columns.Add(d);
                table2.Columns.Add("x");
                table2.Columns.Add("y");
                table2.Columns.Add("z");

                dataGridView2.DataSource = table2;

                dataGridView2.Columns["P"].ReadOnly = true;
                dataGridView2.Columns["x"].ReadOnly = true;
                dataGridView2.Columns["y"].ReadOnly = true;
                dataGridView2.Columns["z"].ReadOnly = true;

                using (var reader = new StreamReader(csv2))
                {
                    int count = 1;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(';');

                        rows2.Add(new Row2(count, values[1], values[2], values[3]));

                        count++;
                    }
                }

                foreach (Row2 r in rows2)
                {
                    DataRow rei2 = table2.NewRow();

                    rei2["P"] = r.index;
                    rei2["x"] = r.x;
                    rei2["y"] = r.y;
                    rei2["z"] = r.z;

                    table2.Rows.Add(rei2);
                }

                dataGridView2.Sort(dataGridView2.Columns["P"], ListSortDirection.Ascending);

            }

            if (!Settings.Default.csv3Path.Equals(""))
            {
                // CSV3 file
                List<Row3> rows3 = new List<Row3>();

                DataColumn e = new DataColumn();
                e.DataType = typeof(int);
                e.ColumnName = "Index";
                table3.Columns.Add(e);
                table3.Columns.Add("x1");
                table3.Columns.Add("y1");
                table3.Columns.Add("z1");
                table3.Columns.Add("x2");
                table3.Columns.Add("y2");
                table3.Columns.Add("z2");
                table3.Columns.Add("D");
                table3.Columns.Add("F");

                dataGridView3.DataSource = table3;

                dataGridView3.Columns["Index"].ReadOnly = true;
                dataGridView3.Columns["x1"].ReadOnly = true;
                dataGridView3.Columns["y1"].ReadOnly = true;
                dataGridView3.Columns["z1"].ReadOnly = true;
                dataGridView3.Columns["x2"].ReadOnly = true;
                dataGridView3.Columns["y2"].ReadOnly = true;
                dataGridView3.Columns["z2"].ReadOnly = true;
                dataGridView3.Columns["D"].ReadOnly = true;
                dataGridView3.Columns["F"].ReadOnly = true;

                using (var reader = new StreamReader(csv3))
                {
                    int count = 1;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(';');

                        rows3.Add(new Row3(count, values[0], values[1], values[2], values[3], values[4], values[5], values[6], values[7]));

                        count++;
                    }
                }

                foreach (Row3 r in rows3)
                {
                    DataRow rei3 = table3.NewRow();

                    rei3["Index"] = r.index;
                    rei3["x1"] = r.x1;
                    rei3["y1"] = r.y1;
                    rei3["z1"] = r.z1;
                    rei3["x2"] = r.x2;
                    rei3["y2"] = r.y2;
                    rei3["z2"] = r.z2;
                    rei3["D"] = r.diameter;
                    rei3["F"] = r.force;

                    table3.Rows.Add(rei3);
                }

                dataGridView3.Sort(dataGridView3.Columns["Index"], ListSortDirection.Ascending);

            }
        }

        // Shows chosen csv file
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings set = Settings.Default;

            if (comboBox1.SelectedIndex == 0)
            {
                try
                {
                    label4.Text = csv1;
                    dataGridView1.BringToFront();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                try
                {
                    label4.Text = csv2;
                    dataGridView2.BringToFront();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                try
                {
                    label4.Text = csv3;
                    dataGridView3.BringToFront();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                // change csv file
                List<Row1> rows1 = new List<Row1>();
                List<Row2> rows2 = new List<Row2>();
                List<Row3> rows3 = new List<Row3>();

                string path = "";

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Choose csv file";
                ofd.Filter = "CSV |*.csv";
                ofd.InitialDirectory = Settings.Default.ProjectPath + "\\CSV";

                if (ofd.ShowDialog() == DialogResult.OK) // if user didn't cancel
                {
                    path = ofd.FileName; // full File Path

                    string line1 = File.ReadLines(path).First(); // gets the first line from file.
                    int csvType = getCSVType(line1);

                     
                    if (comboBox1.SelectedIndex == 0)
                    {
                        try
                        {
                            if (csvType == 1)
                            {
                                table.Clear();


                                // new datatable
                                DataTable table4 = new DataTable();
                                dataGridView1.DataSource = null;

                                DataColumn c = new DataColumn();
                                c.DataType = typeof(int);
                                c.ColumnName = "Index";
                                table4.Columns.Add(c);
                                table4.Columns.Add("P1");
                                table4.Columns.Add("P2");
                                table4.Columns.Add("D");
                                table4.Columns.Add("F");

                                dataGridView1.DataSource = table4;

                                dataGridView1.Columns["Index"].ReadOnly = true;
                                dataGridView1.Columns["P1"].ReadOnly = true;
                                dataGridView1.Columns["P2"].ReadOnly = true;
                                dataGridView1.Columns["D"].ReadOnly = true;
                                dataGridView1.Columns["F"].ReadOnly = true;


                                using (var reader = new StreamReader(path))
                                {
                                    int count = 1;
                                    while (!reader.EndOfStream)
                                    {
                                        var line = reader.ReadLine();
                                        var values = line.Split(';');

                                        rows1.Add(new Row1(count, values[0], values[1], values[2], values[3]));

                                        count++;
                                    }
                                }

                                foreach (Row1 r in rows1)
                                {
                                    DataRow rei1 = table4.NewRow();

                                    rei1["Index"] = r.index;
                                    rei1["P1"] = r.p1;
                                    rei1["P2"] = r.p2;
                                    rei1["D"] = r.diameter;
                                    rei1["F"] = r.force;

                                    table4.Rows.Add(rei1);
                                }

                                dataGridView1.Sort(dataGridView1.Columns["Index"], ListSortDirection.Ascending);

                                label4.Text = path;
                                csv1 = path;
                            }
                            else
                            {
                                MessageBox.Show("Wrong data format", "Info");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Wrong data format " + ex.ToString(), "Info");
                        }
                    }
                    else if (comboBox1.SelectedIndex == 1)
                    {
                        try
                        {
                            if (csvType == 2)
                            {
                                table2.Clear();

                                // new datatable
                                DataTable table5 = new DataTable();
                                dataGridView2.DataSource = null;

                                DataColumn d = new DataColumn();
                                d.DataType = typeof(int);
                                d.ColumnName = "P";
                                table5.Columns.Add(d);
                                table5.Columns.Add("x");
                                table5.Columns.Add("y");
                                table5.Columns.Add("z");

                                dataGridView2.DataSource = table5;

                                dataGridView2.Columns["P"].ReadOnly = true;
                                dataGridView2.Columns["x"].ReadOnly = true;
                                dataGridView2.Columns["y"].ReadOnly = true;
                                dataGridView2.Columns["z"].ReadOnly = true;

                                using (var reader = new StreamReader(path))
                                {
                                    int count = 1;
                                    while (!reader.EndOfStream)
                                    {
                                        var line = reader.ReadLine();
                                        var values = line.Split(';');

                                        rows2.Add(new Row2(count, values[1], values[2], values[3]));

                                        count++;
                                    }
                                }

                                foreach (Row2 r in rows2)
                                {
                                    DataRow rei2 = table5.NewRow();

                                    rei2["P"] = r.index;
                                    rei2["x"] = r.x;
                                    rei2["y"] = r.y;
                                    rei2["z"] = r.z;

                                    table5.Rows.Add(rei2);
                                }

                                dataGridView2.Sort(dataGridView2.Columns["P"], ListSortDirection.Ascending);

                                label4.Text = path;
                                csv2 = path;
                            }
                            else
                            {
                                MessageBox.Show("Wrong data format", "Info");
                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Wrong data format" + ex.ToString(), "Info");
                        }
                    }
                    else if (comboBox1.SelectedIndex == 2)
                    {
                        try
                        {
                            if (csvType == 3)
                            {
                                table3.Clear();

                                // new datatable
                                DataTable table6 = new DataTable();
                                dataGridView3.DataSource = null;

                                DataColumn f = new DataColumn();
                                f.DataType = typeof(int);
                                f.ColumnName = "Index";
                                table6.Columns.Add(f);
                                table6.Columns.Add("x1");
                                table6.Columns.Add("y1");
                                table6.Columns.Add("z1");
                                table6.Columns.Add("x2");
                                table6.Columns.Add("y2");
                                table6.Columns.Add("z2");
                                table6.Columns.Add("D");
                                table6.Columns.Add("F");

                                dataGridView3.DataSource = table6;

                                dataGridView3.Columns["Index"].ReadOnly = true;
                                dataGridView3.Columns["x1"].ReadOnly = true;
                                dataGridView3.Columns["y1"].ReadOnly = true;
                                dataGridView3.Columns["z1"].ReadOnly = true;
                                dataGridView3.Columns["x2"].ReadOnly = true;
                                dataGridView3.Columns["y2"].ReadOnly = true;
                                dataGridView3.Columns["z2"].ReadOnly = true;
                                dataGridView3.Columns["D"].ReadOnly = true;
                                dataGridView3.Columns["F"].ReadOnly = true;

                                using (var reader = new StreamReader(path))
                                {
                                    int count = 1;
                                    while (!reader.EndOfStream)
                                    {
                                        var line = reader.ReadLine();
                                        var values = line.Split(';');

                                        rows3.Add(new Row3(count, values[0], values[1], values[2], values[3], values[4], values[5], values[6], values[7]));

                                        count++;
                                    }
                                }

                                foreach (Row3 r in rows3)
                                {
                                    DataRow rei3 = table6.NewRow();

                                    rei3["Index"] = r.index;
                                    rei3["x1"] = r.x1;
                                    rei3["y1"] = r.y1;
                                    rei3["z1"] = r.z1;
                                    rei3["x2"] = r.x2;
                                    rei3["y2"] = r.y2;
                                    rei3["z2"] = r.z2;
                                    rei3["D"] = r.diameter;
                                    rei3["F"] = r.force;

                                    table6.Rows.Add(rei3);
                                }

                                dataGridView3.Sort(dataGridView3.Columns["Index"], ListSortDirection.Ascending);

                                label4.Text = path;
                                csv3 = path;
                            }
                            else
                            {
                                MessageBox.Show("Wrong data format", "Info");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Wrong data format" + ex.ToString(), "Info");
                        }
                    }
                }
            }catch(Exception exe)
            {
                MessageBox.Show(exe.ToString()); 
            }
            
        }

        // Get type of csv file
        private int getCSVType(string line)
        {
            String[] values = line.Split(';');
            if (values[0].FirstOrDefault().Equals('P'))
            {
                return 2;
            }
            else if (values.Length == 4)
            {
                return 1;
            }
            else if (values.Length == 8)
            {
                return 3;
            }

            return -1;
        }

        private void logfilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel2.BringToFront();
        }

        private void csvFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.BringToFront();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings set = Settings.Default;

            if (comboBox2.SelectedIndex == 0)
            {
                textBox1.Clear();

                try
                {
                    if (!set.lplogPath.Equals(""))
                    {
                        string[] lines = System.IO.File.ReadAllLines(set.lplogPath);

                        // Display the file contents by using a foreach loop.

                        foreach (string line in lines)
                        {
                            // Use a tab to indent each line of the file.
                            textBox1.AppendText(line);
                            textBox1.AppendText(Environment.NewLine);
                        }

                        label2.Text = set.lplogPath;
                    }
                    else
                    {
                        label2.Text = "No log file created.";
                    }
                }
                catch (Exception ex)
                {
                    
                }

            }
            else if (comboBox2.SelectedIndex == 1)
            {
                textBox1.Clear();

                try
                {
                    if (!set.cplexlogPath.Equals(""))
                    {
                        string[] lines = System.IO.File.ReadAllLines(set.cplexlogPath);

                        // Display the file contents by using a foreach loop.

                        foreach (string line in lines)
                        {
                            // Use a tab to indent each line of the file.
                            textBox1.AppendText(line);
                            textBox1.AppendText(Environment.NewLine);
                        }

                        label2.Text = set.cplexlogPath;
                    }
                    else
                    {
                        label2.Text = "No log file created.";
                    }
                }
                catch (Exception ex)
                {
                    
                }


            }
        }
    }


    // Represents a row of csv1 file
    public class Row1
    {
        public int index;
        public string p1;
        public string p2;
        public string diameter;
        public string force;

        public Row1(int _index, String P1, String P2, String Dia, String For)
        {
            index = _index;
            p1 = P1;
            p2 = P2;
            diameter = Dia;
            force = For;
        }
    }

    // Represents a row of csv2 file
    public class Row2
    {
        public int index;
        public string x;
        public string y;
        public string z;

        public Row2(int _index, String _x, String _y, String _z)
        {
            index = _index;
            x = _x;
            y = _y;
            z = _z;
        }
    }

    // Represents a row of csv3 file
    public class Row3
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

        public Row3(int _index, String X1, String Y1, String Z1, String X2, String Y2, String Z2, String Dia, String For)
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
