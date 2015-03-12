using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpectrumXNA
{
    /**
     * -2 = end goal
     * -1 = where you start
     *  0 = empty square
     *  1 = grey/unmovable square
     *  2 = red
     *  3 = orange
     *  4 = yellow
     *  5 = green
     *  6 = blue
     *  7 = purple/violet
     *  > 0 -> solid object
     * <= 0 -> walkthough-ables (though, currently they just get removed during initialization of the level
     */
    class LevelInfo
    {
        public static int[][][] getLevel1 ()
        {
            int[][] ceiling  = {   new int[] {0,0,0,0},
                                   new int[] {0,0,0,0},
                                   new int[] {0,0,0,0},
                                   new int[] {0,0,0,0},
                                   new int[] {0,0,0,0}};

            int[][] secondFloor = {new int[] {0,0,0,0},
                                   new int[] {0,0,0,0},
                                   new int[] {0,0,0,0},
                                   new int[] {0,0,0,0},
                                   new int[] {0,0,0,0}};
            int[][] firstFloor = new int[5][]
                                 { new int[] {0,0,-2,0},
                                   new int[] {0,0,0,0},
                                   new int[] {1,0,1,1},
                                   new int[] {0,2,0,0},
                                   new int[] {0,0,0,-1}};

            int[][] basement = {   new int[] {1,1,1,1},
                                   new int[] {1,1,1,1},
                                   new int[] {1,1,1,1},
                                   new int[] {1,1,1,1},
                                   new int[] {1,1,1,1}};

            int[][][] toRet = new int[][][] { basement, firstFloor, secondFloor, ceiling};

            return toRet;
        } 

        public static int[][][] getLevel2()
        {
            int[][] ceiling = {   new int[] {0,0,0,0},
                                  new int[] {0,0,0,0},
                                  new int[] {0,0,0,0},
                                  new int[] {0,0,0,0},
                                  new int[] {0,0,0,0},
                                  new int[] {0,0,0,0}};
            
            int[][] firstFloor = new int[6][]
                                 { new int[] {1,0,0,1},
                                   new int[] {0,2,0,0},
                                   new int[] {0,0,2,0},
                                   new int[] {1,0,0,2},
                                   new int[] {0,0,0,0},
                                   new int[] {0,0,0,-1}};

            int[][] secondFloor = new int[6][]
                                 { new int[] {1,0,0,1},
                                   new int[] {0,0,0,0},
                                   new int[] {0,0,0,0},
                                   new int[] {1,0,0,0},
                                   new int[] {0,0,0,0},
                                   new int[] {0,0,0,0}};

            int[][] thirdFloor = new int[6][]
                                 { new int[] {1,1,1,1},
                                   new int[] {1,0,0,0},
                                   new int[] {1,0,0,0},
                                   new int[] {1,0,0,0},
                                   new int[] {0,0,0,0},
                                   new int[] {0,0,0,0}};

            int[][] fourthFloor = new int[6][]
                                 { new int[] {0,0,0,0},
                                   new int[] {0,0,0,0},
                                   new int[] {0,0,0,0},
                                   new int[] {-2,0,0,0},
                                   new int[] {0,0,0,0},
                                   new int[] {0,0,0,0}};

            int[][] basement = {   new int[] {1,1,1,1},
                                   new int[] {1,1,1,1},
                                   new int[] {1,1,1,1},
                                   new int[] {1,1,1,1},
                                   new int[] {1,1,1,1},
                                   new int[] {1,1,1,1}};

            int[][][] toRet = new int[][][] { basement, firstFloor, secondFloor, thirdFloor, fourthFloor, ceiling };

            return toRet;
        }

        public static int[][][] getLevel3()
        {
            int[][] ceiling = {   new int[] {0,0,0,0},
                                  new int[] {0,0,0,0},
                                  new int[] {0,0,0,0},
                                  new int[] {0,0,0,0},
                                  new int[] {0,0,0,0},
                                  new int[] {0,0,0,0}};

            int[][] firstFloor = new int[4][]
                                { new int[] {1,1,1,1},
                                  new int[] {1,1,0,0},
                                  new int[] {1,0,3,0},
                                  new int[] {0,0,0,-1}};

            int[][] secondFloor = new int[4][]
                                { new int[] {1,1,1,0},
                                  new int[] {1,1,0,0},
                                  new int[] {1,0,0,0},
                                  new int[] {0,0,0,0}};

            int[][] thirdFloor = new int[4][]
                                { new int[] {1,1,1,-2},
                                  new int[] {1,0,0,0},
                                  new int[] {1,0,0,0},
                                  new int[] {0,0,0,0}};

            int[][] fourthFloor = new int[4][]
                                { new int[] {0,1,1,0},
                                  new int[] {0,0,0,0},
                                  new int[] {0,0,0,0},
                                  new int[] {0,0,0,0}};

            int[][] fifthFloor = new int[4][]
                                { new int[] {0,0,5,0},
                                  new int[] {0,0,0,0},
                                  new int[] {0,0,0,0},
                                  new int[] {0,0,0,0}};

            int[][] sixthFloor = {new int[] {0,0,0,0},
                                  new int[] {0,0,0,0},
                                  new int[] {0,0,0,0},
                                  new int[] {0,0,0,0},
                                  new int[] {0,0,0,0},
                                  new int[] {0,0,0,0}};

            int[][] basement = {  new int[] {1,1,1,1},
                                  new int[] {1,1,1,1},
                                  new int[] {1,1,1,1},
                                  new int[] {1,1,1,1}};

            int[][][] toRet = new int[][][] { basement, firstFloor, secondFloor,
                                              thirdFloor, fourthFloor, fifthFloor, 
                                              sixthFloor, ceiling };

            return toRet;
        }

        public static int[][][] getLevel4()
        {
            int[][] ceiling = {   new int[] {0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0}};

            int[][] firstFloor = new int[5][]
                                { new int[] {1,0,0,0,0,1,1,1,1},
                                  new int[] {1,0,0,2,2,1,1,1,1},
                                  new int[] {-2,2,0,0,0,0,0,0,0},
                                  new int[] {1,0,1,1,1,1,1,2,0},
                                  new int[] {1,1,-1,0,0,0,0,0,1}};

            int[][] secondFloor = new int[5][]
                                { new int[] {1,0,0,0,0,1,1,1,1},
                                  new int[] {1,0,0,2,2,1,1,1,1},
                                  new int[] {-2,2,0,0,0,0,0,0,0},
                                  new int[] {1,0,1,1,1,1,1,2,0},
                                  new int[] {1,1,0,0,0,0,0,0,1}};

            int[][] thirdFloor = {new int[] {0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0}};

            int[][] basement = {  new int[] {1,1,1,1,1,1,1,1,1},
                                  new int[] {1,1,1,1,1,1,1,1,1},
                                  new int[] {1,1,1,1,1,1,1,1,1},
                                  new int[] {1,1,1,1,1,1,1,1,1},
                                  new int[] {1,1,1,1,1,1,1,1,1}};

            int[][][] toRet = new int[][][] {basement, firstFloor, secondFloor, thirdFloor, ceiling};

            return toRet;
        }

        /*  this doesn't work!!!
        public static int[][][] getLevel6()
        {
            int[][] ceiling = {   new int[] {0,0,0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0,0,0}};

            int[][] firstFloor = new int[6][]
                                { new int[] {2,2,2,2,2,0,0,0,0,0,0},
                                  new int[] {2,2,2,2,2,0,0,0,0,0,-2},
                                  new int[] {2,2,2,2,2,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0,0,0}};

            int[][] secondFloor = new int[6][]
                                { new int[] {2,2,2,2,0,0,0,0,0,0,0},
                                  new int[] {2,2,2,2,0,0,0,0,0,0,0},
                                  new int[] {2,2,2,2,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0,0,0}};

            int[][] thirdFloor = new int[6][]
                                { new int[] {2,2,2,0,0,0,0,0,0,0,0},
                                  new int[] {2,2,2,0,0,0,0,0,0,0,0},
                                  new int[] {2,2,2,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0,0,0}};

            int[][] fourthFloor = new int[6][]
                                { new int[] {2,2,0,0,0,0,0,0,0,0,0},
                                  new int[] {2,2,0,0,0,0,0,0,0,0,0},
                                  new int[] {2,2,0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0,0,0}};

            int[][] fifthFloor = new int[6][]
                                { new int[] {2,0,0,0,0,0,0,0,0,0,0},
                                  new int[] {-1,0,0,0,0,0,0,0,0,0,0},
                                  new int[] {2,0,0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0,0,0}};

            int[][] basement = {  new int[] {1,1,1,1,1,1,1,1,1,1,1},
                                  new int[] {1,1,1,1,1,1,1,1,1,1,1},
                                  new int[] {1,1,1,1,1,1,1,1,1,1,1},
                                  new int[] {1,1,1,1,1,1,1,1,1,1,1},
                                  new int[] {1,1,1,1,1,1,1,1,1,1,1},
                                  new int[] {1,1,1,1,1,1,1,1,1,1,1}};

            int[][][] toRet = new int[][][] { basement, firstFloor, secondFloor,
                                              thirdFloor, fourthFloor, fifthFloor,
                                              ceiling }; ;

            return toRet;
        } */

        public static int[][][] getLevel5()
        {
            int[][] ceiling = {   new int[] {0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0},
                                  new int[] {1,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0}};

            int[][] firstFloor = new int[5][]
                                { new int[] {1,0,0,0,0,1,1,1,1},
                                  new int[] {1,0,0,2,3,1,1,1,1},
                                  new int[] {-2,4,0,0,0,0,0,0,0},
                                  new int[] {1,0,1,1,1,1,1,5,0},
                                  new int[] {1,1,-1,0,0,0,0,0,1}};
            int[][] secondFloor = new int[5][]
                                { new int[] {1,0,0,0,0,1,1,1,1},
                                  new int[] {1,0,0,6,7,1,1,1,1},
                                  new int[] {0,2,0,0,0,0,0,0,0},
                                  new int[] {1,0,1,1,1,1,1,5,0},
                                  new int[] {1,1,0,0,0,0,0,0,1}};

            int[][] thirdFloor = {new int[] {0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0},
                                  new int[] {1,0,0,0,0,0,0,0,0},
                                  new int[] {0,0,0,0,0,0,0,0,0}};

            int[][] basement = {  new int[] {1,1,1,1,1,1,1,1,1},
                                  new int[] {1,1,1,1,1,1,1,1,1},
                                  new int[] {1,1,1,1,1,1,1,1,1},
                                  new int[] {1,1,1,1,1,1,1,1,1},
                                  new int[] {1,1,1,1,1,1,1,1,1}};

            int[][][] toRet = new int[][][] { basement, firstFloor, secondFloor, thirdFloor, ceiling };

            return toRet;
        }

        public static int[][][] getLevel6()
        {
            int[][] ceiling = {   new int[] {0,0,0,0},
                                  new int[] {0,0,0,0},
                                  new int[] {0,0,0,0},
                                  new int[] {0,0,0,0},
                                  new int[] {0,0,0,0},
                                  new int[] {0,0,0,0}};

            int[][] firstFloor = new int[6][]
                                 { new int[] {1,2,0,1},
                                   new int[] {0,0,0,0},
                                   new int[] {0,0,4,0},
                                   new int[] {1,0,0,2},
                                   new int[] {0,0,0,0},
                                   new int[] {0,3,0,-1}};

            int[][] secondFloor = new int[6][]
                                 { new int[] {1,0,0,1},
                                   new int[] {0,0,0,0},
                                   new int[] {0,0,0,0},
                                   new int[] {1,0,0,0},
                                   new int[] {0,0,0,0},
                                   new int[] {0,0,0,0}};

            int[][] thirdFloor = new int[6][]
                                 { new int[] {1,1,1,1},
                                   new int[] {0,0,0,0},
                                   new int[] {0,0,0,0},
                                   new int[] {1,0,0,0},
                                   new int[] {0,0,0,0},
                                   new int[] {0,0,0,0}};

            int[][] fourthFloor = new int[6][]
                                 { new int[] {0,0,7,0},
                                   new int[] {0,0,0,0},
                                   new int[] {0,0,0,0},
                                   new int[] {1,0,0,0},
                                   new int[] {0,0,0,0},
                                   new int[] {0,0,0,0}};
            
            int[][] fifthFloor = new int[6][]
                                 { new int[] {0,0,0,0},
                                   new int[] {0,0,0,0},
                                   new int[] {0,0,0,0},
                                   new int[] {-2,0,0,0},
                                   new int[] {0,0,0,0},
                                   new int[] {0,0,0,0}};

            int[][] basement = {   new int[] {1,1,1,1},
                                   new int[] {1,1,1,1},
                                   new int[] {1,1,1,1},
                                   new int[] {1,1,1,1},
                                   new int[] {1,1,1,1},
                                   new int[] {1,1,1,1}};

            int[][][] toRet = new int[][][] { basement, firstFloor, secondFloor, thirdFloor, fourthFloor, fifthFloor, ceiling };

            return toRet;
        }

        public static int[][][] getLevel7()
        {
            int[][] ceiling =     { new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                    new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                    new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                    new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                    new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0}}; 
            
            int[][] fifthFloor =  { new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                    new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                    new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,-2},
                                    new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                    new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,1}}; 
            
            int[][] fourthFloor = { new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                    new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                    new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                    new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                    new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,1}};

            int[][] thirdFloor =  { new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                    new int[] {0,3,0,0,5,0,0,0,0,0,0,0,0,1},
                                    new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                    new int[] {0,-1,0,0,0,0,0,0,7,0,0,0,0,1},
                                    new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,1}};

            int[][] secondFloor = { new int[] {1,1,1,0,0,0,0,0,0,0,0,1,1,1},
                                    new int[] {1,1,1,0,0,0,0,0,0,0,0,1,1,1},
                                    new int[] {0,1,1,0,0,0,0,0,0,0,0,1,0,1},
                                    new int[] {0,1,1,0,0,0,0,0,0,0,0,1,1,1},
                                    new int[] {1,1,1,0,0,0,0,0,0,0,0,1,1,1}};

            int[][] firstFloor =  { new int[] {1,1,1,0,0,0,0,0,0,0,0,1,1,1},
                                    new int[] {1,1,1,0,0,0,0,0,0,0,0,1,1,1},
                                    new int[] {0,0,0,0,0,0,0,0,0,0,0,1,0,1},
                                    new int[] {1,1,1,0,0,0,0,0,0,0,0,1,1,1},
                                    new int[] {1,1,1,0,0,0,0,0,0,0,0,1,1,1}};

            int[][] basement =    { new int[] {1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                                    new int[] {1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                                    new int[] {1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                                    new int[] {1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                                    new int[] {1,1,1,1,1,1,1,1,1,1,1,1,1,1}};

            int[][][] toRet = new int[][][] { basement, firstFloor, secondFloor, 
                                              thirdFloor, fourthFloor, fifthFloor, ceiling };

            return toRet;
        }

        public static int[][][] getLevel8(){

            int[][] ceiling =     { new int[] {0,0,0,0},
                                    new int[] {0,0,0,0},
                                    new int[] {0,0,0,0},
                                    new int[] {0,0,0,0},
                                    new int[] {0,0,0,0},
                                    new int[] {0,0,0,0}};

            int[][] sixthFloor =  { new int[] {0,0,0,0},
                                    new int[] {0,0,0,0},
                                    new int[] {0,-2,0,0},
                                    new int[] {0,0,0,0},
                                    new int[] {0,0,0,0},
                                    new int[] {0,0,0,0}};

            int[][] fifthFloor =  { new int[] {0,0,0,0},
                                    new int[] {0,0,0,0},
                                    new int[] {0,1,0,0},
                                    new int[] {0,0,0,0},
                                    new int[] {0,0,0,0},
                                    new int[] {0,0,0,0}};

            int[][] fourthFloor = { new int[] {0,0,0,0},
                                    new int[] {0,0,0,0},
                                    new int[] {0,1,0,0},
                                    new int[] {1,1,0,0},
                                    new int[] {0,0,0,0},
                                    new int[] {0,0,0,0}};

            int[][] thirdFloor =  { new int[] {0,0,0,0},
                                    new int[] {0,0,0,0},
                                    new int[] {1,1,0,0},
                                    new int[] {1,1,0,0},
                                    new int[] {0,0,0,0},
                                    new int[] {0,0,0,0}}; 

            int[][] secondFloor = { new int[] {0,0,0,0},
                                    new int[] {0,0,0,0},
                                    new int[] {1,1,0,0},
                                    new int[] {1,1,0,0},
                                    new int[] {-1,0,0,0},
                                    new int[] {0,0,0,0}}; 
            
            int[][] firstFloor =  { new int[] {0,0,0,0},
                                    new int[] {0,0,0,0},
                                    new int[] {1,1,0,0},
                                    new int[] {1,1,0,0},
                                    new int[] {7,3,0,0},
                                    new int[] {5,0,0,0}};

            int[][] basement =    { new int[] {1,1,1,1},
                                    new int[] {1,1,1,1},
                                    new int[] {1,1,1,1},
                                    new int[] {1,1,1,1},
                                    new int[] {1,1,1,1},
                                    new int[] {1,1,1,1}};

            int[][][] toRet = new int[][][] { basement, firstFloor, secondFloor, thirdFloor, fourthFloor, fifthFloor, sixthFloor, ceiling };

            return toRet;
        }


        public static GameBarrier[] getBarrier1()
        {
            return new GameBarrier[0];
        }
        public static GameBarrier[] getBarrier2()
        {
            return new GameBarrier[0];
        }
        public static GameBarrier[] getBarrier3()
        {
            return new GameBarrier[0];
        }
        public static GameBarrier[] getBarrier4()
        {
            return new GameBarrier[0];
        }
        public static GameBarrier[] getBarrier5()
        {
            return new GameBarrier[0];
        }
        public static GameBarrier[] getBarrier6()
        {
            return new GameBarrier[]{ new GameBarrier(1, 2, 3, GameConstants.WALL_FLOOR, GameConstants.COLOUR_ORANGE) };
        }
        public static GameBarrier[] getBarrier7()
        {
            return new GameBarrier[] { new GameBarrier(3, 3, 0, GameConstants.WALL_FLOOR, GameConstants.COLOUR_RED),
                                       new GameBarrier(3, 3, 1, GameConstants.WALL_FLOOR, GameConstants.COLOUR_RED),
                                       new GameBarrier(3, 3, 2, GameConstants.WALL_FLOOR, GameConstants.COLOUR_RED),
                                       new GameBarrier(3, 3, 3, GameConstants.WALL_FLOOR, GameConstants.COLOUR_RED),
                                       new GameBarrier(3, 3, 4, GameConstants.WALL_FLOOR, GameConstants.COLOUR_RED),
                                       new GameBarrier(4, 3, 0, GameConstants.WALL_FLOOR, GameConstants.COLOUR_RED),
                                       new GameBarrier(4, 3, 1, GameConstants.WALL_FLOOR, GameConstants.COLOUR_RED),
                                       new GameBarrier(4, 3, 2, GameConstants.WALL_FLOOR, GameConstants.COLOUR_RED),
                                       new GameBarrier(4, 3, 3, GameConstants.WALL_FLOOR, GameConstants.COLOUR_RED),
                                       new GameBarrier(4, 3, 4, GameConstants.WALL_FLOOR, GameConstants.COLOUR_RED),
                                       new GameBarrier(5, 3, 0, GameConstants.WALL_FLOOR, GameConstants.COLOUR_RED),
                                       new GameBarrier(5, 3, 1, GameConstants.WALL_FLOOR, GameConstants.COLOUR_RED),
                                       new GameBarrier(5, 3, 2, GameConstants.WALL_FLOOR, GameConstants.COLOUR_RED),
                                       new GameBarrier(5, 3, 3, GameConstants.WALL_FLOOR, GameConstants.COLOUR_RED),
                                       new GameBarrier(5, 3, 4, GameConstants.WALL_FLOOR, GameConstants.COLOUR_RED),
                                       new GameBarrier(6, 3, 0, GameConstants.WALL_FLOOR, GameConstants.COLOUR_RED),
                                       new GameBarrier(6, 3, 1, GameConstants.WALL_FLOOR, GameConstants.COLOUR_RED),
                                       new GameBarrier(6, 3, 2, GameConstants.WALL_FLOOR, GameConstants.COLOUR_RED),
                                       new GameBarrier(6, 3, 3, GameConstants.WALL_FLOOR, GameConstants.COLOUR_RED),
                                       new GameBarrier(6, 3, 4, GameConstants.WALL_FLOOR, GameConstants.COLOUR_RED),
                                       new GameBarrier(7, 3, 0, GameConstants.WALL_FLOOR, GameConstants.COLOUR_YELLOW),
                                       new GameBarrier(7, 3, 1, GameConstants.WALL_FLOOR, GameConstants.COLOUR_YELLOW),
                                       new GameBarrier(7, 3, 2, GameConstants.WALL_FLOOR, GameConstants.COLOUR_YELLOW),
                                       new GameBarrier(7, 3, 3, GameConstants.WALL_FLOOR, GameConstants.COLOUR_YELLOW),
                                       new GameBarrier(7, 3, 4, GameConstants.WALL_FLOOR, GameConstants.COLOUR_YELLOW),
                                       new GameBarrier(8, 3, 0, GameConstants.WALL_FLOOR, GameConstants.COLOUR_YELLOW),
                                       new GameBarrier(8, 3, 1, GameConstants.WALL_FLOOR, GameConstants.COLOUR_YELLOW),
                                       new GameBarrier(8, 3, 2, GameConstants.WALL_FLOOR, GameConstants.COLOUR_YELLOW),
                                       new GameBarrier(8, 3, 3, GameConstants.WALL_FLOOR, GameConstants.COLOUR_YELLOW),
                                       new GameBarrier(8, 3, 4, GameConstants.WALL_FLOOR, GameConstants.COLOUR_YELLOW),
                                       new GameBarrier(9, 3, 0, GameConstants.WALL_FLOOR, GameConstants.COLOUR_YELLOW),
                                       new GameBarrier(9, 3, 1, GameConstants.WALL_FLOOR, GameConstants.COLOUR_YELLOW),
                                       new GameBarrier(9, 3, 2, GameConstants.WALL_FLOOR, GameConstants.COLOUR_YELLOW),
                                       new GameBarrier(9, 3, 3, GameConstants.WALL_FLOOR, GameConstants.COLOUR_YELLOW),
                                       new GameBarrier(9, 3, 4, GameConstants.WALL_FLOOR, GameConstants.COLOUR_YELLOW),
                                       new GameBarrier(10, 3, 0, GameConstants.WALL_FLOOR, GameConstants.COLOUR_YELLOW),
                                       new GameBarrier(10, 3, 1, GameConstants.WALL_FLOOR, GameConstants.COLOUR_YELLOW),
                                       new GameBarrier(10, 3, 2, GameConstants.WALL_FLOOR, GameConstants.COLOUR_YELLOW),
                                       new GameBarrier(10, 3, 3, GameConstants.WALL_FLOOR, GameConstants.COLOUR_YELLOW),
                                       new GameBarrier(10, 3, 4, GameConstants.WALL_FLOOR, GameConstants.COLOUR_YELLOW),
                                       new GameBarrier(12, 3, 2, GameConstants.WALL_FLOOR, GameConstants.COLOUR_BLUE)};
        }
        public static GameBarrier[] getBarrier8()
        {
            return new GameBarrier[] { new GameBarrier(2, 1, 2, GameConstants.WALL_DOWN, GameConstants.COLOUR_GREEN),
                                       new GameBarrier(3, 2, 2, GameConstants.WALL_DOWN, GameConstants.COLOUR_GREEN),
                                       new GameBarrier(2, 2, 2, GameConstants.WALL_DOWN, GameConstants.COLOUR_GREEN),
                                       new GameBarrier(3, 1, 2, GameConstants.WALL_DOWN, GameConstants.COLOUR_GREEN)};
        }
        
    }
}
