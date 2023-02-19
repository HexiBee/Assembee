using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game {
    public static class ContentRegistry {


        public enum spr {
            a_bee,

            r_honey_small,
            r_propolis_small,
            r_wax_small,

            t_apartments,
            t_grass_0,
            t_grass_1,
            t_grass_2,
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

        private const spr MISSING_TEXTURE = spr.t_hex;

        private static Dictionary<spr, Texture2D> TextureRegistry = new Dictionary<spr, Texture2D>();
        private static Dictionary<spr, int> AnimationRegistry = new Dictionary<spr, int>();

        public static void LoadContent(ContentManager content) {
            TextureRegistry.Add(spr.a_bee, content.Load<Texture2D>("actor_bee_up"));

            TextureRegistry.Add(spr.r_honey_small, content.Load<Texture2D>("resource_honey_small"));
            TextureRegistry.Add(spr.r_propolis_small, content.Load<Texture2D>("resource_propolis_small"));
            TextureRegistry.Add(spr.r_wax_small, content.Load<Texture2D>("resource_wax_small"));

            TextureRegistry.Add(spr.t_apartments, content.Load<Texture2D>("tile_apartments"));
            TextureRegistry.Add(spr.t_grass_0, content.Load<Texture2D>("tile_grass_0"));
            TextureRegistry.Add(spr.t_grass_1, content.Load<Texture2D>("tile_grass_1"));
            TextureRegistry.Add(spr.t_grass_2, content.Load<Texture2D>("tile_grass_2"));
            TextureRegistry.Add(spr.t_hex, content.Load<Texture2D>("tile_hex"));
            TextureRegistry.Add(spr.t_hive, content.Load<Texture2D>("tile_hive"));
            TextureRegistry.Add(spr.t_honey_producer, content.Load<Texture2D>("tile_honey_producer"));
            TextureRegistry.Add(spr.t_wax_producer, content.Load<Texture2D>("tile_wax_producer"));
            TextureRegistry.Add(spr.t_flowers, content.Load<Texture2D>("tile_flowers"));
            TextureRegistry.Add(spr.t_helipad_honey, content.Load<Texture2D>("tile_helipad_honey"));
            TextureRegistry.Add(spr.t_helipad_wax, content.Load<Texture2D>("tile_helipad_wax"));

            TextureRegistry.Add(spr.entity_selector, content.Load<Texture2D>("entity_selector"));

            TextureRegistry.Add(spr.u_panel, content.Load<Texture2D>("ui_panel"));

            AnimationRegistry.Add(spr.a_bee, 26);
            AnimationRegistry.Add(spr.t_honey_producer, 257);
            AnimationRegistry.Add(spr.t_apartments, 244);
            AnimationRegistry.Add(spr.t_wax_producer, 276);
            AnimationRegistry.Add(spr.entity_selector, 256);
        }

        public static Texture2D GetTexture(spr textureEnum) {
            Texture2D texture;
            if (!TextureRegistry.TryGetValue(textureEnum, out texture))
                TextureRegistry.TryGetValue(MISSING_TEXTURE, out texture);
            return texture;
        }

        public static int GetFrameHeight(spr textureEnum) {
            int frameHeight;
            if (!AnimationRegistry.TryGetValue(textureEnum, out frameHeight))
                frameHeight = GetTexture(textureEnum).Height;

            return frameHeight;
        }

    }
}
