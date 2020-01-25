using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Items {
	//day 4
	public class Gyrojet : ModItem {
		public override bool CloneNewInstances => true;
		public int Reload = 1;
		public bool held = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gyrojet Carbine");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.damage = 75;
			item.ranged = true;
			item.noMelee = true;
			item.width = 60;
			item.height = 18;
			item.useTime = 1;
			item.useAnimation = 15;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = null;
			item.shoot = 1;
			item.useAmmo = AmmoID.Rocket;
			//item.shoot = ProjectileID.DD2FlameBurstTowerT1Shot;
			item.shootSpeed = 12.5f;
			item.autoReuse = true;
		}
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat){
			mult*=player.bulletDamage;
		}
		public override Vector2? HoldoutOffset(){
			return new Vector2(-12, 0);//Reload>10&&Reload<20?new Vector2(-24, 0):
		}
		public override void AddRecipes()
		{
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
            if(Reload>0){
				player.itemRotation = Reload>5&&Reload<20?player.direction/2f:0;
				if(++Reload>25){
					Reload = 0;
					item.holdStyle = 0;
					Main.PlaySound(40, player.itemLocation);//22
				}
			}
		}
		public override bool AltFunctionUse(Player player) => Reload==-1;
		public override bool CanUseItem(Player player){
			if(player.altFunctionUse==2){
				Reload=1;
				item.holdStyle = ItemHoldStyleID.HarpHoldingOut;
				Main.PlaySound(40, player.itemLocation);//22
				return false;
			}
			return Reload==0?base.CanUseItem(player):false;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			
			//Main.PlaySound(useSound, position);
			if(player.itemAnimation!=14){
				if(player.controlUseTile){
					player.itemAnimation = 0;
					Reload=1;
					item.holdStyle = ItemHoldStyleID.HarpHoldingOut;
					Main.PlaySound(40, position);//22
				}
				return false;
			}
			type--;
			type/=3;
			type+=ModContent.ProjectileType<Gyrojet_P1>();
			Reload=-1;
			Projectile.NewProjectile(position + new Vector2(speedX,speedY), new Vector2(speedX,speedY), type, damage, knockBack, item.owner);
			Main.PlaySound(new LegacySoundStyle(2, 98), position).Pitch = 1;
			Main.PlaySound(new LegacySoundStyle(2, 11), position).Pitch = 1;
			//Main.PlaySound(2, position, 13);
			return false;
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale){
            if(held){
                Player player = Main.player[item.owner];
				Utils.DrawBorderStringFourWay(spriteBatch, Main.fontCombatText[1], Reload==0?"1/1":"0/1", Main.screenWidth*0.90f, Main.screenHeight*0.85f, Color.White, Color.Black, new Vector2(0.3f), 1);
				held = false;
            }
        }
	}
	
	public class Gyrojet_P1 : ModProjectile{
		public override void SetDefaults(){
			projectile.CloneDefaults(ProjectileID.RocketI);
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 3;
			projectile.penetrate = 3;
			projectile.timeLeft = 1200;
			projectile.extraUpdates = 4;
			projectile.aiStyle = 0;
		}
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Gyrojet Carbine");
		}
		public override void AI(){
			projectile.rotation = projectile.velocity.ToRotation()+(float)(Math.PI/2);
		}
	}
	public class Gyrojet_P2 : ModProjectile{
		public override void SetDefaults(){
			projectile.CloneDefaults(ProjectileID.RocketII);
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 0;
			projectile.penetrate = 3;
			projectile.timeLeft = 1200;
			projectile.extraUpdates = 4;
			projectile.aiStyle = 0;
		}
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Gyrojet Carbine");
		}
		public override void AI(){
			projectile.rotation = projectile.velocity.ToRotation()+(float)(Math.PI/2);
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
			damage+=(int)(target.defense*0.2f);
			target.GetGlobalNPC<ArtificeGlobalNPC>().defreduc+=2;
		}
	}
	public class Gyrojet_P3 : ModProjectile{
		public override void SetDefaults(){
			projectile.CloneDefaults(ProjectileID.RocketIII);
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 3;
			projectile.penetrate = 5;
			projectile.timeLeft = 1200;
			projectile.extraUpdates = 5;
			projectile.aiStyle = 0;
		}
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Gyrojet Carbine");
		}
		public override void AI(){
			projectile.rotation = projectile.velocity.ToRotation()+(float)(Math.PI/2);
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
			damage+=(int)(target.defense*0.1f);
		}
	}
	public class Gyrojet_P4 : ModProjectile{
		public override void SetDefaults(){
			projectile.CloneDefaults(ProjectileID.RocketIV);
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 0;
			projectile.penetrate = 5;
			projectile.timeLeft = 1200;
			projectile.extraUpdates = 5;
			projectile.aiStyle = 0;
		}
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Gyrojet Carbine");
		}
		public override void AI(){
			projectile.rotation = projectile.velocity.ToRotation()+(float)(Math.PI/2);
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
			damage+=(int)(target.defense*0.4f);
			target.GetGlobalNPC<ArtificeGlobalNPC>().defreduc+=10;
		}
	}
}
