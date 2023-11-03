using Assembee.Game.GameMath;
using Assembee.Game.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Assembee.Game.Entities {
    /// <summary>
    /// A line of bees that will take materials from position A and drop off at position B.
    /// </summary>
    public class SimpleBeeLine : Entity {

        /* How fast the bees move in world units per second. (Uses hex size for conversion). */
        private static readonly float SPEED = HexPosition.HexSize * 2.0f;
        private static readonly float CAPACITY = 2.0f;
        private static readonly float PICKUP_AMOUNT = 1.0f;

        public HexPosition PositionA { get; }
        public HexPosition PositionB { get; }

        private IInventoryTile targetTileA;
        private IInventoryTile targetTileB;

        private Dictionary<Material, float> inventory;
        public ImmutableHashSet<Material> TransportMaterials { get; private set; }

        private Vector2 aVector;
        private Vector2 bVector;
        private Vector2 aToB;

        private float distance;
        private float t = 0.0f;
        private float angle;

        private int bees;
        private float tIncrement;
        private int previousABee;
        private int previousBBee;

        public SimpleBeeLine(HexPosition positionA, HexPosition positionB, World world, int bees) : base(ContentRegistry.spr.a_bee, HexPosition.HexPositionToPosition(positionA)) {
            PositionA = positionA;
            PositionB = positionB;
            aVector = HexPosition.HexPositionToPosition(positionA);
            bVector = HexPosition.HexPositionToPosition(positionB);
            position = (aVector + bVector) / 2.0f;

            inventory = new Dictionary<Material, float>();

            aToB = bVector - aVector;
            distance = aToB.Length();

            angle = MathF.Atan2(aToB.Y, aToB.X) + MathF.PI / 2.0f;
            Sprite.rotation = angle;

            this.bees = bees;
            tIncrement = 2.0f / bees;

            previousABee = (int)(t / tIncrement);
            previousBBee = (int)((1 - t) / tIncrement);

            Tile tileA = world.GetTile(PositionA);
            Tile tileB = world.GetTile(PositionB);

            if (tileA is IInventoryTile && tileB is IInventoryTile) {
                targetTileA = (IInventoryTile)tileA;
                targetTileB = (IInventoryTile)tileB;

                TransportMaterials = IInventoryTile.CompatableTransportMaterials(targetTileA, targetTileB);
                foreach (Material mat in TransportMaterials) {
                    inventory.Add(mat, 0.0f);
                }

            } else {
                throw new ArgumentException("Target of Bee Line must be valid inventory tile.");
            }
        }

        public override void Draw(SpriteBatch spriteBatch, int animTick) {
            float tBee;
            Vector2 beePosition;

            for (int bee = 0; bee < bees; bee++) {
                tBee = (tIncrement * bee + t) % 2.0f;
                if (tBee < 1.0f) {
                    beePosition = aToB * tBee + aVector;
                    Sprite.rotation = angle;
                } else {
                    beePosition = aToB * (2.0f - tBee) + aVector;
                    Sprite.rotation = MathF.PI + angle;

                }
                
                Sprite.Draw(spriteBatch, beePosition, AnimationFrameFromTick(animTick));
            }
        }

        public override void Update(GameTime gameTime, World world) {
            t += SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds / distance;

            int nextABee = (int)(t / tIncrement);
            int nextBBee = (int)((t + 1) / tIncrement);

            //Util.Log(inventory[GameRegistry.MaterialRegistry[GameRegistry.mat.nectar]]);

            if (nextABee != previousABee) {
                /* Pick up from A. */

                foreach (Material material in TransportMaterials) {
                    if (inventory[material] < CAPACITY) {
                        inventory[material] += targetTileA.OutputMaterial(material, PICKUP_AMOUNT);
                    }
                }

                previousABee = nextABee;
            }

            if (nextBBee != previousBBee) {
                /* Drop off at B. */

                foreach (Material material in TransportMaterials) {
                    if (inventory[material] <= PICKUP_AMOUNT) {
                        inventory[material] = targetTileB.InputMaterial(material, inventory[material]);
                    } else {
                        inventory[material] -= PICKUP_AMOUNT - targetTileB.InputMaterial(material, PICKUP_AMOUNT);
                    }
                }

                previousBBee = nextBBee;
            }

            t %= 2.0f;
        }

    }
}
