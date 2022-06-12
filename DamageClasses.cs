using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Artifice {
	public class DamageClasses : ILoadable {
		private static DamageClass ranged_Magic;
		public static DamageClass Ranged_Magic => ranged_Magic ??= ModContent.GetInstance<Ranged_Magic>();
		public void Unload() {
			ranged_Magic = null;
		}
		public void Load(Mod mod) {}
	}
    public class Ranged_Magic : DamageClass {
        public override void SetStaticDefaults() {
            ClassName.SetDefault("ranged/magic damage");
        }
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass) {
            if (damageClass == Generic || damageClass == Ranged || damageClass == Magic) {
                return StatInheritanceData.Full;
            }
            return StatInheritanceData.None;
        }
        public override bool GetEffectInheritance(DamageClass damageClass) {
            return damageClass == Ranged || damageClass == Magic;
        }
        public override void SetDefaultStats(Player player) {
            player.GetCritChance(this) += 4;
        }
    }
}
