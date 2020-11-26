using System;
using System.Collections.Generic;
using Artifice.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Items {
	//day 7
	public class Catherine_Wheel : ModItem {
		public override bool CloneNewInstances => true;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Catherine Wheel");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults(){
			item.CloneDefaults(ItemID.MonkStaffT3);
			item.damage = 60;
			item.melee = true;
			item.width = 56;
			item.height = 18;
			item.useAnimation = item.useTime = 40;
			//item.useTime = 15;
			//item.useAnimation = 15;
			//item.useStyle = 5;
			item.knockBack = 6;
			item.value*=10;
			item.rare+=5;
			item.scale = 1.1f;
			//item.shoot = ProjectileID.DD2FlameBurstTowerT1Shot;
			item.shootSpeed = 12.5f;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(mod, "ArtificerBonus", "Melee");
            line.overrideColor = new Color(179, 50, 0);
            tooltips.Insert(1, line);
        }
		public override bool AltFunctionUse(Player player) => true;
		public override bool CanUseItem(Player player){
			if(player.altFunctionUse == 2){
				item.shoot = ModContent.ProjectileType<Catherine_Wheel_Throw>();
			    //item.useAnimation = item.useTime = 24;
				item.shootSpeed = 12.5f;
			}else{
				item.shoot = ModContent.ProjectileType<Catherine_Wheel_Spin>();
			    //item.useAnimation = item.useTime = 12;
				item.shootSpeed = 0f;
			}
			return true;
		}
		public override void AddRecipes(){
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wood, 10);
			recipe.AddIngredient(ItemID.FireworkFountain, 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class Catherine_Wheel_Spin : ModProjectile {
        public override string Texture => "Artifice/Items/Catherine_Wheel_P";
		public override void SetDefaults(){
			projectile.CloneDefaults(ProjectileID.MonkStaffT3);
			//projectile.timeLeft = 25;
			projectile.penetrate = -1;
			projectile.light = 0;
			projectile.aiStyle = 0;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 1;
		}
		public override void AI(){
			Player player = Main.player[projectile.owner];
			projectile.Center = player.MountedCenter;
			//Lighting.AddLight(projectile.Center, 0.75f, 0.9f, 1.15f);
			bool kil = false;
			if (Main.myPlayer == projectile.owner)
            {
                if (!player.channel || player.noItems || player.CCed)
                {
					kil = true;
                }
            }
            player.itemTime = 2;
            player.itemAnimation = 2;
            projectile.rotation += 0.23f * player.direction;
			for(int i = 0; i < 8; i++){
				Vector2 value4 = (projectile.rotation - 0.7853982f*player.direction*i).ToRotationVector2().RotatedBy((double)(1.57079637f * (float)projectile.spriteDirection), default(Vector2));
				if (Main.rand.Next(6) != 0){
					Dust dust4 = Dust.NewDustDirect(projectile.position, 0, 0, 130, 0f, 0f, 100, default(Color), 1f);//226
					dust4.position = projectile.Center + (projectile.rotation - 0.7853982f * player.direction*i).ToRotationVector2() * (60f + Main.rand.NextFloat() * 20f);
					dust4.velocity = -value4 * (4f + 4f * Main.rand.NextFloat());
					dust4.noGravity = true;
					dust4.noLight = true;
					dust4.scale = 0.5f;
					dust4.customData = projectile;
					if (Main.rand.Next(4) == 0){
						dust4.noGravity = false;
					}
				}
				value4 = (projectile.rotation - 0.7853982f*-player.direction*i).ToRotationVector2().RotatedBy((double)(1.57079637f * (float)projectile.spriteDirection), default(Vector2));
				if (Main.rand.Next(6) != 0){
					Dust dust4 = Dust.NewDustDirect(projectile.position, 0, 0, 130, 0f, 0f, 100, default(Color), 1f);//226
					dust4.position = projectile.Center + (projectile.rotation - 0.7853982f * player.direction*i).ToRotationVector2() * (60f + Main.rand.NextFloat() * 20f);
					dust4.velocity = -value4 * (4f + 4f * Main.rand.NextFloat());
					dust4.noGravity = true;
					dust4.noLight = true;
					dust4.scale = 0.5f;
					dust4.customData = projectile;
					if (Main.rand.Next(4) == 0){
						dust4.noGravity = false;
					}
				}
			}
            Projectile target;
            ArtificeGlobalProjectile globalTarget;
            for (int i = 0; i<Main.projectile.Length; i++){
                target = Main.projectile[i];
				Vector2 intersect = new Vector2(MathHelper.Clamp(projectile.Center.X, target.Hitbox.Left, target.Hitbox.Right),MathHelper.Clamp(projectile.Center.Y, target.Hitbox.Top, target.Hitbox.Bottom));
				float dist = (projectile.Center-intersect).Length();
                if (dist<80&&target.type != projectile.type&&(target.damage > 0 || target.npcProj)&&(target.owner!=projectile.owner||target.npcProj||target.trap||target.hostile)) {
                    //player.chatOverhead.NewMessage((ray.Intersects(Main.projectile[i].Hitbox.toBB())??(object)"null").ToString(),5);
                    //float angle = Main.projectile[i].velocity.ToRotation();
                    //Main.projectile[i].velocity = (Main.projectile[i].velocity.RotatedBy(-angle)*new Vector2(-1, 1)).RotatedBy(angle);
                    //Main.NewText(angle+" "+Main.projectile[i].velocity.ToRotation());
                    globalTarget = target.GetGlobalProjectile<ArtificeGlobalProjectile>();
                    if(globalTarget.spinResistHP<target.damage) {
                        globalTarget.spinResistHP+=projectile.damage/8;
                    } else {
					    if(target.tileCollide){
						    target.velocity = target.velocity.RotatedBy(player.direction*0.25f);
					    }else{
						    target.velocity = Vector2.Lerp(target.velocity, new Vector2(kil?dist:640/dist,0).RotatedBy((target.Center-projectile.Center).ToRotation()+player.direction*1.9f), 1f);
					    }
					    target.friendly = true;
					    target.hostile = false;
                    }
                }
            }
			if(kil)projectile.Kill();
			//* player.direction;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
			Vector2 intersect = new Vector2(MathHelper.Clamp(projectile.Center.X, target.Hitbox.Left, target.Hitbox.Right),MathHelper.Clamp(projectile.Center.Y, target.Hitbox.Top, target.Hitbox.Bottom));
			projectile.localNPCImmunity[target.whoAmI] = (int)(projectile.Distance(intersect)/8)+5;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox){
			Vector2 intersect = new Vector2(MathHelper.Clamp(projectile.Center.X, targetHitbox.Left, targetHitbox.Right),MathHelper.Clamp(projectile.Center.Y, targetHitbox.Top, targetHitbox.Bottom));
			float dist = (projectile.Center-intersect).Length();
            return dist<80;//&&Main.rand.NextBool((int)(dist * dist));
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = ModContent.GetTexture("Artifice/Items/Catherine_Wheel_P");
            spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, lightColor, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0f);
            return true;
        }
	}
	public class Catherine_Wheel_Throw : ModProjectile{
        public override string Texture => "Artifice/Items/Catherine_Wheel";
		//public static int id;
		public override void SetDefaults(){
			projectile.CloneDefaults(ProjectileID.LightDisc);
			projectile.width = projectile.height = 32;
			projectile.penetrate = -1;
			projectile.ai[0] = 1f;
			//id = ModContent.ProjectileType<Catherine_Wheel_Throw>();
		}
		public override void AI(){
			//projectile.type = id;
			Player player = Main.player[projectile.owner];
			for(int i = 0; i < 8; i++){
				Vector2 value4 = (projectile.rotation - 0.7853982f * player.direction*i).ToRotationVector2().RotatedBy((double)(1.57079637f * (float)projectile.spriteDirection), default(Vector2));
				for (int j = 0; j < 2; j++){
					if (Main.rand.Next(6) != 0){
						Dust dust4 = Dust.NewDustDirect(projectile.position, 0, 0, 130, 0f, 0f, 100, default(Color), 1f);//226
						dust4.position = projectile.Center + (projectile.rotation - 0.7853982f * player.direction*i).ToRotationVector2() * (15f + Main.rand.NextFloat() * 5f);
						dust4.velocity = -value4 * (4f + 4f * Main.rand.NextFloat());
						dust4.noGravity = true;
						dust4.noLight = true;
						dust4.scale = 0.5f;
						dust4.customData = projectile;
						if (Main.rand.Next(4) == 0){
							dust4.noGravity = false;
						}
					}
				}
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
			Vector2 intersect = new Vector2(MathHelper.Clamp(projectile.Center.X, target.Hitbox.Left, target.Hitbox.Right),MathHelper.Clamp(projectile.Center.Y, target.Hitbox.Top, target.Hitbox.Bottom));
			projectile.localNPCImmunity[target.whoAmI] = (int)(projectile.Distance(intersect)/4)+2;
			projectile.aiStyle = 3;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox){
			//projectile.type = 301;
			Vector2 intersect = new Vector2(MathHelper.Clamp(projectile.Center.X, targetHitbox.Left, targetHitbox.Right),MathHelper.Clamp(projectile.Center.Y, targetHitbox.Top, targetHitbox.Bottom));
			float dist = (projectile.Center-intersect).Length();
			if(dist<40)projectile.aiStyle = 0;
            return dist<40;//&&Main.rand.NextBool((int)(dist * dist));
        }
	}
	public class Catherine_Wheel_Mistake : ModProjectile {
        public override string Texture => "Artifice/Items/Catherine_Wheel_P";
		public override void SetDefaults(){
			projectile.CloneDefaults(ProjectileID.MonkStaffT3);
			//projectile.timeLeft = 25;
			projectile.penetrate = -1;
			projectile.light = 0;
			projectile.aiStyle = 0;
		}
		public override void AI(){
			Player player = Main.player[projectile.owner];
			Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
			projectile.direction = player.direction;
			player.heldProj = projectile.whoAmI;
			projectile.Center = vector;
			if (player.dead){
				projectile.Kill();
				return;
			}
			if (!player.frozen){
				Lighting.AddLight(player.Center, 0.75f, 0.9f, 1.15f);
				projectile.spriteDirection = (projectile.direction = player.direction);
				projectile.alpha -= 127;
				if (projectile.alpha < 0){
					projectile.alpha = 0;
				}
				float num8 = (float)player.itemAnimation / (float)player.itemAnimationMax;
				float num9 = 1f - num8;
				float num10 = projectile.velocity.ToRotation();
				float num11 = projectile.velocity.Length();
				float num12 = 22f;
				Vector2 spinningpoint2 = new Vector2(1f, 0f).RotatedBy((double)(3.14159274f + num9 * 6.28318548f), default(Vector2)) * new Vector2(num11, projectile.ai[0]);
				projectile.position += spinningpoint2.RotatedBy((double)num10, default(Vector2)) + new Vector2(num11 + num12, 0f).RotatedBy((double)num10, default(Vector2));
				Vector2 destination2 = vector + spinningpoint2.RotatedBy((double)num10, default(Vector2)) + new Vector2(num11 + num12 + 40f, 0f).RotatedBy((double)num10, default(Vector2));
				projectile.rotation = player.AngleTo(destination2) + 0.7853982f * (float)player.direction;
				if (projectile.spriteDirection == -1){
					projectile.rotation += 3.14159274f;
				}
				player.DirectionTo(projectile.Center);
				player.DirectionTo(destination2);
				Vector2 vector3 = projectile.velocity.SafeNormalize(Vector2.UnitY);
				if ((player.itemAnimation == 2 || player.itemAnimation == 6 || player.itemAnimation == 10) && projectile.owner == Main.myPlayer){
					Vector2 vector4 = vector3 + Main.rand.NextVector2Square(-0.2f, 0.2f);
					vector4 *= 12f;
					int itemAnimation = player.itemAnimation;
					if (itemAnimation != 2){
						if (itemAnimation != 6){
							if (itemAnimation == 10){
								vector4 = vector3.RotatedBy(0.0, default(Vector2));
							}
						}
						else{
							vector4 = vector3.RotatedBy(-0.38397244612375897, default(Vector2));
						}
					}
					else{
						vector4 = vector3.RotatedBy(0.38397244612375897, default(Vector2));
					}
					vector4 *= 10f + (float)Main.rand.Next(4);
					Projectile.NewProjectile(projectile.Center, vector4, 709, projectile.damage, 0f, projectile.owner, 0f, 0f);
				}
				for (int j = 0; j < 3; j += 2){
					float scaleFactor = 1f;
					float num13 = 1f;
					switch (j){
					case 1:
						num13 = -1f;
						break;
					case 2:
						num13 = 1.25f;
						scaleFactor = 0.5f;
						break;
					case 3:
						num13 = -1.25f;
						scaleFactor = 0.5f;
						break;
					}
					if (Main.rand.Next(6) != 0){
						num13 *= 1.2f;
						Dust dust3 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 226, 0f, 0f, 100, default(Color), 1f);
						dust3.velocity = vector3 * (4f + 4f * Main.rand.NextFloat()) * num13 * scaleFactor;
						dust3.noGravity = true;
						dust3.noLight = true;
						dust3.scale = 0.75f;
						dust3.fadeIn = 0.8f;
						dust3.customData = projectile;
						if (Main.rand.Next(3) == 0){
							dust3.noGravity = false;
							dust3.fadeIn = 0f;
						}
					}
				}
			}
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, lightColor, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1f, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            return false;
        }
	}
}
