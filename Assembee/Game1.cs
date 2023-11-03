using Assembee.Game;
using Assembee.Game.Entities;
using Assembee.Game.GameMath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assembee
{
    public class Game1 : Microsoft.Xna.Framework.Game {

        public static WindowHandler windowHandler;

        public Camera camera;
        public World world;

        public static Audio audio;
        public static SpriteFont font24;
        public static SpriteFont font36;

        public enum Building {
            Apartment,
            HoneyProducer,
            WaxProducer
        }

        Building selectedBuilding = Building.Apartment;

        public enum AppState {
            TitleScreen,
            InGame
        }

        public enum GameState {
            Base,
            BuildingTiles,
            BuildingLines,
            Paused
        }

        public static AppState appState = AppState.TitleScreen;
        public static GameState gameState = GameState.Base;

        public static HexPosition beeLineStart;

        public Game1() {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            this.camera = new Camera();
            this.world = new World(camera);

            windowHandler = new WindowHandler(this, world);
        }

        protected override void Initialize() {
            windowHandler.Init();
            base.Initialize();
        }

        protected override void LoadContent() {
            ContentRegistry.LoadContent(Content);
            GameRegistry.Load();

            // Sets the initial resolution
            //graphics = new GraphicsDeviceManager(this);

            font24 = Content.Load<SpriteFont>("arial_24");
            font36 = Content.Load<SpriteFont>("arial_36");

            audio = new Audio(Content);

            HUD.InitHUD(world);
        }

        protected override void Update(GameTime gameTime) {
            // Check for released bettons and update Input
            Input.GetState();

            if (Input.keyPressed(Keys.F)) {
                windowHandler.ToggleFullscreen();
            }

            switch (appState) {
                case AppState.TitleScreen:
                    break;

                case AppState.InGame:
                    GameUpdate(gameTime);
                    break;
            }


            base.Update(gameTime);
        }

        private void GameUpdate(GameTime gameTime) {
            if (Input.keyPressed(Input.SaveGame)) {
                SaveManager.Save(world);
            }

            if (Input.keyPressed(Input.Enter)) {
                SaveManager.Save(world);
                appState = AppState.TitleScreen;
                audio.StopMusic();
                audio.StopAllSounds();
                return;
            }

            if (Input.keyPressed(Input.Mute)) {
                audio.ToggleMute();
            }

            if (Input.keyPressed(Keys.Escape)) {
                gameState = GameState.Base;
            }

            if (Input.keyPressed(Keys.B)) {
                if (gameState == GameState.BuildingTiles) {
                    gameState = GameState.Base;
                } else {
                    gameState = GameState.BuildingTiles;
                }
            }

            if (Input.keyPressed(Keys.V)) {
                if (gameState == GameState.BuildingLines) {
                    gameState = GameState.Base;
                } else {
                    gameState = GameState.BuildingLines;
                    beeLineStart = null;
                }
            }

            /* TODO this should be moved to a separate class called UserInterface or something. */
            switch (gameState) {
                case GameState.Base:
                    if (Input.Click(0) && !HUD.OverActiveElement(Input.getMousePosition().ToPoint())) {
                        Tile tileClicked = world.GetTile(Input.getMouseHexTile(camera));
                        world.SelectTile(tileClicked);
                    }

                    break;

                case GameState.BuildingTiles:
                    if (Input.Click(1) && world.GetTile(Input.getMouseHexTile(camera)) is null && !HUD.OverActiveElement(Input.getMousePosition())) {
                        Tile newTile = null;
                        switch (selectedBuilding) {
                            case Building.Apartment:
                                newTile = new Apartment(ContentRegistry.spr.t_apartments, Input.getMouseHexTile(camera));
                                break;

                            case Building.HoneyProducer:
                                newTile = new FactoryTile(GameRegistry.fct.honeyProduction, Input.getMouseHexTile(camera));
                                break;

                            case Building.WaxProducer:
                                newTile = new FactoryTile(GameRegistry.fct.waxProduction, Input.getMouseHexTile(camera));
                                break;
                        }

                        bool canBuild = !(newTile is null) && IBuildable.CanBuild((IBuildable)newTile, world.MainHive.Inventory);

                        if (canBuild) {
                            foreach (var kvp in ((IBuildable)newTile).BuildingRequirement) {
                                world.MainHive.Inventory[kvp.Key] -= kvp.Value;
                            }

                            world.AddTile(newTile);
                            audio.PlaySound(Audio.sfx.place, 1f, 0f);
                        }
                    }

                    for (int i = 0; i < Input.numKeys.Count; i++) {
                        if (Input.keyPressed(Input.numKeys[i])) {
                             selectedBuilding = (Building)(i + 1);
                        }
                    }

                    break;

                case GameState.BuildingLines:
                    if (Input.Click(1) && !HUD.OverActiveElement(Input.getMousePosition()) && world.GetTile(Input.getMouseHexTile(camera)) is IInventoryTile) {
                        HexPosition inputPosition = Input.getMouseHexTile(camera);
                        if (beeLineStart is null) {
                            beeLineStart = inputPosition;
                        } else {
                            SimpleBeeLine beeLine = new SimpleBeeLine(beeLineStart, inputPosition, world, 10);

                            /* Only add bee line if it actually transports something. */
                            if (!beeLine.TransportMaterials.IsEmpty) {
                                world.AddEntity(beeLine);
                                beeLineStart = null;
                            }
                        }
                    }

                    break;
            }

            camera.Update();
            world?.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            switch (appState) {
                case AppState.InGame:
                    windowHandler.RenderWorld();
                    windowHandler.RenderHUD(selectedBuilding);
                    break;

                case AppState.TitleScreen:
                    windowHandler.RenderMenu();
                    break;

            }

            base.Draw(gameTime);
        }
    }
}
