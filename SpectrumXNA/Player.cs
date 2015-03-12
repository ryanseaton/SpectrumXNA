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
    class Player:ModelExtend
    {
        // sry for hackish... but
        // 1 = black
        // 2 = red
        // 3 = orange.. so on
        // 7 = purple.
        // ROYGBV (v = purple in this case)
        // Player can only be 1, 2, 4 or 6.

        public GameColour spriteColour;

        public Player(float x, float y, float z, Model type, float r)
        {
            position = new Vector3(x, y, z);
            model = type;
            rotation = r;
        }
    }
}
