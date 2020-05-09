using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Items {
	//update 2, day 1
	public class Vulcan : ModItem {
		public override bool CloneNewInstances => true;
		public int Rate = 0;
		public int SlowTime = 0;
		public bool realConsume = false;
		SoundEffectInstance SEI = null;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("M61 Vulcan");
			Tooltip.SetDefault("\"It costs four hundred thousand dollars to fire this weapon...  for twelve seconds.\"");
		}
		public override void SetDefaults(){
			item.damage = 125;
			item.ranged = true;
			item.noMelee = true;
			item.width = 54;
			item.height = 15;
			item.useTime = 1;
			item.useAnimation = 15;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = null;
			item.shoot = 1;
			item.useAmmo = AmmoID.Bullet;
			//item.shoot = ProjectileID.DD2FlameBurstTowerT1Shot;
			item.shootSpeed = 12.5f;
			item.autoReuse = true;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(mod, "ArtificerBonus", "Ranged:Gun");
            line.overrideColor = new Color(179, 50, 0);
            tooltips.Insert(1, line);
        }
		public override void AddRecipes(){
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ChainGun, 1);
			recipe.AddIngredient(ItemID.FragmentVortex, 20);
			recipe.AddIngredient(ItemID.LunarBar, 20);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool ConsumeAmmo(Player player){
			return realConsume;
		}
		public override Vector2? HoldoutOffset(){
			return new Vector2(-4, 12);//Reload>10&&Reload<20?new Vector2(-24, 0):
		}
		public override void HoldStyle(Player player){
			realConsume = false;
			if(Rate>0&&++SlowTime>=8-Rate/2){
				Rate--;
				SlowTime = 0;
			}else if (Rate==0&&SlowTime<15){
				SlowTime++;
				if(SlowTime>=15&&SEI!=null&&SEI.State!=SoundState.Stopped){
					SEI.Stop();
				}
			}
		}
		public override void HoldItem(Player player){
			if(Rate>0){
				SEI = Main.PlaySound(new LegacySoundStyle(2, 113), player.Center);//109
				SEI.Pitch = (Rate-(SlowTime/(8-Rate/2f)))/15f;
			}
			if(Rate==0&&SlowTime<15){
				SEI = Main.PlaySound(new LegacySoundStyle(2, 113), player.Center);//109
				SEI.Pitch = 0;
			}
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			
			//Main.PlaySound(useSound, position);
			if(player.itemAnimation<=Rate+2){
				Vector2 oofset = new Vector2(speedX,speedY).RotatedBy(player.direction*Math.PI/2).SafeNormalize(-Vector2.UnitY)*13;
				position+=oofset;
				float s = 0;
				bool canShoot = false;
				realConsume = true;
				if(Rate<13){
					player.PickAmmo(item, ref type, ref s, ref canShoot, ref damage, ref knockBack, false);
					if(canShoot)Projectile.NewProjectile(position + new Vector2(speedX,speedY), new Vector2(speedX,speedY).RotatedByRandom(type==ProjectileID.ChlorophyteBullet?0.2f:0.075f), type, damage, knockBack, item.owner);
				}else{
					for(int i = Rate; i > 12; i--){
						player.PickAmmo(item, ref type, ref s, ref canShoot, ref damage, ref knockBack, false);
						if(canShoot)Projectile.NewProjectileDirect(position + new Vector2(speedX,speedY), new Vector2(speedX,speedY).RotatedByRandom(type==ProjectileID.ChlorophyteBullet?0.2f:0.075f), type, damage, knockBack, item.owner).timeLeft/=(int)(Rate/2.5f);
					}
				}
				realConsume = false;
				//Main.PlaySound(new LegacySoundStyle(2, 98), position).Pitch = 1;
				Main.PlaySound(new LegacySoundStyle(2, 11), position).Volume/=2;//110
				if(Rate<15)Rate++;
			}
			SlowTime = 0;
			//Main.PlaySound(2, position, 13);
			return false;
		}
	}
}
