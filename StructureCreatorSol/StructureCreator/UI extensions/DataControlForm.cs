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


namespace StructureCreator.UI_extensions
{
    public partial class DataControlForm : Form
    {
        public DataControlForm()
        {
            InitializeComponent();
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataTable table = new DataTable();

            table.Columns.Add("id",typeof(int));
            table.Columns.Add("Start Node", typeof(double));
            table.Columns.Add("End Node", typeof(double));
            table.Columns.Add("Diameter", typeof(double));
            table.Columns.Add("Force", typeof(double));

            table.Rows.Add(1,2,7,5,0);

            dataGridView1.DataSource = table;

        }

        private void Button1_Click(object sender, EventArgs e)
        {

        }
        
        private void ChooseDataButton_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream and display them on Datagridview
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }

            MessageBox.Show(fileContent, "Find a way to display that data on datagrid table " + filePath, MessageBoxButtons.OK);
        }
    }
}
