using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Items {
	public class Deathwind : ModItem {
		public override bool CloneNewInstances => true;
		public int Reload = 1;
		public bool held = false;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Deathwind Carbine");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults(){
			item.damage = 95;
            item.crit = 11;
			item.ranged = true;
			item.noMelee = true;
			item.width = 60;
			item.height = 18;
			item.useTime = 1;
			item.useAnimation = 19;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 50000;
			item.rare = 2;
			item.UseSound = null;
			item.shoot = 1;//do not change
			item.useAmmo = AmmoID.Rocket;
			//item.shoot = ProjectileID.DD2FlameBurstTowerT1Shot;
			item.shootSpeed = 12.5f;
			item.autoReuse = true;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(mod, "ArtificerBonus", "Ranged:Gun/Launcher");
            line.overrideColor = new Color(179, 50, 0);
            tooltips.Insert(1, line);
        }
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat){
			mult*=player.bulletDamage;
		}
		public override Vector2? HoldoutOffset(){
			return new Vector2(-12, 0);//Reload>10&&Reload<20?new Vector2(-24, 0):
		}
		public override void HoldItem(Player player){
			held = true;
            if(Reload>0){
				player.itemRotation = Reload>3&&Reload<13?player.direction/2f:0;
				if(++Reload>18){
					Reload = 0;
					item.holdStyle = 0;
					Main.PlaySound(SoundID.Camera, player.itemLocation);//22
				}
			}
            if(player.itemAnimation!=0&&player.itemAnimation!=player.itemAnimationMax-1) {
				if(Main.myPlayer==player.whoAmI&&PlayerInput.Triggers.JustPressed.MouseRight){
					player.itemAnimation = 0;
					Reload=1;
					item.holdStyle = ItemHoldStyleID.HarpHoldingOut;
					Main.PlaySound(SoundID.Camera, player.itemLocation);
				}
            }
		}
        public override void HoldStyle(Player player) {
            if(Reload==-1) {
				Reload=1;
				item.holdStyle = ItemHoldStyleID.HarpHoldingOut;
				Main.PlaySound(SoundID.Camera, player.itemLocation);
            }
        }
        public override bool AltFunctionUse(Player player) => Reload==-1;
		public override bool CanUseItem(Player player){
			if(player.altFunctionUse==2){
				Reload=1;
				item.holdStyle = ItemHoldStyleID.HarpHoldingOut;
				Main.PlaySound(SoundID.Camera, player.itemLocation);//22
				return false;
			}
			return Reload==0?base.CanUseItem(player):false;
		}
        public override bool ConsumeAmmo(Player player) {
            return player.itemAnimation == player.itemAnimationMax-1;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){

			//Main.PlaySound(useSound, position);
			if(player.itemAnimation!=player.itemAnimationMax-1){
				return false;
			}
			type--;
			type/=3;
			type+=Deathwind_P1.id;
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
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Gyrojet>());
            recipe.AddIngredient(ItemID.FragmentVortex, 18);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

	public class Deathwind_P1 : ModProjectile{
        public static int id = 0;
		public override void SetDefaults(){
			projectile.CloneDefaults(ProjectileID.RocketI);
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 6;
			projectile.penetrate = 3;
			projectile.timeLeft = 1200;
			projectile.extraUpdates = 4;
			projectile.aiStyle = 0;
		}
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Deathwind Carbine");
            if(id==0)id = projectile.type;
		}
		public override void AI(){
			projectile.rotation = projectile.velocity.ToRotation()+(float)(Math.PI/2);
            /*
            if(projectile.type%2==0) {
                int d = Dust.NewDust(projectile.Center - projectile.velocity * 0.5f-new Vector2(0, 4), 0, 0, 6, 0f, 0f, 100, Scale: 0.75f);
                Dust dust = Main.dust[d];
                dust.scale *= 1f + Main.rand.Next(10) * 0.1f;
                dust.velocity *= 0.2f;
            }//*/
            for(int i = 0; i < 3; i++) {
                Dust dust = Dust.NewDustPerfect(projectile.Center - (projectile.velocity * (i/3f)), 229, default, 200, Scale:0.5f);
                dust.noGravity = true;
                dust.velocity = default;
            }
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            Projectile.NewProjectile(projectile.Center, default, Deathwind_Explosion.id, projectile.damage/2, projectile.knockBack*2, projectile.owner, projectile.type-id);
        }
        public override void Kill(int timeLeft) {
            Projectile.NewProjectile(projectile.Center, default, Deathwind_Explosion.id, projectile.damage, projectile.knockBack*2, projectile.owner, projectile.type-id);
        }
	}
	public class Deathwind_P2 : Deathwind_P1{
		public override void SetDefaults(){
			projectile.CloneDefaults(ProjectileID.RocketII);
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 4;
			projectile.penetrate = 3;
			projectile.timeLeft = 1200;
			projectile.extraUpdates = 4;
			projectile.aiStyle = 0;
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
            ArtificeGlobalNPC globaltarget = target.GetGlobalNPC<ArtificeGlobalNPC>();
            int defense = Math.Max(target.defense-globaltarget.defreduc, 0);
			damage+=(int)(defense*0.2f);
			globaltarget.defreduc+=2;
		}
	}
	public class Deathwind_P3 : Deathwind_P1{
		public override void SetDefaults(){
			projectile.CloneDefaults(ProjectileID.RocketIII);
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 6;
			projectile.penetrate = 4;
			projectile.timeLeft = 1200;
			projectile.extraUpdates = 5;
			projectile.aiStyle = 0;
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
            ArtificeGlobalNPC globaltarget = target.GetGlobalNPC<ArtificeGlobalNPC>();
            int defense = Math.Max(target.defense-globaltarget.defreduc, 0);
			damage+=(int)(defense*0.1f);
		}
	}
	public class Deathwind_P4 : Deathwind_P1{
		public override void SetDefaults(){
			projectile.CloneDefaults(ProjectileID.RocketIV);
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 4;
			projectile.penetrate = 4;
			projectile.timeLeft = 1200;
			projectile.extraUpdates = 5;
			projectile.aiStyle = 0;
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
            ArtificeGlobalNPC globaltarget = target.GetGlobalNPC<ArtificeGlobalNPC>();
            int defense = Math.Max(target.defense-globaltarget.defreduc, 0);
			damage+=(int)(defense*0.4f);
			globaltarget.defreduc+=10;
		}
	}
    public class Deathwind_Explosion : ModProjectile {
        public static int id = 0;
        public override string Texture => "Artifice/Items/Deathwind";
        public override void SetDefaults() {
            projectile.CloneDefaults(ProjectileID.RocketI);
            projectile.width = projectile.height = 0;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 5;
            projectile.aiStyle = 0;
            projectile.timeLeft = 0;
        }
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Deathwind Carbine");
            id = projectile.type;
		}
        /*public override bool PreKill(int timeLeft) {
            projectile.type = ProjectileID.RocketI;//+(2&(int)projectile.ai[0])*3;
            return true;
        }*/
        public override void Kill(int timeLeft) {
            //int type = (int)projectile.ai[0];
            bool large = ((int)projectile.ai[0]&2)!=0;
			projectile.position.X += projectile.width / 2;
			projectile.position.Y += projectile.height / 2;
            projectile.width = large?48:32;
			projectile.height = projectile.width;
			projectile.position.X -= projectile.width / 2;
			projectile.position.Y -= projectile.height / 2;
			projectile.Damage();
            //projectile.width = large?48:32;
            explosionEffect();
        }
        private void explosionEffect() {
            Main.PlaySound(SoundID.Item, projectile.Center, 14);
			for (int i = 0; i < 10; i++){
				int d = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default, 3.5f);
				Dust dust = Main.dust[d];
				dust.noGravity = true;
				dust.velocity *= 3.5f;
				d = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default, 1.5f);
				dust = Main.dust[d];
				dust.velocity *= 1.5f;
			}
			float gorescale = 0.3f;
			int g = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default, Main.rand.Next(61, 64));
			Gore gore = Main.gore[g];
			gore.velocity *= gorescale;
			Main.gore[g].velocity.X += 1f;
			Main.gore[g].velocity.Y += 1f;
			g = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default, Main.rand.Next(61, 64));
			gore = Main.gore[g];
			gore.velocity *= gorescale;
			Main.gore[g].velocity.X -= 1f;
			Main.gore[g].velocity.Y += 1f;
			g = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default, Main.rand.Next(61, 64));
			gore = Main.gore[g];
			gore.velocity *= gorescale;
			Main.gore[g].velocity.X += 1f;
			Main.gore[g].velocity.Y -= 1f;
			g = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default, Main.rand.Next(61, 64));
			gore = Main.gore[g];
			gore.velocity *= gorescale;
			Main.gore[g].velocity.X -= 1f;
			Main.gore[g].velocity.Y -= 1f;
        }
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
            ArtificeGlobalNPC globaltarget = target.GetGlobalNPC<ArtificeGlobalNPC>();
            int defense = Math.Max(target.defense-globaltarget.defreduc, 0);
            switch((int)projectile.ai[0]) {
                case 1:
			    damage+=(int)(defense*0.2f);
                target.GetGlobalNPC<ArtificeGlobalNPC>().defreduc+=1;
                break;
                case 2:
			    damage+=(int)(defense*0.1f);
                break;
                case 3:
			    damage+=(int)(defense*0.4f);
                target.GetGlobalNPC<ArtificeGlobalNPC>().defreduc+=5;
                break;
            }
		}
    }
}
