using System;
using System.Windows.Forms;
using System.IO;

namespace StructureCreator.UI_extensions
{
    public partial class SolFileForm : Form
    {
        public SolFileForm()
        {
            InitializeComponent();
        }

        public void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog(); // This line opens a import window
            ofd.Title = "Choose Text File";                     
            ofd.Filter = "Solution File|*.sol|Text File|*txt";      // We can increase the filter file types

            if (ofd.ShowDialog() == DialogResult.OK) // if user didn't cancel
            {
                StreamReader sr = new StreamReader(File.OpenRead(ofd.FileName));

                textBox1.Text = sr.ReadToEnd();
                
                sr.Dispose(); 
            }
        }
    }
}
