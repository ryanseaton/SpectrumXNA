using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace SpectrumXNA
{
    class GameRoom
    {
        public int contentObject;
        public int wall_up;
        public int wall_down;
        public int wall_left;
        public int wall_right;
        public int wall_floor;

        public GameRoom()
        {
            contentObject = 0;
            wall_up = 1;
            wall_down = 1;
            wall_left = 1;
            wall_right = 1;
            wall_floor = 1;
        }

        public GameRoom(int contents, int up, int down, int left, int right, int floor)
        {
            contentObject = contents;
            wall_up = up;
            wall_down = down;
            wall_left = left;
            wall_right = right;
            wall_floor = floor;
        }
    }
}
