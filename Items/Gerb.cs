using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Items {
	//day 1
	public class Gerb : ModItem {
		protected override bool CloneNewInstances => true;
		public float Ammo = 1f;
		public bool held = false;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Gerb");
			Tooltip.SetDefault("Gerb");
			SacrificeTotal = 1;
		}
		public override void SetDefaults(){
			Item.damage = 15;
			Item.TryMakeExplosive(DamageClass.Magic);
			Item.noMelee = true;
			Item.width = 34;
			Item.height = 16;
			Item.useTime = 5;
			Item.useAnimation = 5;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item34;
			Item.shoot = ProjectileID.FlamesTrap;
			//item.shoot = ProjectileID.DD2FlameBurstTowerT1Shot;
			Item.shootSpeed = 12.5f;
			Item.autoReuse = true;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(Mod, "ArtificerBonus", "Magic");
            line.OverrideColor = new Color(179, 50, 0);
            tooltips.Insert(1, line);
        }
		public override Vector2? HoldoutOffset(){
			return new Vector2(-2, -2);
		}
		public override void AddRecipes() {
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Fireblossom, 1);
			recipe.AddIngredient(ItemID.ExplosivePowder, 5);
			recipe.AddIngredient(ItemID.FireworkFountain, 3);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
		public override void HoldItem(Player player){
			held = true;
			Ammo = MathHelper.Clamp(Ammo, 0, 1);
		}
		public override bool AltFunctionUse(Player player) => Ammo<1;
		public override bool CanUseItem(Player player){
			Item.noUseGraphic = player.altFunctionUse==2;
			Item.UseSound = player.altFunctionUse==2?null:SoundID.Item34;
			return base.CanUseItem(player);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(player.altFunctionUse==2){
				if(player.controlUseItem){
					Ammo+=0.025f;
					player.itemAnimation = Item.useAnimation-1;
				}else{
					player.itemAnimation = 0;
				}
			}
			if(player.altFunctionUse==2||Ammo<=0)return false;
			if(player.itemAnimation>2){
				if(player.controlUseItem){
					player.itemAnimation = Item.useAnimation-1;
				}else{
					player.itemAnimation = 0;
				}
				//Main.PlaySound(useSound, position);
				Ammo-=0.02f;
				for(int i = Ammo<=0?5:1; i > 0; i--){
					Projectile proj = Projectile.NewProjectileDirect(source, position + velocity, velocity.RotatedByRandom(Ammo<=0?0.4:0.2), type, damage, knockback, player.whoAmI);
					proj.DamageType = Item.DamageType;
					proj.friendly = true;
					proj.hostile = false;
					proj.usesLocalNPCImmunity = true;
					proj.localNPCHitCooldown = Ammo<=0?0:9;
					proj.timeLeft = Ammo<=0?35:20;
					proj.npcProj = false;
					proj.trap = false;
					if(Ammo<=0)SoundEngine.PlaySound(SoundID.Item14, position);
				}
				return false;
			}
			return false;
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale){
            if(held){
				Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.CombatText[1].Value, (int)(Ammo*50)+"/50", Main.screenWidth*0.90f, Main.screenHeight*0.85f, Color.White, Color.Black, new Vector2(0.3f), 1);
				held = false;
            }
        }
	}
	public class Tubri : ModItem {
		protected override bool CloneNewInstances => true;
		public float Ammo = 1f;
		public int count = 0;
		public bool held = false;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Tubri");
			Tooltip.SetDefault("Not Gerb");
		}
		public override void SetDefaults(){
			Item.damage = 35;
			Item.TryMakeExplosive(DamageClass.Magic);
			Item.noMelee = true;
			Item.width = 40;
			Item.height = 16;
			Item.useTime = 5;
			Item.useAnimation = 5;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item34;
			Item.shoot = ModContent.ProjectileType<TubriShot>();
			//item.shoot = ProjectileID.DD2FlameBurstTowerT1Shot;
			Item.shootSpeed = 12.5f;
			Item.autoReuse = true;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(Mod, "ArtificerBonus", "Magic");
            line.OverrideColor = new Color(179, 50, 0);
            tooltips.Insert(1, line);
        }
		public override void AddRecipes(){
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SoulofLight, 10);
			recipe.AddIngredient(ItemID.FireworkFountain, 3);
			recipe.AddTile(TileID.LihzahrdAltar);
			recipe.Register();
		}
		public override Vector2? HoldoutOffset(){
			return new Vector2(-2, -2);
		}
		public override void HoldStyle(Player player, Rectangle heldItemFrame){
			count = 0;
		}
		public override void HoldItem(Player player){
			held = true;
			Ammo = MathHelper.Clamp(Ammo, 0, 1);
		}
		public override bool AltFunctionUse(Player player) => Ammo<1;
		public override bool CanUseItem(Player player){
			Item.noUseGraphic = player.altFunctionUse==2;
			Item.UseSound = player.altFunctionUse==2?null:SoundID.Item34;
			return base.CanUseItem(player);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(player.altFunctionUse==2){
				if(player.controlUseItem){
					Ammo+=0.06f;
					player.itemAnimation = Item.useAnimation-1;
				}else{
					player.itemAnimation = 0;
				}
			}
			if(player.altFunctionUse==2||Ammo<=0)return false;
			if(player.itemAnimation>2){
				if(player.controlUseItem){
					player.itemAnimation = Item.useAnimation-1;
				}else{
					player.itemAnimation = 0;
				}
				//Main.PlaySound(useSound, position);
				float speed = velocity.Length();
				Ammo-=0.01f;
				count++;
				Vector2 dist = Main.MouseWorld - position;
				velocity.Y-= (Math.Abs(dist.X/512)-dist.Y/2048);
				player.itemRotation = (velocity*player.direction).ToRotation();
                Projectile.NewProjectile(source, position + velocity, (velocity.SafeNormalize(Vector2.Zero)*speed).RotatedByRandom(0.1), type, (int)(damage*(1+(count/100f))), knockback, player.whoAmI);
				return false;
			}
			return false;
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale){
            if(held){
				Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.CombatText[1].Value, (int)(Ammo*100)+"/100", Main.screenWidth*0.90f, Main.screenHeight*0.85f, Color.White, Color.Black, new Vector2(0.3f), 1);
				held = false;
            }
        }
	}
	public class TubriShot : ModProjectile{
        public override string Texture => "Terraria/Images/Projectile_188";
		public override void SetDefaults(){
			Projectile.CloneDefaults(188);
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 7;
			Projectile.timeLeft = 120;
			Projectile.npcProj = false;
			Projectile.trap = false;
		}
		public override void AI(){
			float num297 = 1f;
			if (Projectile.ai[0] == 0f){
				num297 = 0.25f;
			}
			else if (Projectile.ai[0] == 1f){
				num297 = 0.5f;
			}
			else if (Projectile.ai[0] == 2f){
				num297 = 0.75f;
			}
			else if (Projectile.ai[0] == 10f){
				Projectile.aiStyle = 1;
			}
			Projectile.ai[0] += 1f;
			if (Main.rand.NextBool(1)){
				int num3;
				for (int num299 = 0; num299 < 1; num299 = num3 + 1){
					int num300 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default(Color), 1f);
					Dust dust3;
					if (!Main.rand.NextBool(3)){
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
			Projectile.rotation += 0.3f * (float)Projectile.direction;
		}
	}
}
