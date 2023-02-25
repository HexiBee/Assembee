using Assembee.Game.GameMath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.Entities {
    public class Tile : Entity {
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
