using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Items {
	//update 2, day 4
	public class Sandblaster : ModItem {
		public override bool CloneNewInstances => true;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Sandblaster");
			Tooltip.SetDefault("'This is a great idea!'");
		}
		public override void SetDefaults(){
			item.CloneDefaults(ItemID.ChainGun);
			LegacySoundStyle us = item.UseSound;
			item.CloneDefaults(ItemID.Sandgun);
			item.UseSound = us;
			item.damage = 27;
			item.ranged = true;
			item.noMelee = true;
			item.width = 44;
			item.height = 24;
			item.useAnimation = item.useTime;
			item.useTime = 7;
			item.useAnimation = 7;
			//item.useStyle = 5;
			item.knockBack*=2;
			item.value*=2;
			item.rare = ItemRarityID.Pink;
			item.shoot = 42;// ModContent.ProjectileType<Sandblast_0Normal>();
			item.useAmmo = AmmoID.Sand;
			//item.shoot = ProjectileID.DD2FlameBurstTowerT1Shot;
			//item.shootSpeed = 12.5f;
		}
		/*public override void PickAmmo(Item weapon, Player player, ref int shoot, ref float speed, ref int damage, ref float knockback){
			int dmg = 5;
			bool canShoot = true;
			player.PickAmmo(weapon, ref shoot, ref speed, ref canShoot, ref dmg, ref knockback, true);
			switch(shoot){
				case ProjectileID.SandBallGun:
				break;
				case ProjectileID.PearlSandBallGun:
				speed*=1.1f;
				break;
				case ProjectileID.EbonsandBallGun:
				knockback++;
				break;
				case ProjectileID.CrimsandBallGun:
				dmg+=3;
				break;
				default:
				break;
			}
			damage+=dmg*2;
		}*/
        public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(mod, "ArtificerBonus", "Ranged:Sandgun");
            line.overrideColor = new Color(179, 50, 0);
            tooltips.Insert(1, line);
        }
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			switch(type){
				case ProjectileID.SandBallGun:
				type = ModContent.ProjectileType<Sandblast_0Normal>();
				break;
				case ProjectileID.PearlSandBallGun:
				type = ModContent.ProjectileType<Sandblast_1Pearl>();
				break;
				case ProjectileID.EbonsandBallGun:
				type = ModContent.ProjectileType<Sandblast_2Ebon>();
				break;
				case ProjectileID.CrimsandBallGun:
				type = ModContent.ProjectileType<Sandblast_3Crim>();
				break;
				default:
				break;
			}
			return true;
		}
		public override Vector2? HoldoutOffset(){
			return new Vector2(-6, 0);
		}
		public override void AddRecipes(){
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Sandgun, 1);
			recipe.AddIngredient(ItemID.HallowedBar, 18);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class Sandblast_0Normal : ModProjectile{
        public override string Texture => "Terraria/Projectile_145";
		public virtual int color => 32;
		public override void SetDefaults(){
			projectile.CloneDefaults(ProjectileID.FlamethrowerTrap);
			projectile.trap = false;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
			projectile.penetrate = -1;
			projectile.timeLeft = 66;
			projectile.extraUpdates = 1;
			projectile.aiStyle = 0;
			projectile.tileCollide = true;
			projectile.Size/=2;
		}
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Sand Blast");
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox){
			float point = 0f;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center-projectile.velocity*2, projectile.Center, 22, ref point)){
				return true;
			}
            return null;
        }
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
			if(target.HasBuff(BuffID.Ichor)||target.HasBuff(BuffID.BetsysCurse)){
				int a = (target.HasBuff(BuffID.Ichor)?20:0)+(target.HasBuff(BuffID.BetsysCurse)?40:0);
				damage+=a/10;
				if(target.defense<a){
					a-=target.defense;
					damage+=a/3;
				}
			}
		}
		public override void AI(){
			projectile.velocity = projectile.velocity.RotatedByRandom(0.02);
			float num347 = 1f;
			if (projectile.ai[0] == 0f){
				num347 = 0.2f;
			}
			else if (projectile.ai[0] == 1f){
				num347 = 0.4f;
			}
			else if (projectile.ai[0] == 2f){
				num347 = 0.6f;
			}
			else if (projectile.ai[0] == 3f){
				num347 = 0.8f;
			}
			float speed = 0.2f;
			//if(color<110||color>114)speed = 0.4f;
			for (int i = 0; i < 3; i++){
				int num349 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, color, projectile.velocity.X * speed, projectile.velocity.Y * speed, 100, default(Color), 0.5f);
				Main.dust[num349].noGravity = true;
				Dust dust3 = Main.dust[num349];
				dust3.scale *= 1.75f;
				Dust dust56 = Main.dust[num349];
				dust56.velocity.X = dust56.velocity.X * 2f;
				Dust dust57 = Main.dust[num349];
				dust57.velocity.Y = dust57.velocity.Y * 2f;
				dust3 = Main.dust[num349];
				dust3.scale *= num347;
			}
			projectile.ai[0] += 1f;
			projectile.rotation += 0.3f * (float)projectile.direction;
			return;
		}
	}
	public class Sandblast_1Pearl : Sandblast_0Normal {
		public override int color => 51;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Pearlsand Blast");
		}
		public override void AI(){
			base.AI();
			projectile.tileCollide = projectile.ai[1]<=12;
			if(projectile.ai[1]>=0)projectile.ai[1]--;
		}
		public override bool OnTileCollide(Vector2 oldVelocity){
			if(projectile.timeLeft < 24)return true;
			Vector2 dir = -(oldVelocity - projectile.velocity).SafeNormalize(Vector2.Zero);
			int a = Main.rand.Next(3,5);
			for(int i = 0; i < a; i++){
				Projectile p = Projectile.NewProjectileDirect(projectile.position, dir.RotatedBy((3/a)*(i-a/2f))*8, projectile.type, projectile.damage-10, projectile.knockBack, projectile.owner, 0, 18);
				p.timeLeft = projectile.timeLeft-(12+(int)projectile.ai[1]);
				p.localNPCImmunity = projectile.localNPCImmunity;
				p.position+=p.velocity;
			}
			return true;
		}
	}
	public class Sandblast_2Ebon : Sandblast_0Normal {
		public override int color => 14;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Ebonsand Blast");
		}
		public override void AI(){
			base.AI();
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 75, projectile.velocity.X/5, projectile.velocity.Y/5, 100, default(Color), 1f);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
			if(target.HasBuff(BuffID.CursedInferno))target.AddBuff(BuffID.ShadowFlame, 120);
			target.AddBuff(BuffID.CursedInferno, 360);
		}
	}
	public class Sandblast_3Crim : Sandblast_0Normal {
		public override int color => 36;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Crimsand Blast");
		}
		public override void AI(){
			base.AI();
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 170, projectile.velocity.X/5, projectile.velocity.Y/5, 100, default(Color), 0.5f);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
			if(target.HasBuff(BuffID.Ichor))target.AddBuff(BuffID.BetsysCurse, 90);
			target.AddBuff(BuffID.Ichor, 180);
		}
	}
	public class InfinitePearlsand : ModItem {
        public override string Texture => "Terraria/Item_"+ItemID.PearlsandBlock;
        public override bool Autoload(ref string name){
            return ModLoader.GetMod("Infinity")!=null;
        }
		public override void SetDefaults(){
			item.CloneDefaults(ItemID.PearlsandBlock);
			item.useStyle = 0;
			item.damage = 5;
			item.shoot = ProjectileID.PearlSandBallGun;
			item.maxStack = 1;
			item.consumable = false;
		}
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Unlimited "+Lang.GetItemNameValue(ItemID.PearlsandBlock).Replace(" Block",""));
		}
		public override void AddRecipes(){
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.PearlsandBlock, 3996);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
		}
	}
	public class InfiniteEbonsand : ModItem {
        public override string Texture => "Terraria/Item_"+ItemID.EbonsandBlock;
        public override bool Autoload(ref string name){
            return ModLoader.GetMod("Infinity")!=null;
        }
		public override void SetDefaults(){
			item.CloneDefaults(ItemID.EbonsandBlock);
			item.useStyle = 0;
			item.damage = 5;
			item.shoot = ProjectileID.EbonsandBallGun;
			item.maxStack = 1;
			item.consumable = false;
		}
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Unlimited "+Lang.GetItemNameValue(ItemID.EbonsandBlock).Replace(" Block",""));
		}
		public override void AddRecipes(){
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.EbonsandBlock, 3996);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
		}
	}
	public class InfiniteCrimsand : ModItem {
        public override string Texture => "Terraria/Item_"+ItemID.CrimsandBlock;
        public override bool Autoload(ref string name){
            return ModLoader.GetMod("Infinity")!=null;
        }
		public override void SetDefaults(){
			item.CloneDefaults(ItemID.CrimsandBlock);
			item.useStyle = 0;
			item.damage = 5;
			item.shoot = ProjectileID.CrimsandBallGun;
			item.maxStack = 1;
			item.consumable = false;
		}
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Unlimited "+Lang.GetItemNameValue(ItemID.CrimsandBlock).Replace(" Block",""));
		}
		public override void AddRecipes(){
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CrimsandBlock, 3996);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
		}
	}
}
