using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Assembee.Game.Materials {
    /// <summary>
    /// Represents an amount of inputs and outputs which can take place during production.
    /// </summary>
    public class Recipe {

        public ImmutableDictionary<Material, float> Requirements { get; private set; }
        public ImmutableDictionary<Material, float> Products { get; private set; }
        public float Time { get; private set; }

        /// <summary>
        /// Whether or not the recipe can be used at fractional values of the required amounts.
        /// </summary>
        public bool IsDiscrete { get; private set; }

        public Recipe(ImmutableDictionary<Material, float> requirements, ImmutableDictionary<Material, float> products, float time, bool isDiscrete) {
            Requirements = requirements;
            Products = products;
            Time = time;
            IsDiscrete = isDiscrete;
        }

        public Recipe(ImmutableDictionary<Material, float> requirements, ImmutableDictionary<Material, float> products, float time) : this(requirements, products, time, true) {
        }

        public Recipe(Material input, float inAmount, Material output, float outAmount, float time) {
            Requirements = new Dictionary<Material, float>() { { input, inAmount } }.ToImmutableDictionary();
            Products = new Dictionary<Material, float>() { { output, outAmount } }.ToImmutableDictionary();
            Time = time;
            IsDiscrete = true;
        }
    }
}
