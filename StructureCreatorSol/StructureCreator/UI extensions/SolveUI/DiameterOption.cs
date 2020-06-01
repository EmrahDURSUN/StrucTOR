using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StructureCreator.UI_extensions.SolveUI
{
    public partial class DiameterOption : UserControl
    {
        public DiameterOption()
        {
            InitializeComponent();
        }
        
        private String name;
        private decimal _value;
        private int index;

        [Category("Options Item")]
        public String Name
        {
            get { return name; }
            set { name = value; label1.Text = value; }
        }

        [Category("Options Item")]
        public decimal Value
        {
            get { return _value; }
        }

        [Category("Options Item")]
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            _value = numericUpDown1.Value;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
