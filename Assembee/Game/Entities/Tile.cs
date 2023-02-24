using Assembee.Game.GameMath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.Entities {
    public class Tile : Entity {
        private static readonly Matrix2 hexMatrix = new Matrix2((float)Math.Sqrt(3), (float)Math.Sqrt(3) / 2.0f, 0.0f, 3.0f / 2.0f);

        public Bee BeeInside { get; set; } = null;

        [JsonProperty]
        public HexPosition HexPosition { get; private set; }

        public Tile(ContentRegistry.spr texture, HexPosition hexPosition) : base(texture, Vector2.Zero) {
            this.HexPosition = hexPosition;
            position = HexPosition.HexPositionToPosition(hexPosition);
        }

        public override void Update(GameTime gameTime, World world) {
        }

        public virtual string GetInfoString() {
            return "";
        }

        public virtual bool BuildingReqs(out int honeyRequired, out int waxRequired) {
            honeyRequired = 0;
            waxRequired = 0;
            return false;
        }

    }
}
