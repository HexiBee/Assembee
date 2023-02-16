using Assembee.Game.Entities;
using Assembee.Game.Entities.Tiles;
using Assembee.Game.GameMath;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game {
    /// <summary>
    /// Holds all of the data pertaining to a world including entities, actors, background tiles, bees.
    /// </summary>
    public class World {

        public const int WORLD_GRID_SIZE = 100;

        public List<Entity> entities = new List<Entity>();
        public List<Actor> actors = new List<Actor>();
        public List<Tile> background = new List<Tile>();
        public Tile selectedTile;

        public Tile Selector;

        public Camera camera;
        public Audio audio;
        public Hive hive;

        public Bee selectedBee = null;
        public List<Bee> bees = new List<Bee>();

        private Tile[,] tiles = new Tile[WORLD_GRID_SIZE * 2,WORLD_GRID_SIZE * 2];
        public List<Tile> tileList = new List<Tile>(); // This will make saving/loading easier, it won't be used other than for that

        public World() {
        }

        /// <summary>
        /// Adds a bee to the world.
        /// </summary>
        public void Add(Bee bee) {
            actors.Add(bee);
            bees.Add(bee);
        }

        /// <summary>
        /// Adds an actor to the world.
        /// </summary>
        public void Add(Actor actor) {
            actors.Add(actor);
        }

        /// <summary>
        /// Adds or replaces a tile in the world.
        /// </summary>
        public void Add(Tile tile) {
            Tile tOld = GetTile(tile.gridPos);
            if (tOld != null) {
                Remove(tOld);
            }
            tiles[(int)tile.gridPos.X + WORLD_GRID_SIZE, (int)tile.gridPos.Y + WORLD_GRID_SIZE] = tile;
            tileList.Add(tile);
            if (tile.GetType() == typeof(Hive)) {
                hive = (Hive)tile;
            }
        }

        /// <summary>
        /// Adds a background tile to the world.
        /// </summary>
        public void AddBackground(Tile tile) {
            background.Add(tile);
        }

        /// <summary>
        /// Adds an entity into the world.
        /// </summary>\
        public void Add(Entity entity) {
            entities.Add(entity);
        }

        /// <summary>
        /// Gets a tile from a given grid position.
        /// </summary>
        /// <param name="gridPos">The grid position from where to retrieve the tile</param>
        /// <returns>The tile at gridPos</returns>
        public Tile GetTile(Vector2 gridPos) {
            if (!InBounds(gridPos)) return null;
            return tiles[(int)gridPos.X + WORLD_GRID_SIZE, (int)gridPos.Y + WORLD_GRID_SIZE];
        }

        /// <summary>
        /// Do not use this.
        /// </summary>
        private void SetTile(Vector2 gridPos, Tile tile) {
            if (!InBounds(gridPos)) return;
            tiles[(int)gridPos.X + WORLD_GRID_SIZE, (int)gridPos.Y + WORLD_GRID_SIZE] = tile;
        }

        /// <summary>
        /// Removes a tile from the world.
        /// </summary>
        public void Remove(Tile tile) {
            tiles[(int)tile.gridPos.X + WORLD_GRID_SIZE, (int)tile.gridPos.Y + WORLD_GRID_SIZE] = null;
        }

        /// <summary>
        /// Whether or not the gridPos is within the world.
        /// </summary>
        private bool InBounds(Vector2 gridPos) {
            return (gridPos.X < WORLD_GRID_SIZE && gridPos.X > -WORLD_GRID_SIZE && gridPos.Y < WORLD_GRID_SIZE && gridPos.Y > -WORLD_GRID_SIZE);
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
        public void StartGame(bool freshStart) {

            for (int x = -World.WORLD_GRID_SIZE; x < World.WORLD_GRID_SIZE; x++) {
                for (int y = -World.WORLD_GRID_SIZE; y < World.WORLD_GRID_SIZE; y++) {
                    this.AddBackground(new Tile(ContentRegistry.spr.t_grass_0, new Vector2(x, y), this));
                }
            }

            if (freshStart) {
                GenerateWorld();
            }

            camera.scaleLerp = camera.scale;


            if (!Game1.audio.noAudio) {
                Game1.audio.StartSong();
            }
            audio = Game1.audio;

        }

        /// <summary>
        /// Generates a new world with starting buildings and random flowers
        /// </summary>
        private void GenerateWorld() {
            Hive hive = new Hive(ContentRegistry.spr.t_hive, new Vector2(0, 0), this);
            Bee bee = new Bee(ContentRegistry.spr.a_bee, new Vector2(0, 0), this);

            Add(bee);
            hive.beeInside = bee;
            Add(hive);


            Add(new HoneyOutput(ContentRegistry.spr.t_helipad_honey, new Vector2(0, 1), this));
            Add(new WaxOutput(ContentRegistry.spr.t_helipad_wax, new Vector2(-1, 1), this));

            Add(new HoneyFactory(ContentRegistry.spr.t_honey_producer, new Vector2(1, 0), this));
            Add(new WaxFactory(ContentRegistry.spr.t_wax_producer, new Vector2(-1, 0), this));
            Add(new Apartment(ContentRegistry.spr.t_apartments, new Vector2(1, -1), this));

            Random rand = new Random();
            for (int x = -World.WORLD_GRID_SIZE; x < World.WORLD_GRID_SIZE; x++) {
                for (int y = -World.WORLD_GRID_SIZE; y < World.WORLD_GRID_SIZE; y++) {
                    if (rand.NextDouble() < 0.05 && GetTile(new Vector2(x, y)) is null) {
                        Random r = new Random();
                        int amt = r.Next(300, 650);
                        Add(new Flowers(amt, ContentRegistry.spr.t_flowers, new Vector2(x, y), this));
                    }
                }
            }
        }

        /// <summary>
        /// Starts a new fresh game
        /// </summary>
        public void NewGame() {
            StartGame(true);
            Game1.gameState = Game1.GameState.InGame;
        }

        /// <summary>
        /// Loads a game from an existing file
        /// </summary>
        public void LoadGame() {
            StartGame(false);
            if (!SaveManager.Load(this)) {
                GenerateWorld();
            }
            Game1.gameState = Game1.GameState.InGame;
        }

    }

}
