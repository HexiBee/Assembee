using Assembee.Game.Entities;
using Assembee.Game.Entities.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game {
    class World {

        public const int WORLD_GRID_SIZE = 100;

        public List<Entity> entities = new List<Entity>();
        public List<Actor> actors = new List<Actor>();
        public List<Tile> background = new List<Tile>();
        public Tile selectedTile;

        public Camera camera;
        public Audio audio;
        public Hive hive;

        public Bee selectedBee = null;
        public List<Bee> bees = new List<Bee>();

        private Tile[,] tiles = new Tile[WORLD_GRID_SIZE * 2,WORLD_GRID_SIZE * 2];

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
   
    }
}
