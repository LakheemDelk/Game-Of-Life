using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOLLakheemDelk
{
    public partial class Form1 : Form
    {


        // Counting the neighborstoroidal
        private int CountNeighborsToroidal(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);
            for (int yOffest = -1; yOffest <=1; yOffest++)
            {
                for (int xOffest = -1; xOffest <= 1; xOffest++)
                {
                    int xCheck = x + xOffest;
                    int yCheck = y + yOffest;

                    if (xOffest == 0 && yOffest == 0)
                    {
                        continue;
                    }
                        if (xCheck < 0)
                        {
                            xCheck = xLen - 1;
                        }
                        if (yCheck < 0)
                        {
                            yCheck = yLen - 1;
                        }
                        if (xCheck >= xLen)
                        {
                            xCheck = 0;
                        }
                        if (yCheck >= yLen)
                        {
                            yCheck = 0;
                        }

                    
                    if (universe[xCheck, yCheck] == true)
                    {
                        count++;
                    }


                }
            }
           


            return count;
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
                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }
                    if (xCheck < 0)
                    {
                        continue;
                    }
                    if (yCheck < 0) 
                    {
                        continue;
                    }
                    if (xCheck >= xLen)
                    {
                        continue;
                    }
                    if(yCheck >= yLen) 
                    {
                        continue;
                    }

                    if (universe[xCheck, yCheck] == true) count++;
                }
            }
            return count;
        }
        // The universe array
        bool[,] universe = new bool[5, 5];

        // Drawing colors
        Color gridColor = Color.Black;
        Color cellColor = Color.Gray;

        // The Timer class
        Timer timer = new Timer();
        

        // Generation count
        int generations = 0;

        public Form1()
        {
            InitializeComponent();
              
            // Setup the timer
            timer.Interval = 100; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false; // start timer running
        }

        // Calculate the next generation of cells
        private void NextGeneration()
        {

         

            // Increment generation count
            generations++;
            
            bool[,] scratch = new bool[5, 5];
            for (int x = 0; x < universe.GetLength(0); x++)
            {
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    int count = CountNeighborsToroidal(x, y);
                    

                    if (universe[x, y])
                    {
                        if (count == 2 || count == 3)
                        {
                            scratch[x, y] = true;
                        }
                        if (count < 2 || count > 3)
                        {
                            scratch[x, y] = false;
                        }
                    }
                    else
                    {
                        if (count == 3)
                        {
                            scratch[x, y] = true;
                        }
                    }
                }
            }
            universe = scratch;

            


            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            graphicsPanel1.Invalidate();
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
            int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

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
                    Rectangle cellRect = Rectangle.Empty;
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

                   

                    
                }
            }
           

            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
        }


        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
           
            
            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            { 
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
     

        private void Form1_Load(object sender, EventArgs e)
        {
         
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Stop();
            timer.Enabled = false;
            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x, y] = false;
                }
                graphicsPanel1.Invalidate();
            }
            generations = generations - generations;
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
        }
     


    private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Start();
            timer.Enabled = true;
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Stop();
            timer.Enabled = false;
        }

        private void nextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Stop();
            timer.Enabled = false;
            NextGeneration();
        }
    }
}
