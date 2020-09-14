using System;
using System.Drawing;
using System.Windows.Forms;

namespace LightsOut
{
    public partial class MainForm : Form
    {
        private const int GridOffset = 25;
        private const int GridLength = 200;

        private LightsOutGame lightsOutGame;

        public MainForm()
        {
            InitializeComponent();

            lightsOutGame = new LightsOutGame();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            int CellLength = GridLength / lightsOutGame.GridSize;

            Graphics g = e.Graphics;

            for (int r = 0; r < lightsOutGame.GridSize; r++)
            {
                for (int c = 0; c < lightsOutGame.GridSize; c++)
                {
                    // Get proper pen and brush for on/off
                    // grid selection
                    Brush brush;
                    Pen pen;

                    if (lightsOutGame.GetGridValue(r, c))
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
            int CellLength = GridLength / lightsOutGame.GridSize;
            int NumCells = lightsOutGame.GridSize;

            // Make sure click was inside the grid
            if (e.X < GridOffset || e.X > CellLength * NumCells + GridOffset ||
                e.Y < GridOffset || e.Y > CellLength * NumCells + GridOffset)
                return;

            // Find row, col of mouse press
            int r = (e.Y - GridOffset) / CellLength;
            int c = (e.X - GridOffset) / CellLength;

            lightsOutGame.Move(r, c);

            // Redraw grid
            this.Invalidate();

            // Cehck to see if puzzle has been solved
            if (lightsOutGame.IsGameOver())
            {
                // Display winner dialog box
                MessageBox.Show(this, "Congratulations! You've won!", "Lights Out!", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void newGameButton_Click(object sender, EventArgs e)
        {
            lightsOutGame.NewGame();

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
