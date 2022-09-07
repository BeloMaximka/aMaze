using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Maze
{
    internal class Player
    {
        static Random random = new Random();
        public event Action<int> Step;
        public event Action<int> HealthChange;
        public event Action MedalPickup;
        public event Action ExitFound;
        Point pos;
        Labirint lab;

        int steps;
        public int Steps
        {
            get { return steps; }
            set { steps = value; }
        }
        int health = 100;
        public int Health
        {
            get { return health; }
            set
            {
                if (value < 0)
                    health = 0;
                else if (value > 100)
                    health = 100;
                else
                    health = value;
                HealthChange(health);
            }
        }
        public int X
        {
            get { return pos.X; }
            set
            {
                Point oldPos = pos;
                if (value < 0)
                    pos.X = 0;
                else if (value >= lab.Width)
                    pos.X = lab.Width - 1;
                else
                    pos.X = value;
                UpdatePlayerPos(oldPos);
            }
        }
        public int Y
        {
            get { return pos.Y; }
            set
            {
                Point oldPos = pos;
                if (value < 0)
                    pos.Y = 0;
                else if (value >= lab.Width)
                    pos.Y = lab.Width - 1;
                else
                    pos.Y = value;
                UpdatePlayerPos(oldPos);
            }
        }

        public Player(int x, int y, Labirint labirint)
        {
            pos.X = x;
            pos.Y = y;
            lab = labirint;
        }

        void UpdatePlayerPos(Point oldPos)
        {
            if (pos == oldPos) return;
            GameObjectTypes.Types tile = lab[pos];

            bool playerMoved = true;
            switch (tile)
            {
                case GameObjectTypes.Types.HALL:
                    lab[oldPos] = GameObjectTypes.Types.HALL;
                    lab[pos] = GameObjectTypes.Types.PLAYER;
                    break;
                case GameObjectTypes.Types.MEDAL:
                    lab[oldPos] = GameObjectTypes.Types.HALL;
                    lab[pos] = GameObjectTypes.Types.PLAYER;
                    MedalPickup.Invoke();
                    break;
                case GameObjectTypes.Types.HEAL:
                    if (Health != 100)
                    {
                        lab[oldPos] = GameObjectTypes.Types.HALL;
                        lab[pos] = GameObjectTypes.Types.PLAYER;
                        Health += 5;
                    }
                    else
                        playerMoved = false;
                    break;
                case GameObjectTypes.Types.ENEMY:
                    lab[oldPos] = GameObjectTypes.Types.HALL;
                    lab[pos] = GameObjectTypes.Types.PLAYER;
                    Health -= random.Next(20, 26);
                    break;
                case GameObjectTypes.Types.EXIT:
                    lab[oldPos] = GameObjectTypes.Types.HALL;
                    lab[pos] = GameObjectTypes.Types.PLAYER;
                    ExitFound.Invoke();
                    break;
                default:
                    playerMoved = false;
                    break;
            }
            if (playerMoved)
            {
                Steps++;
                Step(steps);
            }
            else
                pos = oldPos;
        }
    }
}
