using System.Collections.Generic;
using Artifice.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace Artifice {
    public class ArtificerPlayer : ModPlayer {
        public bool hasRC = false;
        public byte ShroomiteBoost = 0;
        public override void ResetEffects(){
            hasRC = false;
            ShroomiteBoost = 0;
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage){
			if (ShroomiteBoost > 0 && (item.useAmmo != AmmoID.Bullet && item.useAmmo != AmmoID.Rocket && item.useAmmo != AmmoID.Arrow && (item.useAmmo != AmmoID.None || item.CountsAsClass(DamageClass.Ranged)))){
                //if(ShroomiteBoost > 1)flat+=10;
                //mult*=item.useAmmo != AmmoID.None?1.1f:1.20f;
                if (ShroomiteBoost > 1) damage.Base += 10;
                damage *= item.useAmmo != AmmoID.None ? 1.10f : 1.20f;
            }
		}
		public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo) {
			if(Player.HeldItem.type == ModContent.ItemType<AbSolution>()) {
                //drawInfo.drawPlayer.backpack = 2;
			}
		}
    }
}