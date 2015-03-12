#region Using ...
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using BloomPostprocess;
#endregion

namespace SpectrumXNA
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region global vars
        float scaleFactor = 0.0095f;

        Camera _camera;
        Settings gameSettings = new Settings();

        GraphicsDeviceManager graphics;

        BloomComponent bloom;

        GraphicsDevice device;
        MouseState originalMouseState;
        MouseState pastMouseState;
        SpriteBatch spriteBatch;
        SpriteFont menuFont;
        SpriteFont tipsFont;

        Model greyBlock;
        Model redBlock;
        Model orangeBlock;
        Model yellowBlock;
        Model greenBlock;
        Model blueBlock;
        Model purpleBlock;
        Model[] barrierBlockModel = new Model[8]; //it's 8... even tho there are 6. xD
        Model cat;
        Model rcat, ycat, bcat, wcat;
        //Texture2D colorWheelTexture, 
        Texture2D menuTitle;
        Texture2D blank;
        Texture2D colourRestoredTexture;
        Texture2D theEndTexture;

        // lol gonna waste memory here though there's only 4. Just pointers anyways. :P
        Texture2D[] colorWheelTextures = new Texture2D[8]; 
        Player player;

        Texture2D[] menuText = new Texture2D[7];
        Texture2D[] settingText = new Texture2D[4];

        GenericItems theEnd;
        GenericItems directionArrow;

        SoundEffectInstance soundEngineInstance2;
        SoundEffect soundChange;
        SoundEffect soundWin;
        Song soundMusic;

        float aspectRatio;

        // game state holders
        int toDisplay = GameConstants.DISPLAY_MENU;
        GameRoom[][][] state;
        int currentLevel;

        // 
        bool[] keyState = new bool[GameConstants.MAX_KEYS];
        bool[] buttonState = new bool[GameConstants.MAX_BUTTONS];
        bool[] currentAction = new bool [2];
        GameColour[] spriteColours = new GameColour [7];

        // for vibration
        float elapsedTime;

        int screenwidth;
        int screenheight;

        int menuSelectOption = 0;
        int menuSelectCount;

        int transitionCounter = 0;
        float moveDist = 0f;
        Vector3 horizDir = new Vector3(0, 0, 0);
        Vector3 vertDir = new Vector3(0, 0, 0);

        //vectors can't be declared as constants.
        Vector3 DIR_LEFT = -Vector3.UnitX;
        Vector3 DIR_RIGHT = Vector3.UnitX;
        Vector3 DIR_UP = -Vector3.UnitZ;
        Vector3 DIR_DOWN = Vector3.UnitZ;


        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            if (gameSettings.BLOOM_ON)
            {
                // Bloom!!!
                graphics.MinimumPixelShaderProfile = ShaderProfile.PS_2_0;

                bloom = new BloomComponent(this);
                bloom.Settings = BloomSettings.PresetSettings[5];

                Components.Add(bloom);
            }
            else if (gameSettings.ANTIALIAS_ON)
            {
                // THIS HAS TO BE ELSE IF!
                // Bloom and anti alias cannot go together at the same time.

                graphics.PreferMultiSampling = true;
            }

            graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);
            
            // Full Screen!
            this.graphics.IsFullScreen = gameSettings.FULLSCREEN_ON;
        }


        /* 
         ------------------------------------------------------
         *                 MAIN GAME FUNCTIONS
         ------------------------------------------------------ */

        #region Initialize
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this._camera = new Camera(graphics.GraphicsDevice.Viewport);
            this._camera.LookAt = new Vector3(0.0f, 0.0f, 0f);
            this._camera.Position = new Vector3(4.0f, 10.0f, 9.0f);
            // dummy vars for non null
            player = new Player(0, 0, 0, null, 0); //dummy for non null
            theEnd = new GenericItems(0, 0, 0, null); // dummy for theend
            directionArrow = new GenericItems(0, 0, 0, null);

            elapsedTime = 0f;

            // Add initializing logic here and below
            currentLevel = 1;
            
            // Initalize state information.
            flushKeyState();
            for (int i = 0; i < 7; i++)
            {
                spriteColours[i] = new GameColour(i);
            }

            base.Initialize();
        }
        #endregion

        #region LoadContent
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            device = GraphicsDevice;
            Mouse.SetPosition(device.Viewport.Width / 2, device.Viewport.Height / 2);
            originalMouseState = Mouse.GetState();
            pastMouseState = originalMouseState;           
            soundChange = Content.Load<SoundEffect>("Audio\\changeColour");
            soundWin = Content.Load<SoundEffect>("Audio\\win");
            soundMusic = Content.Load<Song>("Audio\\The_street_is_merciless");

            // Load but don't play yet. Only play in game.
            MediaPlayer.Play(soundMusic);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Pause();
            
            greyBlock = Content.Load<Model>("Model\\box_grey");
            redBlock = Content.Load<Model>("Model\\box_red");
            orangeBlock = Content.Load<Model>("Model\\box_orange");
            yellowBlock = Content.Load<Model>("Model\\box_yellow");
            greenBlock = Content.Load<Model>("Model\\box_green");
            blueBlock = Content.Load<Model>("Model\\box_blue");
            purpleBlock = Content.Load<Model>("Model\\box_purple");
            barrierBlockModel[GameConstants.COLOUR_RED] = Content.Load<Model>("Model\\box_red_no_outline");
            barrierBlockModel[GameConstants.COLOUR_PURPLE] = Content.Load<Model>("Model\\box_purple_no_outline");
            barrierBlockModel[GameConstants.COLOUR_ORANGE] = Content.Load<Model>("Model\\box_orange_no_outline");
            barrierBlockModel[GameConstants.COLOUR_YELLOW] = Content.Load<Model>("Model\\box_yellow_no_outline");
            barrierBlockModel[GameConstants.COLOUR_BLUE] = Content.Load<Model>("Model\\box_blue_no_outline");
            barrierBlockModel[GameConstants.COLOUR_GREEN] = Content.Load<Model>("Model\\box_green_no_outline");
            theEnd.model = Content.Load<Model>("Model\\wtf");
            cat = Content.Load<Model>("Model\\cat");
            rcat = Content.Load<Model>("Model\\cat_red");
            ycat = Content.Load<Model>("Model\\cat_yellow");
            bcat = Content.Load<Model>("Model\\cat_blue");
            wcat = Content.Load<Model>("Model\\cat_white");
            colorWheelTextures[GameConstants.COLOUR_BLACK] = Content.Load<Texture2D>("Textures\\color_wheel");
            colorWheelTextures[GameConstants.COLOUR_RED] = Content.Load<Texture2D>("Textures\\color_wheel_red");
            colorWheelTextures[GameConstants.COLOUR_BLUE] = Content.Load<Texture2D>("Textures\\color_wheel_blue");
            colorWheelTextures[GameConstants.COLOUR_YELLOW] = Content.Load<Texture2D>("Textures\\color_wheel_yellow");
            colourRestoredTexture = Content.Load<Texture2D>("Textures\\colour_restored");
            theEndTexture = Content.Load<Texture2D>("Textures\\the_end");
            menuTitle = Content.Load<Texture2D>("Textures\\spectrum");
            blank = Content.Load<Texture2D>("Textures\\blank");
            menuFont = Content.Load<SpriteFont>("menuFont");
            if (screenwidth < 1100)
                tipsFont = Content.Load<SpriteFont>("tipsFont_Small");
            else
                tipsFont = Content.Load<SpriteFont>("tipsFont");
            directionArrow.model = Content.Load<Model>("Model\\arrow");

            menuText[GameConstants.MENU_NEW_GAME] = Content.Load<Texture2D>("MenuText\\newgame");
            menuText[GameConstants.MENU_CONTINUE] = Content.Load<Texture2D>("MenuText\\continue");
            //menuText[GameConstants.MENU_CUSTOM] = Content.Load<Texture2D>("MenuText\\custom");
            //menuText[GameConstants.MENU_EDITOR] = Content.Load<Texture2D>("MenuText\\editor");
            menuText[GameConstants.MENU_SETTINGS] = Content.Load<Texture2D>("MenuText\\settings");
            menuText[GameConstants.MENU_EXIT] = Content.Load<Texture2D>("MenuText\\exit");
            menuText[GameConstants.MENU_SELECT] = Content.Load<Texture2D>("MenuText\\face");
            settingText[GameConstants.MENU_SETTINGS_SOUND] = Content.Load<Texture2D>("MenuText\\sound");
            settingText[GameConstants.MENU_SETTINGS_TIPS] = Content.Load<Texture2D>("MenuText\\show_tips");
            settingText[GameConstants.MENU_SETTINGS_ON] = Content.Load<Texture2D>("MenuText\\on");
            settingText[GameConstants.MENU_SETTINGS_OFF] = Content.Load<Texture2D>("MenuText\\off");

            player.model = cat;

            spriteColours[GameConstants.COLOUR_BLACK].initialize(true, cat, Color.DarkGray);
            spriteColours[GameConstants.COLOUR_RED].initialize(true, rcat, new Color(255,120,150));
            spriteColours[GameConstants.COLOUR_BLUE].initialize(true, bcat, Color.CornflowerBlue);
            spriteColours[GameConstants.COLOUR_YELLOW].initialize(true, ycat, Color.Goldenrod);

        }
        #endregion

        #region UnloadContent
        /* Nothing to see here */
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        #endregion

        #region Update
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit - Don't necessarily want this
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            //    this.Exit();

            Controller.UpdateGameControllerState();
            // TODO: Add your update logic here
            if (toDisplay == GameConstants.DISPLAY_GAME)
            {
                keyboardInputPickup();
                keyboardInputMovement();
                keyboardInputColorChange();
                mouseUpdateMovement();
                keyboardInputMisc();

                if (Controller.IsConnected)
                {
                    gamepadInputPickup();
                    gamepadInputMisc();
                    gamepadInputColorChange();
                    gamepadInputMovement();
                    JoystickLook();

                    elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Controller.updateVibrations(elapsedTime);
                }
                
            }
            else if (toDisplay == GameConstants.DISPLAY_MENU || toDisplay == GameConstants.DISPLAY_SETTINGS)
            {
                MediaPlayer.Pause();
                if (toDisplay == GameConstants.DISPLAY_MENU)
                    menuSelectCount = GameConstants.MENU_NUM_OPTIONS;
                else
                    menuSelectCount = GameConstants.MENU_SETTINGS_NUM_OPTIONS;

                keyboardMenuInput();
                if (Controller.IsConnected)
                {
                    gamepadMenuInput();
                }
                
            }
            else if (toDisplay == GameConstants.DISPLAY_END)
            {
                if (Keyboard.GetState().GetPressedKeys().Length > 0)
                {
                    setLevel(++currentLevel);
                }
                if (Controller.IsConnected)
                {
                    Controller.UpdateGameControllerState();
                    if (Controller.UpButton || Controller.DownButton
                        || Controller.LeftButton || Controller.RightButton
                        || Controller.AButton || Controller.BButton
                        || Controller.XButton || Controller.YButton)
                    {
                        setLevel(++currentLevel);
                    }
                }
            }
            else // End of game.
            {
                if (Keyboard.GetState().GetPressedKeys().Length > 0)
                {
                    toDisplay = GameConstants.DISPLAY_MENU;
                    MediaPlayer.Pause();
                }
            }
            base.Update(gameTime);
        }
        #endregion

        /* 
         ------------------------------------------------------
         *                 KEYBOARD RELATED
         ------------------------------------------------------ */
        #region KeyboardDuringGame

        /**
         * Game state related input.
         */
        private void keyboardInputMisc()
        {
            KeyboardState keybState = Keyboard.GetState();

            // R key pressed: reset the level.
            if (keybState.IsKeyDown(Keys.R))
                setLevel(currentLevel);

            // Tab key pressed: skip level.
            if (keybState.IsKeyDown(Keys.Tab))
                keyState[GameConstants.KEY_SKIP] = true;

            if (keyState[GameConstants.KEY_SKIP] && keybState.IsKeyUp(Keys.Tab))
            {
                keyState[GameConstants.KEY_SKIP] = false;
                player.position = theEnd.position;
                checkWin();
            }

            // Esc key pressed: go to title or exit game.
            if (keybState.IsKeyDown(Keys.Escape))
                keyState[GameConstants.KEY_EXIT] = true;

            if (keyState[GameConstants.KEY_EXIT] && keybState.IsKeyUp(Keys.Escape))
            {
                /* -- Separate Keyboard for menu now
                // If we weren't in a level, exit the game.
                if (toDisplay != GameConstants.DISPLAY_GAME)
                    this.Exit();
                 */
                
                // Otherwise, go to the menu screen.
                bgColorGoal = Color.DarkGray;
                toDisplay = GameConstants.DISPLAY_MENU;
                MediaPlayer.Pause();
                menuSelectOption = GameConstants.MENU_CONTINUE;
                keyState[GameConstants.KEY_EXIT] = false;
            }

            /* -- separate keyboard for menu now
            // Enter key: if we're at the title screen, go to the current level.
            if (!currentlyPlaying && keybState.IsKeyDown(Keys.Enter))
            {
                // Start game.
                setLevel(currentLevel);
                bgColorGoal = Color.Black;
            } */
            
        }

        /* Handles the issues of cat changing colors */
        private void keyboardInputColorChange()
        {
            
            // If the player is carrying a block, we cannot change our colour.
            if (currentAction[GameConstants.ACTION_LIFT])
                return;
            
            KeyboardState keybState = Keyboard.GetState();

            if (keybState.IsKeyDown(Keys.X))
                keyState[GameConstants.KEY_CHANGE_FORWARDS] = true;

            if (keybState.IsKeyDown(Keys.Z))
                keyState[GameConstants.KEY_CHANGE_BACKWARDS] = true;

            // Change color.
            if (keyState[GameConstants.KEY_CHANGE_FORWARDS] && keybState.IsKeyUp(Keys.X))
            {
                changeCatColor(true);
                keyState[GameConstants.KEY_CHANGE_FORWARDS] = false;
            }
            else if (keyState[GameConstants.KEY_CHANGE_BACKWARDS] && keybState.IsKeyUp(Keys.Z))
            {
                changeCatColor(false);
                keyState[GameConstants.KEY_CHANGE_BACKWARDS] = false;
            }
            else
                return;

            flushKeyState();

            // We might need to phase through the floor.
            float rot = player.rotation;
            playerMovement(new Vector3(0, 0, 0));
            player.rotation = rot;  //because moving can reset your rotation.
        }

        /**
         * Cycle throught the colour wheel.
         */
        private void changeCatColor(bool forward)
        {
            // Get the next possible colour ID.
            GameColour nextColour = player.spriteColour;
            
            int cycleDirection = -1;
            if (forward)
                cycleDirection = 1;

            do
            {
                int tempId = nextColour.colourId + (1 * cycleDirection);

                if (tempId >= spriteColours.Length)
                    tempId = 0;
                if (tempId < 0)
                    tempId = spriteColours.Length - 1;

                nextColour = spriteColours[tempId];
            } while (!nextColour.colourEnabled);

            // Update player and background.
            player.model = nextColour.colourModel;
            player.spriteColour = nextColour;
            bgColorGoal = nextColour.colourBackground;
            soundEngineInstance2 = soundChange.CreateInstance();
            soundEngineInstance2.Play();
        }

        /**
         * Handle picking up blocks.
         */
        private void keyboardInputPickup()
        {
            KeyboardState keybState = Keyboard.GetState();
            if (keybState.IsKeyDown(Keys.Q) || keybState.IsKeyDown(Keys.E))
                keyState[GameConstants.KEY_LIFT] = true;

            // Button is pressed
            if (keyState[GameConstants.KEY_LIFT] && (keybState.IsKeyUp(Keys.Q) && keybState.IsKeyUp(Keys.E)))
            {
                // Kill all other executable actions that may exist.
                flushKeyState();

                Vector3 lookingAt = whereAmILookingAt();
                int xbox = (int)lookingAt.X + (int)player.position.X;
                int zbox = (int)lookingAt.Z + (int)player.position.Z;

                // are we at end of the world?
                if (xbox < 0 || xbox >= state[0][0].Length)
                    return; // EotW
                if (zbox < 0 || zbox >= state[0].Length)
                    return; // EotW

                if (!currentAction[GameConstants.ACTION_LIFT])
                {
                    // Check to see if we're already as high as we can go.
                    if ((int)player.position.Y == state.Length - 1)
                        return;

                    GameRoom aboveBox = state[(int)player.position.Y + 1][zbox][xbox];
                    GameRoom resultRoom = state[(int)player.position.Y + 1][(int)player.position.Z][(int)player.position.X];
                    int currentObject = getGridContents(xbox, (int)player.position.Y, zbox);

                    // is there something on top of you?
                    if (getGridContents((int)player.position.X, (int)player.position.Y + 1, (int)player.position.Z) > 0
                        || !colourMatch(resultRoom.wall_floor, player.spriteColour.colourId))
                        return; // no room to pick up

                    // is there something to pick up?
                    if (getGridContents(xbox, (int)player.position.Y, zbox) <= 0 && getGridContents(xbox, (int)player.position.Y + 1, zbox) <= 0)
                        return; // Nothing to pick up

                    // Check above the box.
                    if (getGridContents(xbox, (int)player.position.Y + 1, zbox) <= 0)
                    {
                        // Is the character the right colour?
                        if (!yourColorMatch((int)player.position.Y, zbox, xbox))
                            return;

                        // Don't move it if there are barriers in the way.
                        if (!colourMatch(aboveBox.wall_floor, currentObject))
                            return;

                        if ((int)-lookingAt.X == -1)
                        {
                            // We're looking at a move left. We check the left wall of
                            // the current room, and the right wall of the new room.
                            if (!colourMatch(aboveBox.wall_left, currentObject)
                                || !colourMatch(resultRoom.wall_right, currentObject))
                                return;
                        }
                        else if ((int)-lookingAt.X == 1)
                        {
                            // We're mvoing right.
                            if (!colourMatch(aboveBox.wall_right, currentObject)
                                || !colourMatch(resultRoom.wall_left, currentObject))
                                return;
                        }
                        if ((int)-lookingAt.Z == -1)
                        {
                            // We are moving forwards.
                            if (!colourMatch(aboveBox.wall_up, currentObject)
                                || !colourMatch(resultRoom.wall_down, currentObject))
                                return;
                        }
                        else if ((int)-lookingAt.Z == 1)
                        {
                            // Moving backwards.
                            if (!colourMatch(aboveBox.wall_down, currentObject)
                                || !colourMatch(resultRoom.wall_up, currentObject))
                                return;
                        }

                        // Nothing on top. Pick it up!
                        currentAction[GameConstants.ACTION_LIFT] = true;

                        moveBlock(xbox, (int)player.position.Y, zbox, (int)-lookingAt.X, 1, (int)-lookingAt.Z);
                    }
                    // Is there something on top of this box?
                    // TODO: remove this?
                    else if (getGridContents(xbox, (int)player.position.Y + 1, zbox) == 1)
                        return;

                    // there's a 3rd box. can't pick up. too high. (???)
                    else if (getGridContents(xbox, (int)player.position.Y + 2, zbox) > 0)
                        return;

                    // There is a block on top of the block directly infront. Can we pick this
                    // block up?
                    else if (getGridContents(xbox, (int)player.position.Y + 1, zbox) > 1)
                    {
                        // Is the character the right colour?
                        if (!yourColorMatch((int)player.position.Y + 1, zbox, xbox))
                            return;

                        currentAction[GameConstants.ACTION_LIFT] = true;

                        moveBlock(xbox, (int)player.position.Y + 1, zbox, (int)-lookingAt.X, 0, (int)-lookingAt.Z);

                    }

                }
                else // put it down now
                {
                    if (getGridContents(xbox, (int)player.position.Y + 1, zbox) == 0)
                    {
                        moveBlock((int)player.position.X, (int)player.position.Y + 1, (int)player.position.Z,
                            (int)lookingAt.X, 0, (int)lookingAt.Z);
                        currentAction[GameConstants.ACTION_LIFT] = false;
                    }
                    else 
                            return;

                    // does it need to fall???
                    int ybox = (int)player.position.Y + 1;
                    while (validMove(ybox, zbox, xbox, -1, 0, 0))
                    {
                        moveBlock(xbox, ybox, zbox, 0, -1, 0);
                        ybox--;
                    }
                }
            }
        }
        
        private Vector3 whereAmILookingAt()
        {
            // There are four directions that the player can face: 90, -90, 180, 0.
            float playerRot = MathHelper.ToDegrees(player.rotation);
            if (playerRot == 90)
                return DIR_RIGHT;
            else if (playerRot == 180)
                return DIR_UP;
            else if (playerRot == -90)
                return DIR_LEFT;
            else
                return DIR_DOWN;
        }

        /* Keyboard'R'Us */
        private void keyboardInputMovement()
        {
            // This entire function looks absolutely horrible
            // because it is entirely hacked together. lol 
            // We should have fluid movement, not blocky. But for the time being
            // this is faster!
            KeyboardState keybState = Keyboard.GetState();

            // face a direction first.
            if (keybState.IsKeyDown(Keys.Left) || keybState.IsKeyDown(Keys.A))
            {
                keyState[GameConstants.KEY_LEFT] = true;
            }
            else if (keybState.IsKeyDown(Keys.Right) || keybState.IsKeyDown(Keys.D))
            {
                keyState[GameConstants.KEY_RIGHT] = true;
            }
            else if (keybState.IsKeyDown(Keys.Up) || keybState.IsKeyDown(Keys.W))
            {
                keyState[GameConstants.KEY_UP] = true;
            }
            else if (keybState.IsKeyDown(Keys.Down) || keybState.IsKeyDown(Keys.S))
            {
                keyState[GameConstants.KEY_DOWN] = true;
            }            

            // try to jump
            else if (keybState.IsKeyDown(Keys.Space))
                keyState[GameConstants.KEY_CLIMB] = true;

            // jump up. this cancels the release of keys to go another step.
            else if (keyState[GameConstants.KEY_CLIMB] && keybState.IsKeyUp(Keys.Space))
                handleClimb();

            // rotate or go to the direction when the key is released.
            else if (keyState[GameConstants.KEY_LEFT] && keybState.IsKeyUp(Keys.Left) && keybState.IsKeyUp(Keys.A))
            {
                
                if (whereAmILookingAt() == adaptToCameraAngle(DIR_LEFT))
                    handleHorizontalMovement(-1, 0);
                else
                    rotatePlayer(DIR_LEFT);
            }

            else if (keyState[GameConstants.KEY_RIGHT] && keybState.IsKeyUp(Keys.Right) && keybState.IsKeyUp(Keys.D))
            {
                if (whereAmILookingAt() == adaptToCameraAngle(DIR_RIGHT))
                    handleHorizontalMovement(1, 0); 
                else
                    rotatePlayer(DIR_RIGHT);
            }

            else if (keyState[GameConstants.KEY_UP] && keybState.IsKeyUp(Keys.Up) && keybState.IsKeyUp(Keys.W))
            {

                if (whereAmILookingAt() == adaptToCameraAngle(DIR_UP))
                    handleHorizontalMovement(0, -1);
                else
                    rotatePlayer(DIR_UP);
            }

            else if (keyState[GameConstants.KEY_DOWN] && keybState.IsKeyUp(Keys.Down) && keybState.IsKeyUp(Keys.S))
            {
                if (whereAmILookingAt() == adaptToCameraAngle(DIR_DOWN))
                    handleHorizontalMovement(0, 1);
                else
                    rotatePlayer(DIR_DOWN);
            }
            
        }

        public void rotatePlayer(Vector3 dir)
        {
            Vector3 go = adaptToCameraAngle(dir);
            player.rotation = (float)Math.Atan2((int)go.X, (int)go.Z);
            flushKeyState();
            flushButtonState();
        }
        
        public Vector3 adaptToCameraAngle(Vector3 dir)
        {
            //changes direction if it would make more sense with the current camera angle.

            Vector3 camPos = _camera.Position - _camera.LookAt;
            int camCos = 0;
            int camSin = 0;

            if (Math.Abs(camPos.X) >= Math.Abs(camPos.Z)) { camCos = -Math.Sign(camPos.X); }
            else { camSin = Math.Sign(camPos.Z); }

            float x1 = camSin * dir.X - camCos * dir.Z;   //bah, really oughta use a matrix instead
            float z1 = camCos * dir.X + camSin * dir.Z;

            return new Vector3(x1, dir.Y, z1);
        }

        /**
         * Move the player (not jumping).
         */
        public void handleHorizontalMovement(int x, int z)
        {
            
            //////////////////////////////
            // invalid movement!!! (for now)
            //////////////////////////////
            if (x != 0 && z != 0)
                return;
            
            //make the arrow keys change direction depending on camera facing
            Vector3 go = adaptToCameraAngle(new Vector3(x, 0, z));

            x = (int)go.X;
            z = (int)go.Z;

            // holding something. You can't move dude.
            if (currentAction[GameConstants.ACTION_LIFT] || keyState[GameConstants.KEY_LIFT])
                return;

            // You're at the edge; you can't go any further. These also
            // eliminate INDEX OUT OF BOUNDS EXCEPTION that may occur in below
            // code
            int temp = getGridContents((int)player.position.X + x, (int)player.position.Y, (int)player.position.Z + z);
            if (temp < 0 && temp != GameConstants.COLOUR_NONE)
            {
                flushKeyState();
                flushButtonState();
                return;
            }

            // we got nothing left!
            if (x == 0 && z == 0)
            {
                flushKeyState();
                flushButtonState();
                return;
            }

            // If we just climbed up something, we shouldn't walk immediately aftwards.
            if (!currentAction[GameConstants.ACTION_CLIMB])
            {

                // Check if there are any block collisions we need to handle.
                if (!handleBlockCollision(x, z))
                {
                    flushKeyState();
                    flushButtonState();
                    return;
                }
                playerMovement(go);
            }
            
            // Make sure we can still climb or whatever afterwards.
            currentAction[GameConstants.ACTION_CLIMB] = false;
            flushKeyState();
            flushButtonState();
        }

        /**
         * Move the player around the grid.
         */
        private void playerMovement(Vector3 dir) {           

            ///////////////////////////
            // make the move
            ///////////////////////////

            /*if (whereAmILookingAt() != dir)
            {
                player.rotation = (float)Math.Atan2(dir.X, dir.Z);
                return;
            }*/

            //for smooth movement
            moveDist = 1f;
            horizDir = new Vector3(dir.X, 0, dir.Z);
            vertDir = new Vector3(0, dir.Y, 0);

            player.position += dir;

            while (true)
            {
                if (player.position.Y == 0)
                    break; // we're done - BUT should have never reached here in the first place.
                if (getGridContents((int)player.position.X, (int)player.position.Y - 1, (int)player.position.Z) <= 0
                    && colourMatch(player.spriteColour.colourId, state[(int)player.position.Y][(int)player.position.Z][(int)player.position.X].wall_floor))
                {
                    player.position.Y--;
                    vertDir.Y--;
                }
                else
                    break;
            }

            // all movements completed. Did we win?
            checkWin();
        }

        #endregion keyboard

        #region KeyboardDuringMenu
        private void keyboardMenuInput()
        {
            KeyboardState keybState = Keyboard.GetState();

            if (keybState.IsKeyDown(Keys.Escape))
                keyState[GameConstants.KEY_EXIT] = true;
            else if (keyState[GameConstants.KEY_EXIT] && keybState.IsKeyUp(Keys.Escape)) {
                keyState[GameConstants.KEY_EXIT] = false;
                if (toDisplay == GameConstants.DISPLAY_MENU)
                    this.Exit();
                else if (toDisplay == GameConstants.DISPLAY_SETTINGS)
                    toDisplay = GameConstants.DISPLAY_MENU;
            }

            // Down is pressed & lifted?
            if (keybState.IsKeyDown(Keys.Down))
                keyState[GameConstants.KEY_DOWN] = true;
            else if (keyState[GameConstants.KEY_DOWN] && keybState.IsKeyUp(Keys.Down))
            {
                keyState[GameConstants.KEY_DOWN] = false;
                menuSelectOption = (menuSelectOption + 1) % menuSelectCount;
                return;
            }

            // Up is is pressed & lifted?
            if (keybState.IsKeyDown(Keys.Up))
                keyState[GameConstants.KEY_UP] = true;
            else if (keyState[GameConstants.KEY_UP] && keybState.IsKeyUp(Keys.Up))
            {
                keyState[GameConstants.KEY_UP] = false;
                menuSelectOption = (menuSelectOption + menuSelectCount - 1) % menuSelectCount;
                return;
            }

            // Select is pressed & lifted
            if (keybState.IsKeyDown(Keys.Space) || keybState.IsKeyDown(Keys.Enter))
                keyState[GameConstants.KEY_CLIMB] = true;
            else if (keyState[GameConstants.KEY_CLIMB] && keybState.IsKeyUp(Keys.Enter) && keybState.IsKeyUp(Keys.Space)) 
            {
                keyState[GameConstants.KEY_CLIMB] = false;
                // Some selection was made. Depending on which, do something.
                menuLogic_ItemSelected();
            }
        }

        /* When something was selected on the menu, what do we do? */
        private void menuLogic_ItemSelected()
        {
            // we're in main menu.
            if (toDisplay == GameConstants.DISPLAY_MENU)
            {
                switch (menuSelectOption)
                {
                    case GameConstants.MENU_NEW_GAME:
                        setLevel(1);
                        break;
                    case GameConstants.MENU_CONTINUE:
                        toDisplay = GameConstants.DISPLAY_GAME;
                        pastMouseState = Mouse.GetState();
                        if (state == null)
                            setLevel(currentLevel);
                        bgColorGoal = player.spriteColour.colourBackground;
                        break;
                    case GameConstants.MENU_SETTINGS:
                        setSettingsDisplay();
                        break;
                    case GameConstants.MENU_EXIT:
                        this.Exit();
                        break; // Gives error without this.
                }
            }
            // we're in settings menu.
            else if (toDisplay == GameConstants.DISPLAY_SETTINGS)
            {
                switch (menuSelectOption)
                {
                    case GameConstants.MENU_SETTINGS_SOUND:
                        if (gameSettings.Sound_On)
                        {
                            gameSettings.Sound_On = false;
                            // lol this is so cheat....
                            MediaPlayer.Volume = 0;
                            //soundEngineInstance2.Volume = 0;
                        }
                        else
                        {
                            gameSettings.Sound_On = true;
                            MediaPlayer.Volume = 1;
                            //soundEngineInstance2.Volume = 1;
                        }
                        break;
                    case GameConstants.MENU_SETTINGS_TIPS:
                        if (gameSettings.Show_Tips)
                            gameSettings.Show_Tips = false;
                        else
                            gameSettings.Show_Tips = true;
                        break;
                    case GameConstants.MENU_SETTINGS_NUM_OPTIONS-1: //ie exit
                        toDisplay = GameConstants.DISPLAY_MENU;
                        break;
                }
            }
        }
        #endregion

        /* 
         ------------------------------------------------------
         *                 GAMEPAD RELATED
         ------------------------------------------------------ */
        #region GamePadDuringGame


        /**
         * Handle picking up blocks.
         */
        private void gamepadInputPickup()
        {
            Controller.UpdateGameControllerState();
            if (Controller.LeftShoulderButton || Controller.RightShoulderButton)
                buttonState[GameConstants.BUTTON_LIFT] = true;

            // Button is pressed
            if (buttonState[GameConstants.BUTTON_LIFT] && !Controller.LeftShoulderButton && !Controller.RightShoulderButton)
            {
                // Kill all other executable actions that may exist.
                flushKeyState();
                flushButtonState();

                Vector3 lookingAt = whereAmILookingAt();
                int xbox = (int)lookingAt.X + (int)player.position.X;
                int zbox = (int)lookingAt.Z + (int)player.position.Z;

                // are we at end of the world?
                if (xbox < 0 || xbox >= state[0][0].Length)
                    return; // EotW
                if (zbox < 0 || zbox >= state[0].Length)
                    return; // EotW

                if (!currentAction[GameConstants.ACTION_LIFT])
                {
                    // Check to see if we're already as high as we can go.
                    if ((int)player.position.Y == state.Length - 1)
                        return;

                    GameRoom aboveBox = state[(int)player.position.Y + 1][zbox][xbox];
                    GameRoom resultRoom = state[(int)player.position.Y + 1][(int)player.position.Z][(int)player.position.X];
                    int currentObject = getGridContents(xbox, (int)player.position.Y, zbox);

                    // is there something on top of you?
                    if (getGridContents((int)player.position.X, (int)player.position.Y + 1, (int)player.position.Z) > 0
                        || !colourMatch(resultRoom.wall_floor, player.spriteColour.colourId))
                        return; // no room to pick up

                    // is there something to pick up?
                    if (getGridContents(xbox, (int)player.position.Y, zbox) <= 0 && getGridContents(xbox, (int)player.position.Y + 1, zbox) <= 0)
                        return; // Nothing to pick up

                    // Check above the box.
                    if (getGridContents(xbox, (int)player.position.Y + 1, zbox) <= 0)
                    {
                        // Is the character the right colour?
                        if (!yourColorMatch((int)player.position.Y, zbox, xbox))
                            return;

                        // Don't move it if there are barriers in the way.
                        if (!colourMatch(aboveBox.wall_floor, currentObject))
                            return;

                        if ((int)-lookingAt.X == -1)
                        {
                            // We're looking at a move left. We check the left wall of
                            // the current room, and the right wall of the new room.
                            if (!colourMatch(aboveBox.wall_left, currentObject)
                                || !colourMatch(resultRoom.wall_right, currentObject))
                                return;
                        }
                        else if ((int)-lookingAt.X == 1)
                        {
                            // We're mvoing right.
                            if (!colourMatch(aboveBox.wall_right, currentObject)
                                || !colourMatch(resultRoom.wall_left, currentObject))
                                return;
                        }
                        if ((int)-lookingAt.Z == -1)
                        {
                            // We are moving forwards.
                            if (!colourMatch(aboveBox.wall_up, currentObject)
                                || !colourMatch(resultRoom.wall_down, currentObject))
                                return;
                        }
                        else if ((int)-lookingAt.Z == 1)
                        {
                            // Moving backwards.
                            if (!colourMatch(aboveBox.wall_down, currentObject)
                                || !colourMatch(resultRoom.wall_up, currentObject))
                                return;
                        }

                        // Nothing on top. Pick it up!
                        currentAction[GameConstants.ACTION_LIFT] = true;

                        moveBlock(xbox, (int)player.position.Y, zbox, (int)-lookingAt.X, 1, (int)-lookingAt.Z);
                    }
                    // Is there something on top of this box?
                    // TODO: remove this?
                    else if (getGridContents(xbox, (int)player.position.Y + 1, zbox) == 1)
                        return;

                    // there's a 3rd box. can't pick up. too high. (???)
                    else if (getGridContents(xbox, (int)player.position.Y + 2, zbox) > 0)
                        return;

                    // There is a block on top of the block directly infront. Can we pick this
                    // block up?
                    else if (getGridContents(xbox, (int)player.position.Y + 1, zbox) > 1)
                    {
                        // Is the character the right colour?
                        if (!yourColorMatch((int)player.position.Y + 1, zbox, xbox))
                            return;

                        currentAction[GameConstants.ACTION_LIFT] = true;

                        moveBlock(xbox, (int)player.position.Y + 1, zbox, (int)-lookingAt.X, 0, (int)-lookingAt.Z);

                    }

                }
                else // put it down now
                {
                    if (getGridContents(xbox, (int)player.position.Y, zbox) == 0)
                    { // empty space. put it down!
                        moveBlock((int)player.position.X, (int)player.position.Y + 1, (int)player.position.Z,
                            (int)lookingAt.X, 0, (int)lookingAt.Z);

                        currentAction[GameConstants.ACTION_LIFT] = false;

                        // does it need to fall???
                        int ybox = (int)player.position.Y + 1;
                        while (validMove(ybox, zbox, xbox, -1, 0, 0))
                        {
                            moveBlock(xbox, ybox, zbox, 0, -1, 0);
                            ybox--;
                        }
                    }
                    else if (getGridContents(xbox, (int)player.position.Y + 1, zbox) == 0)
                    {
                        moveBlock((int)player.position.X, (int)player.position.Y + 1, (int)player.position.Z,
                            (int)lookingAt.X, 0, (int)lookingAt.Z);
                        currentAction[GameConstants.ACTION_LIFT] = false;
                    }
                    else
                    {
                        // do nothing. something's there
                        return;
                    }
                }
            }
        }
        
        /**
         * Handle picking up blocks.
         */
        private void gamepadInputPickupNew()
        {
            Controller.UpdateGameControllerState();
            if (Controller.XButton)
                buttonState[GameConstants.BUTTON_LIFT] = true;

            // Button is pressed
            if (buttonState[GameConstants.BUTTON_LIFT] && !Controller.XButton)
            {
                // Kill all other executable actions that may exist.
                flushKeyState();
                flushButtonState();

                Vector3 lookingAt = whereAmILookingAt();
                int xbox = (int)lookingAt.X + (int)player.position.X;
                int zbox = (int)lookingAt.Z + (int)player.position.Z;

                // are we at end of the world?
                if (xbox < 0 || xbox >= state[0][0].Length)
                    return; // EotW
                if (zbox < 0 || zbox >= state[0].Length)
                    return; // EotW

                if (!currentAction[GameConstants.ACTION_LIFT])
                {
                    // Check to see if we're already as high as we can go.
                    if ((int)player.position.Y == state.Length - 1)
                        return;

                    GameRoom aboveBox = state[(int)player.position.Y + 1][zbox][xbox];
                    GameRoom resultRoom = state[(int)player.position.Y + 1][(int)player.position.Z][(int)player.position.X];
                    int currentObject = getGridContents(xbox, (int)player.position.Y, zbox);

                    // is there something on top of you?
                    if (getGridContents((int)player.position.X, (int)player.position.Y + 1, (int)player.position.Z) > 0
                        || !colourMatch(resultRoom.wall_floor, player.spriteColour.colourId))
                        return; // no room to pick up

                    // is there something to pick up?
                    if (getGridContents(xbox, (int)player.position.Y, zbox) <= 0 && getGridContents(xbox, (int)player.position.Y + 1, zbox) <= 0)
                        return; // Nothing to pick up

                    // Check above the box.
                    if (getGridContents(xbox, (int)player.position.Y + 1, zbox) <= 0)
                    {
                        // Is the character the right colour?
                        if (!yourColorMatch((int)player.position.Y, zbox, xbox))
                            return;

                        // Don't move it if there are barriers in the way.
                        if (!colourMatch(aboveBox.wall_floor, currentObject))
                            return;

                        if ((int)-lookingAt.X == -1)
                        {
                            // We're looking at a move left. We check the left wall of
                            // the current room, and the right wall of the new room.
                            if (!colourMatch(aboveBox.wall_left, currentObject)
                                || !colourMatch(resultRoom.wall_right, currentObject))
                                return;
                        }
                        else if ((int)-lookingAt.X == 1)
                        {
                            // We're mvoing right.
                            if (!colourMatch(aboveBox.wall_right, currentObject)
                                || !colourMatch(resultRoom.wall_left, currentObject))
                                return;
                        }
                        if ((int)-lookingAt.Z == -1)
                        {
                            // We are moving forwards.
                            if (!colourMatch(aboveBox.wall_up, currentObject)
                                || !colourMatch(resultRoom.wall_down, currentObject))
                                return;
                        }
                        else if ((int)-lookingAt.Z == 1)
                        {
                            // Moving backwards.
                            if (!colourMatch(aboveBox.wall_down, currentObject)
                                || !colourMatch(resultRoom.wall_up, currentObject))
                                return;
                        }

                        // Nothing on top. Pick it up!
                        currentAction[GameConstants.ACTION_LIFT] = true;

                        moveBlock(xbox, (int)player.position.Y, zbox, (int)-lookingAt.X, 1, (int)-lookingAt.Z);
                    }
                    // Is there something on top of this box?
                    // TODO: remove this?
                    else if (getGridContents(xbox, (int)player.position.Y + 1, zbox) == 1)
                        return;

                    // there's a 3rd box. can't pick up. too high. (???)
                    else if (getGridContents(xbox, (int)player.position.Y + 2, zbox) > 0)
                        return;

                    // There is a block on top of the block directly infront. Can we pick this
                    // block up?
                    else if (getGridContents(xbox, (int)player.position.Y + 1, zbox) > 1)
                    {
                        // Is the character the right colour?
                        if (!yourColorMatch((int)player.position.Y + 1, zbox, xbox))
                            return;

                        currentAction[GameConstants.ACTION_LIFT] = true;

                        moveBlock(xbox, (int)player.position.Y + 1, zbox, (int)-lookingAt.X, 0, (int)-lookingAt.Z);

                    }

                }
                else // put it down now
                {
                    if (getGridContents(xbox, (int)player.position.Y, zbox) == 0)
                    { // empty space. put it down!
                        moveBlock((int)player.position.X, (int)player.position.Y + 1, (int)player.position.Z,
                            (int)lookingAt.X, 0, (int)lookingAt.Z);

                        currentAction[GameConstants.ACTION_LIFT] = false;

                        // does it need to fall???
                        int ybox = (int)player.position.Y + 1;
                        while (validMove(ybox, zbox, xbox, -1, 0, 0))
                        {
                            moveBlock(xbox, ybox, zbox, 0, -1, 0);
                            ybox--;
                        }
                    }
                    else if (getGridContents(xbox, (int)player.position.Y + 1, zbox) == 0)
                    {
                        moveBlock((int)player.position.X, (int)player.position.Y + 1, (int)player.position.Z,
                            (int)lookingAt.X, 0, (int)lookingAt.Z);
                        currentAction[GameConstants.ACTION_LIFT] = false;
                    }
                    else
                    {
                        // do nothing. something's there
                        return;
                    }
                }
            }
        }

        /** Player orientation, movement and jumping*/
        private void gamepadInputMovement()
        {

            Controller.UpdateGameControllerState();
            // face a direction first.
            if (Controller.LeftButton || Controller.LeftThumbLeft)
            {
                buttonState[GameConstants.BUTTON_LEFT] = true;
            }
            else if (Controller.RightButton || Controller.LeftThumbRight)
            {
                buttonState[GameConstants.BUTTON_RIGHT] = true;
            }
            else if (Controller.UpButton || Controller.LeftThumbUp)
            {
                buttonState[GameConstants.BUTTON_UP] = true;
            }
            else if (Controller.DownButton || Controller.LeftThumbDown)
            {
                buttonState[GameConstants.BUTTON_DOWN] = true;
            }

            // try to jump
            if (Controller.LeftTrigger || Controller.RightTrigger)
                buttonState[GameConstants.BUTTON_CLIMB] = true;

            // jump up. this cancels the release of keys to go another step.
            if (buttonState[GameConstants.BUTTON_CLIMB] && !Controller.LeftTrigger && !Controller.RightTrigger)
                handleClimb();

            // go to the direction when the key is released.
            if (buttonState[GameConstants.BUTTON_LEFT] && !Controller.LeftButton && !Controller.LeftThumbLeft)
            {
                if (whereAmILookingAt() == adaptToCameraAngle(DIR_LEFT))
                    handleHorizontalMovement(-1, 0);
                else
                    rotatePlayer(DIR_LEFT);
            }

            if (buttonState[GameConstants.BUTTON_RIGHT] && !Controller.RightButton && !Controller.LeftThumbRight)
            {
                if (whereAmILookingAt() == adaptToCameraAngle(DIR_RIGHT))
                    handleHorizontalMovement(1, 0);
                else
                    rotatePlayer(DIR_RIGHT);
            }

            if (buttonState[GameConstants.BUTTON_UP] && !Controller.UpButton && !Controller.LeftThumbUp)
            {
                if (whereAmILookingAt() == adaptToCameraAngle(DIR_UP))
                    handleHorizontalMovement(0, -1);
                else
                    rotatePlayer(DIR_UP);
            }

            if (buttonState[GameConstants.BUTTON_DOWN] && !Controller.DownButton && !Controller.LeftThumbDown)
            {
                if (whereAmILookingAt() == adaptToCameraAngle(DIR_DOWN))
                    handleHorizontalMovement(0, 1);
                else
                    rotatePlayer(DIR_DOWN);
            }
        }

        /** Player orientation, movement and jumping
         * This version uses the right thumbstick for colour, not movement*/
        private void gamepadInputMovementNew()
        {

            Controller.UpdateGameControllerState();
            // face a direction first.
            if (Controller.LeftButton)
            {
                buttonState[GameConstants.BUTTON_LEFT] = true;
            }
            else if (Controller.RightButton)
            {
                buttonState[GameConstants.BUTTON_RIGHT] = true;
            }
            else if (Controller.UpButton)
            {
                buttonState[GameConstants.BUTTON_UP] = true;
            }
            else if (Controller.DownButton)
            {
                buttonState[GameConstants.BUTTON_DOWN] = true;
            }

            // try to jump
            if (Controller.AButton)
                buttonState[GameConstants.BUTTON_CLIMB] = true;

            // jump up. this cancels the release of keys to go another step.
            if (buttonState[GameConstants.BUTTON_CLIMB] && !Controller.AButton)
                handleClimb();

            // go to the direction when the key is released.
            if (buttonState[GameConstants.BUTTON_LEFT] && !Controller.LeftButton)
            {
                if (whereAmILookingAt() == adaptToCameraAngle(DIR_LEFT))
                    handleHorizontalMovement(-1, 0);
                else
                    rotatePlayer(DIR_LEFT);
            }

            if (buttonState[GameConstants.BUTTON_RIGHT] && !Controller.RightButton)
            {
                if (whereAmILookingAt() == adaptToCameraAngle(DIR_RIGHT))
                    handleHorizontalMovement(1, 0);
                else
                    rotatePlayer(DIR_RIGHT);
            }

            if (buttonState[GameConstants.BUTTON_UP] && !Controller.UpButton)
            {
                if (whereAmILookingAt() == adaptToCameraAngle(DIR_UP))
                    handleHorizontalMovement(0, -1);
                else
                    rotatePlayer(DIR_UP);
            }

            if (buttonState[GameConstants.BUTTON_DOWN] && !Controller.DownButton)
            {
                if (whereAmILookingAt() == adaptToCameraAngle(DIR_DOWN))
                    handleHorizontalMovement(0, 1);
                else
                    rotatePlayer(DIR_DOWN);
            }
        }

        private void changeToColor(int colour)
        {
            // Ensure we're trying to switch to a 'real' colour
            if (!(colour == GameConstants.COLOUR_BLUE
                || colour == GameConstants.COLOUR_YELLOW
                || colour == GameConstants.COLOUR_RED
                || colour == GameConstants.COLOUR_BLACK))
            { return; }

            // See if we're already that colour
            GameColour currentColour = player.spriteColour;
            if (colour != currentColour.colourId)
            {
                GameColour nextColour = spriteColours[colour];

                // Update player and background.
                player.model = nextColour.colourModel;
                player.spriteColour = nextColour;
                bgColorGoal = nextColour.colourBackground;
                soundEngineInstance2 = soundChange.CreateInstance();
                soundEngineInstance2.Play();
                if (Controller.IsConnected) {
                    Controller.Vibrate_Short();
                    elapsedTime = 0f;
                }

            }
        }

        private void changeToColorNew(int colour)
        {
            // Ensure we're trying to switch to a 'real' colour
            if (!(colour == GameConstants.COLOUR_BLUE
                || colour == GameConstants.COLOUR_YELLOW
                || colour == GameConstants.COLOUR_RED
                || colour == GameConstants.COLOUR_BLACK))
            { return; }

            // See if we're already that colour
            GameColour currentColour = player.spriteColour;
            if (colour != currentColour.colourId)
            {
                GameColour nextColour = spriteColours[colour];

                // Update player and background.
                player.model = nextColour.colourModel;
                player.spriteColour = nextColour;
                bgColorGoal = nextColour.colourBackground;
                //soundEngineInstance2 = soundChange.CreateInstance();
                //soundEngineInstance2.Play();
                if (Controller.IsConnected)
                {
                    Controller.Vibrate_Short();
                    elapsedTime = 0f;
                }

            }
        }

        /** Handles the issues of cat changing colors */
        private void gamepadInputColorChange()
        {
            // If the player is carrying a block, we cannot change our colour.
            if (currentAction[GameConstants.ACTION_LIFT])
                return;

            Controller.UpdateGameControllerState();

            if (Controller.XButton)
                buttonState[GameConstants.BUTTON_CHANGE_BLUE] = true;

            if (Controller.YButton)
                buttonState[GameConstants.BUTTON_CHANGE_YELLOW] = true;

            if (Controller.BButton)
                buttonState[GameConstants.BUTTON_CHANGE_RED] = true;

            if (Controller.AButton)
                buttonState[GameConstants.BUTTON_CHANGE_BLACK] = true;

            // Change color.
            if (buttonState[GameConstants.BUTTON_CHANGE_BLUE] && !Controller.XButton)
            {
                changeToColor(GameConstants.COLOUR_BLUE);
                buttonState[GameConstants.BUTTON_CHANGE_BLUE] = false;
            }
            else if (buttonState[GameConstants.BUTTON_CHANGE_YELLOW] && !Controller.YButton)
            {
                changeToColor(GameConstants.COLOUR_YELLOW);
                buttonState[GameConstants.BUTTON_CHANGE_YELLOW] = false;
            }
            else if (buttonState[GameConstants.BUTTON_CHANGE_RED] && !Controller.BButton)
            {
                changeToColor(GameConstants.COLOUR_RED);
                buttonState[GameConstants.BUTTON_CHANGE_RED] = false;
            }
            else if (buttonState[GameConstants.BUTTON_CHANGE_BLACK] && !Controller.AButton)
            {
                changeToColor(GameConstants.COLOUR_BLACK);
                buttonState[GameConstants.BUTTON_CHANGE_BLACK] = false;
            }
            else
                return;

            flushButtonState();
        }

        /** Handles the issues of cat changing colors 
         * This version uses the right thumbstick for colour-choice
         */
        private void gamepadInputColorChangeNew()
        {
            // If the player is carrying a block, we cannot change our colour.
            if (currentAction[GameConstants.ACTION_LIFT])
                return;

            Controller.UpdateGameControllerState();

            if (Controller.RightThumbUp && ! Controller.RightThumbRight)
                buttonState[GameConstants.BUTTON_CHANGE_BLUE] = true;

            if (Controller.RightThumbDown && !Controller.RightThumbRight)
                buttonState[GameConstants.BUTTON_CHANGE_YELLOW] = true;

            if (Controller.RightThumbRight)
                buttonState[GameConstants.BUTTON_CHANGE_RED] = true;

            if (Controller.RightThumbButton)
                buttonState[GameConstants.BUTTON_CHANGE_BLACK] = true;

            // Change color.
            if (buttonState[GameConstants.BUTTON_CHANGE_BLUE])
            {
                changeToColor(GameConstants.COLOUR_BLUE);
                buttonState[GameConstants.BUTTON_CHANGE_BLUE] = false;
            }
            else if (buttonState[GameConstants.BUTTON_CHANGE_YELLOW])
            {
                changeToColor(GameConstants.COLOUR_YELLOW);
                buttonState[GameConstants.BUTTON_CHANGE_YELLOW] = false;
            }
            else if (buttonState[GameConstants.BUTTON_CHANGE_RED])
            {
                changeToColor(GameConstants.COLOUR_RED);
                buttonState[GameConstants.BUTTON_CHANGE_RED] = false;
            }
            else if (buttonState[GameConstants.BUTTON_CHANGE_BLACK] && !Controller.RightThumbButton)
            {
                changeToColor(GameConstants.COLOUR_BLACK);
                buttonState[GameConstants.BUTTON_CHANGE_BLACK] = false;
            }
            else
                return;

            flushButtonState();
        }

        private void gamepadInputMisc()
        {
            Controller.UpdateGameControllerState();

            // 'Start' pressed: reset the level.
            if (Controller.StartButton)
                setLevel(currentLevel);

            // 'Back' pressed: go to title or exit game.
            if (Controller.BackButton)
                buttonState[GameConstants.BUTTON_EXIT] = true;

            if (buttonState[GameConstants.BUTTON_EXIT] && !Controller.BackButton)
            {

                // Otherwise, go to the menu screen.
                bgColorGoal = Color.DarkGray;
                toDisplay = GameConstants.DISPLAY_MENU;
                MediaPlayer.Pause();
                menuSelectOption = GameConstants.MENU_CONTINUE;
                keyState[GameConstants.BUTTON_EXIT] = false;
            }

        }

        #region JoystickLook
        private void JoystickLook()
        {
            Controller.UpdateGameControllerState();
            
            //MouseState current_mouse = Mouse.GetState();
            float joyX = Controller.RightThumbX;
            float joyY = Controller.RightThumbY;
            Vector3 camLook = _camera.LookAt;
            Vector3 camPos = _camera.Position - camLook;    //so camera will follow player 

            // set the new camera position with a rotation matrix based on 
            // x-dir stick movement...
            float angle = (joyX) / 30.0f;
            Matrix camRot = Matrix.CreateRotationY(angle);
            camPos = Vector3.Transform(camPos, camRot);

            // ...and a height/zoom based on y-dir stick movement.
            float height = (joyY) / 2.0f;
            camPos.Y += height;
            if (camPos.Y < -2) { camPos.Y = -2; }

            _camera.Position = camLook + camPos;

            // press the stick to reset the camera
            if (Controller.RightThumbButton)
            {
                this._camera.Position = new Vector3(4.0f, 10.0f, 9.0f);
            }

        }
        #endregion JoystickLook

        #endregion GamePadDuring Game
        #region GamePadDuringMenu
        private void gamepadMenuInput()
        {
            Controller.UpdateGameControllerState();

            // Back Button
            if (Controller.BackButton)
            {   // Exits the game no matter what
                // even if you don't write this code.... strange
                this.Exit();
            }

            // Down Button - D-Pad
            if (Controller.DownButton)
            {
                buttonState[GameConstants.BUTTON_DOWN] = true;
            }
            else if (buttonState[GameConstants.BUTTON_DOWN] && !Controller.DownButton)
            {
                buttonState[GameConstants.BUTTON_DOWN] = false;
                menuSelectOption = (menuSelectOption + 1) % menuSelectCount;
                return;
            }

            // Up Button - D-Pad
            if (Controller.UpButton)
            {
                buttonState[GameConstants.BUTTON_UP] = true;
            }
            else if (buttonState[GameConstants.BUTTON_UP] && !Controller.UpButton)
            {
                buttonState[GameConstants.BUTTON_UP] = false;
                menuSelectOption = (menuSelectOption + menuSelectCount - 1) % menuSelectCount;
                return;
            }

            // A or X Buttons
            if (Controller.AButton || Controller.XButton)
            {
                menuLogic_ItemSelected();
            }

        }
        #endregion

        /* 
         ------------------------------------------------------
         *                 MOUSE RELATED
         ------------------------------------------------------ */
        #region Mouse
        private void mouseUpdateMovement()
        {
            MouseState current_mouse = Mouse.GetState();
            int mouseX = current_mouse.X;
            int mouseY = current_mouse.Y;
            Vector3 camLook = _camera.LookAt;
            Vector3 camPos = _camera.Position - camLook;    //so camera will follow player 

            // set the new camera position with a rotation matrix based on 
            // x-dir mouse movement...
            float angle = (pastMouseState.X - mouseX) / 80.0f;
            Matrix camRot = Matrix.CreateRotationY(angle);
            camPos = Vector3.Transform(camPos, camRot);

            // ...and a height/zoom based on y-dir mouse movement.
            float height = (mouseY - pastMouseState.Y) / 10.0f;
            camPos.Y += height;
            if (camPos.Y < -2) { camPos.Y = -2; }
            
            _camera.Position = camLook + camPos;
            
            // update the last known mouse position
            pastMouseState = Mouse.GetState();

            // left-click to reset the camera
            if (Mouse.GetState().LeftButton == ButtonState.Pressed) {
                this._camera.Position = new Vector3(4.0f, 10.0f, 9.0f);
            }

        }
        #endregion


        /* 
         ------------------------------------------------------
         *                 LOGIC RELATED
         ------------------------------------------------------ */
        #region Logic Related

        // set the level to the desired level
        protected void setLevel(int level)
        {
            currentLevel = level;
            // Create and load up the contents of the rooms.
            // TODO: add support for barriers.
            state = GetLevel.getLevel(level);

            toDisplay = GameConstants.DISPLAY_GAME;
            MediaPlayer.Resume();

            pastMouseState = Mouse.GetState();
            this._camera.Position = new Vector3(4.0f, 10.0f, 9.0f);

            Vector3 p = findTheDamnCharacter(-1);
            theEnd.position = findTheDamnCharacter(-2);
            setGridContents((int)p.X, (int)p.Y, (int)p.Z, 0);  // we know where he was now. no longer need that info.
            setGridContents((int)theEnd.position.X, (int)theEnd.position.Y, (int)theEnd.position.Z, 0); //same
            if (cat == null)
                player = new Player(p.X, p.Y, p.Z, null, 0); // cat's not loaded yet.
            else
                player = new Player(p.X, p.Y, p.Z, cat, 0);

            player.spriteColour = spriteColours[GameConstants.COLOUR_BLACK];

            bgColorGoal = Color.DarkGray;

            // Flush all actions.
            for (int i = 0; i < currentAction.Length; i++)
                currentAction[i] = false;
        }


        // Find out where the character/end game poit/etc is
        private Vector3 findTheDamnCharacter(int type)
        {
            for (int y = 0; y < state.Length; y++)
            { // height
                GameRoom[][] plane = state[y];
                for (int z = 0; z < plane.Length; z++)
                { // depth
                    GameRoom[] line = plane[z];
                    for (int x = 0; x < line.Length; x++)
                    { // width
                        if (line[x].contentObject == type)
                            return new Vector3(x, y, z);
                    }
                }
            }
            return new Vector3(0, 0, 0);
        }

        // Did we win?
        private void checkWin()
        {
            if (player.position.Equals(theEnd.position))
            {
                // WE WON!!!

                //shake the controller
                if (Controller.IsConnected)
                {
                    Controller.Vibrate_Very_Long();
                    elapsedTime = 0f;
                }
                if (currentLevel == GetLevel.totalLevels())
                { // Game over!!!
                    currentLevel = 1;
                    toDisplay = GameConstants.DISPLAY_COMPLETE;
                    bgColorGoal = Color.DarkGray;
                }
                else
                {
                    toDisplay = GameConstants.DISPLAY_END;
                    soundEngineInstance2 = soundWin.CreateInstance();
                    soundEngineInstance2.IsLooped = false;
                    soundEngineInstance2.Play();
                }
            }
        }

        private bool yourColorMatch(int y, int z, int x)
        {
            // already assume this is a colored movable block
            return yourColorMatch(state[y][z][x].contentObject);
        }

        private bool yourColorMatch(int blockcolor)
        {
            // If the object is colourless, then just let it pass through.
            if (blockcolor == GameConstants.COLOUR_NONE)
                return true;

            // black cat... i got nothing for it now.
            if (player.spriteColour.colourId == 1 || blockcolor == 1)
                return false;
            
            // colored cat... yes, you can move some stuff
            if (Math.Abs(player.spriteColour.colourId - blockcolor) <= 1)
                return true;
            if (blockcolor == 7 && player.spriteColour.colourId == 2)
                return true;
            if (blockcolor == 2 && player.spriteColour.colourId == 7)
                return true;

            return false;
        }
        
        private bool colourMatch(int objectOne, int objectTwo)
        {
            // If either object is colourless, then just let it pass through.
            if (objectOne == GameConstants.COLOUR_NONE || objectTwo == GameConstants.COLOUR_NONE)
                return true;

            if (objectOne == GameConstants.COLOUR_BLACK || objectTwo == GameConstants.COLOUR_BLACK)
                return false;

            if (Math.Abs(objectOne - objectTwo) <= 1)
                return true;
            if (objectOne == 7 && objectTwo == 2)
                return true;
            if (objectOne == 2 && objectTwo == 7)
                return true;
            return false;
        }

        /* check that there's nothing behind the block being pushed.
         * Additionally, check that there's not too many stuff on top of it to be pushed.
         * 
         * the block is located at the coords x, y, z in the order below.
         * The block is to be moved in the dx and dz direction. They are mutually exclusive.
         * 
         * Implementation of this function will solve index out of bounds and
         * killing other blocks.
         */
        private bool validMove(int y, int z, int x, int dy, int dz, int dx)
        {
            // check index out of bounds
            if ((y == 0 && dx < 0) || (y == state.Length - 1 && dx > 0))
                return false;
            if ((z == 0 && dz < 0) || (z == state[0].Length - 1 && dz > 0))
                return false;
            if ((x == 0 && dx < 0) || (x == state[0][0].Length - 1 && dx > 0))
                return false;

            GameRoom currentRoom = state[y][z][x];
            GameRoom newRoom = state[y + dy][z + dz][x + dx];
            int currentObject = currentRoom.contentObject;

            // Ensure that there is nothing in the new room.
            if (getGridContents(x + dx, y + dy, z + dz) == 0)
            {
                // We need to check to ensure that we follow the rules if we
                // pass through coloured barriers.
                if (dx == -1)
                {
                    // We're looking at a move left. We check the left wall of
                    // the current room, and the right wall of the new room.
                    if (!colourMatch(currentRoom.wall_left, currentObject)
                        || !colourMatch(newRoom.wall_right, currentObject))
                        return false;
                }
                else if (dx == 1)
                {
                    // We're mvoing right.
                    if (!colourMatch(currentRoom.wall_right, currentObject)
                        || !colourMatch(newRoom.wall_left, currentObject))
                        return false;
                }
                if (dz == -1)
                {
                    // We are moving forwards.
                    if (!colourMatch(currentRoom.wall_up, currentObject)
                        || !colourMatch(newRoom.wall_down, currentObject))
                        return false;
                }
                else if (dz == 1)
                {
                    // Moving backwards.
                    if (!colourMatch(currentRoom.wall_down, currentObject)
                        || !colourMatch(newRoom.wall_up, currentObject))
                        return false;
                }

                if (dy == 1)
                {
                    // We are moving up.
                    if (!colourMatch(newRoom.wall_floor, currentObject))
                        return false;
                }
                else if (dy == -1)
                {
                    // We are moving down.
                    if (!colourMatch(currentRoom.wall_floor, currentObject))
                        return false;
                }

                // We checked the boundaries. We're good.
                return true;
            }

            return false;
        }

        private bool validMove(Vector3 from, Vector3 to)
        {
            return validMove((int)from.Y, (int)from.Z, (int)from.X, (int)to.Y, (int)to.Z, (int)to.X);
        }

        /**
         * Function for moving blocks. I moved it out of the huge playerMovement method. If
         * we want to implement coloured floors, here's where the logic should go.
         */
        public bool handleBlockCollision(int x, int z)
        {
            // Get the content of the grid block we want to move to. Current implementation
            // has a int value (stored in 3D array state). Positive values currently indicate
            // blocks.
            int gridContent = getGridContents((int)player.position.X + x, (int)player.position.Y, (int)player.position.Z + z);

            // you just ran into an unmovable wall. kthxbye
            if (gridContent == GameConstants.COLOUR_BLACK)
                return false;

            // you just ran into a movable block... but... ???
            else if (gridContent > GameConstants.COLOUR_BLACK || gridContent == GameConstants.COLOUR_NONE)
            {
                if (yourColorMatch(gridContent)
                    && validMove((int)player.position.Y, (int)player.position.Z + z, (int)player.position.X + x, 0, z, x))
                {
                    // there's something on top of this box.
                    int above = getGridContents((int)player.position.X + x, (int)player.position.Y + 1, (int)player.position.Z + z);
                    bool ontop = false;
                    if (above != 0 && above != 1 && above != -1)
                    {
                        // something on top cannot be moved with this.
                        if (!yourColorMatch(above))
                            return false;
                        else if (!validMove((int)player.position.Y + 1, (int)player.position.Z + z, (int)player.position.X + x, 0, z, x))
                            return false;
                        else // you CAN move it.
                        {
                            // Make sure there aren't too many blocks ontop.
                            // NOTE: If we want to do the keystone thing, (i.e., move other blocks
                            // in to support a structure so we can take other blocks out) we might
                            // need to change this. Actually, we'd need to change a lot.
                            int stackedBlock = getGridContents((int)player.position.X + x, (int)player.position.Y + 2, (int)player.position.Z + z);

                            if (stackedBlock != 0 && stackedBlock != 1 && stackedBlock != GameConstants.COLOUR_NONE)
                                return false;

                            ontop = true;
                        }
                    }
                    ///////////////////////////
                    // move the box
                    ///////////////////////////
                    moveBlock((int)player.position.X + x, (int)player.position.Y, (int)player.position.Z + z, x, 0, z);

                    if (ontop)
                        moveBlock((int)player.position.X + x, (int)player.position.Y + 1, (int)player.position.Z + z, x, 0, z);

                    // does it ned to fall?
                    int ybox = (int)player.position.Y;
                    int xbox = (int)player.position.X + x + x;
                    int zbox = (int)player.position.Z + z + z;

                    while (validMove(ybox, zbox, xbox, -1, 0, 0))
                    {
                        if (ybox <= 0) // at bottom
                            break;
                        moveBlock(xbox, ybox, zbox, 0, -1, 0);

                        if (ontop && validMove(ybox + 1, zbox, xbox, -1, 0, 0))
                            moveBlock(xbox, ybox + 1, zbox, 0, -1, 0);
                        else
                            ontop = false;
                        ybox--;
                    }
                }
                else // you can't push it. something's behind it or your color doesn't match.
                    return false;
            }
            else
            {
                // You didn't run into anything! Just make sure we didn't hit a wall we can't go through.
                // TODO: tidy up.
                GameRoom currentRoom = state[(int)player.position.Y][(int)player.position.Z][(int)player.position.X];
                GameRoom newRoom = state[(int)player.position.Y][z + (int)player.position.Z][x + (int)player.position.X];

                if (x == -1)
                {
                    // We're looking at a move left. We check the left wall of
                    // the current room, and the right wall of the new room.
                    if (!colourMatch(currentRoom.wall_left, player.spriteColour.colourId)
                        || !colourMatch(newRoom.wall_right, player.spriteColour.colourId))
                        return false;
                }
                else if (x == 1)
                {
                    // We're mvoing right.
                    if (!colourMatch(currentRoom.wall_right, player.spriteColour.colourId)
                        || !colourMatch(newRoom.wall_left, player.spriteColour.colourId))
                        return false;
                }
                if (z == -1)
                {
                    // We are moving forwards.
                    if (!colourMatch(currentRoom.wall_up, player.spriteColour.colourId)
                        || !colourMatch(newRoom.wall_down, player.spriteColour.colourId))
                        return false;
                }
                else if (z == 1)
                {
                    // Moving backwards.
                    if (!colourMatch(currentRoom.wall_down, player.spriteColour.colourId)
                        || !colourMatch(newRoom.wall_up, player.spriteColour.colourId))
                        return false;
                }
            }
            return true;
        }

        /**
         * Method to handle climbing/jumping.
         */
        private void handleClimb()
        {
            Vector3 facing = whereAmILookingAt();

            // can't jump while lifting
            if (currentAction[GameConstants.ACTION_LIFT])
                return;

            // the basic jump up.
            Vector3 go = facing + Vector3.UnitY;
            Vector3 to = player.position + go;
            
            if (getGridContents(to) < 0)
            {
                //out of bounds
                flushKeyState();
                flushButtonState();
                return;
            }

            GameRoom loc = state[(int)to.Y][(int)to.Z][(int)to.X];
            
            if (player.position.Y < state.Length - 1 && getGridContents(player.position + Vector3.UnitY) != 0)
            {
                // something's on top. You can't jump. >.<
                flushKeyState();
                flushButtonState();
                return;
            }
            if (getGridContents(player.position + facing) <= 0 && yourColorMatch(loc.wall_floor))
                // there's nothing to stand on, so just move straight ahead
                go = facing;
            if (getGridContents(player.position + go) > 0)
            {
                // jumping into a wall
                flushKeyState();
                flushButtonState();
                return;
            }

            int x = (int)go.X;
            int y = (int)go.Y;
            int z = (int)go.Z;

            GameRoom aboveCurrentRoom = state[(int)player.position.Y+y][(int)player.position.Z][(int)player.position.X];
            GameRoom nextRoom = state[(int)player.position.Y + y][(int)player.position.Z + z][(int)player.position.X + x];

            if (nextRoom.contentObject > 0)
            {
                flushKeyState();
                flushButtonState();
                return;
            }

            // Check to make sure you aren't breaking any barrier rules.
            if (y > 0 && !yourColorMatch(aboveCurrentRoom.wall_floor))
            {
                flushKeyState();
                flushButtonState();
                return;
            }

            if (x == -1)
            {
                // We're looking at a move left. We check the left wall of
                // the current room, and the right wall of the new room.
                if (!yourColorMatch(aboveCurrentRoom.wall_left) || !yourColorMatch(nextRoom.wall_right))
                {
                    flushKeyState();
                    flushButtonState();
                    return;
                }
            }
            else if (x == 1)
            {
                // We're mvoing right.
                if (!yourColorMatch(aboveCurrentRoom.wall_right) || !yourColorMatch(nextRoom.wall_left))
                {
                    flushKeyState();
                    flushButtonState();
                    return;
                }
            }
            if (z == -1)
            {
                // We are moving forwards.
                if (!yourColorMatch(aboveCurrentRoom.wall_up) || !yourColorMatch(nextRoom.wall_down))
                {
                    flushKeyState();
                    flushButtonState();
                    return;
                }
            }
            else if (z == 1)
            {
                // Moving backwards.
                if (!yourColorMatch(aboveCurrentRoom.wall_down) || !yourColorMatch(nextRoom.wall_up))
                {
                    flushKeyState();
                    flushButtonState();
                    return;
                }
            }

            playerMovement(go);
            currentAction[GameConstants.ACTION_CLIMB] = false;
            flushKeyState();
            flushButtonState();
        }

        #endregion

        #region Menu Logic
        private void setSettingsDisplay()
        {
            toDisplay = GameConstants.DISPLAY_SETTINGS;
        }
        #endregion

        /* 
         ------------------------------------------------------
         *                 DRAW STUFF RELATED
         ------------------------------------------------------ */
        #region Drawing

        Color bgColor = Color.Black;
        Color bgColorGoal = Color.Bisque;

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            float moveHeight = moveQuad();
            this._camera.Position = _camera.Position - _camera.LookAt + player.position - moveDist * horizDir - moveHeight * vertDir;
            this._camera.LookAt = player.position - moveDist * horizDir - moveHeight * vertDir;

            this._camera.Update();
            GraphicsDevice.Clear(bgColor);

            gotoColor();

            switch (toDisplay)
            {
                case GameConstants.DISPLAY_GAME:
                    DrawCharacter();
                    DrawTheEnd();
                    DrawBoxes();
                    DrawBarriers();

                    spriteBatch.Begin();
                    DrawColorWheel();
                    DrawInGameText();
                    DrawFade(transitionCounter * 5 / 6);
                    spriteBatch.End();
                    GraphicsDevice.RenderState.DepthBufferEnable = true;
                    break;
                case GameConstants.DISPLAY_MENU:
                    bgColorGoal = Color.Black;
                    spriteBatch.Begin();
                    DrawMainMenu();
                    spriteBatch.End();
                    break;
                case GameConstants.DISPLAY_END:
                    DrawCharacter();
                    recolorBoxes();
                    DrawBoxes();
                    DrawBarriers();

                    spriteBatch.Begin();
                    DrawFade(++transitionCounter);
                    DrawEndLevel();
                    DrawInGameText();
                    spriteBatch.End();
                    break;
                case GameConstants.DISPLAY_COMPLETE:
                    spriteBatch.Begin();
                    DrawEndofGame();
                    spriteBatch.End();
                    break;
                case GameConstants.DISPLAY_SETTINGS:
                    spriteBatch.Begin();
                    DrawSettingsMenu();
                    spriteBatch.End();
                    break;
            }

            base.Draw(gameTime);
        }
        
        private void gotoColor()
        {
            if (bgColor.R < bgColorGoal.R)
                bgColor.R++;
            if (bgColor.G < bgColorGoal.G)
                bgColor.G++;
            if (bgColor.B < bgColorGoal.B)
                bgColor.B++;
            if (bgColor.R > bgColorGoal.R)
                bgColor.R--;
            if (bgColor.G > bgColorGoal.G)
                bgColor.G--;
            if (bgColor.B > bgColorGoal.B)
                bgColor.B--;
        }

        /* 
         ---------------------------------------
         *          DRAW - SETTINGS
         --------------------------------------- */
        private void DrawSettingsMenu()
        {
            int widescreenOffset = ((float)screenwidth / (float)screenheight > 1.4f) ? -45 : 0;

            float resolutionScale = screenwidth / 800f;
            float menu_item_scale = 0.4f;
            int xpos_menu_items = (int)(240 * resolutionScale);
            int ypos_menu_items = (int)((250 + widescreenOffset) * resolutionScale);
            int ypos_menu_increment = (int)(55 * resolutionScale);
            int x_size = (int)(800 * menu_item_scale * resolutionScale);
            int y_size = (int)(150 * menu_item_scale * resolutionScale);
            int x_select_size = (int)(253 * menu_item_scale * .5f * resolutionScale);
            int y_select_size = (int)(239 * menu_item_scale * .5f * resolutionScale);
            int xpos_selection = (int)(230 * resolutionScale);
            int ypos_selection = (int)((255 + widescreenOffset) * resolutionScale);

            Rectangle title = new Rectangle(0, (int)((-150 + widescreenOffset) * resolutionScale), (int)(800 * resolutionScale), (int)(600 * resolutionScale));
            spriteBatch.Draw(menuTitle, title, Color.White);
            
            // Sound
            spriteBatch.Draw(settingText[GameConstants.MENU_SETTINGS_SOUND],
                new Rectangle(xpos_menu_items, ypos_menu_items + GameConstants.MENU_SETTINGS_SOUND * ypos_menu_increment,
                    x_size, y_size), Color.White);
            if (gameSettings.Sound_On)
                spriteBatch.Draw(settingText[GameConstants.MENU_SETTINGS_ON],
                    new Rectangle(xpos_menu_items, ypos_menu_items + GameConstants.MENU_SETTINGS_SOUND * ypos_menu_increment,
                        x_size, y_size), Color.White);
            else
                spriteBatch.Draw(settingText[GameConstants.MENU_SETTINGS_OFF],
                    new Rectangle(xpos_menu_items, ypos_menu_items + GameConstants.MENU_SETTINGS_SOUND * ypos_menu_increment,
                        x_size, y_size), Color.White);

            // Show Tips
            spriteBatch.Draw(settingText[GameConstants.MENU_SETTINGS_TIPS],
                new Rectangle(xpos_menu_items, ypos_menu_items + GameConstants.MENU_SETTINGS_TIPS * ypos_menu_increment,
                    x_size, y_size), Color.White);
            if (gameSettings.Show_Tips)
                spriteBatch.Draw(settingText[GameConstants.MENU_SETTINGS_ON],
                    new Rectangle(xpos_menu_items, ypos_menu_items + GameConstants.MENU_SETTINGS_TIPS * ypos_menu_increment,
                        x_size, y_size), Color.White);
            else
                spriteBatch.Draw(settingText[GameConstants.MENU_SETTINGS_OFF],
                    new Rectangle(xpos_menu_items, ypos_menu_items + GameConstants.MENU_SETTINGS_TIPS * ypos_menu_increment,
                        x_size, y_size), Color.White);

            spriteBatch.Draw(menuText[GameConstants.MENU_EXIT],
                new Rectangle(xpos_menu_items, ypos_menu_items + (GameConstants.MENU_SETTINGS_NUM_OPTIONS-1) * ypos_menu_increment,
                    x_size, y_size), Color.White);

            // Which one is selected
            spriteBatch.Draw(menuText[GameConstants.MENU_SELECT],
                new Rectangle(xpos_selection, ypos_selection + menuSelectOption * ypos_menu_increment,
                    x_select_size, y_select_size), Color.White);
        }

        /* 
         ---------------------------------------
         *          DRAW - MENU & STUFF
         --------------------------------------- */
        private void DrawMainMenu()
        {
            int widescreenOffset = ((float)screenwidth / (float)screenheight > 1.4f) ? -45 : 0;
            
            float resolutionScale = screenwidth / 800f;
            float menu_item_scale = 0.4f;
            int xpos_menu_items = (int)(240 * resolutionScale);
            int ypos_menu_items = (int)((250 + widescreenOffset) * resolutionScale);
            int ypos_menu_increment =  (int)(55 * resolutionScale);
            int x_size = (int)(800 * menu_item_scale * resolutionScale);
            int y_size = (int)(150 * menu_item_scale * resolutionScale);
            int x_select_size = (int)(253 * menu_item_scale * .5f * resolutionScale);
            int y_select_size = (int)(239 * menu_item_scale * .5f * resolutionScale);
            int xpos_selection =  (int)(230 * resolutionScale);
            int ypos_selection = (int)((255 + widescreenOffset) * resolutionScale);

            Rectangle title = new Rectangle(0, (int)((-150+widescreenOffset) * resolutionScale), (int)(800 * resolutionScale), (int)(600 * resolutionScale));
            spriteBatch.Draw(menuTitle, title, Color.White);
            spriteBatch.Draw(menuText[GameConstants.MENU_NEW_GAME],
                new Rectangle(xpos_menu_items, ypos_menu_items + GameConstants.MENU_NEW_GAME * ypos_menu_increment, 
                    x_size, y_size), Color.White);
            spriteBatch.Draw(menuText[GameConstants.MENU_CONTINUE],
                new Rectangle(xpos_menu_items, ypos_menu_items + GameConstants.MENU_CONTINUE * ypos_menu_increment, 
                    x_size, y_size), Color.White);
            /* spriteBatch.Draw(menuText[GameConstants.MENU_CUSTOM],
                new Rectangle(xpos_menu_items, ypos_menu_items + GameConstants.MENU_CUSTOM * ypos_menu_increment, 
                    x_size, y_size), Color.White);
            spriteBatch.Draw(menuText[GameConstants.MENU_EDITOR],
                new Rectangle(xpos_menu_items, ypos_menu_items + GameConstants.MENU_EDITOR * ypos_menu_increment, 
                    x_size, y_size), Color.White); */
            spriteBatch.Draw(menuText[GameConstants.MENU_SETTINGS],
                new Rectangle(xpos_menu_items, ypos_menu_items + GameConstants.MENU_SETTINGS * ypos_menu_increment, 
                    x_size, y_size), Color.White);
            spriteBatch.Draw(menuText[GameConstants.MENU_EXIT],
                new Rectangle(xpos_menu_items, ypos_menu_items + GameConstants.MENU_EXIT * ypos_menu_increment, 
                    x_size, y_size), Color.White);

            // Which one is selected
            spriteBatch.Draw(menuText[GameConstants.MENU_SELECT],
                new Rectangle(xpos_selection, ypos_selection + menuSelectOption * ypos_menu_increment,
                    x_select_size, y_select_size), Color.White);
        }

        /* 
         ---------------------------------------
         *          DRAW - In between game stuff
         --------------------------------------- */

        /*Gives colour to uncoloured boxes, spreading outward from player*/
        private void recolorBoxes()
        {
            for (int y = 0; y < state.Length; y++)
            { // height
                GameRoom[][] plane = state[y];
                for (int z = 0; z < plane.Length; z++)
                { // depth
                    GameRoom[] row = plane[z];
                    for (int x = 0; x < row.Length; x++)
                    { // width
                        float dist = Vector3.Distance(new Vector3(x, y, z), player.position);
                        if (transitionCounter / 4f > dist && row[x].contentObject == 1)
                        {
                            row[x].contentObject = ((transitionCounter/2) % 7) + 1; //nice rainbow effect
                        }
                    }
                }
            }
        }

        /*fade to white*/
        private void DrawFade(int count)
        {
            transitionCounter = (int)MathHelper.Clamp(count, 0, 8192);
            Color fadeVal = new Color(Color.White, MathHelper.Clamp(transitionCounter / 40f, 0f, 1f));
            spriteBatch.Draw(blank, new Rectangle(0, 0, screenwidth, screenheight), fadeVal);
        }

        private void DrawEndLevel()
        {
            float resolutionScale = screenheight / 600f;
            int offsetx = (screenwidth - (screenheight * 800 / 600)) / 2;

            Rectangle rect = new Rectangle(offsetx, 0, (int)(800 * resolutionScale), (int)(600 * resolutionScale));
            spriteBatch.Draw(colourRestoredTexture, rect, Color.White);
            /*
            // Future ---
            switch (currentLevel)
            {
                case 1:
                    break;
            } */
        }

        /* 
         ---------------------------------------
         *          DRAW - End of the game
         --------------------------------------- */
        private void DrawEndofGame()
        {
            float resolutionScale = screenheight / 600f;
            int offsetx = (screenwidth - (screenheight * 800 / 600)) / 2;

            Rectangle rect = new Rectangle(offsetx, 0, (int)(800 * resolutionScale), (int)(600 * resolutionScale));
            spriteBatch.Draw(theEndTexture, rect, Color.White);

            GraphicsDevice.RenderState.DepthBufferEnable = true;

            _camera.Position = new Vector3(0f, 0f, -10f);
            _camera.LookAt = new Vector3(0f, 0f, 0f);
            player.rotation += 0.02f;
            Matrix[] transforms = new Matrix[player.model.Bones.Count];
            player.model.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in player.model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index];
                    effect.World *= Matrix.CreateScale(scaleFactor * 10, scaleFactor * 10, scaleFactor * 10);
                    effect.World *= Matrix.CreateRotationY(player.rotation);
                    effect.World *= Matrix.CreateTranslation(new Vector3(0f,-10f,0f));
                    effect.View = this._camera.ViewMatrix;
                    effect.Projection = this._camera.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }

        /* 
         ---------------------------------------
         *        DRAW - CURRENTLY PLAYING
         --------------------------------------- */
        
        private void DrawColorWheel()
        {
            float resolutionScale = screenwidth / 800f;
            Rectangle rect = new Rectangle((int)(680 * resolutionScale),(int)(20 *resolutionScale), 
                (int)(100 * resolutionScale), (int)(100 * resolutionScale));
            spriteBatch.Draw(colorWheelTextures[player.spriteColour.colourId], rect, Color.White);
        }

        private void DrawInGameText()
        {
            float resolutionScale = screenwidth / 800f;
            spriteBatch.DrawString(menuFont, "Level " + currentLevel.ToString(), 
                new Vector2((int)(360 * resolutionScale), (int)(20 * resolutionScale)), Color.White);
            if (gameSettings.Show_Tips)
                spriteBatch.DrawString(tipsFont, GetLevel.getTips(currentLevel), new Vector2(20, 120), Color.White);
        }

        private void DrawCharacter()
        {
            #region Somewhat const vars
            // Constants that can't be declared as constants... o.O
            Vector3 ARROW_HEIGHT = new Vector3(0f, 0.5f, 0f);
            #endregion

            //smooth movement
            float moveHeight = moveQuad();
            moveDist = MathHelper.Clamp(moveDist - 0.13f, 0f, 1f); 

            Matrix[] transforms = new Matrix[player.model.Bones.Count];
            player.model.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in player.model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] *
                        Matrix.CreateScale(scaleFactor, scaleFactor, scaleFactor) *
                        Matrix.CreateRotationY(player.rotation) *
                        Matrix.CreateTranslation(player.position - horizDir * moveDist - vertDir * moveHeight);
                    effect.View = this._camera.ViewMatrix;
                    effect.Projection = this._camera.ProjectionMatrix;
                }
                mesh.Draw();
            }
            /// And the arrow that points at the direction.
            /// Only show when we're lifting something.
            if (currentAction[GameConstants.ACTION_LIFT])
            {
                transforms = new Matrix[directionArrow.model.Bones.Count];
                directionArrow.model.CopyAbsoluteBoneTransformsTo(transforms);
                directionArrow.rotation += 0.05f;
                foreach (ModelMesh mesh in directionArrow.model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.World = transforms[mesh.ParentBone.Index];
                        effect.World *= Matrix.CreateScale(scaleFactor / 2, scaleFactor / 2, scaleFactor / 2);
                        effect.World *= Matrix.CreateRotationX(directionArrow.rotation);
                        if (whereAmILookingAt().Equals(DIR_LEFT))
                            effect.World *= Matrix.CreateRotationY((float)Math.PI);
                        else if (whereAmILookingAt().Equals(DIR_UP))
                            effect.World *= Matrix.CreateRotationY((float)Math.PI * 0.5f);
                        else if (whereAmILookingAt().Equals(DIR_DOWN))
                            effect.World *= Matrix.CreateRotationY((float)Math.PI * 1.5f);
                        effect.World *= Matrix.CreateTranslation(player.position + whereAmILookingAt() + ARROW_HEIGHT);
                        effect.View = this._camera.ViewMatrix;
                        effect.Projection = this._camera.ProjectionMatrix;
                    }
                    mesh.Draw();
                }
            }
        }

        private void DrawTheEnd()
        {
            theEnd.rotation += 0.01f;
            Matrix[] transforms = new Matrix[theEnd.model.Bones.Count];
            theEnd.model.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in theEnd.model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effect.Alpha = 0.5f;
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationY(theEnd.rotation) * Matrix.CreateScale(scaleFactor * 0.6f, scaleFactor * 0.6f, scaleFactor * 0.6f) * Matrix.CreateTranslation(theEnd.position);
                    effect.View = this._camera.ViewMatrix;
                    effect.Projection = this._camera.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }

        private void DrawBoxes()
        {
            for (int y = 0; y < state.Length; y++)
            { // height
                GameRoom[][] plane = state[y];
                for (int z = 0; z < plane.Length; z++)
                { // depth
                    GameRoom[] row = plane[z];
                    for (int x = 0; x < row.Length; x++)
                    { // width
                        if (row[x].contentObject == 0) continue;
                        else drawABox(x, y, z, row[x].contentObject);
                    }
                }
            }
        }

        private void DrawBarriers()
        {
            for (int y = 0; y < state.Length; y++)
            { // height
                GameRoom[][] plane = state[y];
                for (int z = 0; z < plane.Length; z++)
                { // depth
                    GameRoom[] row = plane[z];
                    for (int x = 0; x < row.Length; x++)
                    { // width
                        if (row[x].wall_down != GameConstants.COLOUR_NONE)
                            drawABarrier(x, y, z, GameConstants.WALL_DOWN, row[x].wall_down);

                        if (row[x].wall_up != GameConstants.COLOUR_NONE)
                            drawABarrier(x, y, z, GameConstants.WALL_UP, row[x].wall_up);

                        if (row[x].wall_left != GameConstants.COLOUR_NONE)
                            drawABarrier(x, y, z, GameConstants.WALL_LEFT, row[x].wall_left);

                        if (row[x].wall_right != GameConstants.COLOUR_NONE)
                            drawABarrier(x, y, z, GameConstants.WALL_RIGHT, row[x].wall_right);

                        if (row[x].wall_floor != GameConstants.COLOUR_NONE)
                            drawABarrier(x, y, z, GameConstants.WALL_FLOOR, row[x].wall_floor);
                    }
                }
            }
        }

        private void drawABox(int x, int y, int z, int type)
        {
            Model thisModel;
            if (type == 1)
                thisModel = greyBlock;
            else if (type == 2)
                thisModel = redBlock;
            else if (type == 3)
                thisModel = orangeBlock;
            else if (type == 4)
                thisModel = yellowBlock;
            else if (type == 5)
                thisModel = greenBlock;
            else if (type == 6)
                thisModel = blueBlock;
            // This is wrong. once the level has been loaded, it no longer has this info.
            // TODO remove this once read.
            //else if (type == -2)
            //    thisModel = whiteBall;
            else if (type == 7)
                thisModel = purpleBlock;
            else
                return; // bad block
            Vector3 position = new Vector3(x, y, z);

            Matrix[] transforms = new Matrix[thisModel.Bones.Count];
            thisModel.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in thisModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    if (currentAction[GameConstants.ACTION_LIFT]
                        && x == (int)player.position.X && z == (int)player.position.Z && y == (int)player.position.Y + 1)
                        effect.Alpha = 0.6f;
                    else
                        effect.Alpha = 1f;
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateScale(scaleFactor,scaleFactor,scaleFactor) * Matrix.CreateTranslation(position);
                    effect.View = this._camera.ViewMatrix;
                    effect.Projection = this._camera.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }

        private void drawABarrier(int x, int y, int z, int wall, int type)
        {
            Model thisModel;
            if (type == 1)
                thisModel = greyBlock;
            else if (type == GameConstants.COLOUR_RED)
                thisModel = barrierBlockModel[GameConstants.COLOUR_RED];
            else if (type == GameConstants.COLOUR_ORANGE)
                thisModel = barrierBlockModel[GameConstants.COLOUR_ORANGE];
            else if (type == GameConstants.COLOUR_YELLOW)
                thisModel = barrierBlockModel[GameConstants.COLOUR_YELLOW];
            else if (type == GameConstants.COLOUR_GREEN)
                thisModel = barrierBlockModel[GameConstants.COLOUR_GREEN];
            else if (type == GameConstants.COLOUR_BLUE)
                thisModel = barrierBlockModel[GameConstants.COLOUR_BLUE];
            else if (type == GameConstants.COLOUR_PURPLE)
                thisModel = barrierBlockModel[GameConstants.COLOUR_PURPLE];
            else
                return; // bad block

            Vector3 position = new Vector3(x, y, z);
            float scaleX = scaleFactor, scaleY = scaleFactor, scaleZ = scaleFactor;

            // Change values depending on what wall we have.
            if (wall == GameConstants.WALL_DOWN)
            {
                scaleZ *= 0.0001f;
                position.Z += 0.5f;
            }
            else if (wall == GameConstants.WALL_UP)
            {
                scaleZ *= 0.0001f;
                position.Z -= 0.5f;
            }
            else if (wall == GameConstants.WALL_LEFT)
            {
                scaleX *= 0.0001f;
                position.X -= 0.5f;
            }
            else if (wall == GameConstants.WALL_RIGHT)
            {
                scaleX *= 0.0001f;
                position.X += 0.5f;
            }
            else
                scaleY *= 0.0001f;

            Matrix[] transforms = new Matrix[thisModel.Bones.Count];
            thisModel.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in thisModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.Alpha = 0.4f;
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateScale(scaleX, scaleY, scaleZ) * Matrix.CreateTranslation(position);
                    effect.View = this._camera.ViewMatrix;
                    effect.Projection = this._camera.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }
        
        #endregion

        /*----------------------------------------------
         *  NEW HELPER METHODS
         *----------------------------------------------*/
        #region New Helper Methods

        /**
         * Flush the last key pressed.
         */
        private void flushKeyState()
        {
            for (int i = 0; i < keyState.Length; i++)
                keyState[i] = false;
        }

        private void flushButtonState()
        {
            for (int i = 0; i < buttonState.Length; i++)
                buttonState[i] = false;
        }

        /**
         * These method will help if/once we implement more complex "rooms" (i.e., special
         * floors, etc.) We want to separate the contents of the room from the room itself.
         */
        public int getGridContents(int x, int y, int z)
        {
            // We use the "none" colour to indicate index-out-of-bounds.
            if (y < 0 || z < 0 || x < 0)
                //return GameConstants.COLOUR_NONE;
                return -1;
            if (y > state.Length - 1 || z > state[0].Length - 1 || x > state[0][0].Length - 1)
                //return GameConstants.COLOUR_NONE;
                return -1;

            return state[y][z][x].contentObject;
        }

        public int getGridContents(Vector3 loc)
        {
            return getGridContents((int)loc.X, (int)loc.Y, (int)loc.Z);
        }

        public bool setGridContents(int x, int y, int z, int content)
        {
            if (y < 0 || z < 0 || x < 0)
                return false;
            if (y > state.Length - 1 || z > state[0].Length - 1 || x > state[0][0].Length - 1)
                return false;

            state[y][z][x].contentObject = content;
            return true;
        }

        /**
         * Also here if we want to implement more complex rooms. We want to change the
         * contents of the room, not the state (barriers, etc.)
         * 
         * Note that it still refers to the state array. We'll need to change this with the
         * new implementations.
         */
        public void moveBlock(int x, int y, int z, int dx, int dy, int dz)
        {
            setGridContents(x + dx, y + dy, z + dz, getGridContents(x, y, z));
            setGridContents(x, y, z, 0);
        }

        /**
         * Makes vertical movement look more realistic.
         */
        public float moveQuad()
        {
            double bar = Math.Pow(3.0 + 1.5*Math.Sign(vertDir.Y), -vertDir.Y);
            return 1f-(float)Math.Pow(1f-moveDist, bar);
        }

        /**
         * This helps in detecting the screen size and then changing the display size as well
         */
        void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            DisplayMode displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            if (gameSettings.MAXIMIZE_SCREEN)
            {
                e.GraphicsDeviceInformation.PresentationParameters.BackBufferFormat = displayMode.Format;
                e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth = displayMode.Width;
                e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight = displayMode.Height;
                screenheight = displayMode.Height;
                screenwidth = displayMode.Width;
            }
            else // aka do nothing
            {
                screenheight = 600;
                screenwidth = 800;
            }
        }

        #endregion
    }
}
