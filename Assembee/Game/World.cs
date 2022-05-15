using Assembee.Game.Entities;
using Assembee.Game.Entities.Tiles;
using Assembee.Game.GameMath;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game {
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

        public void Add(Bee bee) {
            actors.Add(bee);
            bees.Add(bee);
        }

        public void Add(Actor actor) {
            actors.Add(actor);
        }

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

        public void AddBackground(Tile tile) {
            background.Add(tile);
        }

        public void Add(Entity entity) {
            entities.Add(entity);
        }

        public Tile GetTile(Vector2 gridPos) {
            if (!InBounds(gridPos)) return null;
            return tiles[(int)gridPos.X + WORLD_GRID_SIZE, (int)gridPos.Y + WORLD_GRID_SIZE];
        }

        public void SetTile(Vector2 gridPos, Tile tile) {
            if (!InBounds(gridPos)) return;
            tiles[(int)gridPos.X + WORLD_GRID_SIZE, (int)gridPos.Y + WORLD_GRID_SIZE] = tile;
        }

        public void Remove(Tile tile) {
            tiles[(int)tile.gridPos.X + WORLD_GRID_SIZE, (int)tile.gridPos.Y + WORLD_GRID_SIZE] = null;
        }

        private bool InBounds(Vector2 gridPos) {
            return (gridPos.X < WORLD_GRID_SIZE && gridPos.X > -WORLD_GRID_SIZE && gridPos.Y < WORLD_GRID_SIZE && gridPos.Y > -WORLD_GRID_SIZE);
        }

        public static Vector2 GridPosToPos(Vector2 gridPos) {
            return new Matrix2((float)Math.Sqrt(3), (float)Math.Sqrt(3) / 2.0f, 0.0f, 3.0f / 2.0f) * new Vector2(gridPos.X * 127, gridPos.Y * 127);
        }
   
    }
}
