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
		protected override bool CloneNewInstances => true;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Sandblaster");
			Tooltip.SetDefault("'This is a great idea!'");
		}
		public override void SetDefaults(){
			Item.CloneDefaults(ItemID.ChainGun);
			SoundStyle? us = Item.UseSound;
			Item.CloneDefaults(ItemID.Sandgun);
			Item.UseSound = us;
			Item.damage = 27;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 44;
			Item.height = 24;
			Item.useAnimation = Item.useTime;
			Item.useTime = 7;
			Item.useAnimation = 7;
			//item.useStyle = 5;
			Item.knockBack*=2;
			Item.value*=2;
			Item.rare = ItemRarityID.Pink;
			Item.shoot = ProjectileID.SandBallGun;// ModContent.ProjectileType<Sandblast_0Normal>();
			Item.useAmmo = AmmoID.Sand;
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
            TooltipLine line = new TooltipLine(Mod, "ArtificerBonus", "Ranged:Sandgun");
			//float m = Main.mouseTextColor / 255f;
            //line.overrideColor = new Color((int)(179 * m), (int)(50 * m), 0);
            line.OverrideColor = new Color(179, 50, 0);
            tooltips.Insert(1, line);
        }
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
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
		}
		public override Vector2? HoldoutOffset(){
			return new Vector2(-6, 0);
		}
		public override void AddRecipes() {
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Sandgun, 1);
			recipe.AddIngredient(ItemID.HallowedBar, 18);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
	public class Sandblast_0Normal : ModProjectile{
        public override string Texture => "Terraria/Images/Projectile_145";
		public virtual int color => 32;
		public override void SetDefaults(){
			Projectile.CloneDefaults(ProjectileID.FlamethrowerTrap);
			Projectile.trap = false;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 66;
			Projectile.extraUpdates = 1;
			Projectile.aiStyle = 0;
			Projectile.tileCollide = true;
			Projectile.Size/=2;
		}
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Sand Blast");
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox){
			float point = 0f;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center-Projectile.velocity*2, Projectile.Center, 22, ref point)){
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
			Projectile.velocity = Projectile.velocity.RotatedByRandom(0.02);
			float num347 = 1f;
			if (Projectile.ai[0] == 0f){
				num347 = 0.2f;
			}
			else if (Projectile.ai[0] == 1f){
				num347 = 0.4f;
			}
			else if (Projectile.ai[0] == 2f){
				num347 = 0.6f;
			}
			else if (Projectile.ai[0] == 3f){
				num347 = 0.8f;
			}
			float speed = 0.2f;
			//if(color<110||color>114)speed = 0.4f;
			for (int i = 0; i < 3; i++){
				int num349 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, color, Projectile.velocity.X * speed, Projectile.velocity.Y * speed, 100, default(Color), 0.5f);
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
			Projectile.ai[0] += 1f;
			Projectile.rotation += 0.3f * (float)Projectile.direction;
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
			Projectile.tileCollide = Projectile.ai[1]<=12;
			if(Projectile.ai[1]>=0)Projectile.ai[1]--;
		}
		public override bool OnTileCollide(Vector2 oldVelocity){
			if(Projectile.timeLeft < 24)return true;
			Vector2 dir = -(oldVelocity - Projectile.velocity).SafeNormalize(Vector2.Zero);
			int a = Main.rand.Next(3,5);
			for(int i = 0; i < a; i++){
				Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.position, dir.RotatedBy((3/a)*(i-a/2f))*8, Projectile.type, Projectile.damage-10, Projectile.knockBack, Projectile.owner, 0, 18);
				p.timeLeft = Projectile.timeLeft-(12+(int)Projectile.ai[1]);
				p.localNPCImmunity = Projectile.localNPCImmunity;
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
			Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.CursedTorch, Projectile.velocity.X/5, Projectile.velocity.Y/5, 100, default(Color), 1f);
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
			Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Ichor, Projectile.velocity.X/5, Projectile.velocity.Y/5, 100, default(Color), 0.5f);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
			if(target.HasBuff(BuffID.Ichor))target.AddBuff(BuffID.BetsysCurse, 90);
			target.AddBuff(BuffID.Ichor, 180);
		}
	}
	public class InfinitePearlsand : ModItem {
        public override string Texture => "Terraria/Images/Item_" + ItemID.PearlsandBlock;
		public override bool IsLoadingEnabled(Mod mod) {
            return ModLoader.HasMod("Infinity");
        }
		public override void SetDefaults(){
			Item.CloneDefaults(ItemID.PearlsandBlock);
			Item.useStyle = ItemUseStyleID.None;
			Item.damage = 5;
			Item.shoot = ProjectileID.PearlSandBallGun;
			Item.maxStack = 1;
			Item.consumable = false;
		}
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Unlimited "+Lang.GetItemNameValue(ItemID.PearlsandBlock).Replace(" Block",""));
		}
		public override void AddRecipes() {
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.PearlsandBlock, 3996);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
		}
	}
	public class InfiniteEbonsand : ModItem {
        public override string Texture => "Terraria/Images/Item_" + ItemID.EbonsandBlock;
		public override bool IsLoadingEnabled(Mod mod) {
			return ModLoader.HasMod("Infinity");
		}
		public override void SetDefaults(){
			Item.CloneDefaults(ItemID.EbonsandBlock);
			Item.useStyle = ItemUseStyleID.None;
			Item.damage = 5;
			Item.shoot = ProjectileID.EbonsandBallGun;
			Item.maxStack = 1;
			Item.consumable = false;
		}
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Unlimited "+Lang.GetItemNameValue(ItemID.EbonsandBlock).Replace(" Block",""));
		}
		public override void AddRecipes() {
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.EbonsandBlock, 3996);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
		}
	}
	public class InfiniteCrimsand : ModItem {
        public override string Texture => "Terraria/Images/Item_" + ItemID.CrimsandBlock;
		public override bool IsLoadingEnabled(Mod mod) {
			return ModLoader.HasMod("Infinity");
		}
		public override void SetDefaults(){
			Item.CloneDefaults(ItemID.CrimsandBlock);
			Item.useStyle = ItemUseStyleID.None;
			Item.damage = 5;
			Item.shoot = ProjectileID.CrimsandBallGun;
			Item.maxStack = 1;
			Item.consumable = false;
		}
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Unlimited "+Lang.GetItemNameValue(ItemID.CrimsandBlock).Replace(" Block",""));
		}
		public override void AddRecipes() {
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CrimsandBlock, 3996);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
		}
	}
}
