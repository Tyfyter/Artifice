using System;
using Artifice.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice {
    public class ArtificeGlobalNPC : GlobalNPC {
        public override bool InstancePerEntity => true;
        protected override bool CloneNewInstances => true;
        public int defreduc = 0;
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit){
            damage+=(Math.Min(defreduc,npc.defense))/2;
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
            damage+=(Math.Min(defreduc,npc.defense))/2;
        }
		public override void ModifyGlobalLoot(GlobalLoot globalLoot) {
			globalLoot.Add(ItemDropRule.ByCondition(new InHellCondition(), ModContent.ItemType<Sulfur>(), 11));
		}
        public override void SetupShop(int type, Chest shop, ref int nextSlot){
            switch(type) {
            case NPCID.TravellingMerchant:
                if(Main.rand.NextBool(10) && (NPC.downedPlantBoss | NPC.downedFishron)) {
                    shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Whip_Sword>());
                }
            break;
            case NPCID.PartyGirl:
                if(Main.bloodMoon && NPC.downedBoss2) {
                    shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Roman_Candle>());
                }
            break;
            case NPCID.Cyborg:
                if(Main.LocalPlayer.GetModPlayer<ArtificerPlayer>().hasRC) {
                    shop.item[nextSlot++].SetDefaults(ModContent.ItemType<P4>());
                }
            break;
            case NPCID.ArmsDealer:
                if(NPC.downedPlantBoss) {
                    shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Gyrojet>());
                }
            break;
            }
        }
    }
	public class InHellCondition : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
            return info.npc.Center.Y > ((Main.maxTilesY - 200) * 16);
		}

		public bool CanShowItemDropInUI() {
            return true;
		}

		public string GetConditionDescription() {
            return "in Hell";
		}
	}
}