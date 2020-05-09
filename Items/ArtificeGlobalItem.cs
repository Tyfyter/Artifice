using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Items{
    class ArtificeGlobalItem : GlobalItem{
        public override void SetDefaults(Item item){
            if(item.type == ItemID.Glass || (item.type >= ItemID.AmethystGemsparkBlock && item.type <=ItemID.AmberGemsparkBlock) || (item.type >= 2693 && item.type < 2698 && item.createWall == -1) || item.type == ItemID.HoneyfallBlock || item.type == ItemID.SandFallBlock || item.type == ItemID.SnowFallBlock){
                item.ammo = ItemID.Glass;
            }
        }
		public override void PickAmmo(Item weapon, Item ammo, Player player, ref int shoot, ref float speed, ref int damage, ref float knockback){
            if(ammo.ammo == ItemID.Glass){
                int type = ammo.type;
                if(ammo.modItem is InfiniteGlass ig)type = ig.type;
                shoot = 0;
                damage+=4;
                switch(type){
                    case ItemID.Glass:
				    shoot = 0;
                    damage-=4;
                    break;
                    case ItemID.WaterfallBlock:
				    shoot = 1;
                    speed*=1.1f;
                    break;
                    case ItemID.LavafallBlock:
				    shoot = 2;
                    damage+=2;
                    break;
                    case ItemID.HoneyfallBlock:
				    shoot = 3;
                    break;
                    case ItemID.ConfettiBlock:
				    shoot = 4;
                    break;
                    case ItemID.ConfettiBlockBlack:
				    shoot = 5;
                    break;
                    case ItemID.SandFallBlock:
				    shoot = 6;
                    break;
                    case ItemID.SnowFallBlock:
				    shoot = 7;
                    break;
                    case ItemID.AmethystGemsparkBlock:
				    shoot = 8;
                    damage+=4;
                    break;
                    case ItemID.TopazGemsparkBlock:
				    shoot = 9;
                    damage+=8;
                    break;
                    case ItemID.SapphireGemsparkBlock:
				    shoot = 10;
                    damage+=12;
                    break;
                    case ItemID.EmeraldGemsparkBlock:
				    shoot = 11;
                    damage+=16;
                    break;
                    case ItemID.RubyGemsparkBlock:
				    shoot = 12;
                    damage+=20;
                    break;
                    case ItemID.DiamondGemsparkBlock:
				    shoot = 13;
                    damage+=24;
                    break;
                    case ItemID.AmberGemsparkBlock:
				    shoot = 14;
                    damage+=28;
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