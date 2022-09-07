using System;
using System.Windows.Forms;
using System.Drawing;

namespace Maze
{
    class Labirint
    {
        const int tileSize = 16;

        PictureBox[,] maze;
        public Player Character { get; private set; }

        static Random r = new Random();
        Form parent;

        public int Height { get; private set; } // высота лабиринта (количество строк)
        public int Width { get; private set; } // ширина лабиринта (количество столбцов в каждой строке)

        public Labirint(Form parent, int width, int height, int offsetY)
        {
            Width = width;
            Height = height;
            this.parent = parent;

            maze = new PictureBox[height, width];
            Character = new Player(0, 2, this);

            Generate(offsetY);
        }

        public event PlayerStepHandler PlayerStep
        {
            add { Character.PlayerStep += value; }
            remove { Character.PlayerStep -= value; }
        }
        public GameObjectTypes.Types this[Point pos]
        {
            get { return (GameObjectTypes.Types)maze[pos.Y, pos.X].Tag; }
            set
            {
                maze[pos.Y, pos.X].Tag = value;
                maze[pos.Y, pos.X].BackgroundImage = GameObjectTypes.Textures[(int)value];
            }
        }
        private void Generate(int offsetY)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    GameObjectTypes.Types id = GameObjectTypes.Types.HALL;

                    // в 1 случае из 5 - ставим стену
                    if (r.Next(5) == 0)
                        id = GameObjectTypes.Types.WALL;

                    // в 1 случае из 250 - кладём денежку
                    if (r.Next(250) == 0)
                        id = GameObjectTypes.Types.MEDAL;

                    // в 1 случае из 250 - размещаем врага
                    if (r.Next(250) == 0)
                        id = GameObjectTypes.Types.ENEMY;

                    // стены по периметру обязательны
                    if (y == 0 || x == 0 || y == Height - 1 | x == Width - 1)
                        id = GameObjectTypes.Types.WALL;

                    // наш персонажик
                    if (x == Character.X && y == Character.Y)
                        id = GameObjectTypes.Types.PLAYER;

                    // соседняя ячейка справа всегда свободна
                    if (x == Character.X + 1 && y == Character.Y)
                        id = GameObjectTypes.Types.HALL;

                    // выход
                    if (x == Width - 1 && y == Height - 3)
                        id = GameObjectTypes.Types.EXIT;

                    maze[y, x] = new PictureBox();
                    maze[y, x].Location = new Point(x * tileSize, y * tileSize + offsetY);
                    maze[y, x].Parent = parent;
                    maze[y, x].Width = tileSize;
                    maze[y, x].Height = tileSize;
                    maze[y, x].BackgroundImage = GameObjectTypes.Textures[(int)id];
                    maze[y, x].Visible = false;
                    maze[y, x].Tag = id;
                }
            }
        }

        public void Show()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    maze[y, x].Visible = true;
                }
            }
        }
    }
}
