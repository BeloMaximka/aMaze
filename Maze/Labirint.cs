using System;
using System.Windows.Forms;
using System.Drawing;

namespace Maze
{
    class Labirint
    {
        Form parent;
        static Random r = new Random();

        const int tileSize = 16;
        PictureBox[,] maze;

        public Player Character { get; private set; }
        public int maxMedals;
        public int medalCount;

        public int Height { get; private set; } // высота лабиринта (количество строк)
        public int Width { get; private set; } // ширина лабиринта (количество столбцов в каждой строке)

        public Labirint(Form parent, int width, int height, int offsetY)
        {
            Width = width;
            Height = height;
            this.parent = parent;

            maze = new PictureBox[height, width];
            Character = new Player(0, 2, this);
            Character.MedalPickup += OnMedalPickup;
            Character.Step += OnPlayerStep;
            Character.HealthChange += OnHealthChanged;
            Character.ExitFound += OnExitFound;
            medalCount = 0;
            maxMedals = 0;

            Generate(offsetY);
        }

        public event Action<int> PlayerStep;
        public event Action<int> HealthChanged;
        public event Action<int> MedalPickup;
        public event Action<string> GameEnd;

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

                    // в 1 случае из 150 - кладём денежку
                    if (r.Next(150) == 0)
                    {
                        id = GameObjectTypes.Types.MEDAL;
                        maxMedals++;
                    }

                    // в 1 случае из 150 - кладём аптечку
                    if (r.Next(150) == 0)
                        id = GameObjectTypes.Types.HEAL;

                    // в 1 случае из 150 - размещаем врага
                    if (r.Next(150) == 0)
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
        protected virtual void OnMedalPickup()
        {
            medalCount++;
            MedalPickup(medalCount);
            if (medalCount == maxMedals)
                GameEnd("Победа - медали собраны!");
        }
        protected virtual void OnPlayerStep(int steps)
        {
            PlayerStep(steps);
        }
        protected virtual void OnHealthChanged(int health)
        {
            HealthChanged(health);
            if (health == 0)
                GameEnd("Поражение - закончилось здоровье.");
        }
        protected virtual void OnExitFound()
        {
            GameEnd("Победа - найден выход!");
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
