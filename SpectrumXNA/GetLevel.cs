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

    /** 
     * purpose of this class is to hide my ugly static codes
     * 
     * ALL STUFF HERE ARE MANUALLY GENERATED!!!
     */
    class GetLevel
    {
        public static GameRoom[][][] getLevel(int level)
        {
            int [][][] roomGrid;
            GameBarrier[] levelBarriers;

            switch (level)
            {
                case 1:
                    roomGrid = LevelInfo.getLevel1();
                    levelBarriers = LevelInfo.getBarrier1();
                    break;
                case 2:
                    roomGrid = LevelInfo.getLevel2();
                    levelBarriers = LevelInfo.getBarrier2();
                    break;
                case 3:
                    levelBarriers = LevelInfo.getBarrier3();
                    roomGrid = LevelInfo.getLevel3();
                    break;
                case 4:
                    roomGrid = LevelInfo.getLevel4();
                    levelBarriers = LevelInfo.getBarrier4();
                    break;
                case 5:
                    roomGrid = LevelInfo.getLevel6();
                    levelBarriers = LevelInfo.getBarrier6();
                    break;
                case 6:
                    roomGrid = LevelInfo.getLevel7();
                    levelBarriers = LevelInfo.getBarrier7();
                    break;
                case 7:
                    roomGrid = LevelInfo.getLevel8();
                    levelBarriers = LevelInfo.getBarrier8();
                    break;
                default:
                    roomGrid = LevelInfo.getLevel1();
                    levelBarriers = LevelInfo.getBarrier1();
                    break;
            }
            GameRoom [][][] state = new GameRoom[roomGrid.Length][][];

            for (int i = 0; i < roomGrid.Length; i++)
            {
                state[i] = new GameRoom[roomGrid[i].Length][];
                for (int j = 0; j < roomGrid[i].Length; j++)
                {
                    state[i][j] = new GameRoom[roomGrid[i][j].Length];
                    for (int k = 0; k < roomGrid[i][j].Length; k++)
                        state[i][j][k] = new GameRoom(roomGrid[i][j][k],
                            GameConstants.COLOUR_NONE, GameConstants.COLOUR_NONE,
                            GameConstants.COLOUR_NONE, GameConstants.COLOUR_NONE,
                            GameConstants.COLOUR_NONE);
                }
            }

            for (int i = 0; i < levelBarriers.Length; i++)
            {
                if (levelBarriers[i].barrier_wall == GameConstants.WALL_UP)
                    state[levelBarriers[i].barrier_Y][levelBarriers[i].barrier_Z][levelBarriers[i].barrier_X].wall_up = levelBarriers[i].barrier_colour;

                if (levelBarriers[i].barrier_wall == GameConstants.WALL_LEFT)
                    state[levelBarriers[i].barrier_Y][levelBarriers[i].barrier_Z][levelBarriers[i].barrier_X].wall_left = levelBarriers[i].barrier_colour;

                if (levelBarriers[i].barrier_wall == GameConstants.WALL_RIGHT)
                    state[levelBarriers[i].barrier_Y][levelBarriers[i].barrier_Z][levelBarriers[i].barrier_X].wall_right = levelBarriers[i].barrier_colour;

                if (levelBarriers[i].barrier_wall == GameConstants.WALL_DOWN)
                    state[levelBarriers[i].barrier_Y][levelBarriers[i].barrier_Z][levelBarriers[i].barrier_X].wall_down = levelBarriers[i].barrier_colour;

                if (levelBarriers[i].barrier_wall == GameConstants.WALL_FLOOR)
                    state[levelBarriers[i].barrier_Y][levelBarriers[i].barrier_Z][levelBarriers[i].barrier_X].wall_floor = levelBarriers[i].barrier_colour;
            }

            return state;
        }

        public static String getTips(int level)
        {
            String tips = "";

            switch (level)
            {
                case 1:
                    tips = "Move with the left stick.\r\nChange hues with X, Y, and B.\r\nA is black, not green.\r\n\r\nChange colours often.\r\nYou can move the red block by\r\nbeing red yourself.";
                    break;
                case 2:
                    tips = "Jump with the trigger.\r\nLift with the shoulders.  You can't\r\nwalk with your hands full!";
                    break;
                case 3:
                    tips = "You can only move\r\nsame or adjacent colours.\r\nSee the colour wheel?";
                    break;
                case 4:
                    tips = "You are so strong that\r\nyou can push two stacked boxes!\r\nThree is too many.";
                    break;
                case 5:
                    tips = "The transparent floor\r\nlets adjacent colours through.\r\n(Red, orange, yellow.)";
                    break;
                case 6:
                    tips = "Stacked blocks of two hues:\r\nMatch both colours to push them.\r\nSTART lets you reset.";
                    break;
                case 7:
                    tips = "Walls can also be\r\ntransparent.  This level is\r\nnot an easy one.";
                    break;
                default:
                    break;
            }
            return tips;
        }

        public static int totalLevels()
        {
            return 7;
        }
    }
}
