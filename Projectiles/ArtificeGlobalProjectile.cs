using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace Artifice.Projectiles {
    public class ArtificeGlobalProjectile : GlobalProjectile {
        public override bool InstancePerEntity => true;
        public override bool CloneNewInstances => true;
        public int spinResistHP = 0;
    }
}
