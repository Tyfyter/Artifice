using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Items {
	//day 4
	public class Gyrojet : ModItem {
		protected override bool CloneNewInstances => true;
		public int Reload = 1;
		public bool held = false;
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Gyrojet Carbine");
			Tooltip.SetDefault("");
			SacrificeTotal = 1;
			if (AmmoID.Sets.SpecificLauncherAmmoProjectileMatches.TryGetValue(ItemID.RocketLauncher, out var matches)) {
				Dictionary<int, int> dict = new Dictionary<int, int>(matches);

				foreach (var match in matches) {
					dict[match.Key] = match.Value switch {
						ProjectileID.ClusterRocketI => ModContent.ProjectileType<Gyrojet_Cluster_1>(),
						ProjectileID.ClusterRocketII => ModContent.ProjectileType<Gyrojet_Cluster_2>(),
						ProjectileID.DryRocket => ModContent.ProjectileType<Dryrojet_P>(),
						ProjectileID.WetRocket => ModContent.ProjectileType<Gyrowet_P>(),
						ProjectileID.LavaRocket => ModContent.ProjectileType<Gyrojet_Lava_P>(),
						ProjectileID.HoneyRocket => ModContent.ProjectileType<Gyrojet_Honey_P>(),
						ProjectileID.MiniNukeRocketI => ModContent.ProjectileType<Gyronuke_P1>(),
						ProjectileID.MiniNukeRocketII => ModContent.ProjectileType<Gyronuke_P2>(),
						ProjectileID.RocketI or
						ProjectileID.RocketII or
						ProjectileID.RocketIII or
						ProjectileID.RocketIV => ((match.Value - ProjectileID.RocketI) / 3) + ModContent.ProjectileType<Gyrojet_P1>(),
						_ => match.Value
					};
				}
				AmmoID.Sets.SpecificLauncherAmmoProjectileMatches[Type] = dict;
			}
		}
		public override void SetDefaults() {
			Item.damage = 50;
			Item.TryMakeExplosive(DamageClass.Ranged);
			Item.noMelee = true;
			Item.width = 60;
			Item.height = 18;
			Item.useTime = 1;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 6;
			Item.value = 50000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = null;
			Item.shoot = ProjectileID.RocketI;
			Item.useAmmo = AmmoID.Rocket;
			//item.shoot = ProjectileID.DD2FlameBurstTowerT1Shot;
			Item.shootSpeed = 12.5f;
			Item.autoReuse = true;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            TooltipLine line = new TooltipLine(Mod, "ArtificerBonus", "Ranged:Gun/Launcher");
            line.OverrideColor = new Color(179, 50, 0);
            tooltips.Insert(1, line);
        }
		public override void ModifyWeaponDamage(Player player, ref StatModifier damage) {
			damage = damage.CombineWith(player.bulletDamage);
		}
		public override Vector2? HoldoutOffset() {
			return new Vector2(-12, 0);//Reload>10&&Reload<20?new Vector2(-24, 0):
		}
		public override void HoldItem(Player player) {
			held = true;
            if(Reload>0) {
				player.itemRotation = Reload>5&&Reload<20?player.direction/2f:0;
				if(++Reload>25) {
					Reload = 0;
					Item.holdStyle = 0;
					SoundEngine.PlaySound(SoundID.Item149, player.itemLocation);//22
				}
			}
            if(player.itemAnimation!=0&&player.itemAnimation!=player.itemAnimationMax-1) {
				if(Main.myPlayer==player.whoAmI&&PlayerInput.Triggers.JustPressed.MouseRight) {
					player.itemAnimation = 0;
					Reload=1;
					Item.holdStyle = ItemHoldStyleID.HoldFront;
					SoundEngine.PlaySound(SoundID.Camera, player.itemLocation);
				}
            }
		}
        public override void HoldStyle(Player player, Rectangle heldItemFrame) {
            if(Reload == -1) {
				Reload = 1;
				Item.holdStyle = ItemHoldStyleID.HoldFront;
				SoundEngine.PlaySound(SoundID.Camera, player.itemLocation);
            }
        }
        public override bool AltFunctionUse(Player player) => Reload == -1;
		public override bool CanUseItem(Player player) {
			if(player.altFunctionUse==2) {
				Reload=1;
				Item.holdStyle = ItemHoldStyleID.HoldFront;
				SoundEngine.PlaySound(SoundID.Camera, player.itemLocation);//22
				return false;
			}
			return Reload==0?base.CanUseItem(player):false;
		}
        public override bool CanConsumeAmmo(Item ammo, Player player) {
            return player.itemAnimation == player.itemAnimationMax-1;
        }
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		
			//Main.PlaySound(useSound, position);
			if(player.itemAnimation!=player.itemAnimationMax-1) {
				return false;
			}
			Reload =-1;
			Projectile.NewProjectile(source, position + velocity, velocity, type, damage, knockback, player.whoAmI);
			SoundEngine.PlaySound(SoundID.Item98.WithPitch(1), position);
			SoundEngine.PlaySound(SoundID.Item11.WithPitch(1), position);
			//Main.PlaySound(2, position, 13);
			return false;
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
            if(held) {
				Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.CombatText[1].Value, Reload==0?"1/1":"0/1", Main.screenWidth*0.90f, Main.screenHeight*0.85f, Color.White, Color.Black, new Vector2(0.3f), 1);
				held = false;
            }
        }
	}

	public class Gyrojet_P1 : ModProjectile {
		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.RocketI);
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 6;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 1200;
			Projectile.extraUpdates = 4;
			Projectile.aiStyle = 0;
		}
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Gyrojet Carbine");
		}
		public override void AI() {
			Projectile.rotation = Projectile.velocity.ToRotation()+(float)(Math.PI/2);
            int d = Dust.NewDust(Projectile.Center - Projectile.velocity * 0.5f-new Vector2(0,4), 0, 0, DustID.Torch, 0f, 0f, 100, Scale:0.75f);
			Dust dust = Main.dust[d];
			dust.scale *= 1f + Main.rand.Next(10) * 0.1f;
			dust.velocity *= 0.2f;
		}
	}
	public class Gyrojet_P2 : Gyrojet_P1 {
		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.RocketII);
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 3;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 1200;
			Projectile.extraUpdates = 4;
			Projectile.aiStyle = 0;
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
            ArtificeGlobalNPC globaltarget = target.GetGlobalNPC<ArtificeGlobalNPC>();
            int defense = Math.Max(target.defense-globaltarget.defreduc, 0);
			damage+=(int)(defense*0.2f);
			globaltarget.defreduc+=2;
		}
	}
	public class Gyrojet_P3 : Gyrojet_P1 {
		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.RocketIII);
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 6;
			Projectile.penetrate = 4;
			Projectile.timeLeft = 1200;
			Projectile.extraUpdates = 5;
			Projectile.aiStyle = 0;
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
            ArtificeGlobalNPC globaltarget = target.GetGlobalNPC<ArtificeGlobalNPC>();
            int defense = Math.Max(target.defense-globaltarget.defreduc, 0);
			damage+=(int)(defense*0.1f);
		}
	}
	public class Gyrojet_P4 : Gyrojet_P1 {
		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.RocketIV);
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 3;
			Projectile.penetrate = 4;
			Projectile.timeLeft = 1200;
			Projectile.extraUpdates = 5;
			Projectile.aiStyle = 0;
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
            ArtificeGlobalNPC globaltarget = target.GetGlobalNPC<ArtificeGlobalNPC>();
            int defense = Math.Max(target.defense-globaltarget.defreduc, 0);
			damage+=(int)(defense*0.4f);
			globaltarget.defreduc+=10;
		}
	}
	public class Gyrojet_Cluster_1 : Gyrojet_P1 {
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.ClusterRocketI;
		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.ClusterRocketI);
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 6;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 1200;
			Projectile.extraUpdates = 3;
			Projectile.aiStyle = 0;
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
			ArtificeGlobalNPC globaltarget = target.GetGlobalNPC<ArtificeGlobalNPC>();
			int defense = Math.Max(target.defense - globaltarget.defreduc, 0);
			damage += (int)(defense * 0.1f);
		}
		public override void Kill(int timeLeft) {
			for (int i = 0; i < 3; i++) {
				Projectile.NewProjectileDirect(
					Projectile.GetSource_FromThis(),
					Projectile.Center,
					Projectile.velocity + Main.rand.NextVector2Circular(2, 2),
					ProjectileID.ClusterFragmentsI,
					Projectile.damage,
					Projectile.knockBack,
					Projectile.owner
				).localNPCImmunity = Projectile.localNPCImmunity.ToArray();
			}
		}
	}
	public class Gyrojet_Cluster_2 : Gyrojet_P1 {
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.ClusterRocketII;
		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.ClusterRocketII);
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 3;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 1200;
			Projectile.extraUpdates = 4;
			Projectile.aiStyle = 0;
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
			ArtificeGlobalNPC globaltarget = target.GetGlobalNPC<ArtificeGlobalNPC>();
			int defense = Math.Max(target.defense - globaltarget.defreduc, 0);
			damage += (int)(defense * 0.2f);
		}
		public override void Kill(int timeLeft) {
			for (int i = 0; i < 3; i++) {
				Projectile.NewProjectileDirect(
					Projectile.GetSource_FromThis(),
					Projectile.Center,
					Projectile.velocity + Main.rand.NextVector2Circular(2, 2),
					ProjectileID.ClusterFragmentsI,
					Projectile.damage,
					Projectile.knockBack,
					Projectile.owner
				).localNPCImmunity = Projectile.localNPCImmunity.ToArray();
			}
		}
	}
	public class Dryrojet_P : Gyrojet_P1 {
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.DryRocket;
		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.DryRocket);
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 6;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 1200;
			Projectile.extraUpdates = 4;
			Projectile.aiStyle = 0;
		}
		public override void AI() {
			Projectile.rotation = Projectile.velocity.ToRotation() + (float)(Math.PI / 2);
			for (int i = 0; i < 3; i++) {
				Dust dust = Dust.NewDustPerfect(Projectile.Center - (Projectile.velocity * (i / 3f)), 229, default, 200, Scale: 0.5f);
				dust.noGravity = true;
				dust.velocity = default;
			}
			Projectile.Kill_DirtAndFluidProjectiles_RunDelegateMethodPushUpForHalfBricks(Projectile.Center.ToTileCoordinates(), 1.5f, DelegateMethods.SpreadDry);
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
			ArtificeGlobalNPC globaltarget = target.GetGlobalNPC<ArtificeGlobalNPC>();
			int defense = Math.Max(target.defense - globaltarget.defreduc, 0);
			damage += (int)(defense * 0.4f);
			globaltarget.defreduc += 10;
		}
	}
	public class Gyrowet_P : Gyrojet_P1 {
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.WetRocket;
		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.WetRocket);
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 6;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 1200;
			Projectile.extraUpdates = 4;
			Projectile.aiStyle = 0;
		}
		public override void AI() {
			Projectile.rotation = Projectile.velocity.ToRotation() + (float)(Math.PI / 2);
			for (int i = 0; i < 3; i++) {
				Dust dust = Dust.NewDustPerfect(Projectile.Center - (Projectile.velocity * (i / 3f)), 229, default, 200, Scale: 0.5f);
				dust.noGravity = true;
				dust.velocity = default;
			}
			Projectile.Kill_DirtAndFluidProjectiles_RunDelegateMethodPushUpForHalfBricks(Projectile.Center.ToTileCoordinates(), 0.005f, DelegateMethods.SpreadWater);
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
			ArtificeGlobalNPC globaltarget = target.GetGlobalNPC<ArtificeGlobalNPC>();
			int defense = Math.Max(target.defense - globaltarget.defreduc, 0);
			damage += (int)(defense * 0.2f);
			globaltarget.defreduc += 2;
		}
	}
	public class Gyrojet_Lava_P : Gyrojet_P1 {
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.LavaRocket;
		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.LavaRocket);
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 6;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 1200;
			Projectile.extraUpdates = 4;
			Projectile.aiStyle = 0;
		}
		public override void AI() {
			Projectile.rotation = Projectile.velocity.ToRotation() + (float)(Math.PI / 2);
			for (int i = 0; i < 3; i++) {
				Dust dust = Dust.NewDustPerfect(Projectile.Center - (Projectile.velocity * (i / 3f)), 229, default, 200, Scale: 0.5f);
				dust.noGravity = true;
				dust.velocity = default;
			}
			Projectile.Kill_DirtAndFluidProjectiles_RunDelegateMethodPushUpForHalfBricks(Projectile.Center.ToTileCoordinates(), 0.005f, DelegateMethods.SpreadLava);
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
			ArtificeGlobalNPC globaltarget = target.GetGlobalNPC<ArtificeGlobalNPC>();
			int defense = Math.Max(target.defense - globaltarget.defreduc, 0);
			damage += (int)(defense * 0.1f);
		}
	}
	public class Gyrojet_Honey_P : Gyrojet_P1 {
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.HoneyRocket;
		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.HoneyRocket);
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 6;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 1200;
			Projectile.extraUpdates = 4;
			Projectile.aiStyle = 0;
		}
		public override void AI() {
			Projectile.rotation = Projectile.velocity.ToRotation() + (float)(Math.PI / 2);
			for (int i = 0; i < 3; i++) {
				Dust dust = Dust.NewDustPerfect(Projectile.Center - (Projectile.velocity * (i / 3f)), 229, default, 200, Scale: 0.5f);
				dust.noGravity = true;
				dust.velocity = default;
			}
			Projectile.Kill_DirtAndFluidProjectiles_RunDelegateMethodPushUpForHalfBricks(Projectile.Center.ToTileCoordinates(), 0.005f, DelegateMethods.SpreadHoney);
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
			ArtificeGlobalNPC globaltarget = target.GetGlobalNPC<ArtificeGlobalNPC>();
			int defense = Math.Max(target.defense - globaltarget.defreduc, 0);
			damage += (int)(defense * 0.4f);
			globaltarget.defreduc += 10;
		}
	}
	public class Gyronuke_P1 : Gyrojet_P1 {
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.MiniNukeRocketI;
		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.MiniNukeRocketI);
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 6;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 1200;
			Projectile.extraUpdates = 3;
			Projectile.aiStyle = 0;
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
			ArtificeGlobalNPC globaltarget = target.GetGlobalNPC<ArtificeGlobalNPC>();
			int defense = Math.Max(target.defense - globaltarget.defreduc, 0);
			damage += (int)(defense * 0.4f);
			globaltarget.defreduc += 10;
		}
		public override bool PreKill(int timeLeft) {
			Projectile.type = ProjectileID.MiniNukeRocketI;
			return true;
		}
	}
	public class Gyronuke_P2 : Gyrojet_P1 {
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.MiniNukeRocketII;
		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.MiniNukeRocketII);
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 6;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 1200;
			Projectile.extraUpdates = 4;
			Projectile.aiStyle = 0;
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
			ArtificeGlobalNPC globaltarget = target.GetGlobalNPC<ArtificeGlobalNPC>();
			int defense = Math.Max(target.defense - globaltarget.defreduc, 0);
			damage += (int)(defense * 0.4f);
			globaltarget.defreduc += 10;
		}
		public override bool PreKill(int timeLeft) {
			Projectile.type = ProjectileID.MiniNukeRocketII;
			return true;
		}
	}
}
