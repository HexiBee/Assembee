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

        public static World world = new World();
        public static WindowHandler windowHandler;

        CameraPos cPos;
        public static Audio audio;
        public static SpriteFont font1;

        public enum Building {
            None,
            Apartment,
            HoneyProducer,
            WaxProducer
        }

        Building selectedBuilding = Building.None;

        Camera camera;

        public enum GameState {
            TitleScreen,
            InGame,
        }
        public static GameState gameState = GameState.TitleScreen;


        public Game1() {
            Content.RootDirectory = "Content";
            windowHandler = new WindowHandler(this, world);
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            // TODO: Add your initialization logic here
            windowHandler.Init();
            base.Initialize();
        }

        protected override void LoadContent() {

            ContentRegistry.LoadContent(Content);

            // Sets the initial resolution
            //graphics = new GraphicsDeviceManager(this);

            // TODO: use this.Content to load your game content here

            font1 = Content.Load<SpriteFont>("font1");

            // Audio
            audio = new Audio(Content);
            try {
                audio.PlaySound(Audio.sfx.click, 0f, 0f);
            } catch (Microsoft.Xna.Framework.Audio.NoAudioHardwareException) {
                audio.noAudio = true;
            }
            cPos = new CameraPos(ContentRegistry.spr.a_bee, new Vector2(0, 0), world);
            world.Add(cPos);
            camera = new Camera(cPos);
            world.camera = camera;

            HUD.InitHUD(world);

            //StartGame();

        }


        
        

        protected override void Update(GameTime gameTime) {
            // Check for released
            Input.GetState();

            if (Input.keyPressed(Keys.F)) {
                windowHandler.ToggleFullscreen();
            }

            //if (Input.Click(0))
            //     world.Add(new Entity("actor_bee_up" , Input.getMouseTile(world.camera), world));

            // Press esc to end the game
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (gameState) {
                case GameState.TitleScreen:
                    break;

                case GameState.InGame:
                    // debug
                    if (Input.keyPressed(Input.SaveGame)) {
                        SaveManager.Save(world);
                    }

                    if (Input.keyPressed(Input.Enter)) {
                        //SaveManager.Save(world);
                        gameState = GameState.TitleScreen;
                        audio.StopMusic();
                        world = null;
                        break;
                    }

                    //end debug

                    if (Input.keyPressed(Input.Mute)) {
                        audio.ToggleMute();
                    }

                    if (camera != null)
                        camera.Follow();


                    // Tile conditions
                    if (Input.Click(0) && !HUD.OverActiveElement(Input.getMousePos().ToPoint())) {
                        Tile tileClicked = world.GetTile(Input.getMouseHexTile(camera));

                        // Deselect a building when left click
                        selectedBuilding = Building.None;

                        if (tileClicked != null) {
                            world.Selector = new Selector(ContentRegistry.spr.entity_selector, Input.getMouseHexTile(camera), world);


                            if (world.selectedBee != null) {

                                if (world.selectedTile != tileClicked) {
                                    audio.PlaySound(Audio.sfx.bee, 1f, 0f);
                                }
                                world.selectedBee.SetTarget(tileClicked);

                                world.selectedBee = null;

                            }
                            world.selectedTile = tileClicked;
                            audio.StopSound(Audio.sfx.click);
                            audio.PlaySound(Audio.sfx.click, 0.6f, -0.41f);
                        } else {
                            world.Selector = null;
                            world.selectedBee = null;
                            world.selectedTile = null;
                        }

                    }

                    if (world.selectedTile != null) {
                        world.selectedBee = world.selectedTile.beeInside;
                    }

                    if (Input.Click(1) && world.GetTile(Input.getMouseHexTile(camera)) is null && !HUD.OverActiveElement(Input.getMousePos().ToPoint())) {
                        Tile newTile;
                        switch (selectedBuilding) {
                            case Building.Apartment:
                                newTile = new Apartment(ContentRegistry.spr.t_apartments, Input.getMouseHexTile(camera), world);
                                break;

                            case Building.HoneyProducer:
                                newTile = new HoneyFactory(ContentRegistry.spr.t_honey_producer, Input.getMouseHexTile(camera), world);
                                break;

                            case Building.WaxProducer:
                                newTile = new WaxFactory(ContentRegistry.spr.t_wax_producer, Input.getMouseHexTile(camera), world);
                                break;

                            default:
                                newTile = new Tile(ContentRegistry.spr.t_hex, Input.getMouseHexTile(camera), world);
                                break;
                        }
                        int honey, wax;
                        if (newTile.BuildingReqs(out honey, out wax)) {
                            if (world.hive.honeyAmt >= honey && world.hive.waxAmt >= wax) {
                                world.hive.honeyAmt -= honey;
                                world.hive.waxAmt -= wax;
                                world.Add(newTile);
                                audio.PlaySound(Audio.sfx.place, 1f, 0f);
                                if (selectedBuilding == Building.Apartment) {
                                    world.Add(new Bee(ContentRegistry.spr.a_bee, Input.getMouseHexTile(camera), world));
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


                    

                    // Updates all entities in the world
                    if (world != null) {
                        foreach (Entity entity in world.entities.ToArray()) {
                            entity.Update(gameTime);
                        }
                        foreach (Actor actor in world.actors.ToArray()) {
                            actor.Update(gameTime);
                        }

                        Tile tile;
                        for (int x = -World.WORLD_GRID_SIZE; x < World.WORLD_GRID_SIZE; x++) {
                            for (int y = -World.WORLD_GRID_SIZE; y < World.WORLD_GRID_SIZE; y++) {
                                tile = world.GetTile(new Vector2(x, y));
                                if (!(tile is null)) {
                                    tile.Update(gameTime);

                                }
                            }
                        }
                    }


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
