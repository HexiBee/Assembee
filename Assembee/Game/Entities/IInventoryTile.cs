using Assembee.Game.Materials;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Assembee.Game.Entities {
    public interface IInventoryTile {

        /// <summary>
        /// Set of materials able to be output. Null if can output anything.
        /// </summary>
        public ImmutableHashSet<Material> OutputMaterials { get { return null; } }

        /// <summary>
        /// Set of materials able to be input. Null if can accept anything.
        /// </summary>
        public ImmutableHashSet<Material> InputMaterials { get { return null; } }

        /// <summary>
        /// Whether the tile can accept a material as input.
        /// </summary>
        public bool CanAcceptMaterial(Material material);

        /// <summary>
        /// Whether the tile can output a material. Just because this returns true does not mean there is a nonzero value of the material present.
        /// </summary>
        public bool CanProvideMaterial(Material material);

        /// <summary>
        /// Inputs the given amount of the material into the input inventory.
        /// </summary>
        /// <returns>The amount left over which could not be put into the inventory.</returns>
        public float InputMaterial(Material material, float amount);

        /// <summary>
        /// Outputs a given amount of the material from the output inventory.
        /// </summary>
        /// <returns>The actual amount retrieved</returns>
        public float OutputMaterial(Material material, float amount);

        /// <summary>
        /// Returns the set of compatable materials capable of going from tileA to tileB.
        /// </summary>
        public static ImmutableHashSet<Material> CompatableTransportMaterials(IInventoryTile tileA, IInventoryTile tileB) {
            if (tileA.OutputMaterials is null && tileB.InputMaterials is null) {
                return GameRegistry.MaterialRegistry.Values.ToImmutableHashSet();
            }
            if (tileA.OutputMaterials is null) {
                return tileB.InputMaterials;
            }
            if (tileB.InputMaterials is null) {
                return tileA.OutputMaterials;
            }
            return tileA.OutputMaterials.Union(tileB.InputMaterials);
        }

    }
}
