using System;
using System.Collections.Generic;
using Artifice.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Items {
	//day 7
	public class Catherine_Wheel : ModItem {
		protected override bool CloneNewInstances => true;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Catherine Wheel");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults(){
			Item.CloneDefaults(ItemID.MonkStaffT3);
			Item.damage = 60;
			Item.DamageType = DamageClass.Melee;
			Item.width = 56;
			Item.height = 18;
			Item.useAnimation = Item.useTime = 40;
			//item.useTime = 15;
			//item.useAnimation = 15;
			//item.useStyle = 5;
			Item.knockBack = 6;
			Item.value*=10;
			Item.rare+=5;
			Item.scale = 1.1f;
			//item.shoot = ProjectileID.DD2FlameBurstTowerT1Shot;
			Item.shootSpeed = 12.5f;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(Mod, "ArtificerBonus", "Melee");
            line.OverrideColor = new Color(179, 50, 0);
            tooltips.Insert(1, line);
        }
		public override bool AltFunctionUse(Player player) => true;
		public override bool CanUseItem(Player player){
			if(player.altFunctionUse == 2){
				Item.shoot = ModContent.ProjectileType<Catherine_Wheel_Throw>();
			    //item.useAnimation = item.useTime = 24;
				Item.shootSpeed = 12.5f;
			}else{
				Item.shoot = ModContent.ProjectileType<Catherine_Wheel_Spin>();
			    //item.useAnimation = item.useTime = 12;
				Item.shootSpeed = 0f;
			}
			return true;
		}
		public override void AddRecipes(){
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Wood, 10);
			recipe.AddIngredient(ItemID.FireworkFountain, 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
	public class Catherine_Wheel_Spin : ModProjectile {
        public override string Texture => "Artifice/Items/Catherine_Wheel_P";
		public override void SetDefaults(){
			Projectile.CloneDefaults(ProjectileID.MonkStaffT3);
			//projectile.timeLeft = 25;
			Projectile.penetrate = -1;
			Projectile.light = 0;
			Projectile.aiStyle = 0;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 1;
		}
		public override void AI(){
			Player player = Main.player[Projectile.owner];
			Projectile.Center = player.MountedCenter;
			//Lighting.AddLight(projectile.Center, 0.75f, 0.9f, 1.15f);
			bool kil = false;
			if (Main.myPlayer == Projectile.owner)
            {
                if (!player.channel || player.noItems || player.CCed)
                {
					kil = true;
                }
            }
            player.itemTime = 2;
            player.itemAnimation = 2;
            Projectile.rotation += 0.23f * player.direction;
			for(int i = 0; i < 8; i++){
				Vector2 value4 = (Projectile.rotation - 0.7853982f*player.direction*i).ToRotationVector2().RotatedBy((double)(1.57079637f * (float)Projectile.spriteDirection), default(Vector2));
				if (!Main.rand.NextBool(6)){
					Dust dust4 = Dust.NewDustDirect(Projectile.position, 0, 0, DustID.Firework_Red, 0f, 0f, 100, default(Color), 1f);//226
					dust4.position = Projectile.Center + (Projectile.rotation - 0.7853982f * player.direction*i).ToRotationVector2() * (60f + Main.rand.NextFloat() * 20f);
					dust4.velocity = -value4 * (4f + 4f * Main.rand.NextFloat());
					dust4.noGravity = true;
					dust4.noLight = true;
					dust4.scale = 0.5f;
					dust4.customData = Projectile;
					if (Main.rand.NextBool(4)){
						dust4.noGravity = false;
					}
				}
				value4 = (Projectile.rotation - 0.7853982f*-player.direction*i).ToRotationVector2().RotatedBy((double)(1.57079637f * (float)Projectile.spriteDirection), default(Vector2));
				if (!Main.rand.NextBool(6)){
					Dust dust4 = Dust.NewDustDirect(Projectile.position, 0, 0, DustID.Firework_Red, 0f, 0f, 100, default(Color), 1f);//226
					dust4.position = Projectile.Center + (Projectile.rotation - 0.7853982f * player.direction*i).ToRotationVector2() * (60f + Main.rand.NextFloat() * 20f);
					dust4.velocity = -value4 * (4f + 4f * Main.rand.NextFloat());
					dust4.noGravity = true;
					dust4.noLight = true;
					dust4.scale = 0.5f;
					dust4.customData = Projectile;
					if (Main.rand.NextBool(4)){
						dust4.noGravity = false;
					}
				}
			}
            Projectile target;
            ArtificeGlobalProjectile globalTarget;
            for (int i = 0; i<Main.projectile.Length; i++){
                target = Main.projectile[i];
				Vector2 intersect = new Vector2(MathHelper.Clamp(Projectile.Center.X, target.Hitbox.Left, target.Hitbox.Right),MathHelper.Clamp(Projectile.Center.Y, target.Hitbox.Top, target.Hitbox.Bottom));
				float dist = (Projectile.Center-intersect).Length();
                if (dist<80&&target.type != Projectile.type&&(target.damage > 0 || target.npcProj)&&(target.owner!=Projectile.owner||target.npcProj||target.trap||target.hostile)) {
                    //player.chatOverhead.NewMessage((ray.Intersects(Main.projectile[i].Hitbox.toBB())??(object)"null").ToString(),5);
                    //float angle = Main.projectile[i].velocity.ToRotation();
                    //Main.projectile[i].velocity = (Main.projectile[i].velocity.RotatedBy(-angle)*new Vector2(-1, 1)).RotatedBy(angle);
                    //Main.NewText(angle+" "+Main.projectile[i].velocity.ToRotation());
                    globalTarget = target.GetGlobalProjectile<ArtificeGlobalProjectile>();
                    if(globalTarget.spinResistHP<target.damage) {
                        globalTarget.spinResistHP+=Projectile.damage/8;
                    } else {
					    if(target.tileCollide){
						    target.velocity = target.velocity.RotatedBy(player.direction*0.25f);
					    }else{
						    target.velocity = Vector2.Lerp(target.velocity, new Vector2(kil?dist:640/dist,0).RotatedBy((target.Center-Projectile.Center).ToRotation()+player.direction*1.9f), 1f);
					    }
					    target.friendly = true;
					    target.hostile = false;
                    }
                }
            }
			if(kil)Projectile.Kill();
			//* player.direction;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
			Vector2 intersect = new Vector2(MathHelper.Clamp(Projectile.Center.X, target.Hitbox.Left, target.Hitbox.Right),MathHelper.Clamp(Projectile.Center.Y, target.Hitbox.Top, target.Hitbox.Bottom));
			Projectile.localNPCImmunity[target.whoAmI] = (int)(Projectile.Distance(intersect)/8)+5;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox){
			Vector2 intersect = new Vector2(MathHelper.Clamp(Projectile.Center.X, targetHitbox.Left, targetHitbox.Right),MathHelper.Clamp(Projectile.Center.Y, targetHitbox.Top, targetHitbox.Bottom));
			float dist = (Projectile.Center-intersect).Length();
            return dist<80;//&&Main.rand.NextBool((int)(dist * dist));
        }
        public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0);
            return true;
        }
	}
	public class Catherine_Wheel_Throw : ModProjectile{
        public override string Texture => "Artifice/Items/Catherine_Wheel";
		//public static int id;
		public override void SetDefaults(){
			Projectile.CloneDefaults(ProjectileID.LightDisc);
			Projectile.width = Projectile.height = 32;
			Projectile.penetrate = -1;
			Projectile.ai[0] = 1f;
			//id = ModContent.ProjectileType<Catherine_Wheel_Throw>();
		}
		public override void AI(){
			//projectile.type = id;
			Player player = Main.player[Projectile.owner];
			for(int i = 0; i < 8; i++){
				Vector2 value4 = (Projectile.rotation - 0.7853982f * player.direction*i).ToRotationVector2().RotatedBy((double)(1.57079637f * (float)Projectile.spriteDirection), default(Vector2));
				for (int j = 0; j < 2; j++){
					if (!Main.rand.NextBool(6)){
						Dust dust4 = Dust.NewDustDirect(Projectile.position, 0, 0, DustID.Firework_Red, 0f, 0f, 100, default(Color), 1f);//226
						dust4.position = Projectile.Center + (Projectile.rotation - 0.7853982f * player.direction*i).ToRotationVector2() * (15f + Main.rand.NextFloat() * 5f);
						dust4.velocity = -value4 * (4f + 4f * Main.rand.NextFloat());
						dust4.noGravity = true;
						dust4.noLight = true;
						dust4.scale = 0.5f;
						dust4.customData = Projectile;
						if (Main.rand.NextBool(4)){
							dust4.noGravity = false;
						}
					}
				}
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
			Vector2 intersect = new Vector2(MathHelper.Clamp(Projectile.Center.X, target.Hitbox.Left, target.Hitbox.Right),MathHelper.Clamp(Projectile.Center.Y, target.Hitbox.Top, target.Hitbox.Bottom));
			Projectile.localNPCImmunity[target.whoAmI] = (int)(Projectile.Distance(intersect)/4)+2;
			Projectile.aiStyle = 3;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox){
			//projectile.type = 301;
			Vector2 intersect = new Vector2(MathHelper.Clamp(Projectile.Center.X, targetHitbox.Left, targetHitbox.Right),MathHelper.Clamp(Projectile.Center.Y, targetHitbox.Top, targetHitbox.Bottom));
			float dist = (Projectile.Center-intersect).Length();
			if(dist<40)Projectile.aiStyle = 0;
            return dist<40;//&&Main.rand.NextBool((int)(dist * dist));
        }
	}
	public class Catherine_Wheel_Mistake : ModProjectile {
        public override string Texture => "Artifice/Items/Catherine_Wheel_P";
		public override void SetDefaults(){
			Projectile.CloneDefaults(ProjectileID.MonkStaffT3);
			//projectile.timeLeft = 25;
			Projectile.penetrate = -1;
			Projectile.light = 0;
			Projectile.aiStyle = 0;
		}
		public override void AI(){
			Player player = Main.player[Projectile.owner];
			Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
			Projectile.direction = player.direction;
			player.heldProj = Projectile.whoAmI;
			Projectile.Center = vector;
			if (player.dead){
				Projectile.Kill();
				return;
			}
			if (!player.frozen){
				Lighting.AddLight(player.Center, 0.75f, 0.9f, 1.15f);
				Projectile.spriteDirection = (Projectile.direction = player.direction);
				Projectile.alpha -= 127;
				if (Projectile.alpha < 0){
					Projectile.alpha = 0;
				}
				float num8 = (float)player.itemAnimation / (float)player.itemAnimationMax;
				float num9 = 1f - num8;
				float num10 = Projectile.velocity.ToRotation();
				float num11 = Projectile.velocity.Length();
				float num12 = 22f;
				Vector2 spinningpoint2 = new Vector2(1f, 0f).RotatedBy((double)(3.14159274f + num9 * 6.28318548f), default(Vector2)) * new Vector2(num11, Projectile.ai[0]);
				Projectile.position += spinningpoint2.RotatedBy((double)num10, default(Vector2)) + new Vector2(num11 + num12, 0f).RotatedBy((double)num10, default(Vector2));
				Vector2 destination2 = vector + spinningpoint2.RotatedBy((double)num10, default(Vector2)) + new Vector2(num11 + num12 + 40f, 0f).RotatedBy((double)num10, default(Vector2));
				Projectile.rotation = player.AngleTo(destination2) + 0.7853982f * (float)player.direction;
				if (Projectile.spriteDirection == -1){
					Projectile.rotation += 3.14159274f;
				}
				player.DirectionTo(Projectile.Center);
				player.DirectionTo(destination2);
				Vector2 vector3 = Projectile.velocity.SafeNormalize(Vector2.UnitY);
				if ((player.itemAnimation == 2 || player.itemAnimation == 6 || player.itemAnimation == 10) && Projectile.owner == Main.myPlayer){
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
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vector4, ProjectileID.MonkStaffT3_AltShot, Projectile.damage, 0f, Projectile.owner, 0f, 0f);
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
					if (!Main.rand.NextBool(6)){
						num13 *= 1.2f;
						Dust dust3 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, 0f, 0f, 100, default(Color), 1f);
						dust3.velocity = vector3 * (4f + 4f * Main.rand.NextFloat()) * num13 * scaleFactor;
						dust3.noGravity = true;
						dust3.noLight = true;
						dust3.scale = 0.75f;
						dust3.fadeIn = 0.8f;
						dust3.customData = Projectile;
						if (Main.rand.NextBool(3)){
							dust3.noGravity = false;
							dust3.fadeIn = 0f;
						}
					}
				}
			}
		}

        public override bool PreDraw(ref Color lightColor) {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1f, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            return false;
        }
	}
}
