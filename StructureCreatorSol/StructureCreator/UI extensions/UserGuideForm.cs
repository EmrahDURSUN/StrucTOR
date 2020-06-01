using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StructureCreator.Properties;

namespace StructureCreator.UI_extensions
{
    public partial class UserGuideForm : Form
    {
        int currentPicture = 0;
        public UserGuideForm()
        {
            InitializeComponent();

            pictureBox1.BringToFront();
            button1.BringToFront();
            button2.BringToFront();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Settings set = Settings.Default; 

            if (checkBox1.Checked)
            {
                set.showHintOnTab = false;
            }
            else
            {
                set.showHintOnTab = true; 
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //button1.Text = char.ConvertFromUtf32(0x00002192);
            switch (currentPicture)
            {
                case 0:
                    pictureBox2.BringToFront();
                    button1.BringToFront();
                    button2.BringToFront();
                    currentPicture = 1;
                    break;
                case 1:
                    pictureBox3.BringToFront();
                    button1.BringToFront();
                    button2.BringToFront();
                    currentPicture = 2;
                    break;
                case 2:
                    pictureBox4.BringToFront();
                    button1.BringToFront();
                    button2.BringToFront();
                    currentPicture = 3;
                    break;
                case 3:
                    pictureBox5.BringToFront();
                    button1.BringToFront();
                    button2.BringToFront();
                    currentPicture = 4;
                    break;
                case 4:
                    pictureBox6.BringToFront();
                    button1.BringToFront();
                    button2.BringToFront();
                    currentPicture = 5;
                    break;
                case 5:
                    pictureBox1.BringToFront();
                    button1.BringToFront();
                    button2.BringToFront();
                    currentPicture = 0;
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //button2.Text = char.ConvertFromUtf32(0x00002190);
            switch (currentPicture)
            {
                case 0:
                    pictureBox6.BringToFront();
                    button1.BringToFront();
                    button2.BringToFront();
                    currentPicture = 5;
                    break;
                case 1:
                    pictureBox1.BringToFront();
                    button1.BringToFront();
                    button2.BringToFront();
                    currentPicture = 0;
                    break;
                case 2:
                    pictureBox2.BringToFront();
                    button1.BringToFront();
                    button2.BringToFront();
                    currentPicture = 1;
                    break;
                case 3:
                    pictureBox3.BringToFront();
                    button1.BringToFront();
                    button2.BringToFront();
                    currentPicture = 2;
                    break;
                case 4:
                    pictureBox4.BringToFront();
                    button1.BringToFront();
                    button2.BringToFront();
                    currentPicture = 3;
                    break;
                case 5:
                    pictureBox5.BringToFront();
                    button1.BringToFront();
                    button2.BringToFront();
                    currentPicture = 4;
                    break;
            }
        }
    }
}
