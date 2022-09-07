using System.Drawing;
using System.Windows.Forms;

namespace Maze
{
    public partial class Form1 : Form
    {
        Labirint lab;
        public Form1()
        {
            InitializeComponent();
            Options();
            StartGame();
        }

        public void Options()
        {
            Text = "Maze";
            stepsLabel.BackColor = BackColor;
            BackColor = Color.FromArgb(255, 92, 118, 137);

            int sizeX = 40;
            int sizeY = 20;

            Width = sizeX * 16 + 16;
            Height = sizeY * 16 + 40 + toolStrip1.Height;
            StartPosition = FormStartPosition.CenterScreen;
        }

        public void StartGame()
        {
            lab = new Labirint(this, 40, 20, toolStrip1.Height);
            lab.PlayerStep += UpdateStepCounter;
            lab.Show();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    lab.Character.X--;
                    break;
                case Keys.Right:
                    lab.Character.X++;
                    break;
                case Keys.Up:
                    lab.Character.Y--;
                    break;
                case Keys.Down:
                    lab.Character.Y++;
                    break;
            }
        }

        void UpdateStepCounter(int steps)
        {
            stepsLabel.Text = $"Шаги: {steps}";
        }
    }
}
