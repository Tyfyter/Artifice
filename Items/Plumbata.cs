using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Items {
    //day 3
    public class Plumbata : ModItem {
		protected override bool CloneNewInstances => true;
        bool held = false;
        int proj;
        //public override string Texture => "Artifice/Items/Plumbata";
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Shield & Plumbata");
			Tooltip.SetDefault("");
            SacrificeTotal = 1;
        }
		public override void SetDefaults(){
			Item.damage = 110;
			Item.DamageType = DamageClass.Throwing;
			Item.noMelee = true;
			Item.width = 56;
			Item.height = 22;
			Item.useTime = 17;
			Item.useAnimation = 17;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item34;
			Item.shoot = ModContent.ProjectileType<Plumbata_P>();
			//item.shoot = ProjectileID.DD2FlameBurstTowerT1Shot;
			Item.shootSpeed = 25f;
			Item.autoReuse = true;
            Item.noUseGraphic = true;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(Mod, "ArtificerBonus", "Thrown/Melee");
            line.OverrideColor = new Color(179, 50, 0);
            tooltips.Insert(1, line);
        }
		public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PaladinsShield, 1);
			recipe.AddIngredient(ItemID.EndlessQuiver, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
		public override Vector2? HoldoutOffset(){
			return new Vector2(-8, -2);
		}
		public override bool AltFunctionUse(Player player) => true;
		public override bool CanUseItem(Player player){
			//item.noUseGraphic = player.altFunctionUse==2;
            if(player.altFunctionUse==2){
                Item.UseSound = null;
                Item.useTime = 1;
                Item.useAnimation = 17;
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.DamageType = DamageClass.Melee;
            } else{
                Item.UseSound = SoundID.Item1;
                Item.useTime = 9;
                Item.useAnimation = 9;
                Item.useStyle = ItemUseStyleID.Swing;
                Item.DamageType = DamageClass.Throwing;
            }
			return base.CanUseItem(player);
		}
        public override void HoldStyle(Player player, Rectangle heldItemFrame){
            held = false;
        }
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		    if(player.altFunctionUse!=2&&!player.controlUseTile)return true;
            //Main.PlaySound(useSound, position);
            switch (player.itemAnimation){
                case 13:
                case 12:
                if(Main.projectile[proj].active&&Main.projectile[proj].type==ModContent.ProjectileType<Shield>())return false;
                proj = Projectile.NewProjectile(source, position, new Vector2(0,0), ModContent.ProjectileType<Shield>(), (int)(damage*0.75f), knockback, player.whoAmI);
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
            Projectile.CloneDefaults(1);
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            AIType = 1;
        }
        public override bool PreKill(int timeLeft) => false;
    }
    public class Shield : ModProjectile {
        protected override bool CloneNewInstances => true;
        private const int maxCharge = 3;
        Ray ray;
        public override void SetDefaults() {
            //projectile.name = "Ice Shield";
            Projectile.width = 14;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 360;
            Projectile.light = 0.75f;
            Projectile.extraUpdates = 1;
            Projectile.ignoreWater = true;
            Projectile.scale = 1.25f;
        }
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Shield");
		}
        public override void AI(){
            Player player = Main.player[Projectile.owner];
            Vector2 mousePos = Main.MouseWorld;
            Vector2 unit = (mousePos - player.Center);
            unit.Normalize();
            unit *= 4;
            Projectile.rotation = (float)Math.Atan2((player.Center - mousePos).Y, (player.Center - mousePos).X) + 3.1157f;
            Projectile.Center = player.MountedCenter + new Vector2(-20-8*(float)Math.Abs(Math.Sin(Projectile.rotation-3.1157)),0).RotatedBy(Projectile.rotation-3.1157/*(float)Math.Atan2((player.Center - mousePos).Y, (player.Center - mousePos).X)*/);
            if(player.itemAnimation>=4&&Projectile.ai[0]==0){
                Projectile.alpha = 50;
                return;
            }else{
                Projectile.alpha = 255;
            }
            Projectile target;
            Ray ray2 = ray;
            ray2.Position+=unit.To3();
            for (int i = 0; i<Main.projectile.Length; i++){
                target = Main.projectile[i];
                if(target.type == Projectile.type||!(target.owner!=Projectile.owner||target.npcProj||target.hostile||target.trap)||target.damage<1) continue;
                Rectangle targetHitbox = target.Hitbox;
                for(int i2 = 0; i2<=target.extraUpdates; i2++) {
                    targetHitbox.Offset((target.velocity*i2).ToPoint());
                    float? f = ray.Intersects(targetHitbox.ToBB());
                    if(f==null||f>1)f = ray2.Intersects(targetHitbox.ToBB());
                    if(f!=null&&f<=1) {
                        if(target.damage<=Projectile.damage) {
                            target.Kill();
                        } else {
                            target.penetrate--;
                            target.damage-=Projectile.damage;
                            target.velocity+=unit;
                        }
                        goto skipBlock;
                    }
                }
                skipBlock:;
            }
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox){
            ray = new Ray(hitbox.Center().To3(), new Vector2(43.75f,0).RotatedBy(Projectile.rotation+(float)(Math.PI/2)).To3());
            ray.Position-=ray.Direction/2;
            hitbox = dothething(ray.Position.To2(),(ray.Position+ray.Direction).To2());
            //Main.player[projectile.owner].chatOverhead.NewMessage(ray.ToString(),14);
        }
        /*public override void PostDraw(SpriteBatch spriteBatch, Color lightColor){
            Vector2 pos = ray.Position.to2();
            Vector2 dir = pos+ray.Direction.to2();
            Utils.DrawLine(spriteBatch, pos, dir, new Color(25,0,0,50));
        }*/
        public override bool? CanHitNPC(NPC target){
            Player player = Main.player[Projectile.owner];
            return (Projectile.damage*player.velocity.Length()/5)>=10?base.CanHitNPC(target):false;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
            Player player = Main.player[Projectile.owner];
            knockback*=player.velocity.Length();
            damage = (int)(damage*player.velocity.Length()/5);
        }
        public override bool PreDraw(ref Color lightColor){
            bool right = Projectile.rotation<Math.PI/1.5||Projectile.rotation>=Math.PI*1.5;
            Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 14, 32), new Color(255,255,255,Projectile.alpha), Projectile.rotation, new Vector2(7,16), 1.25f, right?SpriteEffects.None:SpriteEffects.FlipVertically, 0);
            return false;
        }
        static Rectangle dothething(Vector2 a, Vector2 b){
            int x = (int)Math.Min(a.X,b.X);
            int y = (int)Math.Min(a.Y,b.Y);
            return new Rectangle(x,y,(int)Math.Max(a.X,b.X)-x,(int)Math.Max(a.Y,b.Y)-y);
        }
    }
}