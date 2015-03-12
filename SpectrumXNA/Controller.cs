using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Text;

namespace SpectrumXNA
{
    public class Controller
    {
        public static GamePadState state;
        private static float rumbleTimer;
        private static bool enableRumble;

        public static void UpdateGameControllerState()
        {
            // Processing the Controller Input

            //TODO: This assumes only 1 controller is connected
            state = GamePad.GetState(0);


            //if (!state.IsConnected) throw new Exception("Controller is not connected");
        }

        public static bool IsConnected { get { return state.IsConnected; } }

        #region Controller State
        // ABXY buttons
        public static bool AButton { get { return (state.Buttons.A == ButtonState.Pressed); } }
        public static bool BButton { get { return (state.Buttons.B == ButtonState.Pressed); } }
        public static bool XButton { get { return (state.Buttons.X == ButtonState.Pressed); } }
        public static bool YButton { get { return (state.Buttons.Y == ButtonState.Pressed); } }
        //Start and Back buttons
        public static bool BackButton { get { return (state.Buttons.Back == ButtonState.Pressed); } }
        public static bool StartButton { get { return (state.Buttons.Start == ButtonState.Pressed); } }
        //Thumb sticks - As buttons
        public static bool LeftThumbButton { get { return (state.Buttons.LeftStick == ButtonState.Pressed); } }
        public static bool LeftThumbLeft { get { return state.IsButtonDown(Buttons.LeftThumbstickLeft); } }
        public static bool LeftThumbRight { get { return state.IsButtonDown(Buttons.LeftThumbstickRight); } }
        public static bool LeftThumbUp { get { return state.IsButtonDown(Buttons.LeftThumbstickUp); } }
        public static bool LeftThumbDown { get { return state.IsButtonDown(Buttons.LeftThumbstickDown); } }
        public static bool RightThumbButton { get { return (state.Buttons.RightStick == ButtonState.Pressed); } }
        public static bool RightThumbLeft { get { return state.IsButtonDown(Buttons.RightThumbstickLeft); } }
        public static bool RightThumbRight { get { return state.IsButtonDown(Buttons.RightThumbstickRight); } }
        public static bool RightThumbUp { get { return state.IsButtonDown(Buttons.RightThumbstickUp); } }
        public static bool RightThumbDown { get { return state.IsButtonDown(Buttons.RightThumbstickDown); } }
        //Thumb Sticks - As err...sticks
        public static float LeftThumbX { get { return state.ThumbSticks.Left.X; } }
        public static float LeftThumbY { get { return state.ThumbSticks.Left.Y; } }
        public static float RightThumbX { get { return state.ThumbSticks.Right.X; } }
        public static float RightThumbY { get { return state.ThumbSticks.Right.Y; } }

        //Direction Pad
        public static bool DownButton { get { return (state.DPad.Down == ButtonState.Pressed); } }
        public static bool RightButton { get { return (state.DPad.Right == ButtonState.Pressed); } }
        public static bool UpButton { get { return (state.DPad.Up == ButtonState.Pressed); } }
        public static bool LeftButton { get { return (state.DPad.Left == ButtonState.Pressed); } }
        //Shoulder Buttons
        public static bool LeftShoulderButton { get { return (state.Buttons.LeftShoulder == ButtonState.Pressed); } }
        public static bool RightShoulderButton { get { return (state.Buttons.RightShoulder == ButtonState.Pressed); } }
        //Triggers
        public static bool LeftTrigger { get { return state.IsButtonDown(Buttons.LeftTrigger); } }
        public static bool RightTrigger { get { return state.IsButtonDown(Buttons.RightTrigger); } }
        //public static float LeftTrigger { get { return state.Triggers.Left; } }
        //public static float RightTrigger { get { return state.Triggers.Right; } }
        

        //public static GamePadButtons Buttons { get{ return state.GamePad.Buttons; } }
        #endregion

        //Vibrate the controller
        public static void SetVibration(float LeftMotorSpeed, float RightMotorSpeed)
        {
            GamePad.SetVibration(0, LeftMotorSpeed, RightMotorSpeed);
        }

    
        public static void updateVibrations(float elapsed)
        {
            if (enableRumble)
            {
                if (rumbleTimer > 0f)
                {
                    rumbleTimer -= elapsed;
                }
                else
                {
                    enableRumble = false;
                    rumbleTimer = 0f;
                    GamePad.SetVibration(0, 0, 0);
                }
            }
        }

        // Effect when you move
        public static void Vibrate_Short()
        {
            rumbleTimer = .5f;
            enableRumble = true;
            GamePad.SetVibration(0, 1, 1);
        }

        // Effect ?
        public static void Vibrate_Long()
        {
            rumbleTimer = 1.5f;
            enableRumble = true;
            GamePad.SetVibration(0, .6f, .6f);
        }

        // Effect when you win
        public static void Vibrate_Very_Long()
        {
            rumbleTimer = 3.5f;
            enableRumble = true;
            GamePad.SetVibration(0, .2f, .3f);
        }

    }
}
