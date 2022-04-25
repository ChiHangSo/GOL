using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOL
{
    public partial class Randomize : Form
    {
        //Storing the public randomized value
        public int num;
        public int Mnum
        {
            get { return num; }
            set { num = value; }
        }
        public Randomize()
        {
            InitializeComponent();

            //Setting the maximun value
            numericUpDown1.Maximum = int.MaxValue;

            //Setting the minimun value
            numericUpDown1.Minimum = int.MinValue;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //Creating a new ran value
            Random ran = new Random();

            //Setting a value
            Mnum = ran.Next(int.MinValue, int.MaxValue);

            //Setting the value to the numeric up down
            numericUpDown1.Value = num;

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            //Setting the value to the numeric up down
            Mnum = (int)numericUpDown1.Value;
        }
    }
}
