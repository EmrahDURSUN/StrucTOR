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
    // Represents an option in the list of used options
    public partial class CplexOption2 : UserControl
    {
        public CplexOption2()
        {
            InitializeComponent();
        }

        private String name;
        private String _value;
        

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
            set { _value = value; label4.Text = value; }
        }
        
    }
}
