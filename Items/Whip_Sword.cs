using System;
using System.Collections.Generic;
using Artifice.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Items {
	//day 6
	public class Whip_Sword : ModItem {
		bool extended = false;
		protected override bool CloneNewInstances => true;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Whip Sword");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults(){
			Item.CloneDefaults(ItemID.Katana);
			Item.damage = 95;
			Item.DamageType = DamageClass.Melee;
			Item.width = 56;
			Item.height = 18;
			Item.useAnimation = Item.useTime = 12;
			//item.useTime = 15;
			//item.useAnimation = 15;
			//item.useStyle = 5;
			Item.knockBack = 6;
			Item.value*=10;
			Item.rare+=5;
			Item.scale = 1.1f;
			Item.shoot = ModContent.ProjectileType<Whip_Sword_Whip>();
			Item.shootSpeed = 5f;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(Mod, "ArtificerBonus", "Melee");
			float m = Main.mouseTextColor / 255f;
            line.OverrideColor = new Color((int)(179 * m), (int)(50 * m), 0);
            tooltips.Insert(1, line);
        }
		public override bool AltFunctionUse(Player player) => true;
		public override bool CanUseItem(Player player){
			if(player.altFunctionUse == 2){
				ReloadProj p = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, new Vector2(0,0), ModContent.ProjectileType<ReloadProj>(), 0, 0, player.whoAmI, 30).ModProjectile as ReloadProj;
				p.SetDefaults();
				if(extended)p.Tick = (proj,tick)=>{
					SoundEngine.PlaySound(SoundID.Item37.WithPitch(0).WithVolume(0.33f), proj.Center);
				};
				p.Reload = ()=>{extended^=true;};
				p.ticks = new List<ReloadTick>(){3,6,9,12,15,18,21,24,27,30};
				Item.noMelee = true;
				return base.CanUseItem(player);
			}
			return true;
		}
		public override float UseSpeedMultiplier(Player player) {
			return 1f/(player.altFunctionUse*2+1);
		}
		public override bool CanShoot(Player player) {
			return extended;
		}
	}
	public class Whip_Sword_Whip : ModProjectile {
        public override string Texture => "Terraria/Images/Projectile_"+ ProjectileID.MaceWhip;
		public override void SetDefaults(){
			Projectile.CloneDefaults(ProjectileID.MaceWhip);
		}
	}
}
