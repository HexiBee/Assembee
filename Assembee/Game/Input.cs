using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input.Touch;
using Assembee.Game.GameMath;

namespace Assembee.Game {
    class Input {

        public static readonly Btn Up = new Btn(Keys.W, Keys.Up, Buttons.LeftThumbstickUp, Buttons.DPadUp);
        public static readonly Btn Down = new Btn(Keys.S, Keys.Down, Buttons.LeftThumbstickDown, Buttons.DPadDown);
        public static readonly Btn Left = new Btn(Keys.A, Keys.Left, Buttons.LeftThumbstickLeft, Buttons.DPadLeft);
        public static readonly Btn Right = new Btn(Keys.D, Keys.Right, Buttons.LeftThumbstickRight, Buttons.DPadRight);
        public static readonly Btn One = new Btn(Keys.D1);
        public static readonly Btn Two = new Btn(Keys.D2);
        public static readonly Btn Three = new Btn(Keys.D3);
        public static readonly List<Btn> numKeys = new List<Btn>() { One, Two, Three };

        public static Btn Enter = new Btn(Keys.Enter);
        public static Btn Mute = new Btn(Keys.M);

        // DEBUG keys
        public static readonly Btn NewGame = new Btn(Keys.Space);
        public static readonly Btn LoadGame = new Btn(Keys.Enter);
        public static readonly Btn SaveGame = new Btn(Keys.J);

        private static KeyboardState currentKeyState;
        private static KeyboardState previousKeyState;

        private static GamePadState currentButtonState;
        private static GamePadState previousButtonState;

        private static MouseState currentMouseState;
        private static MouseState previousMouseState;

        static int currentScrollValue;
        static int previousScrollValue;

        

        public Vector2 mPos;

        public int mx;
        public int my;

        // Updates the keyboard states (every frame)
        public static void GetState() {
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();

            previousButtonState = currentButtonState;
            currentButtonState = GamePad.GetState(PlayerIndex.One);

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            previousScrollValue = currentScrollValue;
            currentScrollValue = currentMouseState.ScrollWheelValue;


        }

        // Detects when a key is held down
        public static bool keyDown(Btn btn) {
            //bool down = false;
            if (btn.keys != null)
                foreach (Keys key in btn.keys) {
                    if (keyDown(key)) return true;
                }
            if (btn.buttons != null) 
                foreach (Buttons button in btn.buttons) {
                    if (keyDown(button)) return true;
                }
            return false;
        }
        public static bool keyDown(Keys key) {

            return currentKeyState.IsKeyDown(key);
        }
        public static bool keyDown(Buttons button) {
            return currentButtonState.IsButtonDown(button);
        }

        // Detects the frame when a key is pressed
        public static bool keyPressed(Btn btn) {
            //bool down = false;
            if (btn.keys != null)
                foreach (Keys key in btn.keys) {
                    if (keyPressed(key)) return true;
                }
            if (btn.buttons != null)
                foreach (Buttons button in btn.buttons) {
                    if (keyPressed(button)) return true;
                }
            return false;
        }
        public static bool keyPressed(Keys key) {
            return currentKeyState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key);
        }
        public static bool keyPressed(Buttons button) {
            return currentButtonState.IsButtonDown(button) && !previousButtonState.IsButtonDown(button);
        }


        // Detects the frame a key is released
        public static bool keyReleased(Btn btn) {
            foreach (Keys key in btn.keys) {
                if (keyReleased(key)) return true;
            }
            foreach (Buttons button in btn.buttons) {
                if (keyReleased(button)) return true;
            }
            return false;
        }
        public static bool keyReleased(Keys key) {
            return previousKeyState.IsKeyDown(key) && !currentKeyState.IsKeyDown(key);
        }
        public static bool keyReleased(Buttons button) {
            return previousButtonState.IsButtonDown(button) && !currentButtonState.IsButtonDown(button);
        }

        // Returns the mouse's position 
        public static Vector2 getMousePosition() {
            return new Vector2(currentMouseState.X, currentMouseState.Y);
        }

        public static Vector2 getMouseWorldPosition(Camera camera) {
            Vector2 mousePos = getMousePosition();
            float mX = (mousePos.X - Game1.windowHandler.windowWidth / 2) * camera.Scale + camera.Position.X;
            float mY = (mousePos.Y - Game1.windowHandler.windowHeight / 2) * camera.Scale + camera.Position.Y; 
            return new Vector2((int)mX, (int)mY);
        }

        public static HexPosition getMouseHexTile(Camera camera) {
            Vector2 mousePos = getMouseWorldPosition(camera);

            return HexPosition.PositionToHexPosition(mousePos);
        }

        //direction: -1 is up, 1 is down  ?
        public static int scrollPressed() {
            if (currentScrollValue < previousScrollValue) {
                return 1;
            } else if (currentScrollValue > previousScrollValue) {
                return -1;
            } else {
                return 0;
            }
        }



        // 0 = left, 1 = right (maybe make enum)
        public static bool Click(int a) {
            switch (a) {
                case 0: return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
                case 1: return currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released;
            }
            return false;
        }

        public static bool MouseHold(int a) {
            switch (a) {
                case 0: return currentMouseState.LeftButton == ButtonState.Pressed;
                case 1: return currentMouseState.RightButton == ButtonState.Pressed;
            }
            return false;
        }

        public static bool MouseReleased(int a) {
            switch (a) {
                case 0: return currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed;
                case 1: return currentMouseState.RightButton == ButtonState.Released && previousMouseState.RightButton == ButtonState.Pressed;
            }
            return false;
        }


        // Button struct
        // Used for controller support
        public struct Btn {

            public List<Keys> keys;
            public List<Buttons> buttons;

            public Btn(List<Keys> keys, List<Buttons> buttons) {
                this.keys = keys;
                this.buttons = buttons;
            }

            public Btn(Keys key, Buttons button) {
                keys = new List<Keys>() { key };
                buttons = new List<Buttons>() { button };
            }

            public Btn(Keys key1, Keys key2, Buttons button1, Buttons button2) {
                keys = new List<Keys>() { key1, key2 };
                buttons = new List<Buttons>() { button1, button2 };
            }
            public Btn(Keys key1, Buttons button1, Buttons button2) {
                keys = new List<Keys>() { key1 };
                buttons = new List<Buttons>() { button1, button2 };
            }

            public Btn(Keys key1, Keys key2, Buttons button1) {
                keys = new List<Keys>() { key1, key2 };
                buttons = new List<Buttons>() { button1 };
            }

            public Btn(Keys key1) {
                keys = new List<Keys>() { key1 };
                buttons = null;
            }
            public Btn(Keys key1, Keys key2) {
                keys = new List<Keys>() { key1, key2 };
                buttons = null;
            }
            public Btn(Keys key1, Keys key2, Keys key3) {
                keys = new List<Keys>() { key1, key2, key3 };
                buttons = null;
            }

        }

    }
}
