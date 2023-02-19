
using Assembee.Game.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game {
    public class WindowHandler {

        private const int DEFAULT_WINDOW_WIDTH = 960;
        private const int DEFAULT_WINDOW_HEIGHT = 540;
        private const int DEFAULT_FRAME_RATE = 60;

        private int screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        private int screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

        public int windowWidth { get; private set; } = DEFAULT_WINDOW_WIDTH;
        public int windowHeight { get; private set; } = DEFAULT_WINDOW_HEIGHT;

        private Microsoft.Xna.Framework.Game game;

        private World world;

        private GraphicsDeviceManager graphicsManager;
        private SpriteBatch spriteBatch;
        private RenderTarget2D renderTarget;

        public bool isFullscreen { get; private set; } = false;

        private int animTick;

        public WindowHandler(Microsoft.Xna.Framework.Game game, World world) {
            this.game = game;
            this.world = world;

            graphicsManager = new GraphicsDeviceManager(game);
            graphicsManager.PreferMultiSampling = false;
            graphicsManager.SynchronizeWithVerticalRetrace = true;

            animTick = 0;
        }

        public void Init() {
            renderTarget = new RenderTarget2D(game.GraphicsDevice, DEFAULT_WINDOW_WIDTH, DEFAULT_WINDOW_HEIGHT);
            graphicsManager.PreferredBackBufferWidth = windowWidth;
            graphicsManager.PreferredBackBufferHeight = windowHeight;

            // This code doesn't work with fullscreen

            /*
            game.Window.AllowUserResizing = true;
            game.Window.ClientSizeChanged += OnResize;
            */
            graphicsManager.ApplyChanges();

            spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }

        public void RenderWorld() {
            game.GraphicsDevice.SetRenderTarget(renderTarget);
            game.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: world.MainCamera.Transform);

            world?.Draw(spriteBatch, animTick);

            animTick++;

            spriteBatch.End();
            game.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(renderTarget, new Rectangle(0, 0, windowWidth, windowHeight), Color.White);
            spriteBatch.End();

        }

        public void RenderHUD(Game1.Building selectedBuilding) {
            spriteBatch.Begin();

            HUD.DrawHud(spriteBatch, world, selectedBuilding);

            spriteBatch.End();
        }

        public void RenderMenu() {
            spriteBatch.Begin();
            game.GraphicsDevice.Clear(Color.CornflowerBlue);
            HUD.DrawMenu(spriteBatch);
            spriteBatch.End();
        }

        public void SetFullscreen(bool fullscreen) {
            isFullscreen = fullscreen;

            if (fullscreen) {
                graphicsManager.HardwareModeSwitch = false;
                Resolution(screenWidth, screenHeight, true);
            } else {
                Resolution(DEFAULT_WINDOW_WIDTH, DEFAULT_WINDOW_HEIGHT, false);
            }

        }

        public void ToggleFullscreen() {
            SetFullscreen(!isFullscreen);
        }

        private void OnResize(object sender, EventArgs e) {
            Resolution(game.Window.ClientBounds.Width, game.Window.ClientBounds.Height, false);
        }

        public void Resolution(int w, int h, bool fullscreen) {
            renderTarget = new RenderTarget2D(game.GraphicsDevice, w, h);
            graphicsManager.PreferredBackBufferWidth = w;
            graphicsManager.PreferredBackBufferHeight = h;
            windowWidth = w;
            windowHeight = h;
            graphicsManager.IsFullScreen = fullscreen;
            graphicsManager.ApplyChanges();
        }

    }
}
