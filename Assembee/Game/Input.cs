using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input.Touch;

namespace Assembee.Game {
    class Input {


        //public static Btn Up = new Btn(new List<Keys>() { Keys.W, Keys.Up }, new List<Buttons>() { Buttons.LeftThumbstickUp, Buttons.DPadUp });
        public static Btn Up = new Btn(Keys.W, Keys.Up, Buttons.LeftThumbstickUp, Buttons.DPadUp);
        public static Btn Down = new Btn(Keys.S, Keys.Down, Buttons.LeftThumbstickDown, Buttons.DPadDown);
        public static Btn Left = new Btn(Keys.A, Keys.Left, Buttons.LeftThumbstickLeft, Buttons.DPadLeft);
        public static Btn Right = new Btn(Keys.D, Keys.Right, Buttons.LeftThumbstickRight, Buttons.DPadRight);
        public static Btn One = new Btn(Keys.D1);
        public static Btn Two = new Btn(Keys.D2);
        public static Btn Three = new Btn(Keys.D3);
        //public static Btn Four = new Btn(Keys.D4)
        public static List<Btn> numKeys = new List<Btn>() { One, Two, Three };


        public static Btn Enter = new Btn(Keys.Enter);
        public static Btn Mute = new Btn(Keys.M);

        // DEBUG keys
        public static Btn NewGame = new Btn(Keys.Space);
        public static Btn LoadGame = new Btn(Keys.Enter);
        public static Btn SaveGame = new Btn(Keys.J);


        static KeyboardState currentKeyState;
        static KeyboardState previousKeyState;

        static GamePadState currentButtonState;
        static GamePadState previousButtonState;

        public static MouseState currentMouseState;
        public static MouseState previousMouseState;

        static int currentScrollValue;
        static int previousScrollValue;

        

        public Vector2 mPos;

        public int mx;
        public int my;

        private const float SQRT3 = 1.73205080757f;


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
        public static Vector2 getMousePos() {
            return new Vector2(currentMouseState.X, currentMouseState.Y);
        }



        // These were used to get the coordinate of the (square) grid being hovered over
        
        public static Vector2 getMousePosScreen() {
            Vector2 mousePos;
            mousePos.X = Input.getMousePos().X; // (Game1.ScreenWidth / Game1.SCREEN_WIDTH);
            mousePos.Y = Input.getMousePos().Y; // (Game1.ScreenHeight / Game1.SCREEN_HEIGHT);
            return mousePos;
        }

        public static Vector2 getMouseTile(Camera camera) {
            Vector2 mousePos = getMousePosScreen();
            float mX = (mousePos.X - Game1.windowHandler.windowWidth / 2) * camera.scale - camera.approach.X;
            //mX -= mX % Game1.GridSize;
            float mY = (mousePos.Y - Game1.windowHandler.windowHeight / 2) * camera.scale - camera.approach.Y; //mousePos.Y - (camera.approach.Y);
            //mY -= mY % Game1.GridSize;
            return new Vector2((int)mX, (int)mY);
        }

        public static Vector2 getMouseHexTile(Camera camera) {
            Vector2 mousePos = getMouseTile(camera);
            float mX = mousePos.X / (127.0f * (float)Math.Sqrt(3));
            float mY = mousePos.Y / (127.0f * (float)Math.Sqrt(3));

            //float mQ = ((float)Math.Sqrt(3.0f) / 3.0f * mX - 1.0f / 3.0f * mY) / (127.0f);
            //float mR = 2.0f / 3.0f * mY / (127.0f);

            return hexRound(new Vector2(mX, mY));
        }

        private static Vector2 axialRound(Vector2 coord) {
            return cube_to_axial(cube_round(axial_to_cube(coord)));
        }

        private static Vector3 axial_to_cube(Vector2 hex) {
            return new Vector3(hex.X, hex.Y, -hex.X - hex.Y);
        }

        private static Vector2 cube_to_axial(Vector3 cube) {
            return new Vector2(cube.X, cube.Y);
        }

        private static Vector3 cube_round(Vector3 cube) {

            float q = (float)Math.Round(cube.X);
            float r = (float)Math.Round(cube.Y);
            float s = (float)Math.Round(cube.Z);

            float q_diff = (float)Math.Abs(q - cube.X);
            float r_diff = (float)Math.Abs(r - cube.Y);
            float s_diff = (float)Math.Abs(s - cube.Z);

            if (q_diff > r_diff && q_diff > s_diff) {
                q = -r - s;
            } else if (r_diff > s_diff) {
                r = -q - s;
            } else {
                s = -q - r;
            }

            return new Vector3(q, r, s);

        }

        public static Vector2 hexRound(Vector2 coord) {

            float x = 0.5f * (SQRT3 * coord.X - coord.Y) * (float)Math.Sqrt(4.0f / 3.0f);
            float y = coord.Y * (float)Math.Sqrt(4.0f / 3.0f);
            return axialRound(new Vector2(x, y));

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
