using Assembee.Game.Materials;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Assembee.Game
{
    public class GameRegistry {

        // Materials
        public enum mat {
            honey,
            nectar,
            propolis,
            wax
        }

        public static readonly Dictionary<mat, Material> MaterialRegistry = new Dictionary<mat, Material>();

        // Recipes

        public enum rcp {
            makeHoney,
            makeWax,
            makePropolis
        }

        public static readonly Dictionary<rcp, Recipe> RecipeRegistry = new Dictionary<rcp, Recipe>();

        // Factories

        public enum fct {
            honeyProduction,
            propolisProduction,
            waxProduction
        }

        public static readonly Dictionary<fct, FactoryType> FactoryRegistry = new Dictionary<fct, FactoryType>();


        public static void Load() {
            /* Materials: */
            MaterialRegistry.Clear();
            MaterialRegistry.Add(mat.honey, new Material("Honey", "Makes the world go round.", ContentRegistry.spr.a_bee));
            MaterialRegistry.Add(mat.nectar, new Material("Nectar", "Found in flowers and used to make honey.", ContentRegistry.spr.a_bee));
            MaterialRegistry.Add(mat.propolis, new Material("Propolis", "Glue of the bee world.", ContentRegistry.spr.a_bee));
            MaterialRegistry.Add(mat.wax, new Material("Wax", "The go-to building material for bees.", ContentRegistry.spr.a_bee));

            /* Recipes: */
            RecipeRegistry.Clear();

            RecipeRegistry.Add(rcp.makeHoney, new Recipe(
                MaterialRegistry[mat.nectar], 4.0f,
                MaterialRegistry[mat.honey], 1.0f,
                1.0f));

            RecipeRegistry.Add(rcp.makeWax, new Recipe(
                MaterialRegistry[mat.honey], 3.0f,
                MaterialRegistry[mat.wax], 1.0f,
                1.2f));

            RecipeRegistry.Add(rcp.makePropolis, new Recipe(
                MaterialRegistry[mat.wax], 1.0f,
                MaterialRegistry[mat.propolis], 1.0f,
                3.5f));

            /* Factory types: */
            FactoryRegistry.Clear();

            FactoryRegistry.Add(
                fct.honeyProduction,
                new FactoryType(
                    "Honey Factory",
                    ContentRegistry.spr.t_honey_producer,
                    new Dictionary<Material, float>() {
                        { MaterialRegistry[mat.honey], 5 },
                        { MaterialRegistry[mat.wax], 5 }},
                    RecipeRegistry[rcp.makeHoney]
            ));

            FactoryRegistry.Add(
                fct.waxProduction,
                new FactoryType(
                    "Wax Factory",
                    ContentRegistry.spr.t_wax_producer,
                    new Dictionary<Material, float>() {
                        { MaterialRegistry[mat.honey], 5 },
                        { MaterialRegistry[mat.wax], 5 }},
                    RecipeRegistry[rcp.makeWax]
             ));

            FactoryRegistry.Add(
                fct.propolisProduction,
                new FactoryType(
                    "Propolis Factory",
                    ContentRegistry.spr.t_wax_producer,
                    new Dictionary<Material, float>() {
                        { MaterialRegistry[mat.honey], 5 },
                        { MaterialRegistry[mat.wax], 5 }},
                    RecipeRegistry[rcp.makePropolis]
             ));
        }

    }
}
