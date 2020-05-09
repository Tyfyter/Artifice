using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Items {
	//day 1
	public class Gerb : ModItem {
		public override bool CloneNewInstances => true;
		public float Ammo = 1f;
		public bool held = false;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Gerb");
			Tooltip.SetDefault("Gerb");
		}
		public override void SetDefaults(){
			item.damage = 15;
			item.magic = true;
			item.noMelee = true;
			item.width = 34;
			item.height = 16;
			item.useTime = 5;
			item.useAnimation = 5;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item34;
			item.shoot = ProjectileID.FlamesTrap;
			//item.shoot = ProjectileID.DD2FlameBurstTowerT1Shot;
			item.shootSpeed = 12.5f;
			item.autoReuse = true;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(mod, "ArtificerBonus", "Magic");
            line.overrideColor = new Color(179, 50, 0);
            tooltips.Insert(1, line);
        }
		public override Vector2? HoldoutOffset(){
			return new Vector2(-2, -2);
		}
		public override void AddRecipes(){
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Fireblossom, 1);
			recipe.AddIngredient(ItemID.ExplosivePowder, 5);
			recipe.AddIngredient(ItemID.FireworkFountain, 3);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void HoldItem(Player player){
			held = true;
			Ammo = MathHelper.Clamp(Ammo, 0, 1);
		}
		public override bool AltFunctionUse(Player player) => Ammo<1;
		public override bool CanUseItem(Player player){
			item.noUseGraphic = player.altFunctionUse==2;
			item.UseSound = player.altFunctionUse==2?null:SoundID.Item34;
			return base.CanUseItem(player);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			if(player.altFunctionUse==2){
				if(player.controlUseItem){
					Ammo+=0.025f;
					player.itemAnimation = item.useAnimation-1;
				}else{
					player.itemAnimation = 0;
				}
			}
			if(player.altFunctionUse==2||Ammo<=0)return false;
			if(player.itemAnimation>2){
				if(player.controlUseItem){
					player.itemAnimation = item.useAnimation-1;
				}else{
					player.itemAnimation = 0;
				}
				//Main.PlaySound(useSound, position);
				Ammo-=0.02f;
				for(int i = Ammo<=0?5:1; i > 0; i--){
                int proj = Projectile.NewProjectile(position + new Vector2(speedX,speedY), new Vector2(speedX,speedY).RotatedByRandom(Ammo<=0?0.4:0.2), type, damage, knockBack, item.owner);
					Main.projectile[proj].friendly = true;
					Main.projectile[proj].hostile = false;
					Main.projectile[proj].usesLocalNPCImmunity = true;
					Main.projectile[proj].localNPCHitCooldown = Ammo<=0?0:9;
					Main.projectile[proj].timeLeft = Ammo<=0?35:20;
					Main.projectile[proj].npcProj = false;
					Main.projectile[proj].trap = false;
					if(Ammo<=0)Main.PlaySound(2, position, 14);
				}
				return false;
			}
			return false;
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale){
            if(held){
                Player player = Main.player[item.owner];
				Utils.DrawBorderStringFourWay(spriteBatch, Main.fontCombatText[1], (int)(Ammo*50)+"/50", Main.screenWidth*0.90f, Main.screenHeight*0.85f, Color.White, Color.Black, new Vector2(0.3f), 1);
				held = false;
            }
        }
	}
	public class Tubri : ModItem {
		public override bool CloneNewInstances => true;
		public float Ammo = 1f;
		public int count = 0;
		public bool held = false;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Tubri");
			Tooltip.SetDefault("Not Gerb");
		}
		public override void SetDefaults(){
			item.damage = 35;
			item.magic = true;
			item.noMelee = true;
			item.width = 40;
			item.height = 16;
			item.useTime = 5;
			item.useAnimation = 5;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item34;
			item.shoot = ModContent.ProjectileType<TubriShot>();
			//item.shoot = ProjectileID.DD2FlameBurstTowerT1Shot;
			item.shootSpeed = 12.5f;
			item.autoReuse = true;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(mod, "ArtificerBonus", "Magic");
            line.overrideColor = new Color(179, 50, 0);
            tooltips.Insert(1, line);
        }
		public override void AddRecipes(){
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SoulofLight, 10);
			recipe.AddIngredient(ItemID.FireworkFountain, 3);
			recipe.AddTile(TileID.LihzahrdAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override Vector2? HoldoutOffset(){
			return new Vector2(-2, -2);
		}
		public override void HoldStyle(Player player){
			count = 0;
		}
		public override void HoldItem(Player player){
			held = true;
			Ammo = MathHelper.Clamp(Ammo, 0, 1);
		}
		public override bool AltFunctionUse(Player player) => Ammo<1;
		public override bool CanUseItem(Player player){
			item.noUseGraphic = player.altFunctionUse==2;
			item.UseSound = player.altFunctionUse==2?null:SoundID.Item34;
			return base.CanUseItem(player);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			if(player.altFunctionUse==2){
				if(player.controlUseItem){
					Ammo+=0.06f;
					player.itemAnimation = item.useAnimation-1;
				}else{
					player.itemAnimation = 0;
				}
			}
			if(player.altFunctionUse==2||Ammo<=0)return false;
			if(player.itemAnimation>2){
				if(player.controlUseItem){
					player.itemAnimation = item.useAnimation-1;
				}else{
					player.itemAnimation = 0;
				}
				//Main.PlaySound(useSound, position);
				float speed = new Vector2(speedX,speedY).Length();
				Ammo-=0.01f;
				count++;
				Vector2 dist = Main.MouseWorld - position;
				speedY-= (Math.Abs(dist.X/512)-dist.Y/2048);
				player.itemRotation = (new Vector2(speedX,speedY)*player.direction).ToRotation();
                Projectile.NewProjectile(position + new Vector2(speedX,speedY), (new Vector2(speedX,speedY).SafeNormalize(Vector2.Zero)*speed).RotatedByRandom(0.1), type, (int)(damage*(1+(count/100f))), knockBack, item.owner);
				return false;
			}
			return false;
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale){
            if(held){
                Player player = Main.player[item.owner];
				Utils.DrawBorderStringFourWay(spriteBatch, Main.fontCombatText[1], (int)(Ammo*100)+"/100", Main.screenWidth*0.90f, Main.screenHeight*0.85f, Color.White, Color.Black, new Vector2(0.3f), 1);
				held = false;
            }
        }
	}
	public class TubriShot : ModProjectile{
        public override string Texture => "Terraria/Projectile_188";
		public override void SetDefaults(){
			projectile.CloneDefaults(188);
			projectile.aiStyle = 0;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 7;
			projectile.timeLeft = 120;
			projectile.npcProj = false;
			projectile.trap = false;
		}
		public override void AI(){
			float num297 = 1f;
			if (projectile.ai[0] == 0f){
				num297 = 0.25f;
			}
			else if (projectile.ai[0] == 1f){
				num297 = 0.5f;
			}
			else if (projectile.ai[0] == 2f){
				num297 = 0.75f;
			}
			else if (projectile.ai[0] == 10f){
				projectile.aiStyle = 1;
			}
			projectile.ai[0] += 1f;
			if (Main.rand.Next(1) == 0){
				int num3;
				for (int num299 = 0; num299 < 1; num299 = num3 + 1){
					int num300 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100, default(Color), 1f);
					Dust dust3;
					if (Main.rand.Next(3) != 0){
						Main.dust[num300].noGravity = true;
						dust3 = Main.dust[num300];
						dust3.scale *= 3f;
						Dust dust52 = Main.dust[num300];
						dust52.velocity.X = dust52.velocity.X * 2f;
						Dust dust53 = Main.dust[num300];
						dust53.velocity.Y = dust53.velocity.Y * 2f;
					}
					Main.dust[num300].scale *= 1.25f;
					Dust dust54 = Main.dust[num300];
					dust54.velocity.X = dust54.velocity.X * 1.2f;
					Dust dust55 = Main.dust[num300];
					dust55.velocity.Y = dust55.velocity.Y * 1.2f;
					dust3 = Main.dust[num300];
					dust3.scale *= num297;
					num3 = num299;
				}
			}
			projectile.rotation += 0.3f * (float)projectile.direction;
		}
	}
}
