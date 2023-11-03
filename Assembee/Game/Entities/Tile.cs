using Assembee.Game.GameMath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.Entities {
    public class Tile : Entity {

        [JsonProperty]
        public HexPosition HexPosition { get; private set; }

        public Tile(ContentRegistry.spr texture, HexPosition hexPosition) : base(texture, Vector2.Zero) {
            HexPosition = hexPosition;
            position = HexPosition.HexPositionToPosition(hexPosition);
        }

        public virtual string GetInfoString() {
            return "";
        }

    }
}
