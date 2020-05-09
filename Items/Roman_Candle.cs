using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Items {
	//day 8
	//this took ~1.5 hours
	public class Roman_Candle : ModItem {
		public override bool CloneNewInstances => true;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Roman Candle");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults(){
			item.damage = 17;
			item.magic = true;
			item.ranged = true;
			item.noMelee = true;
			item.width = 34;
			item.height = 16;
			item.useTime = 16;
			item.useAnimation = 16;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 2;
			item.crit = 6;
			item.UseSound = SoundID.Item34;
			item.shoot = ModContent.ProjectileType<Roman_Candle_P>();
			item.ammo = item.type;
			item.useAmmo = item.type;
			//item.shoot = ProjectileID.DD2FlameBurstTowerT1Shot;
			item.shootSpeed = 12.5f;
			item.autoReuse = true;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(mod, "ArtificerBonus", "Ranged/Magic");
            line.overrideColor = new Color(179, 50, 0);
            tooltips.Insert(1, line);
        }
		public override Vector2? HoldoutOffset(){
			return new Vector2(-2, -2);
		}
		public override void UpdateInventory(Player player){
			player.GetModPlayer<ArtificerPlayer>().hasRC = true;
		}
		public override bool AltFunctionUse(Player player) => true;
		public override bool CanUseItem(Player player){
			if(player.altFunctionUse == 2){
				item.useTime = 23;
				item.useAnimation = 23;
			}else{
				item.useTime = 17;
				item.useAnimation = 17;
			}
			return base.CanUseItem(player);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			int i = type-ModContent.ProjectileType<Roman_Candle_P>();
            Vector2 oofset = new Vector2(speedX,speedY).RotatedBy(-player.direction*Math.PI/2)/3;
            position+=oofset;
			if(player.altFunctionUse==2){
				damage = (int)(damage*1.5);
                Projectile.NewProjectileDirect(position + new Vector2(speedX,speedY), new Vector2(speedX,speedY), ModContent.ProjectileType<Roman_Candle_P>(), damage, knockBack, item.owner, i, 1).timeLeft-=150;
			} else {
                Projectile.NewProjectile(position + new Vector2(speedX,speedY), new Vector2(speedX,speedY).RotatedByRandom(0.05), ModContent.ProjectileType<Roman_Candle_P>(), damage, knockBack, item.owner, i, 0);
			}
			return false;
		}
	}
	public class KClO4 : ModItem {
		public override bool CloneNewInstances => true;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Cursed Firework Star");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults(){
			item.damage = 33;
			item.magic = true;
			item.ranged = true;
			item.noMelee = true;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 4;
			item.shoot = ModContent.ProjectileType<Roman_Candle_P>()+1;
			item.ammo = ModContent.ItemType<Roman_Candle>();
			item.shootSpeed = 12.5f;
			item.maxStack = 999;
			item.consumable = true;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(mod, "ArtificerBonus", "Ranged/Magic");
            line.overrideColor = new Color(179, 50, 0);
            tooltips.Insert(1, line);
        }
		public override void AddRecipes(){
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ExplosivePowder, 1);
			recipe.AddIngredient(ItemID.CursedFlame, 1);
			recipe.SetResult(this, 5);
			recipe.AddRecipe();
		}
	}
	public class P4 : ModItem {
		public override bool CloneNewInstances => true;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("P4");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults(){
			item.damage = 83;
			item.magic = true;
			item.ranged = true;
			item.noMelee = true;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 4;
			item.shoot = ModContent.ProjectileType<Roman_Candle_P>()+2;
			item.ammo = ModContent.ItemType<Roman_Candle>();
			item.shootSpeed = 12.5f;
			item.maxStack = 999;
			item.consumable = true;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(mod, "ArtificerBonus", "Ranged/Magic");
            line.overrideColor = new Color(179, 50, 0);
            tooltips.Insert(1, line);
			if(tooltips[6].Name=="Ammo")tooltips.RemoveAt(6);
        }
	}
	public class Roman_Candle_P : ModProjectile{
        public override string Texture => "Terraria/Projectile_188";
		public static Color[] colors = new Color[]{Color.DarkGoldenrod,Color.LimeGreen,Color.Goldenrod};
		public static int[][] dusts = new int[][]{
			new int[]{222,0},
			new int[]{75},
			new int[]{170,170,170,222}
		};
		public static float[] scales = new float[]{1.5f,0.5f,2};
		
		public override void SetDefaults(){
			projectile.CloneDefaults(ProjectileID.LightDisc);
			projectile.width = projectile.height = 10;
			projectile.penetrate = -1;
			projectile.aiStyle = 0;
			projectile.timeLeft = 180;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
			projectile.extraUpdates = 2;
			//id = ModContent.ProjectileType<Catherine_Wheel_Throw>();
		}
		public override void AI(){
			//projectile.type = id;
			projectile.rotation = projectile.velocity.ToRotation();
			Player player = Main.player[projectile.owner];
			int[] dust = dusts[(int)projectile.ai[0]];
			if(projectile.ai[1]==1){
				if(projectile.timeLeft<20){
					if(projectile.timeLeft<=1){
						projectile.ai[1] = 2;
						Vector2 vec = projectile.Center;
						projectile.width*=12;
						projectile.height*=12;
						projectile.Center = vec;
						projectile.tileCollide = false;
                		Main.PlaySound(SoundID.Item38, projectile.Center);
						for (int j = 0; j < 25; j++){
							if (Main.rand.Next(6) != 0){
								Dust dust4 = Dust.NewDustDirect(projectile.Center, 0, 0, dust[j%dust.Length], 0f, 0f, 100, colors[(int)projectile.ai[0]], 1f);//226
								dust4.velocity = new Vector2(4,0).RotatedByRandom(Math.PI) * (1f + Main.rand.NextFloat());
								dust4.noGravity = false;
								dust4.scale = scales[(int)projectile.ai[1]];
							}
							if (projectile.ai[0]==2&&Main.rand.Next(6) != 0){
								Dust dust4 = Dust.NewDustDirect(projectile.Center, 0, 0, DustID.Smoke, 0f, 0f, 100, default(Color), 1f);//226
								dust4.velocity = new Vector2(4,0).RotatedByRandom(Math.PI) * Main.rand.NextFloat();
								dust4.noGravity = false;
								dust4.scale = 2.5f;
							}
						}
					}
					return;
				}
			}
			for (int j = 0; j < 5; j++){
				if (Main.rand.Next(6) != 0){
					Dust dust4 = Dust.NewDustDirect(projectile.Center, 0, 0, dust[j%dust.Length], 0f, 0f, 100, colors[(int)projectile.ai[0]], 1f);//226
					dust4.velocity = new Vector2(8,0).RotatedBy(projectile.rotation) * (1f + Main.rand.NextFloat());
					dust4.noGravity = !Main.rand.NextBool(5);
					dust4.scale = scales[(int)projectile.ai[1]];
				}
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity){
			if(projectile.ai[1]==1){
				projectile.timeLeft = 2;
				return false;
			}
			return true;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
			switch ((int)projectile.ai[1]){
				case 1:
				projectile.velocity*=0;
				projectile.timeLeft = 2;
				projectile.localNPCImmunity[target.whoAmI] = 0;
				break;
				case 2:
				if(!target.noGravity){
					Vector2 targ = new Vector2(MathHelper.Clamp(projectile.Center.X, target.position.X, target.position.X + target.width), 
					MathHelper.Clamp(projectile.Center.Y, target.position.Y, target.position.Y + target.height));
					Vector2 vel = (targ-projectile.Center).SafeNormalize(Vector2.Zero);
					float dist = (targ-projectile.Center).Length();
					target.velocity+=vel*48/Math.Max(dist/16,2);
				}
				break;
				default:
				break;
			}
			switch ((int)projectile.ai[0]){
				case 1:
				target.AddBuff(BuffID.CursedInferno, 600);
				break;
				case 2:
				target.AddBuff(BuffID.Daybreak, 1200);
				target.AddBuff(BuffID.OnFire, 1200);
				break;
				default:
				target.AddBuff(BuffID.OnFire, 300);
				break;
			}
		}
		/*public override void AI(){
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
		}*/
	}
}
