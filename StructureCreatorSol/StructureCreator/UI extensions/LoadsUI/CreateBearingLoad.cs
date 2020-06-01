using StructureCreator.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpaceClaim.Api.V19.Geometry;
using SpaceClaim.Api.V19;
using SpaceClaim.Api.V19.Modeler;
using Point = SpaceClaim.Api.V19.Geometry.Point;

namespace StructureCreator.UI_extensions.LoadsUI
{
    /// <summary>
    /// Represents bearing load form.User can select a point and create a force on it.
    /// </summary>
    public partial class CreateBearingLoad : Form
    {
        int currentPage = 0;
        DataTable table = new DataTable();

        public CreateBearingLoad()
        {
            InitializeComponent();

            panel3.BringToFront();
            button4.Enabled = false; 

            Settings set = Settings.Default;

            // check if 2D or 3D is used
            if (set.Dimension == 0)
            {
                numericUpDown2.Enabled = true;
                numericUpDown1.Enabled = false;
                numericUpDown3.Enabled = false;
            }
            else if (set.Dimension == 1)
            {

                numericUpDown1.Enabled = false;
                numericUpDown2.Enabled = false;
                numericUpDown3.Enabled = true;
            }

            label3.Text = "Select Point and click 'Read selected point'";

            // Show all bearing loads in table
            try
            {
                // Initialize DataTable
                //DataColumn c = new DataColumn();
                //c.DataType = typeof(int);
                //c.ColumnName = "Point";
                table.Columns.Add("Point");
                table.Columns.Add("x");
                table.Columns.Add("y");
                table.Columns.Add("z");
                table.Columns.Add("Force x");
                table.Columns.Add("Force y");
                table.Columns.Add("Force z");

                dataGridView1.DataSource = table;

                dataGridView1.Columns["Point"].ReadOnly = true;
                dataGridView1.Columns["x"].ReadOnly = true;
                dataGridView1.Columns["y"].ReadOnly = true;
                dataGridView1.Columns["z"].ReadOnly = true;
                dataGridView1.Columns["Force x"].ReadOnly = true;
                dataGridView1.Columns["Force y"].ReadOnly = true;
                dataGridView1.Columns["Force z"].ReadOnly = true;


                // Fill in Data
                String[] bearingLoads = set.bearingLoads.Split(';');

                foreach (String s in bearingLoads)
                {
                    String[] temp = s.Split(',');

                    DataRow rei = table.NewRow();

                    rei["Point"] = temp[0].Substring(1, temp[0].Length - 1);
                    rei["x"] = temp[1];
                    rei["y"] = temp[2];
                    rei["z"] = temp[3];
                    rei["Force x"] = temp[4];
                    rei["Force y"] = temp[5];
                    rei["Force z"] = temp[6].Substring(0, temp[6].Length - 1);

                    table.Rows.Add(rei);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error: " + ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Save forces in Settings
            Settings set = Settings.Default;

            set.pointForceX = numericUpDown1.Value;
            set.pointForceY = numericUpDown2.Value;
            set.pointForceZ = numericUpDown3.Value;

            set.Save();

            // Update table
            DataRow rei = table.NewRow();

            String[] tempCoord = set.activePointCoord.Split(';');

            rei["Point"] = set.activePointName;
            rei["x"] = tempCoord[0];
            rei["y"] = tempCoord[1];
            rei["z"] = tempCoord[2];
            rei["Force x"] = numericUpDown1.Value;
            rei["Force y"] = numericUpDown2.Value;
            rei["Force z"] = numericUpDown3.Value;

            table.Rows.Add(rei);

            // Create a force arrow at point
            try
            {
                try
                {
                    Command.Execute(BearingLoadsCapsuleCapsule.CommandName); // Call command that creates arrow 

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Info");
                }

                button4.Enabled = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Info");
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel2.BringToFront();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            panel3.BringToFront();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Command.Execute(ReadBearingLoadCapsule.CommandName); // Calls command that saves selected point
                label3.Text = Settings.Default.activePoint;
                button4.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Info");
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Settings.Default.BearingFormOpened = false;
            Settings.Default.Save();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                String forcesNew = "";

                List<String> toDelete = new List<String>();

                List<String> toDeleteForces = new List<String>();

                List<String> toDeleteForcesIndex = new List<String>();

                Settings set = Settings.Default;

                Int32 selectedRowCount =
                dataGridView1.SelectedRows.Count;

                if (selectedRowCount > 0)
                {
                    if (dataGridView1.AreAllCellsSelected(true))
                    {
                        MessageBox.Show("All loads are selected and will be deleted.", "Selected laods");
                    }
                    else
                    {
                        foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                        {
                            toDelete.Add(row.Cells[0].Value.ToString());
                        }

                        foreach (String del in toDelete)
                        {
                            String temp = set.bearingLoads;

                            String[] arr = temp.Split(';');

                            foreach (String forces in arr)
                            {
                                if (!forces.Equals(String.Empty))
                                {
                                    String[] s = forces.Split(',');

                                    if (s[0].Substring(1, s[0].Length - 1).Equals(del))
                                    {
                                        toDeleteForces.Add(forces);
                                        toDeleteForcesIndex.Add(s[0].Substring(1, s[0].Length - 1));
                                    }
                                }
                            }
                        }

                        String[] arr2 = set.bearingLoads.Split(';');
                        foreach (String forces in arr2)
                        {
                            bool exists = false;

                            if (!forces.Equals(String.Empty))
                            {
                                foreach (String i in toDeleteForces)
                                {
                                    if (i.Equals(forces))
                                    {
                                        exists = true;
                                    }
                                }

                                if (!exists)
                                {
                                    forcesNew += forces + ";";
                                }
                            }
                        }

                        set.bearingLoads = forcesNew;
                        set.bearingLoadCount -= toDeleteForces.Count;
                        set.Save();

                        int counter = 0;

                        for (int i = 0; i < selectedRowCount; i++)
                        {
                            if (counter < selectedRowCount)
                            {
                                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[i].Index - counter);
                                counter++;
                            }
                        }

                        dataGridView1.Refresh();



                        // Remove arrows
                        String temp2 = "";
                        foreach (String s in toDeleteForcesIndex)
                        {
                            temp2 += s + ";";
                        }

                        Settings.Default.deleteArrows = temp2;
                        Settings.Default.Save();


                        try
                        {
                            Command.Execute(RemoveArrowLoadsCapsule.CommandName); // Call command that deletes the arrows    
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString(), "Info");
                        }


                        MessageBox.Show("The selected forces will be deleted", "Selected forces");
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}