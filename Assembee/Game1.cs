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
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public const int SCREEN_WIDTH = 960;
        public const int SCREEN_HEIGHT = 540;
        public const int FRAME_RATE = 60;
        
        // Modifiable screen vars
        public static int ScreenWidth = SCREEN_WIDTH;
        public static int ScreenHeight = SCREEN_HEIGHT;

        public static int wScr = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        public static int hScr = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

        
        // a = actor, r = resource, t = tile
        public enum spr {
            a_bee_up,

            r_honey_small,
            r_propolis_small,
            r_wax_small,

            t_apartments,
            t_grass_0,
            t_hex,
            t_hive,
            t_honey_producer,
            t_wax_producer,
            t_flowers,
            t_helipad_honey,
            t_helipad_wax,
            entity_selector,

            u_panel
        }
        public static Dictionary<spr, Texture2D> TextureRegistry = new Dictionary<spr, Texture2D>();
        public static Dictionary<spr, int> AnimationRegistry = new Dictionary<spr, int>();


        //enum Anim {
        //    a_bee_up,
        //    a_bee_down,
        //    a_bee_right,
        //    a_bee_left
        //}

        public static World world;
        CameraPos cPos;
        Audio audio;
        public static SpriteFont font1;

        public enum Building {
            None,
            Apartment,
            HoneyProducer,
            WaxProducer
        }

        Building selectedBuilding = Building.None;

        private int animTick = 0;

        // Camera
        Rectangle CANVAS = new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT);
        private RenderTarget2D renderTarget;
        Camera camera;
        Tile selector;
        //public int buildMode = ;

        float cameraScaleLerp;
        float[] scale = { 1f, -1f };
        int scaleInc = 0;

        public enum GameState {
            TitleScreen,
            InGame,
        }
        public GameState gameState = GameState.TitleScreen;


        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferMultiSampling = false;
            graphics.SynchronizeWithVerticalRetrace = true;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent() {
            renderTarget = new RenderTarget2D(GraphicsDevice, CANVAS.Width, CANVAS.Height);


            // Sets the initial resolution
            //graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.ApplyChanges();

            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            TextureRegistry.Add(spr.a_bee_up, Content.Load<Texture2D>("actor_bee_up"));

            TextureRegistry.Add(spr.r_honey_small, Content.Load<Texture2D>("resource_honey_small"));
            TextureRegistry.Add(spr.r_propolis_small, Content.Load<Texture2D>("resource_propolis_small"));
            TextureRegistry.Add(spr.r_wax_small, Content.Load<Texture2D>("resource_wax_small"));

            TextureRegistry.Add(spr.t_apartments, Content.Load<Texture2D>("tile_apartments"));
            TextureRegistry.Add(spr.t_grass_0, Content.Load<Texture2D>("tile_grass"));
            TextureRegistry.Add(spr.t_hex, Content.Load<Texture2D>("tile_hex"));
            TextureRegistry.Add(spr.t_hive, Content.Load<Texture2D>("tile_hive"));
            TextureRegistry.Add(spr.t_honey_producer, Content.Load<Texture2D>("tile_honey_producer"));
            TextureRegistry.Add(spr.t_wax_producer, Content.Load<Texture2D>("tile_wax_producer"));
            TextureRegistry.Add(spr.t_flowers, Content.Load<Texture2D>("tile_flowers"));
            TextureRegistry.Add(spr.t_helipad_honey, Content.Load<Texture2D>("tile_helipad_honey"));
            TextureRegistry.Add(spr.t_helipad_wax, Content.Load<Texture2D>("tile_helipad_wax"));

            TextureRegistry.Add(spr.entity_selector, Content.Load<Texture2D>("entity_selector"));

            TextureRegistry.Add(spr.u_panel, Content.Load<Texture2D>("ui_panel"));

            AnimationRegistry.Add(spr.a_bee_up, 26);
            AnimationRegistry.Add(spr.t_honey_producer, 257);
            AnimationRegistry.Add(spr.t_apartments, 244);
            AnimationRegistry.Add(spr.t_wax_producer, 276);
            AnimationRegistry.Add(spr.entity_selector, 256);

            font1 = Content.Load<SpriteFont>("font1");

            // Audio
            audio = new Audio(Content);
            try {
                audio.PlaySound(Audio.sfx.click, 0f, 0f);
            } catch (Microsoft.Xna.Framework.Audio.NoAudioHardwareException) {
                audio.noAudio = true;
            }
            world = new World();
            cPos = new CameraPos(spr.a_bee_up, new Vector2(0, 0), world);
            world.Add(cPos);
            camera = new Camera(cPos);

            //StartGame();

        }

        public void FreshStart() {
            Hive hive = new Hive(spr.t_hive, new Vector2(0, 0), world);
            //world.hive = hive;
            Bee bee = new Bee(spr.a_bee_up, new Vector2(0, 0), world);

            world.Add(bee);
            hive.beeInside = bee;
            world.Add(hive);


            world.Add(new HoneyOutput(spr.t_helipad_honey, new Vector2(0, 1), world));
            world.Add(new WaxOutput(spr.t_helipad_wax, new Vector2(-1, 1), world));

            world.Add(new HoneyFactory(spr.t_honey_producer, new Vector2(1, 0), world));
            world.Add(new WaxFactory(spr.t_wax_producer, new Vector2(-1, 0), world));
            world.Add(new Apartment(spr.t_apartments, new Vector2(1, -1), world));

            Random rand = new Random();
            for (int x = -World.WORLD_GRID_SIZE; x < World.WORLD_GRID_SIZE; x++) {
                for (int y = -World.WORLD_GRID_SIZE; y < World.WORLD_GRID_SIZE; y++) {
                    if (rand.NextDouble() < 0.05 && world.GetTile(new Vector2(x, y)) is null) {
                        Random r = new Random();
                        int amt = r.Next(300, 650);
                        world.Add(new Flowers(amt, spr.t_flowers, new Vector2(x, y), world));
                    }
                }
            }
        }
        // This probably shouldn't be put here...
        public void StartGame(bool freshStart) {
            //world = new World();


            // RELOCATED NOW
            //cPos = new CameraPos(spr.a_bee_up, new Vector2(0, 0), world);
            //world.Add(cPos);
            //camera = new Camera(cPos);


            for (int x = -World.WORLD_GRID_SIZE; x < World.WORLD_GRID_SIZE; x++) {
                for (int y = -World.WORLD_GRID_SIZE; y < World.WORLD_GRID_SIZE; y++) {
                    world.AddBackground(new Tile(spr.t_grass_0, new Vector2(x, y), world));
                }
            }

            if (freshStart) {
                FreshStart();
            }


            world.camera = camera;
            cameraScaleLerp = camera.scale;
            

            if (!audio.noAudio) {
                MediaPlayer.Play(audio.song);
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = 0.1f;
            }
            world.audio = audio;

        }
        public void Resolution(int w, int h, bool b) {
            renderTarget = new RenderTarget2D(GraphicsDevice, w, h);
            graphics.PreferredBackBufferWidth = w;
            graphics.PreferredBackBufferHeight = h;
            ScreenWidth = w;
            ScreenHeight = h;
            graphics.IsFullScreen = b;
            graphics.ApplyChanges();
        }

        protected override void Update(GameTime gameTime) {
            // Check for released
            Input.GetState();

            if (Input.keyPressed(Keys.F)) {
                if (scaleInc < scale.Length - 1) {
                    scaleInc++;
                } else {
                    scaleInc = 0;
                }
                float scaleCur = scale[scaleInc];
                if (scaleCur > 0) {
                    Resolution((int)(SCREEN_WIDTH * scaleCur), (int)(SCREEN_HEIGHT * scaleCur), false);
                } else {
                    graphics.HardwareModeSwitch = false;
                    //int scale = wScr / SCREEN_WIDTH;
                    Resolution(wScr, hScr, true);
                }
            }
            //if (Input.Click(0))
            //     world.Add(new Entity("actor_bee_up" , Input.getMouseTile(world.camera), world));

            // Press esc to end the game
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (gameState) {
                case GameState.TitleScreen:
                    if (Input.keyPressed(Input.NewGame)) {
                        StartGame(true);
                        gameState = GameState.InGame;
                    }

                    if (Input.keyPressed(Input.LoadGame)) {
                        StartGame(false);
                        SaveManager.Load(world);
                        gameState = GameState.InGame;

                    }
                    break;

                case GameState.InGame:
                    // debug
                    if (Input.keyPressed(Input.SaveGame)) {
                        SaveManager.Save(world);
                    }


                    //end debug

                    if (camera != null)
                        camera.Follow();


                    // Tile conditions
                    if (Input.Click(0)) {
                        Tile tileClicked = world.GetTile(Input.getMouseHexTile(camera));

                        // Deselect a building when left click
                        selectedBuilding = Building.None;

                        if (tileClicked != null) {
                            selector = new Selector(spr.entity_selector, Input.getMouseHexTile(camera), world);


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
                            selector = null;
                            world.selectedBee = null;
                            world.selectedTile = null;
                        }

                    }

                    if (world.selectedTile != null) {
                        world.selectedBee = world.selectedTile.beeInside;
                    }

                    if (Input.Click(1) && world.GetTile(Input.getMouseHexTile(camera)) is null) {
                        Tile newTile;
                        switch (selectedBuilding) {
                            case Building.Apartment:
                                newTile = new Apartment(spr.t_apartments, Input.getMouseHexTile(camera), world);
                                break;

                            case Building.HoneyProducer:
                                newTile = new HoneyFactory(spr.t_honey_producer, Input.getMouseHexTile(camera), world);
                                break;

                            case Building.WaxProducer:
                                newTile = new WaxFactory(spr.t_wax_producer, Input.getMouseHexTile(camera), world);
                                break;

                            default:
                                newTile = new Tile(spr.t_hex, Input.getMouseHexTile(camera), world);
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
                                    world.Add(new Bee(spr.a_bee_up, Input.getMouseHexTile(camera), world));
                                }
                            }
                        }

                    }


                    if (Input.scrollPressed() != 0) {
                        cameraScaleLerp += 0.2f * Input.scrollPressed();
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


                    camera.scale = Util.Lerp(camera.scale, cameraScaleLerp, camera.spdScale);
                    cPos.moveSpeed = cPos.baseMoveSpeed + camera.scale * 3.0f;

                    if (camera.scale < 1.0f) {
                        camera.scale = 1.0f;
                        cameraScaleLerp = 1.0f;
                    } else if (camera.scale > 10.0f) {
                        camera.scale = 10.0f;
                        cameraScaleLerp = 10.0f;
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
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // Special Begin() to utilize the camera
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.Transform);


            // Draws all actors in the world 
            // If we decide all entities will draw, we can update this appropriately. 
            switch (gameState) {
                case GameState.InGame:

                    if (world != null) {
                        foreach (Tile backgroundTile in world.background.ToArray()) {
                            backgroundTile.Draw(spriteBatch, animTick);
                        }

                        if (selector != null) {
                            selector.Draw(spriteBatch, animTick);
                        }

                        Tile tile;
                        for (int x = -World.WORLD_GRID_SIZE; x < World.WORLD_GRID_SIZE; x++) {
                            for (int y = -World.WORLD_GRID_SIZE; y < World.WORLD_GRID_SIZE; y++) {
                                tile = world.GetTile(new Vector2(x, y));
                                if (!(tile is null)) {
                                    tile.Draw(spriteBatch, animTick);
                                }
                                

                            }
                        }
                        foreach (Actor actor in world.actors.ToArray()) {
                            actor.Draw(spriteBatch, animTick);
                        }
                        foreach (Entity entity in world.entities.ToArray()) {
                            entity.Draw(spriteBatch, animTick);
                        }
                    }
                    break;

            }

            animTick++;

            //Basic bee drawing
            //spriteBatch.Draw(sBee, new Vector2(20, 20), Color.White);

            

            spriteBatch.End();

            // Don't mess with:
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(renderTarget, new Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);
            spriteBatch.End();

            //UI
            spriteBatch.Begin();

            if (gameState == GameState.TitleScreen) {
                spriteBatch.DrawString(font1, "Enter to Load, Space for new", new Vector2(ScreenWidth / 2, 0), Color.White);
            }
            // Text
            HUD.DrawHud(spriteBatch, world, selectedBuilding);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
