using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StructureCreator.UI_extensions
{
    /// <summary>
    /// Represents a CPLEX option. Options are shown in CPLEX Form
    /// </summary>
    public partial class DevelopOptionsItem : UserControl
    {
        public DevelopOptionsItem()
        {
            InitializeComponent();
        }
        

        private Boolean use;
        private String name;
        private String _value;
        private String dataType;
        private String link; 

        [Category("Options Item")]
        public Boolean Use
        {
            get { return use; }
            set { use = value; checkBox1.Checked = value; }
        }

        [Category("Options Item")]
        public String Name
        {
            get { return name; }
            set { name = value; label2.Text = value; }
        }

        [Category("Options Item")]
        public String Value
        {
            get { return _value; }
            set { _value = value; textBox1.Text = value; }
        }

        [Category("Options Item")]
        public String DataType
        {
            get { return dataType; }
            set { dataType = value; }
        }

        [Category("Options Item")]
        public String Link
        {
            get { return link; }
            set { link = value; }
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBox1.Checked)
                {
                    if (int.Parse(Value) >= 0)
                    {
                        Use = true;
                        textBox1.BackColor = System.Drawing.Color.White;
                    }
                    else
                    {
                        MessageBox.Show("Unallowed value!", "Info");
                        textBox1.BackColor = System.Drawing.Color.Red;
                        checkBox1.Checked = false;
                    }
                }
                else if (!checkBox1.Checked)
                {
                    Use = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unallowed value!", "Info");
                textBox1.BackColor = System.Drawing.Color.Red;
                checkBox1.Checked = false;
            }
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(link);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Value = textBox1.Text;
        }
    }
}
