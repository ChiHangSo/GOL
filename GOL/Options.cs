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
        public Options()
        {
            InitializeComponent();

            // Write out the default values to the user
            numericUpDownMili.Value = Properties.Settings.Default.Interval;
            numericUpDownWidCells.Value = Properties.Settings.Default.CellsX;
            numericUpDownHeiCells.Value = Properties.Settings.Default.CellsY;
        }
        // Numeric value in intervals
        private void numericUpDownMili_ValueChanged(object sender, EventArgs e)
        {

        }
        private void numericUpDownWidCells_ValueChanged(object sender, EventArgs e)
        {
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Interval = (int)numericUpDownMili.Value;
            Properties.Settings.Default.CellsX = (int)numericUpDownWidCells.Value;
            Properties.Settings.Default.CellsY = (int)numericUpDownHeiCells.Value;

            // Saving the properties
            Properties.Settings.Default.Save();
        }
    }
}
