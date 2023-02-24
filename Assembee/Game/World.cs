using Assembee.Game.Entities;
using Assembee.Game.Entities.Tiles;
using Assembee.Game.GameMath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game {
    /// <summary>
    /// Holds all of the data pertaining to a world including actors, background tiles, bees.
    /// </summary>
    public class World {

        private const int WORLD_GRID_SIZE = 100;

        public readonly List<Actor> Actors;
        public readonly List<Tile> Background;
        public readonly List<Tile> Tiles; // This will make saving/loading easier, it won't be used other than for that.

        public List<Bee> Bees { get; private set; } = new List<Bee>();

        public Tile SelectedTile { get; private set; }
        public Tile Selector;

        public Camera MainCamera { get; private set; }
        public Audio GameAudio { get; private set; }
        public Hive MainHive { get; private set; }

        public Bee SelectedBee { get; private set; } = null;

        private readonly Tile[,] tileArray;

        public World(Camera camera) {
            MainCamera = camera;
            Actors = new List<Actor>();
            Background = new List<Tile>();
            Tiles = new List<Tile>();

            tileArray = new Tile[WORLD_GRID_SIZE * 2, WORLD_GRID_SIZE * 2];
        }

        /// <summary>
        /// Adds a bee to the world.
        /// </summary>
        public void AddBee(Bee bee) {
            Actors.Add(bee);
            Bees.Add(bee);
        }

        /// <summary>
        /// Adds an actor to the world.
        /// </summary>
        public void AddActor(Actor actor) {
            Actors.Add(actor);
        }

        /// <summary>
        /// Adds or replaces a tile in the world.
        /// </summary>
        public void AddTile(Tile tile) {
            Tile oldTile = GetTile(tile.HexPosition);
            if (oldTile != null) {
                Remove(oldTile);
            }
            
            tileArray[(int)tile.HexPosition.X + WORLD_GRID_SIZE, (int)tile.HexPosition.Y + WORLD_GRID_SIZE] = tile;
            Tiles.Add(tile);
            if (tile.GetType() == typeof(Hive)) {
                MainHive = (Hive)tile;
            }
        }

        /// <summary>
        /// Adds a background tile to the world.
        /// </summary>
        public void AddBackground(Tile tile) {
            Background.Add(tile);
        }

        /// <summary>
        /// Gets a tile from a given grid position. Null if no tile.
        /// </summary>
        /// <param name="gridPos"></param>
        /// <returns></returns>
        public Tile GetTile(HexPosition hexPosition) {
            return GetTile(hexPosition.X, hexPosition.Y);
        }

        /// <summary>
        /// Gets a tile from the world a given x and y. Null if no tile.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Tile GetTile(int x, int y) {
            if (!InBounds(x, y)) return null;
            return tileArray[x + WORLD_GRID_SIZE, y + WORLD_GRID_SIZE];
        }

        /// <summary>
        /// Removes a tile from the world.
        /// </summary>
        public void Remove(Tile tile) {
            tileArray[(int)tile.HexPosition.X + WORLD_GRID_SIZE, (int)tile.HexPosition.Y + WORLD_GRID_SIZE] = null;
        }

        /// <summary>
        /// Whether or not the gridPos is within the world.
        /// </summary>
        private bool InBounds(Vector2 gridPos) {
            return (gridPos.X <= WORLD_GRID_SIZE && gridPos.X > -WORLD_GRID_SIZE && gridPos.Y <= WORLD_GRID_SIZE && gridPos.Y > -WORLD_GRID_SIZE);
        }

        private bool InBounds(int x, int y) {
            return x <= WORLD_GRID_SIZE && x > -WORLD_GRID_SIZE && y <= WORLD_GRID_SIZE && y > -WORLD_GRID_SIZE;
        }

        /// <summary>
        /// Convert a gridPos to a world position.
        /// </summary>
        /// <param name="gridPos"></param>
        /// <returns></returns>
        public static Vector2 GridPosToPos(Vector2 gridPos) {
            return new Matrix2((float)Math.Sqrt(3), (float)Math.Sqrt(3) / 2.0f, 0.0f, 3.0f / 2.0f) * new Vector2(gridPos.X * 127, gridPos.Y * 127);
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        /// <param name="freshStart">Whether or not to load from a save file.</param>
        public void RestartGame() {
            Actors.Clear();
            Background.Clear();
            Bees.Clear();
            
            foreach (Tile tile in Tiles) {
                tileArray[(int)tile.HexPosition.X + WORLD_GRID_SIZE, (int)tile.HexPosition.Y + WORLD_GRID_SIZE] = null;
            }

            Tiles.Clear();

            Random random = new Random();
            ContentRegistry.spr[] grass_sprites = {
                ContentRegistry.spr.t_grass_0,
                ContentRegistry.spr.t_grass_1,
                ContentRegistry.spr.t_grass_2,
            };

            for (int x = -WORLD_GRID_SIZE; x < WORLD_GRID_SIZE; x++) {
                for (int y = -WORLD_GRID_SIZE; y < WORLD_GRID_SIZE; y++) {
                    AddBackground(new Tile(grass_sprites[random.Next(grass_sprites.Length)], new HexPosition(x, y)));
                }
            }

            Selector = null;

            Game1.audio.StartSong();
            GameAudio = Game1.audio;
        }

        /// <summary>
        /// Generates a new world with starting buildings and random flowers. Will reset entire world.
        /// </summary>
        private void GenerateWorld() {
            Hive hive = new Hive(ContentRegistry.spr.t_hive, new HexPosition(0, 0));
            Bee bee = new Bee(ContentRegistry.spr.a_bee, new Vector2(0, 0));

            AddBee(bee);
            hive.BeeInside = bee;
            AddTile(hive);

            AddTile(new HoneyOutput(ContentRegistry.spr.t_helipad_honey, new HexPosition(0, 1)));
            AddTile(new WaxOutput(ContentRegistry.spr.t_helipad_wax, new HexPosition(-1, 1)));

            AddTile(new HoneyFactory(ContentRegistry.spr.t_honey_producer, new HexPosition(1, 0)));
            AddTile(new WaxFactory(ContentRegistry.spr.t_wax_producer, new HexPosition(-1, 0)));
            AddTile(new Apartment(ContentRegistry.spr.t_apartments, new HexPosition(1, -1)));

            Random rand = new Random();
            for (int x = -WORLD_GRID_SIZE; x < WORLD_GRID_SIZE; x++) {
                for (int y = -WORLD_GRID_SIZE; y < WORLD_GRID_SIZE; y++) {
                    if (rand.NextDouble() < 0.05 && GetTile(new HexPosition(x, y)) is null) {
                        Random r = new Random();
                        int amt = r.Next(300, 650);
                        AddTile(new Flowers(amt, ContentRegistry.spr.t_flowers, new HexPosition(x, y)));
                    }
                }
            }
        }

        /// <summary>
        /// Starts a new fresh game
        /// </summary>
        public void NewGame() {
            RestartGame();
            GenerateWorld();
            Game1.gameState = Game1.GameState.InGame;
        }

        /// <summary>
        /// Loads a game from an existing file
        /// </summary>
        public void LoadGame() {
            RestartGame();
            if (!SaveManager.Load(this)) {
                Util.Log("Failed to load world.");
                return;
            }
            Game1.gameState = Game1.GameState.InGame;
        }

        /// <summary>
        /// Sets a tile as the user selected tile.
        /// </summary>
        /// <param name="tile"></param>
        public void SelectTile(Tile tile) {

            if (tile == null) {
                Selector = null;
                SelectedBee = null;
                SelectedTile = null;
                return;
            }

            SelectedTile = tile;
            Selector = new Selector(ContentRegistry.spr.entity_selector, Input.getMouseHexTile(MainCamera));

            if (tile.BeeInside != null) {
                SelectedBee = tile.BeeInside;

            } else if (SelectedBee != null) {
                SelectedBee.SetTarget(tile.position, this);
                Game1.audio.PlaySound(Audio.sfx.bee, 1f, 0f);
                SelectedBee = null;

            } else {
                SelectedBee = null;
            }

            Game1.audio.StopSound(Audio.sfx.click);
            Game1.audio.PlaySound(Audio.sfx.click, 0.6f, -0.41f);
        }

        /// <summary>
        /// Draws the entire world.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="animationTick"></param>
        public void Draw(SpriteBatch spriteBatch, int animationTick) {
            /* Draw every background tile. */
            foreach (Tile backgroundTile in Background) {
                if (MainCamera.InBounds(backgroundTile.position))
                    backgroundTile.Draw(spriteBatch, animationTick);
            }

            /* Draw the selector */
            Selector?.Draw(spriteBatch, animationTick);

            /* Draw every tile */
            Tile selectedTile;
            for (int x = -WORLD_GRID_SIZE; x < WORLD_GRID_SIZE; x++) {
                for (int y = -WORLD_GRID_SIZE; y < WORLD_GRID_SIZE; y++) {
                    selectedTile = GetTile(x, y);
                    if (selectedTile != null && MainCamera.InBounds(selectedTile.position)) {
                        GetTile(x, y).Draw(spriteBatch, animationTick);
                    }
                }
            }

            /* Draw all actors */
            foreach (Actor actor in Actors) {
                if (MainCamera.InBounds(actor.position))
                    actor.Draw(spriteBatch, animationTick);
            }

        }

        /// <summary>
        /// Updates the world including all tiles and actors.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime) {
            /* Update actors */
            foreach (Actor actor in Actors) {
                actor.Update(gameTime, this);
            }

            /* Update tiles */
            for (int x = -WORLD_GRID_SIZE; x < WORLD_GRID_SIZE; x++) {
                for (int y = -WORLD_GRID_SIZE; y < WORLD_GRID_SIZE; y++) {
                    GetTile(x, y)?.Update(gameTime, this);
                }
            }
        }

    }

}
