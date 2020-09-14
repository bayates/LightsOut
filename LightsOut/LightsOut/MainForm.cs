using System;
using System.Drawing;
using System.Windows.Forms;

namespace LightsOut
{
    public partial class MainForm : Form
    {
        private const int GridOffset = 25;
        private const int GridLength = 200;
        private const int NumCells = 3;
        private const int CellLength = GridLength / NumCells;

        private bool[,] grid;
        private Random rand;

        public MainForm()
        {
            InitializeComponent();

            rand = new Random();    // Initializes random number generator

            grid = new bool[NumCells, NumCells];

            // Turn entire grid on
            for (int r = 0; r < NumCells; r++)
                for (int c = 0; c < NumCells; c++)
                    grid[r, c] = true;
        }

        private bool PlayerWon()
        {
            // Return false if any cell is on, true otherwise
            for (int r = 0; r < NumCells; r++)
                for (int c = 0; c < NumCells; c++)
                    if (grid[r, c])
                        return false;

            return true;
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            for (int r = 0; r < NumCells; r++)
            {
                for (int c = 0; c < NumCells; c++)
                {
                    // Get proper pen and brush for on/off
                    // grid selection
                    Brush brush;
                    Pen pen;

                    if (grid[r, c])
                    {
                        pen = Pens.Black;
                        brush = Brushes.White;  // On
                    }
                    else
                    {
                        pen = Pens.White;
                        brush = Brushes.Black;  // Off
                    }

                    // Determine (x,y) coord of row and col to draw rectangle
                    int x = c * CellLength + GridOffset;
                    int y = r * CellLength + GridOffset;

                    // Draw outline and inner rectangle
                    g.DrawRectangle(pen, x, y, CellLength, CellLength);
                    g.FillRectangle(brush, x + 1, y + 1, CellLength - 1, CellLength - 1);
                }
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            // Make sure click was inside the grid
            if (e.X < GridOffset || e.X > CellLength * NumCells + GridOffset ||
                e.Y < GridOffset || e.Y > CellLength * NumCells + GridOffset)
                return;

            // Find row, col of mouse press
            int r = (e.Y - GridOffset) / CellLength;
            int c = (e.X - GridOffset) / CellLength;

            // Invert selected box and all surrounding boxes
            for (int i = r - 1; i <= r + 1; i++)
                for (int j = c - 1; j <= c + 1; j++)
                    if (i >= 0 && i < NumCells && j >= 0 && j < NumCells)
                        grid[i, j] = !grid[i, j];

            // Redraw grid
            this.Invalidate();

            // Cehck to see if puzzle has been solved
            if (PlayerWon())
            {
                // Display winner dialog box
                MessageBox.Show(this, "Congratulations! You've won!", "Lights Out!", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void newGameButton_Click(object sender, EventArgs e)
        {
            // Fill grid with either white or black
            for (int r = 0; r < NumCells; r++)
                for (int c = 0; c < NumCells; c++)
                    grid[r, c] = rand.Next(2) == 1;

            // Redraw grid
            this.Invalidate();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutBox = new AboutForm();
            aboutBox.ShowDialog(this);
        }
    }
}
