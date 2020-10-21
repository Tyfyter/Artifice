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
	//day 6
	public class Whip_Sword : ModItem {
		bool extended = false;
		public override bool CloneNewInstances => true;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Whip Sword");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults(){
			item.CloneDefaults(ItemID.Katana);
			item.damage = 95;
			item.melee = true;
			item.width = 56;
			item.height = 18;
			item.useAnimation = item.useTime = 12;
			//item.useTime = 15;
			//item.useAnimation = 15;
			//item.useStyle = 5;
			item.knockBack = 6;
			item.value*=10;
			item.rare+=5;
			item.scale = 1.1f;
			//item.shoot = ProjectileID.DD2FlameBurstTowerT1Shot;
			//item.shootSpeed = 12.5f;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(mod, "ArtificerBonus", "Melee");
			float m = Main.mouseTextColor / 255f;
            line.overrideColor = new Color((int)(179 * m), (int)(50 * m), 0);
            tooltips.Insert(1, line);
        }
		public override bool AltFunctionUse(Player player) => true;
		public override bool CanUseItem(Player player){
			if(player.altFunctionUse == 2){
				ReloadProj p = Projectile.NewProjectileDirect(player.Center, new Vector2(0,0), ModContent.ProjectileType<ReloadProj>(), 0, 0, item.owner, 30).modProjectile as ReloadProj;
				p.SetDefaults();
				if(extended)p.Tick = (proj,tick)=>{
					SoundEffectInstance sfi = Main.PlaySound(SoundID.Item37, proj.Center);
					sfi.Volume/=3;
					sfi.Pitch = 0;
				};
				p.Reload = ()=>{extended^=true;};
				p.ticks = new List<ReloadTick>(){3,6,9,12,15,18,21,24,27,30};
				item.useAnimation = item.useTime = 30;
				item.shoot = 0;
				item.shootSpeed = 1;
				item.noUseGraphic = true;
				item.useStyle = 5;
				item.noMelee = true;
				return base.CanUseItem(player);
			}
			if(extended){
				item.CloneDefaults(ItemID.Katana);
				item.damage = 95;
				item.useAnimation = item.useTime = 12;
				item.shoot = ModContent.ProjectileType<Whip_Sword_Whip>();
				item.shootSpeed = 1;
				item.noUseGraphic = true;
			}else{
				item.CloneDefaults(ItemID.Katana);
				item.damage = 95;
				item.useAnimation = item.useTime = 12;
				item.shoot = 0;
				item.shootSpeed = 0;
				item.noUseGraphic = false;
			}
			return true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			if(Projectile.NewProjectileDirect(position, new Vector2(0,0), type, damage, knockBack, item.owner, Main.rand.NextFloat(0.5f)+0.2f).modProjectile is Whip_Sword_Whip wsw){
				wsw.init(new Vector2(speedX,speedY).ToRotation());
			}
			return false;
		}
	}
	public class Whip_Sword_Whip : ModProjectile {
        public override string Texture => "Artifice/Items/Plumbata";
		Ray ray;
		float zDir = -1;
		bool flip = false;
		sbyte dir = 0;
		public override void SetDefaults(){
			projectile.CloneDefaults(ProjectileID.SwordBeam);
			projectile.aiStyle = 0;
			//projectile.timeLeft = 25;
			projectile.extraUpdates = 6;
			projectile.penetrate = -1;
			projectile.light = 0;
		}
		public override void AI(){
			Player player = Main.player[projectile.owner];
			player.direction = dir;
			projectile.rotation+=(0.1f*projectile.ai[0])*player.direction;// = projectile.velocity.ToRotation();
			zDir+=(0.04f*(1-projectile.ai[0]));
			projectile.Center = player.MountedCenter + new Vector2();
			if(zDir>=projectile.ai[0])projectile.Kill();
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox){
			float? f = ray.Intersects(targetHitbox.toBB(0.5f));
            return f!=null&&f<=1;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox){
            ray = new Ray(hitbox.Center().to3(), new Vector2(115f,0).RotatedBy(projectile.rotation).to3(zDir));
			Vector3 a = ray.Position;
			Vector3 b = ray.Position+ray.Direction;
            int x = (int)Math.Min(a.X,b.X);
            int y = (int)Math.Min(a.Y,b.Y);
            hitbox = new Rectangle(x,y,(int)Math.Max(a.X,b.X)-x,(int)Math.Max(a.Y,b.Y)-y);
            //Main.player[projectile.owner].chatOverhead.NewMessage(ray.ToString(),14);
        }
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
			//Utils.DrawLine(spriteBatch, (ray.Position).to2(), (ray.Position+ray.Direction).to2(), new Color(255,0,0,(int)(255*Math.Abs(zDir/projectile.ai[0]))));
			Vector2 origin = new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y);
			for(int i = 0; i < 18; i++){
				spriteBatch.Draw(Main.chainTexture, origin+new Vector2(8*i/(Math.Abs(zDir)+1),0).RotatedBy(projectile.rotation),
				new Rectangle(0, 0, Main.chainTexture.Width, Main.chainTexture.Height), lightColor, projectile.rotation+(float)(Math.PI/2),
				new Vector2(Main.chainTexture.Width * 0.5f, Main.chainTexture.Height * 0.5f), (i*(flip?1-zDir:zDir+1)/16)+(1.8f/i), SpriteEffects.None, 0f);
				//Dust d = Dust.NewDustDirect(projectile.Center+new Vector2(8*i/(Math.Abs(zDir)+1),0).RotatedBy(projectile.rotation), 0, 0, 66, 0, 0);
				//d.scale = (i*(flip?1-zDir:zDir+1)/16)+(1.8f/i);
				//d.noGravity = true;
			}
			return false;
		}
		/*public override void PostDraw(SpriteBatch spriteBatch, Color lightColor){
			Utils.DrawLine(spriteBatch, (ray.Position).to2(), (ray.Position+ray.Direction).to2(), Color.Lerp(new Color(255,0,0,100), new Color(0,0,255,100), (float)Math.Abs(zDir/projectile.ai[0])-0.5f));
		}*/
		public void init(float angle){
			Player player = Main.player[projectile.owner];
			projectile.rotation = angle - (2.5f*projectile.ai[0]*player.direction);
			zDir = -(1-projectile.ai[0]);
			flip = Main.rand.NextBool();
			dir = (sbyte)player.direction;
		}
	}
}
