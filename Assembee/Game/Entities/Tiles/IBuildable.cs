using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.Entities.Tiles {
    interface IBuildable {
        static int HONEY_NEEDED { get; set; }
        static int WAX_NEEDED { get; set; }
        static string NAME { get; set; }

        public static string BuildUI(World world, int HONEY_NEEDED, int WAX_NEEDED, string NAME) {
            string uiString = "";
            uiString += "Build " + NAME + ":";
            uiString += "\nHoney Needed: " + world.MainHive.honeyAmt.ToString() + " / " + HONEY_NEEDED.ToString();
            uiString += "\nWax Needed: " + world.MainHive.waxAmt.ToString() + " / " + WAX_NEEDED.ToString();
            return uiString;
        }
    }
}
