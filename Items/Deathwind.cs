using System;
using System.Collections.Generic;
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
	public class Deathwind : ModItem {
		protected override bool CloneNewInstances => true;
		public int Reload = 1;
		public bool held = false;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Deathwind Carbine");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults(){
			Item.damage = 95;
            Item.crit = 11;
			Item.TryMakeExplosive(DamageClass.Ranged);
			Item.noMelee = true;
			Item.width = 60;
			Item.height = 18;
			Item.useTime = 1;
			Item.useAnimation = 19;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 6;
			Item.value = 50000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = null;
			Item.shoot = ProjectileID.WoodenArrowFriendly;//do not change
			Item.useAmmo = AmmoID.Rocket;
			//item.shoot = ProjectileID.DD2FlameBurstTowerT1Shot;
			Item.shootSpeed = 12.5f;
			Item.autoReuse = true;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(Mod, "ArtificerBonus", "Ranged:Gun/Launcher");
            line.OverrideColor = new Color(179, 50, 0);
            tooltips.Insert(1, line);
        }
		public override void ModifyWeaponDamage(Player player, ref StatModifier damage){
			damage = damage.CombineWith(player.bulletDamage);
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
					Item.holdStyle = 0;
					SoundEngine.PlaySound(SoundID.Camera, player.itemLocation);//22
				}
			}
            if(player.itemAnimation!=0&&player.itemAnimation!=player.itemAnimationMax-1) {
				if(Main.myPlayer==player.whoAmI&&PlayerInput.Triggers.JustPressed.MouseRight){
					player.itemAnimation = 0;
					Reload=1;
					Item.holdStyle = ItemHoldStyleID.HoldFront;
					SoundEngine.PlaySound(SoundID.Camera, player.itemLocation);
				}
            }
		}
        public override void HoldStyle(Player player, Rectangle heldItemFrame) {
            if(Reload==-1) {
				Reload=1;
				Item.holdStyle = ItemHoldStyleID.HoldFront;
				SoundEngine.PlaySound(SoundID.Camera, player.itemLocation);
            }
        }
        public override bool AltFunctionUse(Player player) => Reload==-1;
		public override bool CanUseItem(Player player){
			if(player.altFunctionUse==2){
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
			if(player.itemAnimation!=player.itemAnimationMax-1){
				return false;
			}
			type--;
			type/=3;
			type+=Deathwind_P1.id;
			Reload=-1;
			Projectile.NewProjectile(source, position + velocity, velocity, type, damage, knockback, player.whoAmI);
			SoundEngine.PlaySound(SoundID.Item98.WithPitch(1), position);
			SoundEngine.PlaySound(SoundID.Item11.WithPitch(1), position);
			//Main.PlaySound(2, position, 13);
			return false;
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale){
            if(held){
				Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.CombatText[1].Value, Reload==0?"1/1":"0/1", Main.screenWidth*0.90f, Main.screenHeight*0.85f, Color.White, Color.Black, new Vector2(0.3f), 1);
				held = false;
            }
        }
        public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Gyrojet>());
            recipe.AddIngredient(ItemID.FragmentVortex, 18);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }

	public class Deathwind_P1 : ModProjectile{
        internal virtual bool baseProj => true;
        public static int id = 0;
		public override void SetDefaults(){
			Projectile.CloneDefaults(ProjectileID.RocketI);
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 6;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 1200;
			Projectile.extraUpdates = 4;
			Projectile.aiStyle = 0;
		}
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Deathwind Carbine");
            if(baseProj)id = Projectile.type;
		}
		public override void AI(){
			Projectile.rotation = Projectile.velocity.ToRotation()+(float)(Math.PI/2);
            /*
            if(projectile.type%2==0) {
                int d = Dust.NewDust(projectile.Center - projectile.velocity * 0.5f-new Vector2(0, 4), 0, 0, 6, 0f, 0f, 100, Scale: 0.75f);
                Dust dust = Main.dust[d];
                dust.scale *= 1f + Main.rand.Next(10) * 0.1f;
                dust.velocity *= 0.2f;
            }//*/
            for(int i = 0; i < 3; i++) {
                Dust dust = Dust.NewDustPerfect(Projectile.Center - (Projectile.velocity * (i/3f)), 229, default, 200, Scale:0.5f);
                dust.noGravity = true;
                dust.velocity = default;
            }
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, default, Deathwind_Explosion.id, Projectile.damage/2, Projectile.knockBack*2, Projectile.owner, Projectile.type-id);
        }
        public override void Kill(int timeLeft) {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, default, Deathwind_Explosion.id, Projectile.damage, Projectile.knockBack*2, Projectile.owner, Projectile.type-id);
        }
	}
	public class Deathwind_P2 : Deathwind_P1{
        internal override bool baseProj => false;
		public override void SetDefaults(){
			Projectile.CloneDefaults(ProjectileID.RocketII);
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 4;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 1200;
			Projectile.extraUpdates = 4;
			Projectile.aiStyle = 0;
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
            ArtificeGlobalNPC globaltarget = target.GetGlobalNPC<ArtificeGlobalNPC>();
            int defense = Math.Max(target.defense-globaltarget.defreduc, 0);
			damage+=(int)(defense*0.2f);
			globaltarget.defreduc+=2;
		}
	}
	public class Deathwind_P3 : Deathwind_P1{
        internal override bool baseProj => false;
		public override void SetDefaults(){
			Projectile.CloneDefaults(ProjectileID.RocketIII);
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 6;
			Projectile.penetrate = 4;
			Projectile.timeLeft = 1200;
			Projectile.extraUpdates = 5;
			Projectile.aiStyle = 0;
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
            ArtificeGlobalNPC globaltarget = target.GetGlobalNPC<ArtificeGlobalNPC>();
            int defense = Math.Max(target.defense-globaltarget.defreduc, 0);
			damage+=(int)(defense*0.1f);
		}
	}
	public class Deathwind_P4 : Deathwind_P1{
        internal override bool baseProj => false;
		public override void SetDefaults(){
			Projectile.CloneDefaults(ProjectileID.RocketIV);
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 4;
			Projectile.penetrate = 4;
			Projectile.timeLeft = 1200;
			Projectile.extraUpdates = 5;
			Projectile.aiStyle = 0;
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
            Projectile.CloneDefaults(ProjectileID.RocketI);
            Projectile.width = Projectile.height = 0;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 5;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 0;
        }
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Deathwind Carbine");
            id = Projectile.type;
		}
        /*public override bool PreKill(int timeLeft) {
            projectile.type = ProjectileID.RocketI;//+(2&(int)projectile.ai[0])*3;
            return true;
        }*/
        public override void Kill(int timeLeft) {
            //int type = (int)projectile.ai[0];
            bool large = ((int)Projectile.ai[0]&2)!=0;
			Projectile.position.X += Projectile.width / 2;
			Projectile.position.Y += Projectile.height / 2;
            Projectile.width = large?48:32;
			Projectile.height = Projectile.width;
			Projectile.position.X -= Projectile.width / 2;
			Projectile.position.Y -= Projectile.height / 2;
			Projectile.Damage();
            //projectile.width = large?48:32;
            explosionEffect();
        }
        private void explosionEffect() {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
			for (int i = 0; i < 10; i++){
				int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 3.5f);
				Dust dust = Main.dust[d];
				dust.noGravity = true;
				dust.velocity *= 3.5f;
				d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 1.5f);
				dust = Main.dust[d];
				dust.velocity *= 1.5f;
			}
			float gorescale = 0.3f;
			int g = Gore.NewGore(Projectile.GetSource_FromThis(), new Vector2(Projectile.position.X, Projectile.position.Y), default, Main.rand.Next(61, 64));
			Gore gore = Main.gore[g];
			gore.velocity *= gorescale;
			Main.gore[g].velocity.X += 1f;
			Main.gore[g].velocity.Y += 1f;
			g = Gore.NewGore(Projectile.GetSource_FromThis(), new Vector2(Projectile.position.X, Projectile.position.Y), default, Main.rand.Next(61, 64));
			gore = Main.gore[g];
			gore.velocity *= gorescale;
			Main.gore[g].velocity.X -= 1f;
			Main.gore[g].velocity.Y += 1f;
			g = Gore.NewGore(Projectile.GetSource_FromThis(), new Vector2(Projectile.position.X, Projectile.position.Y), default, Main.rand.Next(61, 64));
			gore = Main.gore[g];
			gore.velocity *= gorescale;
			Main.gore[g].velocity.X += 1f;
			Main.gore[g].velocity.Y -= 1f;
			g = Gore.NewGore(Projectile.GetSource_FromThis(), new Vector2(Projectile.position.X, Projectile.position.Y), default, Main.rand.Next(61, 64));
			gore = Main.gore[g];
			gore.velocity *= gorescale;
			Main.gore[g].velocity.X -= 1f;
			Main.gore[g].velocity.Y -= 1f;
        }
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
            ArtificeGlobalNPC globaltarget = target.GetGlobalNPC<ArtificeGlobalNPC>();
            int defense = Math.Max(target.defense-globaltarget.defreduc, 0);
            switch((int)Projectile.ai[0]) {
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
