using System;
using System.Collections.Generic;
using Artifice.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Items {
	//day 6
	public class Whip_Sword : ModItem {
		bool extended = false;
		protected override bool CloneNewInstances => true;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Whip Sword");
			Tooltip.SetDefault("");
			SacrificeTotal = 1;
		}
		public override void SetDefaults(){
			Item.CloneDefaults(ItemID.Katana);
			Item.damage = 95;
			Item.DamageType = DamageClass.Melee;
			Item.width = 56;
			Item.height = 18;
			Item.useAnimation = Item.useTime = 18;
			//item.useTime = 15;
			//item.useAnimation = 15;
			//item.useStyle = 5;
			Item.knockBack = 6;
			Item.value*=10;
			Item.rare+=5;
			Item.scale = 1.1f;
			Item.shoot = ModContent.ProjectileType<Whip_Sword_Whip>();
			Item.shootSpeed = 5f;
			//Item.reuseDelay = 1;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(Mod, "ArtificerBonus", "Melee");
			float m = Main.mouseTextColor / 255f;
            line.OverrideColor = new Color((int)(179 * m), (int)(50 * m), 0);
            tooltips.Insert(1, line);
        }
		public override bool AltFunctionUse(Player player) => true;
		public override bool CanUseItem(Player player) {
			return player.GetModPlayer<ArtificerPlayer>().reloadTime <= 0;
		}
		public override bool? UseItem(Player player) {
			if (player.altFunctionUse == 2) {
				if (!player.ItemAnimationJustStarted) return false;
				ReloadProj p = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, new Vector2(0, 0), ModContent.ProjectileType<ReloadProj>(), 0, 0, player.whoAmI, 30).ModProjectile as ReloadProj;
				p.SetDefaults();
				if (extended) p.Tick = (proj, tick) => {
					SoundEngine.PlaySound(SoundID.Item37.WithPitch(0).WithVolume(0.33f), proj.Center);
				};
				p.Reload = () => {
					extended ^= true;
					Item.noUseGraphic = extended;
					Item.noMelee = extended;
					Item.useTurn = !extended;
					Item.useStyle = ItemUseStyleID.Swing;
					Item.autoReuse = true;
				};
				p.ticks = new List<ReloadTick>() { 3, 6, 9, 12, 15, 18, 21, 24, 27, 30 };
				Item.noUseGraphic = true;
				Item.noMelee = true;
				Item.useStyle = ItemUseStyleID.Guitar;
				Item.autoReuse = false;
				return true;
			}
			Item.noUseGraphic = extended;
			Item.noMelee = extended;
			Item.useTurn = !extended;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.autoReuse = true;
			return null;
		}
		public override float UseSpeedMultiplier(Player player) {
			if (player.altFunctionUse == 2) {
				return Item.useTime / 30f;
			}
			return extended ? 0.75f : 1;
		}
		public override bool CanShoot(Player player) {
			return extended && player.altFunctionUse != 2;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI, ai1: player.altFunctionUse).scale *= Item.scale;
			return false;
		}
	}
	public class Whip_Sword_Whip : ModProjectile, IWhipProjectile {
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Obsidian Spellsword");
			// This makes the projectile use whip collision detection and allows flasks to be applied to it.
			ProjectileID.Sets.IsAWhip[Type] = true;
		}
		public override string Texture => "Terraria/Images/Projectile_"+ ProjectileID.CoolWhip;
		public override void SetDefaults(){
			Projectile.CloneDefaults(ProjectileID.MaceWhip);
			Projectile.DamageType = DamageClass.Melee;
		}

		private float Timer {
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}
		public override void AI() {
			Player owner = Main.player[Projectile.owner];
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2; // Without PiOver2, the rotation would be off by 90 degrees counterclockwise.

			Projectile.Center = Main.GetPlayerArmPosition(Projectile) + Projectile.velocity * Timer;

			Projectile.spriteDirection = Projectile.velocity.X >= 0f ? 1 : -1;

			float swingTime = owner.itemAnimationMax * Projectile.MaxUpdates;

			if (Timer >= swingTime || owner.itemAnimation <= 0) {
				Projectile.Kill();
				return;
			}

			owner.heldProj = Projectile.whoAmI;

			// These two lines ensure that the timing of the owner's use animation is correct.
			owner.itemAnimation = owner.itemAnimationMax - (int)(Timer / Projectile.MaxUpdates);
			owner.itemTime = owner.itemAnimation;
			if (Timer == swingTime / 2 - 1) {
				Timer++;
			}
			if (Timer == swingTime / 2) {
				List<Vector2> points = Projectile.WhipPointsForCollision;
				Projectile.FillWhipControlPoints(Projectile, points);
				//SoundEngine.PlaySound(SoundID.AbigailSummon.WithPitch(1), points[points.Count - 1]);
			}
		}
		public void GetWhipSettings(out float timeToFlyOut, out int segments, out float rangeMultiplier) {
			timeToFlyOut = Main.player[Projectile.owner].itemAnimationMax * Projectile.MaxUpdates;
			segments = 20;
			rangeMultiplier = 1.5f * Projectile.scale;
		}


		// This method draws a line between all points of the whip, in case there's empty space between the sprites.
		private void DrawLine(List<Vector2> list) {
			Texture2D texture = TextureAssets.FishingLine.Value;
			Rectangle frame = texture.Frame();
			Vector2 origin = new Vector2(frame.Width / 2, 2);

			Vector2 pos = list[0];
			for (int i = 0; i < list.Count - 1; i++) {
				Vector2 element = list[i];
				Vector2 diff = list[i + 1] - element;

				float rotation = diff.ToRotation() - MathHelper.PiOver2;
				Color color = Lighting.GetColor(element.ToTileCoordinates(), Color.White);
				Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

				Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

				pos += diff;
			}
		}
		public override bool PreDraw(ref Color lightColor) {
			List<Vector2> list = new List<Vector2>();
			Projectile.FillWhipControlPoints(Projectile, list);
			Main.CurrentDrawnEntityShader = 79;
			DrawLine(list);

			Main.instance.LoadProjectile(Type);
			Main.DrawWhip_CoolWhip(Projectile, list);
			
			return false;
		}
	}
}
