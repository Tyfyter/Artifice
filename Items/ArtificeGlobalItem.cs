using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Items{
    class ArtificeGlobalItem : GlobalItem {
        public override void SetDefaults(Item item){
			switch (item.type) {
                case ItemID.Glass:
                case ItemID.AmethystGemsparkBlock:
                case ItemID.TopazGemsparkBlock:
                case ItemID.SapphireGemsparkBlock:
                case ItemID.EmeraldGemsparkBlock:
                case ItemID.RubyGemsparkBlock:
                case ItemID.DiamondGemsparkBlock:
                case ItemID.WaterfallBlock:
                case ItemID.LavafallBlock:
                case ItemID.HoneyfallBlock:
                case ItemID.ConfettiBlock:
                case ItemID.ConfettiBlockBlack:
                case ItemID.SandFallBlock:
                case ItemID.SnowFallBlock:
                case ItemID.BlueStarryGlassBlock:
                case ItemID.GoldStarryGlassBlock:
                item.ammo = ItemID.Glass;
                break;
			}
        }
		public override void PickAmmo(Item weapon, Item ammo, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback) {
            if(ammo.ammo == ItemID.Glass){
                int ammoType = ammo.type;
                if(ammo.ModItem is InfiniteGlass ig) ammoType = ig.type;
                type = 0;
                damage.Base += 4;
                switch(ammoType) {
                    case ItemID.Glass:
                    type = 0;
                    damage.Base -= 4;
                    break;
                    case ItemID.WaterfallBlock:
                    type = 1;
                    speed*=1.1f;
                    break;
                    case ItemID.LavafallBlock:
                    type = 2;
                    damage.Base += 2;
                    break;
                    case ItemID.HoneyfallBlock:
                    type = 3;
                    break;
                    case ItemID.ConfettiBlock:
                    type = 4;
                    break;
                    case ItemID.ConfettiBlockBlack:
                    type = 5;
                    break;
                    case ItemID.SandFallBlock:
                    type = 6;
                    break;
                    case ItemID.SnowFallBlock:
                    type = 7;
                    break;
                    case ItemID.AmethystGemsparkBlock:
                    type = 8;
                    damage.Base += 4;
                    break;
                    case ItemID.TopazGemsparkBlock:
                    type = 9;
                    damage.Base += 8;
                    break;
                    case ItemID.SapphireGemsparkBlock:
                    type = 10;
                    damage.Base += 12;
                    break;
                    case ItemID.EmeraldGemsparkBlock:
                    type = 11;
                    damage.Base += 16;
                    break;
                    case ItemID.RubyGemsparkBlock:
                    type = 12;
                    damage.Base += 20;
                    break;
                    case ItemID.DiamondGemsparkBlock:
                    type = 13;
                    damage.Base += 24;
                    break;
                    case ItemID.AmberGemsparkBlock:
                    type = 14;
                    damage.Base+=28;
                    break;
                    case ItemID.BlueStarryGlassBlock:
                    type = 15;
                    damage.Base += 28;
                    break;
                    case ItemID.GoldStarryGlassBlock:
                    type = 16;
                    damage.Base += 28;
                    break;
                }
			    //Main.NewText(ammo.Name+": "+type);
            }else if(ammo.ammo == AmmoID.Sand && weapon.type == ModContent.ItemType<Sandblaster>()){
                int dmg = 5;
                switch(ammo.type){
                    case ItemID.PearlsandBlock:
                    speed*=1.1f;
                    break;
                    case ItemID.EbonsandBlock:
                    knockback++;
                    break;
                    case ItemID.CrimsandBlock:
                    dmg+=3;
                    break;
                    default:
                    break;
                }
                damage+=dmg*2;
            }
		}
    }
}