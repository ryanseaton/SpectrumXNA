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
    class GameColour
    {
        public int colourId;
        public bool colourEnabled;
        public Model colourModel;
        public Color colourBackground;

        public GameColour(int id)
        {
            this.colourId = id;
            this.colourEnabled = false;
        }

        public void initialize(bool usable, Model model, Color background) {
            this.colourEnabled = usable;
            this.colourModel = model;
            this.colourBackground = background;
        }

    }
}
