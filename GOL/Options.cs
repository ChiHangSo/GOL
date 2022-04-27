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
    public partial class Options : Form
    {
        public int ninterval;
        public int xUniverse;
        public int yUniverse;
        public int interval
        {
            get { return ninterval; }
            set { interval = ninterval; }
        }
        public Options()
        {
            InitializeComponent();
            ninterval = Properties.Settings.Default.Interval;
            //xUniverse = Properties.Settings.Default.UniverseX;
            //yUniverse = Properties.Settings.Default.UniverseY;
        }
        // Numeric value in intervals
        private void numericUpDownMili_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

    }
}
