using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Assembee.Game.GameMath;
using Assembee.Game.Materials;
using System.Collections.Immutable;

namespace Assembee.Game.Entities {
    public class Flower : Tile, IInventoryTile {

        public ImmutableHashSet<Material> OutputMaterials { get; }
        public ImmutableHashSet<Material> InputMaterials { get; } = ImmutableHashSet<Material>.Empty;

        private float nectarAmount;

        public Flower(int nectarAmount, ContentRegistry.spr texture, HexPosition hexPosition) : base(texture, hexPosition) {
            this.nectarAmount = nectarAmount;

            OutputMaterials = new HashSet<Material>() { GameRegistry.MaterialRegistry[GameRegistry.mat.nectar] }.ToImmutableHashSet();
        }

        public Flower(int nectarAmount, HexPosition hexPosition) : this(nectarAmount, ContentRegistry.spr.t_flowers, hexPosition) {
        }

        public override string GetInfoString() {
            return "Flowers:\nNectar: " + nectarAmount.ToString();
        }

        public bool CanAcceptMaterial(Material material) {
            return false;
        }

        public bool CanProvideMaterial(Material material) {
            return (material == GameRegistry.MaterialRegistry[GameRegistry.mat.nectar]);
        }

        public float InputMaterial(Material material, float amount) {
            if (amount < 0) throw new ArgumentOutOfRangeException("amount", "Input amount must be nonnegative.");
            return amount;
        }

        public float OutputMaterial(Material material, float amount) {
            if (amount < 0) throw new ArgumentOutOfRangeException("amount", "Output amount must be nonnegative.");

            if (nectarAmount <= amount) {
                float result = nectarAmount;
                nectarAmount = 0;
                Sprite.color = Color.DarkSlateGray;
                return result;
            }

            nectarAmount -= amount;
            return amount;

        }
    }
}
