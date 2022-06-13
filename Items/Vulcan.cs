using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Items {
	//update 2, day 1
	public class Vulcan : ModItem {
		protected override bool CloneNewInstances => true;
		public int Rate = 0;
		public int SlowTime = 0;
		public bool realConsume = false;
		SlotId SEI;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("M61 Vulcan");
			Tooltip.SetDefault("\"It costs four hundred thousand dollars to fire this weapon...  for twelve seconds.\"");
			SacrificeTotal = 1;
		}
		public override void SetDefaults(){
			Item.damage = 125;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 54;
			Item.height = 15;
			Item.useTime = 1;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = null;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.useAmmo = AmmoID.Bullet;
			//item.shoot = ProjectileID.DD2FlameBurstTowerT1Shot;
			Item.shootSpeed = 12.5f;
			Item.autoReuse = true;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(Mod, "ArtificerBonus", "Ranged:Gun");
            line.OverrideColor = new Color(179, 50, 0);
            tooltips.Insert(1, line);
        }
		public override void AddRecipes() {
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.ChainGun, 1);
			recipe.AddIngredient(ItemID.FragmentVortex, 20);
			recipe.AddIngredient(ItemID.LunarBar, 20);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
		public override bool CanConsumeAmmo(Item ammo, Player player){
			return realConsume;
		}
		public override Vector2? HoldoutOffset(){
			return new Vector2(-4, 12);//Reload>10&&Reload<20?new Vector2(-24, 0):
		}
		public override void HoldStyle(Player player, Rectangle heldItemFrame){
			realConsume = false;
			if(Rate>0&&++SlowTime>=8-Rate/2){
				Rate--;
				SlowTime = 0;
			}else if (Rate==0&&SlowTime<15){
				SlowTime++;
				if(SlowTime>=15&& SoundEngine.TryGetActiveSound(SEI, out ActiveSound? sound) && sound.IsPlaying){
					sound.Stop();
				}
			}
		}
		public override void HoldItem(Player player){
			if(Rate>0){
				SEI = SoundEngine.PlaySound(SoundID.Item113.WithPitch((Rate - (SlowTime / (8 - Rate / 2f))) / 15f), player.Center);//109
			}
			if(Rate==0&&SlowTime<15){
				SEI = SoundEngine.PlaySound(SoundID.Item113.WithPitch(0), player.Center);//109
			}
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			//Main.PlaySound(useSound, position);
			if(player.itemAnimation<=Rate+2){
				Vector2 oofset = velocity.RotatedBy(player.direction*Math.PI/2).SafeNormalize(-Vector2.UnitY)*13;
				position+=oofset;
				float s = 0;
				bool canShoot = false;
				realConsume = true;
				if(Rate<13){
					canShoot = player.PickAmmo(Item, out type, out s, out damage, out knockback, out _, false);
					if(canShoot)Projectile.NewProjectile(source, position + velocity, velocity.RotatedByRandom(type==ProjectileID.ChlorophyteBullet?0.2f:0.075f), type, damage, knockback, player.whoAmI);
				}else{
					for(int i = Rate; i > 12; i--){
						canShoot = player.PickAmmo(Item, out type, out s, out damage, out knockback, out _, false);
						if(canShoot)Projectile.NewProjectileDirect(source, position + velocity, velocity.RotatedByRandom(type==ProjectileID.ChlorophyteBullet?0.2f:0.075f), type, damage, knockback, player.whoAmI).timeLeft/=(int)(Rate/2.5f);
					}
				}
				realConsume = false;
				//Main.PlaySound(new LegacySoundStyle(2, 98), position).Pitch = 1;
				SoundEngine.PlaySound(SoundID.Item11.WithVolume(0.5f), position);//110
				if(Rate<15)Rate++;
			}
			SlowTime = 0;
			//Main.PlaySound(2, position, 13);
			return false;
		}
	}
}
