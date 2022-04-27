//#define Mark
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


namespace GOL
{

    public partial class Form1 : Form
    {
       
        // The universe array
        bool[,] universe = new bool[10, 10];

        // The scrachpad array
        bool[,] scratchPad = new bool[10, 10];

        // Drawing colors
        Color gridColor = Color.Black;
        Color cellColor = Color.Gray;

        // The Timer class
        Timer timer = new Timer();

        // Generation count
        int generations = 0;

        int count;

        // The seed
        public int seed;

        // On off torodial / finite
        public bool onOff = true;

        // Setting the interval
        public int interval;

        // The cell
        public int cell;

        // Bool grid
        public bool grid = true;

        // toggle finite or torodial
        public bool types = true;

        // HUD

        // Int num Cells
        int Acells =0;
        // Display
        bool hud = true;

        private void graphicsPanel1_Paint_1(object sender, PaintEventArgs e)
        {
            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            float cellWidth = (float)graphicsPanel1.ClientSize.Width / universe.GetLength(0);
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            float cellHeight = (float)graphicsPanel1.ClientSize.Height / universe.GetLength(1);

            
            // Alive cell count
            Acells = 0;

            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 1);
       
            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);

            count = 0;
            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // A rectangle to represent each cell in pixels
                    //RectangleF
                    RectangleF cellRect = RectangleF.Empty;
                    cellRect.X = x * cellWidth;
                    cellRect.Y = y * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;
       
                    // Fill the cell with a brush if alive
                    if (universe[x, y] == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRect);
                        //Will count how many cells are colored
                        Acells++;
                        //Update status strip alive
                        toolStripStatusLabelAlive.Text = "Alive = " + Acells.ToString();
                    }

                    // Check to see which boundary we are
                    if (types == true)
                    {
                        // Set the count
                        count = CountNeightborsToroidal(x, y);
                    }
                    else
                    {
                        // Set the count
                        count = CountNeighborsFinite(x, y);
                    }

                    //Apply the cell rules
                    CellRules(x, y);

                    // Outline the cell with a pen
                    e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                    
                    // New font for the numbers
                    Font font = new Font("Arial", 20f);
                    
                    // Alignment
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;


                    int neighbors = count;

                    // Coloring the numbers
                    if(neighbors > 0 && onOff == true)
                    {
                        //Draw the neighbors
                        e.Graphics.DrawString(neighbors.ToString(), font, Brushes.Black, cellRect, stringFormat);
                    }

                    // On/Off HUD
                    if (hud == true)
                    {
                        //colors the HUD
                        HUD(e);
                    }
                }
            }
       
            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
        }
       
        private void graphicsPanel1_MouseClick_1(object sender, MouseEventArgs e)
        {
            //Floats
            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
       
                //floats
       
                // Calculate the width and height of each cell in pixels
                int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
                int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);
       
                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                int x = e.X / cellWidth;
                // CELL Y = MOUSE Y / CELL HEIGHT
                int y = e.Y / cellHeight;
       
                // Toggle the cell's state
                universe[x, y] = !universe[x, y];
       
                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }

        // Function HUD
        public void HUD(PaintEventArgs e)
        {
            // Store in a string value
            string Sgen;
            Sgen = "Generations: " + generations.ToString();

            // Alive Cells
            string alive;
            alive = "Cell Count: " + Acells.ToString();

            // Type of world
            string Hworld = "Boundary Type: ";
            
            //If statement if the world is either true or false
            if(types == true)
            {
                Hworld += "Torodial";
            }
            else
            {
                Hworld += "Finite";
            }

            // Universe Size
            string uni = "";

            uni = "Universe size: {Width = " + universe.GetLength(0).ToString() + ", Height = " + universe.GetLength(1).ToString() + "}";

            //Display
            Rectangle display = new Rectangle(0,0, graphicsPanel1.ClientSize.Width, graphicsPanel1.ClientSize.Height);

            // String to store all strings
            string all = Sgen + "\n" + alive + "\n" + Hworld + "\n" + uni;

            // HUD settings
            Font font = new Font("Arial", 12);
            e.Graphics.DrawString(all, font, Brushes.Red, display);
        }

        // Timer
        public Form1()
        {
            InitializeComponent();

            // Setup the timer
            interval = timer.Interval = Properties.Settings.Default.Interval; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false; // start timer running
        }

        // Calculate the next generation of cells
        
        private void CellRules(int x, int y)
        {
            //Apply the rules

            //Living cells with less than 2 living neighbors will die in the next generation.
            if (count < 2)
            {
                scratchPad[x, y] = false;
            }
            // Living cells with less than 2 living neighbors will die in the next generations
            if (count > 3)
            {
                scratchPad[x, y] = false;
            }
            // If the universe is true and there's either two or three neighbor count it will stay in the next universe
            if (universe[x, y] == true && count == 2 || count == 3)
            {
                scratchPad[x, y] = true;
            }
            else
            {
                // Else it dies
                scratchPad[x, y] = false;
            }
            // If universe has a neighbor of three it will stay in the next universe
            if (universe[x, y] == false && count == 3)
            {
                scratchPad[x, y] = true;
            }

        }
        private void NextGeneration()
        {
            // Copy from scratchpad to universe

            bool[,] temp = universe;
            universe = scratchPad;
            scratchPad = temp;

            // Increment generation count
            generations++;

            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();

            //Update status strip alive
            toolStripStatusLabelAlive.Text = "Alive = " + Acells.ToString();


            // Repaint the universe
            graphicsPanel1.Invalidate();
        }

        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        // Finite rules
        private int CountNeighborsFinite(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);

            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;

                    // if xOffset and yOffset are both equal to 0 then continue
                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }
                    // if xCheck is less than 0 then continue
                    if (xCheck < 0)
                    {
                        continue;
                    }
                    // if yCheck is less than 0 then continue
                    if (yCheck < 0)
                    {
                        continue;
                    }
                    // if xCheck is greater than or equal too xLen then continue
                    if (xCheck >= xLen)
                    {
                        continue;
                    }
                    // if yCheck is greater than or equal too yLen then continue
                    if (yCheck >= yLen)
                    {
                        continue;
                    }

                    if (universe[xCheck, yCheck] == true) count++;
                }
            }
            return count;
        }

        // Torodial rules
        private int CountNeightborsToroidal(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);

            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;

                    // if xOffset and yOffset are both equal to 0 then continue
                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }
                    // if xCheck is less than 0 then set to xLen - 1
                    if (xCheck < 0)
                    {
                        xCheck = xLen -1;
                    }
                    // if yCheck is less than 0 then set to yLen - 1
                    if (yCheck < 0)
                    {
                        yCheck = yLen -1;
                    }
                    // if xCheck is greater than or equal too xLen then set to 0
                    if (xCheck >= xLen)
                    {
                        xCheck = 0;
                    }
                    // if yCheck is greater than or equal too yLen then set to 0
                    if (yCheck >= yLen)
                    {
                        yCheck = 0;
                    }

                    if (universe[xCheck, yCheck] == true) count++;
                }
            }

            return count;
        }

        //Start button
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;

            //When button one is disabled, button two is enabled
            toolStripButton1.Enabled = false;
            toolStripButton2.Enabled = true;
        }

        //Pause button
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;

            //When button two is disabled, button one is enabled.
            toolStripButton1.Enabled = true;
            toolStripButton2.Enabled = false;
        }

        //Next generation button
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            NextGeneration();
        }

        //Emptying the universe
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            //Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                //Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // Setting everything to false
                    universe[x, y] = false;
                    scratchPad[x, y] = false; 
                }
            }
            generations = 0;

            //This is to recolor the universe
            graphicsPanel1.Invalidate();

        }

        //Exit button
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Making a new universe through the white icon
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            //Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                //Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // Setting everything to false
                    universe[x, y] = false;
                    scratchPad[x, y] = false;
                }
            }

            // Set the generations to 0 when resetting
            generations = 0;

            // Set the seed to 0
            seed = 0;

            //This is to recolor the universe
            graphicsPanel1.Invalidate();
        }

        // Randomize form -> randomize from seed
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //This is the Form universe from Seed
            //Created a new form, and to access the form, created an instance of it to show it.
            Randomize ran = new Randomize();

            // Create a store
            int storage = 0;

            //The code for the new universe
            if (DialogResult.OK == ran.ShowDialog())
            {
                seed = ran.Mnum;
                Random Rseed = new Random(seed);
                //Iterate through the universe in the y, top to bottom
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    //Iterate through the universe in the x, left to right
                    for (int x = 0; x < universe.GetLength(0); x++)
                    {
                        storage = Rseed.Next(0, 3);

                        if (storage == 0)
                        {
                            universe[x, y] = true;
                        }
                    }
                }
            }

            // Update the seed
            toolStripStatusLabelSeed.Text = "Seed =" + seed.ToString();

            //Storing the seed
            Properties.Settings.Default.Seed = seed;
            Properties.Settings.Default.Save();

            //This is to recolor the universe
            graphicsPanel1.Invalidate();
        }

        // Options for mili and resizing the universe
        private void optionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Created a new form, and to access the form, created an instance of it to show it.
            Options op = new Options();
            
            if(DialogResult.OK == op.ShowDialog())
            {
                // Check if the universe is changed
                if(Properties.Settings.Default.CellsX != universe.GetLength(0) && Properties.Settings.Default.CellsY != universe.GetLength(1))
                {
                    // Readjust the universe
                    universe = new bool[Properties.Settings.Default.CellsX, Properties.Settings.Default.CellsY];

                    // Readjust the scratchpad
                    scratchPad = new bool[Properties.Settings.Default.CellsX, Properties.Settings.Default.CellsY];
                }

                // Set the timer to the interval
                timer.Interval = Properties.Settings.Default.Interval;
                // Readjusting the interval
                toolStripStatusLabelInterval.Text = "Interval = " + Properties.Settings.Default.Interval;

                // Repaint the universe
                graphicsPanel1.Invalidate();
            }
        }

        // Toggle the HUD
        private void hUDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //This is the HUD
            // True or false hud

            hud = !hud;

            // To see if the check is on / off
            hUDToolStripMenuItem.Checked = !hUDToolStripMenuItem.Checked;
            hUDToolStripMenuItem1.Checked = !hUDToolStripMenuItem1.Checked;

            // repaint the hud
            graphicsPanel1.Invalidate();

        }

        // Right click back color
        private void backColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Back color
            //Creating a new color dialog
            ColorDialog dlg = new ColorDialog();

            //This will show the back color when opening the menu
            dlg.Color = graphicsPanel1.BackColor;

            //This if statement is here so that when the user selects a color, the system know which color the user chose
            if( DialogResult.OK == dlg.ShowDialog())
            {
                //This will set the back color to whatever color the user chose
                graphicsPanel1.BackColor = dlg.Color;
            }

            graphicsPanel1.Invalidate();
        }

        // Right click cell color
        private void cellColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Cell color
            //Creating a new color dialog
            ColorDialog dlg = new ColorDialog();

            //This will show the cell color when opening the menu
            dlg.Color = cellColor;

            //This if statement is here so that when the user selects a color, the system know which color the user chose
            if (DialogResult.OK == dlg.ShowDialog())
            {
                //This will set the back color to whatever color the user chose
                cellColor = dlg.Color;
            }
            graphicsPanel1.Invalidate();
        }

        // Right click grid color
        private void gridColorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Grid color
            //Creating a new color dialog
            ColorDialog dlg = new ColorDialog();

            //This will show the grid color when opening the menu
            dlg.Color = gridColor;

            //This if statement is here so that when the user selects a color, the system know which color the user chose
            if (DialogResult.OK == dlg.ShowDialog())
            {
                //This will set the back color to whatever color the user chose
                gridColor = dlg.Color;
            }
            graphicsPanel1.Invalidate();
        }

        // Right cick grid x 10 (incomplete)
        private void gridX10ColorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Grid color
            //Creating a new color dialog
            ColorDialog dlg = new ColorDialog();
            dlg.ShowDialog();
            graphicsPanel1.Invalidate();
        }

        // Settings back color
        private void customizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Settings back color
            //Creating a new color dialog
            ColorDialog dlg = new ColorDialog();

            //This will show the back color when opening the menu
            dlg.Color = graphicsPanel1.BackColor;

            //This if statement is here so that when the user selects a color, the system know which color the user chose
            if (DialogResult.OK == dlg.ShowDialog())
            {
                //This will set the back color to whatever color the user chose
                graphicsPanel1.BackColor = dlg.Color;
            }

            // Repaint the universe
            graphicsPanel1.Invalidate();

            // Set the default setting color to the graphics
            Properties.Settings.Default.PanelColor = graphicsPanel1.BackColor;

            // Saving the color
            Properties.Settings.Default.Save();
        }

        // Settings cell color
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Settings cell color
            //Creating a new color dialog
            ColorDialog dlg = new ColorDialog();

            //This will show the cell color when opening the menu
            dlg.Color = cellColor;

            //This if statement is here so that when the user selects a color, the system know which color the user chose
            if (DialogResult.OK == dlg.ShowDialog())
            {
                //This will set the back color to whatever color the user chose
                cellColor = dlg.Color;
            }
            // repaint the window
            graphicsPanel1.Invalidate();
        }

        // Settings grid color
        private void gridColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Settings grid color
            //Creating a new color dialog
            ColorDialog dlg = new ColorDialog();

            //This will show the grid color when opening the menu
            dlg.Color = gridColor;

            //This if statement is here so that when the user selects a color, the system know which color the user chose
            if (DialogResult.OK == dlg.ShowDialog())
            {
                //This will set the back color to whatever color the user chose
                gridColor = dlg.Color;
            }
            graphicsPanel1.Invalidate();
        }

        // Settings grid x 10 color (incomplete)
        private void gridX10ColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Settings grid x10 color
            //Creating a new color dialog
            ColorDialog dlg = new ColorDialog();
            dlg.ShowDialog();
            graphicsPanel1.Invalidate();
        }

        //When the form is closed, this is so that the background color is saved
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Whatever the default color is in settings
            Properties.Settings.Default.PanelColor = graphicsPanel1.BackColor;
            Properties.Settings.Default.CellColor = cellColor;
            Properties.Settings.Default.GridColor = gridColor;

            //When the program closes, it saves the background color 
            Properties.Settings.Default.Save();
        }
        
        // Settings reset tool
        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Read settings
            graphicsPanel1.BackColor = Properties.Settings.Default.PanelColor;
            cellColor = Properties.Settings.Default.CellColor;
            gridColor = Properties.Settings.Default.GridColor;

            Properties.Settings.Default.Reset();
        }

        // Loading the form
        private void Form1_Load(object sender, EventArgs e)
        {
            // When opened, set things to default

            // Default backcolor
            graphicsPanel1.BackColor = Properties.Settings.Default.PanelColor;
            // Default cell color
            cellColor = Properties.Settings.Default.CellColor;
            // Default grid color
            gridColor = Properties.Settings.Default.GridColor;
            // Default seed, saved from last time
            seed = Properties.Settings.Default.Seed;
            // Default interval saved from last time
            interval = Properties.Settings.Default.Interval;

            // Update status strip interval
            toolStripStatusLabelInterval.Text = "Interval = " + interval.ToString();

            // Update status strip seed
            toolStripStatusLabelSeed.Text = "Seed = " + seed.ToString();
        }

        private void toolsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        // File, new settings code
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                //Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // Setting everything to false
                    universe[x, y] = false;
                    scratchPad[x, y] = false;
                }
            }

            // Set generations to 0
            generations = 0;

            // Repaint the universe
            graphicsPanel1.Invalidate();
        }

        // HUD neighbor display
        private void DisplayNeighbor(PaintEventArgs e, RectangleF nrect, int closeby)
        {
            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);

            Font font = new Font("Arial", 20f);

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            e.Graphics.DrawString(closeby.ToString(), font, cellBrush, nrect);
        }
        
        // Settings hud toggle for neighbor display
        private void neighborCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Toggle
            onOff = !onOff;

            // To see if it's checked or not
            neighborCountToolStripMenuItem.Checked = !neighborCountToolStripMenuItem.Checked;
            neighborCountToolStripMenuItem1.Checked = !neighborCountToolStripMenuItem1.Checked;

            // repaint the window
            graphicsPanel1.Invalidate();
        }
        
        // HUD Finite / HUD Torodial toggle (Not the button)
        private void ToggleFinTor()
        {
            // Toggle
            types = !types;

            // If statement to define which is which
            if(types == true)
            {
                finiteToolStripMenuItem.Checked = false;
                toroidalToolStripMenuItem.Checked = true;
            }
            else
            {
                toroidalToolStripMenuItem.Checked = false;
                finiteToolStripMenuItem.Checked = true;
            }
        }

        // Settings button for toggling for finite
        private void finiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleFinTor();
        }

        // Settings button for toggling for torodial
        private void toroidalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleFinTor();
        }

        // Settings HUD grid
        private void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // toggle grid 
            grid = !grid;

            // Show grid
            if (grid == true)
            {
                // If grid is true, the grid stays
                gridColor = Properties.Settings.Default.GridColor;
            }
            else
            {
                // If grid false, the grid bye bye
                gridColor = Color.Empty;
            }

            // To see the check mark
            gridToolStripMenuItem.Checked = !gridToolStripMenuItem.Checked;
            gridToolStripMenuItem1.Checked = !gridToolStripMenuItem1.Checked;

            // Repaint the window
            graphicsPanel1.Invalidate();
        }

        // Right click HUD display
        private void hUDToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //This is the HUD
            // True or false hud

            hud = !hud;

            // Check to see if it's checked or not
            hUDToolStripMenuItem.Checked = !hUDToolStripMenuItem.Checked;
            hUDToolStripMenuItem1.Checked = !hUDToolStripMenuItem1.Checked;

            // repaint the hud
            graphicsPanel1.Invalidate();
        }

        // Right click Neighbor display
        private void neighborCountToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Toggle for neighbor
            onOff = !onOff;

            // To see if it's checked or not
            neighborCountToolStripMenuItem1.Checked = !neighborCountToolStripMenuItem1.Checked;
            neighborCountToolStripMenuItem.Checked = !neighborCountToolStripMenuItem.Checked;

            // repaint the universe
            graphicsPanel1.Invalidate();
        }

        // Right click Grid display
        private void gridToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // toggle grid 
            grid = !grid;

            // Show grid
            if (grid == true)
            {
                // If grid is true, the grid stays
                gridColor = Properties.Settings.Default.GridColor;
            }
            else
            {
                // If grid false, the grid bye bye
                gridColor = Color.Empty;
            }

            // To see the check mark
            gridToolStripMenuItem.Checked = !gridToolStripMenuItem.Checked;
            gridToolStripMenuItem1.Checked = !gridToolStripMenuItem1.Checked;

            // Repaint the window
            graphicsPanel1.Invalidate();
        }

        // Randomize from time
        private void fromTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Two random for time and seed generation
            Random time = new Random(DateTime.Now.Millisecond);
            int store = time.Next(int.MinValue, int.MaxValue);
            Random seeds = new Random(store);

            //Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                //Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    seed = seeds.Next(0, 3);

                    if (seed == 0)
                    {
                        universe[x, y] = true;
                    }
                }
            }

            // repaint the window
            graphicsPanel1.Invalidate();
        }

        private void saveAs()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2; dlg.DefaultExt = "cells";


            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamWriter writer = new StreamWriter(dlg.FileName);

                // Write any comments you want to include first.
                // Prefix all comment strings with an exclamation point.
                // Use WriteLine to write the strings to the file. 
                // It appends a CRLF for you.
                writer.WriteLine("!This is my comment.");

                // Iterate through the universe one row at a time.
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    // Create a string to represent the current row.
                    String currentRow = string.Empty;

                    // Iterate through the current row one cell at a time.
                    for (int x = 0; x < universe.GetLength(0); x++)
                    {
                        // If the universe[x,y] is alive then append 'O' (capital O)
                        // to the row string.

                        // Else if the universe[x,y] is dead then append '.' (period)
                        // to the row string.
                    }

                    // Once the current row has been read through and the 
                    // string constructed then write it to the file using WriteLine.
                }

                // After all rows and columns have been written then close the file.
                writer.Close();
            }
        }

        private void Open()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamReader reader = new StreamReader(dlg.FileName);

                // Create a couple variables to calculate the width and height
                // of the data in the file.
                int maxWidth = 0;
                int maxHeight = 0;

                // Iterate through the file once to get its size.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then it is a comment
                    // and should be ignored.

                    // If the row is not a comment then it is a row of cells.
                    // Increment the maxHeight variable for each row read.

                    // Get the length of the current row string
                    // and adjust the maxWidth variable if necessary.
                }

                // Resize the current universe and scratchPad
                // to the width and height of the file calculated above.

                // Reset the file pointer back to the beginning of the file.
                reader.BaseStream.Seek(0, SeekOrigin.Begin);

                // Iterate through the file again, this time reading in the cells.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then
                    // it is a comment and should be ignored.

                    // If the row is not a comment then 
                    // it is a row of cells and needs to be iterated through.
                    for (int xPos = 0; xPos < row.Length; xPos++)
                    {
                        // If row[xPos] is a 'O' (capital O) then
                        // set the corresponding cell in the universe to alive.

                        // If row[xPos] is a '.' (period) then
                        // set the corresponding cell in the universe to dead.
                    }
                }

                // Close the file.
                reader.Close();
            }
        }
    }
}
