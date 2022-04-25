﻿using System;
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

        // The seed
        static int seed;

       // public int Seed
       // {
       //     get {return seed; }
       //     set {seed = ; }
       // }

        // The cell
        public int cell;


        // HUD

        // Int num Cells
        int Acells = 0;
        //Boudary type
        bool idial = false;
        // Width
        int width = 0;
        // Height
        int height = 0;
        // Display
        bool hud = true;

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
            if(idial == false)
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

        public Form1()
        {
            InitializeComponent();

            //Read settings
            Properties.Settings.Default.PanelColor = graphicsPanel1.BackColor;

            // Setup the timer
            timer.Interval = 100; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false; // start timer running
        }

        // Calculate the next generation of cells
        private void NextGeneration()
        {
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                //Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    //Apply the rules
                    //Living cells with less than 2 living neighbors die in the next generation.
                    if (CountNeighborsFinite(x, y) < 2)
                    {
                        scratchPad[x,y] = false;
                    }

                    if(CountNeighborsFinite(x, y) > 3)
                    {
                        scratchPad[x, y] = false;
                    }

                    if(CountNeighborsFinite(x, y) == 2 || CountNeighborsFinite(x, y) == 3)
                    {
                        scratchPad[x, y] = true;
                    }

                    if(universe[x, y] == false && CountNeighborsFinite(x, y) == 3)
                    {
                        scratchPad[x, y] = true;
                    }
                     //Turn in on/off the scratchPad
                }
            }
            // Copy from scratchpad to universe

            bool[,] temp = universe;
            universe = scratchPad;
            scratchPad = temp;
            graphicsPanel1.Invalidate();

            // Increment generation count
            generations++;

            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
        }

        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            float cellWidth = (float)graphicsPanel1.ClientSize.Width / universe.GetLength(0);
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            float cellHeight = (float)graphicsPanel1.ClientSize.Height / universe.GetLength(1);

            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 1);

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);

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
                    }

                    // Outline the cell with a pen
                    e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);

                    Font font = new Font("Arial", 20f);

                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;

                    int neighbors = CountNeighborsFinite(x, y);

                   if(CountNeighborsFinite(x,y) == 0)
                   {
                      
                   }
                   else
                   {
                       e.Graphics.DrawString(neighbors.ToString(), font, Brushes.Black, cellRect, stringFormat);
                   }
                }
            }

            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
        }

        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
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
                        xLen = -1;
                    }
                    // if yCheck is less than 0 then set to yLen - 1
                    if (yCheck < 0)
                    {
                        yLen = -1;
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
        }

        //Pause button
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
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
                    universe[x, y] = false;
                    scratchPad[x, y] = false; 
                }
            }

            //This is to recolor the universe
            graphicsPanel1.Invalidate();

        }

        //Exit button
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void randomizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripStatusLabelGenerations_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //This is the Form universe from Seed
            //Created a new form, and to access the form, created an instance of it to show it.
            Randomize ran = new Randomize();

            //The code for the new universe

            if(DialogResult.OK == ran.ShowDialog())
            {
                seed = ran.Mnum;
                Random Rseed = new Random(seed);
                //Iterate through the universe in the y, top to bottom
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    //Iterate through the universe in the x, left to right
                    for (int x = 0; x < universe.GetLength(0); x++)
                    {
                        seed = Rseed.Next(0, 3);

                        if (seed == 0)
                        {
                            universe[x, y] = true;
                        }
                    }
                }
            }
            //This is to recolor the universe
            graphicsPanel1.Invalidate();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void optionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Created a new form, and to access the form, created an instance of it to show it.
            Options op = new Options();
            op.ShowDialog();
        }

        private void hUDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //This is the HUD
            // True or false hud

            hud = !hud;
            hUDToolStripMenuItem.Checked = !hUDToolStripMenuItem.Checked;
            graphicsPanel1.Invalidate();

        }

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

            
        }

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
        }

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
        }

        private void gridX10ColorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Grid color
            //Creating a new color dialog
            ColorDialog dlg = new ColorDialog();
            dlg.ShowDialog();
        }

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
        }

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
        }

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
        }

        private void gridX10ColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Settings grid x10 color
            //Creating a new color dialog
            ColorDialog dlg = new ColorDialog();
            dlg.ShowDialog();
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
        
        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();

            //Read settings
            graphicsPanel1.BackColor = Properties.Settings.Default.PanelColor;
            cellColor = Properties.Settings.Default.CellColor;
            gridColor = Properties.Settings.Default.GridColor;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
       
        }

        // SECOND GRAPHICS PANEL

        private void graphicsPanel1_Paint_1(object sender, PaintEventArgs e)
        {
            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            float cellWidth = (float)graphicsPanel1.ClientSize.Width / universe.GetLength(0);
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            float cellHeight = (float)graphicsPanel1.ClientSize.Height / universe.GetLength(1);

            //Adding int for the count
            int count;
       
            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 1);
       
            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);

            Acells = 0;
       
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
                    }

                    // Check if the universe is toridial
                    if(idial = false)
                    {
                        count = CountNeightborsToroidal(x, y);
                    }
                    else
                    {
                        count = CountNeighborsFinite(x, y);
                    }
       
                    // Outline the cell with a pen
                    e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
       
                    Font font = new Font("Arial", 20f);
       
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
       
                    int neighbors = CountNeighborsFinite(x, y);
       
                    if (CountNeighborsFinite(x, y) == 0)
                    {
       
                    }
                    else
                    {
                        e.Graphics.DrawString(neighbors.ToString(), font, Brushes.Black, cellRect, stringFormat);
                    }

                    // On/Off HUD
                    if(hud == true)
                    {
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
    }
}
