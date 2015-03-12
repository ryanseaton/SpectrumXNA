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
    class GameBarrier
    {
        public int barrier_X;
        public int barrier_Y;
        public int barrier_Z;
        public int barrier_colour;
        public int barrier_wall;

        public GameBarrier(int x, int y, int z, int wall, int col)
        {
            this.barrier_colour = col;
            this.barrier_X = x;
            this.barrier_Y = y;
            this.barrier_Z = z;
            this.barrier_wall = wall;
        }
    }
}
