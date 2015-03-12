using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpectrumXNA
{
    public class GameConstants
    {
        public const int MAX_KEYS = 10;
        public const int KEY_DOWN = 0;
        public const int KEY_UP = 1;
        public const int KEY_LEFT = 2;
        public const int KEY_RIGHT = 3;
        public const int KEY_CLIMB = 4;
        public const int KEY_LIFT = 5;
        public const int KEY_EXIT = 6;
        public const int KEY_CHANGE_FORWARDS = 7;
        public const int KEY_CHANGE_BACKWARDS = 8;
        public const int KEY_SKIP = 9;

        public const int MAX_BUTTONS = 11;
        public const int BUTTON_DOWN = 0;
        public const int BUTTON_UP = 1;
        public const int BUTTON_LEFT = 2;
        public const int BUTTON_RIGHT = 3;
        public const int BUTTON_CLIMB = 4;
        public const int BUTTON_LIFT = 5;
        public const int BUTTON_EXIT = 6;
        public const int BUTTON_CHANGE_BLUE = 7;
        public const int BUTTON_CHANGE_YELLOW = 8;
        public const int BUTTON_CHANGE_RED = 9;
        public const int BUTTON_CHANGE_BLACK = 10;


        public const int ACTION_CLIMB = 0;
        public const int ACTION_LIFT = 1;

        public const int COLOUR_RED = 2;
        public const int COLOUR_ORANGE = 3;
        public const int COLOUR_YELLOW = 4;
        public const int COLOUR_GREEN = 5;
        public const int COLOUR_BLUE = 6;
        public const int COLOUR_PURPLE = 7;
        public const int COLOUR_BLACK = 1;
        public const int COLOUR_GREY = 0;
        public const int COLOUR_NONE = -3;

        public const int MENU_NEW_GAME = 0;
        public const int MENU_CONTINUE = 1;
        public const int MENU_SETTINGS = 2;
        public const int MENU_EXIT = 3;
        public const int MENU_SELECT = 4;
        public const int MENU_NUM_OPTIONS = 4;

        public const int MENU_SETTINGS_SOUND = 0;
        public const int MENU_SETTINGS_TIPS = 1;
        public const int MENU_SETTINGS_ON = 2;
        public const int MENU_SETTINGS_OFF = 3;
        public const int MENU_SETTINGS_NUM_OPTIONS = 3; // includes exit

        public const int DISPLAY_MENU = 0;
        public const int DISPLAY_GAME = 1;
        public const int DISPLAY_END = 2; // finish level
        public const int DISPLAY_COMPLETE = 3; // finish game
        public const int DISPLAY_SETTINGS = 4;

        public const int WALL_UP = 0;
        public const int WALL_LEFT = 1;
        public const int WALL_RIGHT = 2;
        public const int WALL_DOWN = 3;
        public const int WALL_FLOOR = 4;

        private GameConstants()
        {
        }
    }
}
