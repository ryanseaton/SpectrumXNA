using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpectrumXNA
{

    class Settings
    {
        // If Bloom is on, antialias is automatically off.
        // Both off to have no effects.
        // Bloom on also means no transparency.
        public bool BLOOM_ON = true;
        public bool ANTIALIAS_ON = true;

        public bool MAXIMIZE_SCREEN = true;
        public bool FULLSCREEN_ON = true;

        public bool Show_Tips = true;
        public bool Sound_On = true;

        public Settings()
        {
            // nothing
        }
    }
}
