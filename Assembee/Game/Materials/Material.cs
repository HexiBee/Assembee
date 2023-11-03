using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.Materials {
    public class Material {
        
        public string Name { get; private set; }
        public string Description { get; private set; }
        public ContentRegistry.spr Texture { get; private set; }

        public Material(string name, string description, ContentRegistry.spr texture) { 
            Name = name;
            Description = description;
            Texture = texture;
        }

        public override string ToString() {
            return Name;
        }

    }
}
