using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Items {
    //day 3
    public class Plumbata : ModItem {
		public override bool CloneNewInstances => true;
        bool held = false;
        int proj;
        //public override string Texture => "Artifice/Items/Plumbata";
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Shield & Plumbata");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults(){
			item.damage = 110;
			item.thrown = true;
			item.noMelee = true;
			item.width = 56;
			item.height = 22;
			item.useTime = 17;
			item.useAnimation = 17;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item34;
			item.shoot = ModContent.ProjectileType<Plumbata_P>();
			//item.shoot = ProjectileID.DD2FlameBurstTowerT1Shot;
			item.shootSpeed = 25f;
			item.autoReuse = true;
            item.noUseGraphic = true;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(mod, "ArtificerBonus", "Thrown/Melee");
            line.overrideColor = new Color(179, 50, 0);
            tooltips.Insert(1, line);
        }
		public override void AddRecipes(){
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.PaladinsShield, 1);
			recipe.AddIngredient(ItemID.EndlessQuiver, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override Vector2? HoldoutOffset(){
			return new Vector2(-8, -2);
		}
		public override bool AltFunctionUse(Player player) => true;
		public override bool CanUseItem(Player player){
			//item.noUseGraphic = player.altFunctionUse==2;
            if(player.altFunctionUse==2){
                item.UseSound = null;
                item.useTime = 1;
                item.useAnimation = 17;
                item.useStyle = 5;
			    item.melee = true;
            }else{
                item.UseSound = SoundID.Item1;
                item.useTime = 9;
                item.useAnimation = 9;
                item.useStyle = 1;
			    item.melee = false;
            }
			return base.CanUseItem(player);
		}
        public override void HoldStyle(Player player){
            held = false;
        }
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
            if(player.altFunctionUse!=2&&!player.controlUseTile)return true;
            //Main.PlaySound(useSound, position);
            switch (player.itemAnimation){
                case 13:
                case 12:
                if(Main.projectile[proj].active&&Main.projectile[proj].type==ModContent.ProjectileType<Shield>())return false;
                proj = Projectile.NewProjectile(position, new Vector2(0,0), ModContent.ProjectileType<Shield>(), (int)(damage*0.75f), knockBack, item.owner);
                Main.projectile[proj].friendly = true;
                Main.projectile[proj].hostile = false;
                Main.projectile[proj].timeLeft/=5;
                Main.projectile[proj].usesLocalNPCImmunity = true;
                Main.projectile[proj].localNPCHitCooldown = 4;
                return false;
                case 5:
                case 4:
                if(!player.controlUseTile)return false;
                Main.projectile[proj].ai[0] = 1;
                Main.projectile[proj].timeLeft = 12;
                player.itemAnimation = 8;
                return false;
                default:
                return false;
            }
		}
    }
    public class Plumbata_P : ModProjectile {
        public override string Texture => "Artifice/Items/Plumbata";
        public override void SetDefaults(){
            projectile.CloneDefaults(1);
            projectile.width = 10;
            projectile.height = 10;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 30;
            aiType = 1;
        }
        public override bool PreKill(int timeLeft) => false;
    }
    public class Shield : ModProjectile {
        public override bool CloneNewInstances => true;
        private const int maxCharge = 3;
        Ray ray;
        public override void SetDefaults()
        {
            //projectile.name = "Ice Shield";
            projectile.width = 14;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 360;
            projectile.light = 0.75f;
            projectile.extraUpdates = 1;
            projectile.ignoreWater = true;
            projectile.scale = 1.25f;
        }
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Shield");
		}
        public override void AI(){
            Player player = Main.player[projectile.owner];
            Vector2 mousePos = Main.MouseWorld;
            Vector2 unit = (mousePos - player.Center);
            unit.Normalize();
            unit *= 4;
            projectile.rotation = (float)Math.Atan2((player.Center - mousePos).Y, (player.Center - mousePos).X) + 3.1157f;
            projectile.Center = player.MountedCenter + new Vector2(-20-8*(float)Math.Abs(Math.Sin(projectile.rotation-3.1157)),0).RotatedBy(projectile.rotation-3.1157/*(float)Math.Atan2((player.Center - mousePos).Y, (player.Center - mousePos).X)*/);
            if(player.itemAnimation>=4&&projectile.ai[0]==0){
                projectile.alpha = 50;
                return;
            }else{
                projectile.alpha = 255;
            }
            Projectile target;
            Ray ray2 = ray;
            ray2.Position+=unit.to3();
            for (int i = 0; i<Main.projectile.Length; i++){
                target = Main.projectile[i];
                if(target.type == projectile.type||!(target.owner!=projectile.owner||target.npcProj||target.hostile||target.trap)||target.damage<1) continue;
                Rectangle targetHitbox = target.Hitbox;
                for(int i2 = 0; i2<=target.extraUpdates; i2++) {
                    targetHitbox.Offset((target.velocity*i2).ToPoint());
                    float? f = ray.Intersects(targetHitbox.toBB());
                    if(f==null||f>1)f = ray2.Intersects(targetHitbox.toBB());
                    if(f!=null&&f<=1) {
                        if(target.damage<=projectile.damage) {
                            target.Kill();
                        } else {
                            target.penetrate--;
                            target.damage-=projectile.damage;
                            target.velocity+=unit;
                        }
                        goto skipBlock;
                    }
                }
                skipBlock:;
            }
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox){
            ray = new Ray(hitbox.Center().to3(), new Vector2(43.75f,0).RotatedBy(projectile.rotation+(float)(Math.PI/2)).to3());
            ray.Position-=ray.Direction/2;
            hitbox = dothething(ray.Position.to2(),(ray.Position+ray.Direction).to2());
            //Main.player[projectile.owner].chatOverhead.NewMessage(ray.ToString(),14);
        }
        /*public override void PostDraw(SpriteBatch spriteBatch, Color lightColor){
            Vector2 pos = ray.Position.to2();
            Vector2 dir = pos+ray.Direction.to2();
            Utils.DrawLine(spriteBatch, pos, dir, new Color(25,0,0,50));
        }*/
        public override bool? CanHitNPC(NPC target){
            Player player = Main.player[projectile.owner];
            return (projectile.damage*player.velocity.Length()/5)>=10?base.CanHitNPC(target):false;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
            Player player = Main.player[projectile.owner];
            knockback*=player.velocity.Length();
            damage = (int)(damage*player.velocity.Length()/5);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            bool right = projectile.rotation<Math.PI/1.5||projectile.rotation>=Math.PI*1.5;
            spriteBatch.Draw(mod.GetTexture("Items/Shield"), projectile.Center - Main.screenPosition, new Rectangle(0, 0, 14, 32), new Color(255,255,255,projectile.alpha), projectile.rotation, new Vector2(7,16), 1.25f, right?SpriteEffects.None:SpriteEffects.FlipVertically, 0f);
            return false;
        }
        static Rectangle dothething(Vector2 a, Vector2 b){
            int x = (int)Math.Min(a.X,b.X);
            int y = (int)Math.Min(a.Y,b.Y);
            return new Rectangle(x,y,(int)Math.Max(a.X,b.X)-x,(int)Math.Max(a.Y,b.Y)-y);
        }
    }
}