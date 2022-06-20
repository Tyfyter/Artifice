using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Items {
	//day 5
	//it was a better name than infurifier, and a way better name than "final solution"
	public class AbSolution : ModItem {
		protected override bool CloneNewInstances => true;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Absolution");
			Tooltip.SetDefault("");
			SacrificeTotal = 1;
		}
		public override void SetDefaults(){
			Item.CloneDefaults(ItemID.Clentaminator);
			Item.damage = 75;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 56;
			Item.height = 18;
			Item.useAnimation = Item.useTime = 7;
			//item.useTime = 15;
			//item.useAnimation = 15;
			//item.useStyle = 5;
			Item.knockBack = 6;
			Item.value*=2;
			Item.rare++;
			Item.shoot = ModContent.ProjectileType<Sol_0Green>();
			Item.useAmmo = AmmoID.Solution;
			//item.shoot = ProjectileID.DD2FlameBurstTowerT1Shot;
			//item.shootSpeed = 12.5f;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(Mod, "ArtificerBonus", "Ranged:Clentaminator");
            line.OverrideColor = new Color(179, 50, 0);
            tooltips.Insert(1, line);
        }
		public override void ModifyWeaponDamage(Player player, ref StatModifier damage){
			if (Item.useAmmo == AmmoID.Gel) return;
			Item.useAmmo = AmmoID.Gel;
			CombinedHooks.ModifyWeaponDamage(player, Item, ref damage);
			Item.useAmmo = AmmoID.Solution;
		}
		public override Vector2? HoldoutOffset(){
			return new Vector2(-12, 0);//Reload>10&&Reload<20?new Vector2(-24, 0):
		}
		public override void AddRecipes(){
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Clentaminator, 1);
			recipe.AddIngredient(ItemID.FragmentVortex, 18);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
		public override void HoldItem(Player player){
			player.cBack = 2;
			//Main.BackPackTexture[2]

			/*
			drawData = new DrawData(Main.BackPackTexture[2], new Vector2((float)((int)(Position.X - Main.screenPosition.X + (float)(drawPlayer.width / 2) - (float)(9 * drawPlayer.direction))) + num135 * (float)drawPlayer.direction, (float)((int)(Position.Y - Main.screenPosition.Y + (float)(drawPlayer.height / 2) + 2f * drawPlayer.gravDir + num136 * drawPlayer.gravDir))), new Rectangle?(new Rectangle(0, 0, Main.BackPackTexture[num134].Width, Main.BackPackTexture[num134].Height)), color12, drawPlayer.bodyRotation, new Vector2((float)(Main.BackPackTexture[num134].Width / 2), (float)(Main.BackPackTexture[num134].Height / 2)), 1f, spriteEffects, 0);
			drawData.shader = shader;
			Main.playerDrawData.Add(drawData);
			*/
		}
	}
	
	/*int num346 = 110;green
	if (projectile.type == 146){blue
		num346 = 111;
	}
	if (projectile.type == 147){purple
		num346 = 112;
	}
	if (projectile.type == 148){dark blue
		num346 = 113;
	}
	if (projectile.type == 149){red
		num346 = 114;
	}*/
	public class Sol_0Green : ModProjectile{
        public override string Texture => "Terraria/Images/Projectile_145";
		public override void SetDefaults(){
			Projectile.CloneDefaults(ProjectileID.PureSpray);
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 133;
			//projectile.extraUpdates = 4;
			Projectile.aiStyle = 0;
			Projectile.tileCollide = true;
		}
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Green Solution");
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox){
			float point = 0f;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center-Projectile.velocity/2, Projectile.Center, 22, ref point)){
				return true;
			}
            return null;
        }
		public override void AI(){
			SprAI(Projectile, 110);
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
			Player player = Main.player[Projectile.owner];
			if(player.ZoneCorrupt||player.ZoneCrimson||player.ZoneHallow){
				damage*=3;
				knockback*=1.5f;
			}
		}
		public static void SprAI(Projectile projectile, int color){
			/*if(color<110||color>114){
				int num349 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, color, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100, default(Color), 1f);
				Main.dust[num349].noGravity = true;
				Dust dust3 = Main.dust[num349];
				dust3.scale *= 1.75f;
				Dust dust56 = Main.dust[num349];
				dust56.velocity.X = dust56.velocity.X * 2f;
				Dust dust57 = Main.dust[num349];
				dust57.velocity.Y = dust57.velocity.Y * 2f;
				dust3 = Main.dust[num349];
				return;
			}*/
			if (projectile.ai[0] > 7f){
				float num347 = 1f;
				if (projectile.ai[0] == 8f){
					num347 = 0.2f;
				}
				else if (projectile.ai[0] == 9f){
					num347 = 0.4f;
				}
				else if (projectile.ai[0] == 10f){
					num347 = 0.6f;
				}
				else if (projectile.ai[0] == 11f){
					num347 = 0.8f;
				}
				float speed = 0.2f;
				if(color<110||color>114)speed = 0.4f;
				int num3;
				for (int num348 = 0; num348 < 1; num348 = num3 + 1){
					int num349 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, color, projectile.velocity.X * speed, projectile.velocity.Y * speed, 100, default(Color), 1f);
					Main.dust[num349].noGravity = true;
					Dust dust3 = Main.dust[num349];
					dust3.scale *= 1.75f;
					Dust dust56 = Main.dust[num349];
					dust56.velocity.X = dust56.velocity.X * 2f;
					Dust dust57 = Main.dust[num349];
					dust57.velocity.Y = dust57.velocity.Y * 2f;
					dust3 = Main.dust[num349];
					dust3.scale *= num347;
					num3 = num348;
				}
			}
			projectile.ai[0] += 1f;
			projectile.rotation += 0.3f * (float)projectile.direction;
			return;
		}
	}
	public class Sol_1Blue : ModProjectile{
        public override string Texture => "Terraria/Images/Projectile_145";
		public override void SetDefaults(){
			Projectile.CloneDefaults(ProjectileID.PureSpray);
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 133;
			//projectile.extraUpdates = 4;
			Projectile.aiStyle = 0;
			Projectile.tileCollide = true;
		}
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Blue Solution");
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox){
			float point = 0f;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center-Projectile.velocity*2, Projectile.Center, 22, ref point)){
				return true;
			}
            return null;
        }
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
			if (Projectile.ai[1] > 1) {
				return;
			}
			Projectile.ai[1]++;
			float rot = 0.15f;
			int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedByRandom(rot), Projectile.type, Projectile.damage / 3, Projectile.knockBack, Projectile.owner, ai1: 2);
			Main.projectile[proj].timeLeft = Projectile.timeLeft;
			Main.projectile[proj].position+=Main.projectile[proj].velocity*3;
			Projectile.velocity = Projectile.velocity.RotatedByRandom(-rot);
			Projectile.position+=Projectile.velocity*3;
		}
		public override bool OnTileCollide(Vector2 oldVelocity){
			Projectile.velocity-=(oldVelocity - Projectile.velocity);
			Projectile.timeLeft /= 4;
			Projectile.ai[1]++;
			return false;
		}
		public override void AI(){
			Sol_0Green.SprAI(Projectile, 21);
			Sol_0Green.SprAI(Projectile, 111);
		}
	}
	public class Sol_2Purple : ModProjectile{
		protected override bool CloneNewInstances => true;
        public override string Texture => "Terraria/Images/Projectile_145";
		int hits = 3;
		public override void SetDefaults(){
			Projectile.CloneDefaults(ProjectileID.PureSpray);
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 133;
			//projectile.extraUpdates = 4;
			Projectile.aiStyle = 0;
			Projectile.tileCollide = true;
		}
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Purple Solution");
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox){
			float point = 0f;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center-Projectile.velocity/2, Projectile.Center, 22, ref point)){
				return true;
			}
            return null;
        }
		public override void AI(){
			Sol_0Green.SprAI(Projectile, 75);
			Sol_0Green.SprAI(Projectile, 112);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
			if(hits>=0)for(int i = hits--; i > 0; i--)Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedByRandom(Math.PI), ProjectileID.TinyEater, damage/7, 0, Projectile.owner);
			if(Main.rand.NextBool(3))target.AddBuff(BuffID.CursedInferno, 300);
		}
	}
	public class Sol_3DarkBlue : ModProjectile{
        public override string Texture => "Terraria/Images/Projectile_145";
		public override void SetDefaults(){
			Projectile.CloneDefaults(ProjectileID.PureSpray);
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 133;
			//projectile.extraUpdates = 4;
			Projectile.aiStyle = 0;
			Projectile.tileCollide = true;
		}
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Dark Blue Solution");
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox){
			float point = 0f;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center-Projectile.velocity/2, Projectile.Center, 22, ref point)){
				return true;
			}
            return null;
        }
		public override void AI(){
			Sol_0Green.SprAI(Projectile, 113);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
			target.AddBuff(BuffID.Poisoned, 300);
			target.AddBuff(BuffID.Venom, 300);
			target.confused = true;
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
			damage = (int)(damage*1.5f);
			knockback*=1.5f;
		}
	}
	public class Sol_4Red : ModProjectile{
		protected override bool CloneNewInstances => true;
        public override string Texture => "Terraria/Images/Projectile_145";
		int hits = 3;
		public override void SetDefaults(){
			Projectile.CloneDefaults(ProjectileID.PureSpray);
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 133;
			//projectile.extraUpdates = 4;
			Projectile.aiStyle = 0;
			Projectile.tileCollide = true;
		}
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Red Solution");
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox){
			float point = 0f;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center-Projectile.velocity/2, Projectile.Center, 22, ref point)){
				return true;
			}
            return null;
        }
		public override void AI(){
			Sol_0Green.SprAI(Projectile, 170);
			Sol_0Green.SprAI(Projectile, 114);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
			if(hits>=0)for(int i = hits--; i > 0; i--)Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedByRandom(Math.PI), ProjectileID.VampireHeal, 0, 0, Projectile.owner, Projectile.owner, 1);
			if(Main.rand.NextBool(3))target.AddBuff(BuffID.Ichor, 300);
		}
	}
}
