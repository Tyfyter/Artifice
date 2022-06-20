using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Items {
	//day 8
	//this took ~1.5 hours
	public class Roman_Candle : ModItem {
		protected override bool CloneNewInstances => true;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Roman Candle");
			Tooltip.SetDefault("");
			SacrificeTotal = 1;
		}
		public override void SetDefaults(){
			Item.damage = 17;
			Item.TryMakeExplosive(DamageClasses.Ranged_Magic);
			Item.mana = 6;
			Item.noMelee = true;
			Item.width = 34;
			Item.height = 16;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 3;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.crit = 6;
			Item.UseSound = SoundID.Item34;
			Item.shoot = ModContent.ProjectileType<Roman_Candle_P>();
			Item.useAmmo = ModContent.ItemType<Roman_Candle_Ammo>();
			//item.shoot = ProjectileID.DD2FlameBurstTowerT1Shot;
			Item.shootSpeed = 12.5f;
			Item.autoReuse = true;
		}
		public override bool NeedsAmmo(Player player) {
			return !player.ItemAnimationJustStarted && !player.CheckMana(Item, Item.mana / 2, pay:true);
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(Mod, "ArtificerBonus", "Ranged/Magic");
            line.OverrideColor = new Color(179, 50, 0);
            tooltips.Insert(1, line);
        }
		public override Vector2? HoldoutOffset(){
			return new Vector2(-2, -2);
		}
		public override void UpdateInventory(Player player){
			player.GetModPlayer<ArtificerPlayer>().hasRC = true;
		}
		public override bool AltFunctionUse(Player player) => true;
		public override float UseSpeedMultiplier(Player player) {
			return player.altFunctionUse == 2 ? 0.75f : 1f;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			int i = type-ModContent.ProjectileType<Roman_Candle_P>();
            Vector2 oofset = velocity.RotatedBy(-player.direction*Math.PI/2)/3;
            position+=oofset;
			if(player.altFunctionUse==2){
                //damage = (int)(damage*1.5);
                Projectile.NewProjectileDirect(source, position + velocity, velocity, ModContent.ProjectileType<Roman_Candle_P>(), damage, knockback * 2f, player.whoAmI, i, 1).timeLeft-=150;
			} else {
                Projectile.NewProjectile(source, position + velocity, velocity.RotatedByRandom(0.05), ModContent.ProjectileType<Roman_Candle_P>(), damage, knockback, player.whoAmI, i, 0);
			}
			return false;
		}
	}
	public class Roman_Candle_Ammo : ModItem {
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Roman Candle");
			Tooltip.SetDefault("");
		}
		public override string Texture => "Artifice/Items/Roman_Candle";
		public override void SetDefaults() {
			Item.damage = 17;
			Item.TryMakeExplosive(DamageClasses.Ranged_Magic);
			Item.width = 34;
			Item.height = 16;
			Item.knockBack = 3;
			Item.value = 10000;
			Item.shoot = ModContent.ProjectileType<Roman_Candle_P>();
			Item.ammo = Item.type;
			//item.shoot = ProjectileID.DD2FlameBurstTowerT1Shot;
			Item.shootSpeed = 12.5f;
			Item.autoReuse = true;
		}
		public override bool CanResearch() {
			return false;
		}
	}
	public class KClO4 : ModItem {
		protected override bool CloneNewInstances => true;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Cursed Firework Star");
			Tooltip.SetDefault("");
			SacrificeTotal = 99;
		}
		public override void SetDefaults(){
			Item.damage = 33;
			Item.TryMakeExplosive(DamageClasses.Ranged_Magic);
			Item.noMelee = true;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.LightRed;
			Item.shoot = ModContent.ProjectileType<Roman_Candle_P>()+1;
			Item.ammo = ModContent.ItemType<Roman_Candle_Ammo>();
			Item.shootSpeed = 12.5f;
			Item.maxStack = 999;
			Item.consumable = true;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(Mod, "ArtificerBonus", "Ranged/Magic");
            line.OverrideColor = new Color(179, 50, 0);
            tooltips.Insert(1, line);
        }
		public override void AddRecipes() {
			Recipe recipe = CreateRecipe(5);
			recipe.AddIngredient(ItemID.ExplosivePowder, 1);
			recipe.AddIngredient(ItemID.CursedFlame, 1);
			recipe.Register();
		}
	}
	public class P4 : ModItem {
		protected override bool CloneNewInstances => true;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("P4");
			Tooltip.SetDefault("");
			SacrificeTotal = 99;
		}
		public override void SetDefaults(){
			Item.damage = 83;
			Item.TryMakeExplosive(DamageClasses.Ranged_Magic);
			Item.noMelee = true;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.LightRed;
			Item.shoot = ModContent.ProjectileType<Roman_Candle_P>()+2;
			Item.ammo = ModContent.ItemType<Roman_Candle_Ammo>();
			Item.shootSpeed = 12.5f;
			Item.maxStack = 999;
			Item.consumable = true;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(Mod, "ArtificerBonus", "Ranged/Magic");
            line.OverrideColor = new Color(179, 50, 0);
            tooltips.Insert(1, line);
			if(tooltips[6].Name=="Ammo")tooltips.RemoveAt(6);
        }
	}
	public class Roman_Candle_P : ModProjectile{
        public override string Texture => "Terraria/Images/Projectile_188";
		public static readonly Color[] colors = new Color[]{Color.DarkGoldenrod,Color.LimeGreen,Color.Goldenrod};
		public static readonly int[][] dusts = new int[][]{
			new int[]{222,0},
			new int[]{75},
			new int[]{170,170,170,222}
		};
		public static readonly float[] scales = new float[]{1.5f,0.5f,2};

		public override void SetDefaults(){
			Projectile.CloneDefaults(ProjectileID.LightDisc);
			Projectile.TryMakeExplosive(DamageClasses.Ranged_Magic);
			Projectile.width = Projectile.height = 10;
			Projectile.penetrate = -1;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 180;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.extraUpdates = 2;
			//id = ModContent.ProjectileType<Catherine_Wheel_Throw>();
		}
		public override void AI(){
			//projectile.type = id;
			Projectile.rotation = Projectile.velocity.ToRotation();
			Player player = Main.player[Projectile.owner];
			int[] dust = dusts[(int)Projectile.ai[0]];
			if(Projectile.ai[1]==1){
				if(Projectile.timeLeft<20){
					if(Projectile.timeLeft<=1){
						Projectile.ai[1] = 2;
						Vector2 vec = Projectile.Center;
						Projectile.width*=12;
						Projectile.height*=12;
						Projectile.Center = vec;
						Projectile.tileCollide = false;
                		SoundEngine.PlaySound(SoundID.Item38, Projectile.Center);
						for (int j = 0; j < 25; j++){
							if (!Main.rand.NextBool(6)){
								Dust dust4 = Dust.NewDustDirect(Projectile.Center, 0, 0, dust[j%dust.Length], 0f, 0f, 100, colors[(int)Projectile.ai[0]], 1f);//226
								dust4.velocity = new Vector2(4,0).RotatedByRandom(Math.PI) * (1f + Main.rand.NextFloat());
								dust4.noGravity = false;
								dust4.scale = scales[(int)Projectile.ai[1]];
							}
							if (Projectile.ai[0]==2&&!Main.rand.NextBool(6)){
								Dust dust4 = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Smoke, 0f, 0f, 100, default(Color), 1f);//226
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
				if (!Main.rand.NextBool(6)){
					Dust dust4 = Dust.NewDustDirect(Projectile.Center, 0, 0, dust[j%dust.Length], 0f, 0f, 100, colors[(int)Projectile.ai[0]], 1f);//226
					dust4.velocity = new Vector2(8,0).RotatedBy(Projectile.rotation) * (1f + Main.rand.NextFloat());
					dust4.noGravity = !Main.rand.NextBool(5);
					dust4.scale = scales[(int)Projectile.ai[1]];
				}
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity){
			if(Projectile.ai[1]==1){
				Projectile.timeLeft = 2;
				return false;
			}
			return true;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
			switch ((int)Projectile.ai[1]){
				case 1:
				Projectile.velocity*=0;
				Projectile.timeLeft = 2;
				Projectile.localNPCImmunity[target.whoAmI] = 0;
				break;
				case 2:
				if(!target.noGravity){
					Vector2 targ = new Vector2(MathHelper.Clamp(Projectile.Center.X, target.position.X, target.position.X + target.width),
					MathHelper.Clamp(Projectile.Center.Y, target.position.Y, target.position.Y + target.height));
					Vector2 vel = (targ-Projectile.Center).SafeNormalize(Vector2.Zero);
					float dist = (targ-Projectile.Center).Length();
					target.velocity+=vel*(48/Math.Max(dist/16,2))*(float)Math.Sqrt(target.knockBackResist);
				}
				break;
				default:
				break;
			}
			switch ((int)Projectile.ai[0]){
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
