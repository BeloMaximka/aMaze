using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Maze
{
    static class GameObjectTypes
    {
        public enum Types
        {
            HALL,
            WALL,
            MEDAL,
            ENEMY,
            PLAYER,
            HEAL,
            EXIT
        };
        public static Bitmap[] Textures { get; private set; } =
        {
            GameResources.hall,
            GameResources.wall,
            GameResources.medal,
            GameResources.enemy,
            GameResources.player,
            GameResources.heal,
            GameResources.exit,
        };
    }
}
