using Assembee.Game;
using Assembee.Game.Entities;
using Assembee.Game.Entities.Tiles;
using Assembee.Game.GameMath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace Assembee {
    public class Game1 : Microsoft.Xna.Framework.Game {

        public static WindowHandler windowHandler;

        public Camera camera;
        public World world;

        public static Audio audio;
        public static SpriteFont font1;

        public enum Building {
            None,
            Apartment,
            HoneyProducer,
            WaxProducer
        }

        Building selectedBuilding = Building.None;

        public enum GameState {
            TitleScreen,
            InGame,
        }
        public static GameState gameState = GameState.TitleScreen;


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

            // Sets the initial resolution
            //graphics = new GraphicsDeviceManager(this);

            font1 = Content.Load<SpriteFont>("font1");

            audio = new Audio(Content);

            HUD.InitHUD(world);
        }

        protected override void Update(GameTime gameTime) {
            // Check for released bettons and update Input
            Input.GetState();

            if (Input.keyPressed(Keys.F)) {
                windowHandler.ToggleFullscreen();
            }

            // Press esc to end the game
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (gameState) {
                case GameState.TitleScreen:
                    break;

                case GameState.InGame:
                    if (Input.keyPressed(Input.SaveGame)) {
                        SaveManager.Save(world);
                    }

                    if (Input.keyPressed(Input.Enter)) {
                        SaveManager.Save(world);
                        gameState = GameState.TitleScreen;
                        audio.StopMusic();
                        audio.StopAllSounds();
                        break;
                    }

                    if (Input.keyPressed(Input.Mute)) {
                        audio.ToggleMute();
                    }

                    camera.Update();

                    // Tile conditions
                    if (Input.Click(0) && !HUD.OverActiveElement(Input.getMousePosition().ToPoint())) {
                        Tile tileClicked = world.GetTile(Input.getMouseHexTile(camera));

                        // Deselect a building when left click
                        selectedBuilding = Building.None;

                        world.SelectTile(tileClicked);
                    }

                    if (Input.Click(1) && world.GetTile(Input.getMouseHexTile(camera)) is null && !HUD.OverActiveElement(Input.getMousePosition().ToPoint())) {
                        Tile newTile;
                        switch (selectedBuilding) {
                            case Building.Apartment:
                                newTile = new Apartment(ContentRegistry.spr.t_apartments, Input.getMouseHexTile(camera));
                                break;

                            case Building.HoneyProducer:
                                newTile = new HoneyFactory(ContentRegistry.spr.t_honey_producer, Input.getMouseHexTile(camera));
                                break;

                            case Building.WaxProducer:
                                newTile = new WaxFactory(ContentRegistry.spr.t_wax_producer, Input.getMouseHexTile(camera));
                                break;

                            default:
                                newTile = new Tile(ContentRegistry.spr.t_hex, Input.getMouseHexTile(camera));
                                break;
                        }
                        int honey, wax;
                        if (newTile.BuildingReqs(out honey, out wax)) {
                            if (world.MainHive.honeyAmt >= honey && world.MainHive.waxAmt >= wax) {
                                world.MainHive.honeyAmt -= honey;
                                world.MainHive.waxAmt -= wax;
                                world.AddTile(newTile);
                                audio.PlaySound(Audio.sfx.place, 1f, 0f);
                                if (selectedBuilding == Building.Apartment) {
                                    world.AddBee(new Bee(ContentRegistry.spr.a_bee, newTile.position));
                                }
                            }
                        }
                    }

                    for (int i = 0; i < Input.numKeys.Count; i++) {
                        if (Input.keyPressed(Input.numKeys[i])) {
                            if (selectedBuilding != (Building)(i + 1)) {
                                selectedBuilding = (Building)(i + 1);
                            } else {
                                selectedBuilding = Building.None;
                            }
                        }
                    }

                    world?.Update(gameTime);

                    break;
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {

            switch (gameState) {
                case GameState.InGame:
                    windowHandler.RenderWorld();
                    windowHandler.RenderHUD(selectedBuilding);
                    break;

                case GameState.TitleScreen:
                    windowHandler.RenderMenu();
                    break;

            }

            base.Draw(gameTime);
        }
    }
}
