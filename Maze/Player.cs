using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Maze
{
    public delegate void PlayerStepHandler(int steps);
    internal class Player
    {
        public event PlayerStepHandler PlayerStep;
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
                if (pos != oldPos)
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
                if (pos != oldPos)
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
                default:
                    pos = oldPos;
                    playerMoved = false;
                    break;
            }
            if (playerMoved)
            {
                Steps++;
                PlayerStep(steps);
            }
        }
    }
}
