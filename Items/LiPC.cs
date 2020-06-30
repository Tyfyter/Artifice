using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Items {
    //day 2
    public class LiPC : ModItem {
		public override bool CloneNewInstances => true;
        int use = 0;
        public short[] glowmasks;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("LiPC");
			Tooltip.SetDefault("\"Laser-induced plasma channel\"");
            glowmasks = new short[]{
                Artifice.SetGlowMask("LiPC_glow_1"),
                Artifice.SetGlowMask("LiPC_glow_2"),
                Artifice.SetGlowMask("LiPC_glow_3"),
                Artifice.SetGlowMask("LiPC_glow_4"),
                Artifice.SetGlowMask("LiPC_glow_5"),
                Artifice.SetGlowMask("LiPC_glow_6")
            };
		}
		public override void SetDefaults(){
			item.damage = 50;
			item.magic = true;
            item.ranged = true;
			item.noMelee = true;
			item.width = 56;
			item.height = 22;
			item.useTime = 1;
			item.useAnimation = 5;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item34;
			item.shoot = ModContent.ProjectileType<LaserGuide>();
			//item.shoot = ProjectileID.DD2FlameBurstTowerT1Shot;
			item.shootSpeed = 1f;
			item.autoReuse = true;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(mod, "ArtificerBonus", "Ranged/Magic");
            line.overrideColor = new Color(179, 50, 0);
            tooltips.Insert(1, line);
        }

		public override void AddRecipes(){
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ElectrosphereLauncher, 1);
			recipe.AddIngredient(ItemID.MartianConduitPlating, 20);
			recipe.AddIngredient(ItemID.MechanicalLens, 1);
			recipe.AddIngredient(ItemID.Lens, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
        public override void HoldItem(Player player){
            if(use>=5){
                switch (use){
                    case 14:
                    item.glowMask = glowmasks[4];
                    break;
                    case 23:
                    item.glowMask = glowmasks[3];
                    break;
                    case 32:
                    item.glowMask = glowmasks[2];
                    break;
                    case 41:
                    item.glowMask = glowmasks[1];
                    break;
                    case 50:
                    item.glowMask = glowmasks[0];
                    break;
                    default:
                    break;
                }
                if(++use>59){
                    item.glowMask = -1;
                    use = 0;
                }
            }
        }
        public override void HoldStyle(Player player){
            if(use<5)use = 0;
        }
		public override Vector2? HoldoutOffset(){
			return new Vector2(-8, -2);
		}
		public override bool AltFunctionUse(Player player) => true;
		public override bool CanUseItem(Player player){
			//item.noUseGraphic = player.altFunctionUse==2;
			item.UseSound = player.altFunctionUse==2?SoundID.Item34:null;
			return base.CanUseItem(player);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			if(!player.controlUseTile&&use<5)use = 0;
            Vector2 oofset = new Vector2(speedX,speedY).RotatedBy(Math.PI/2)*player.direction*-4.75f;
            position+=oofset;
            if(player.altFunctionUse!=2&&!player.controlUseTile)return true;
            if(use>5)return true;
            //Main.PlaySound(useSound, position);
            if(++use==5){
                int proj = Projectile.NewProjectile(position + new Vector2(speedX,speedY), new Vector2(speedX,speedY).RotatedByRandom(0.1), ProjectileID.CultistBossLightningOrbArc, damage, knockBack, item.owner, new Vector2(speedX, speedY).ToRotation(), Main.rand.NextFloat());
                Main.projectile[proj].friendly = true;
                Main.projectile[proj].hostile = false;
                Main.projectile[proj].timeLeft/=5;
                Main.projectile[proj].usesLocalNPCImmunity = true;
                Main.projectile[proj].localNPCHitCooldown = 4;
                item.glowMask = glowmasks[5];
                Main.PlaySound(SoundID.Item38, position);
            }
            if(use<5)Main.PlaySound(SoundID.Item30, position).Pitch = use/3f;//30 35
            return player.altFunctionUse!=2;
		}
        
    }
    public class LaserGuide : ModProjectile {
        public override string Texture => "Terraria/Projectile_188";
        public override bool CloneNewInstances => true;
        public bool notkil = true;
        /*public override void AI(){
            Player player = Main.player[projectile.owner];
            if(player.controlUseItem&&notkil)projectile.timeLeft = 2;
        }*/
        public override void SetDefaults()
        {
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.timeLeft = 4;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.width = 2;
            projectile.height = 2;
            projectile.netImportant = true;
        }
        public override bool PreKill(int timeLeft){
            notkil = false;
            return true;
        }
        public override bool ShouldUpdatePosition() => true;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 endPoint = GetEndPoint();

            float drawRotation = projectile.velocity.ToRotation();
            Vector2 drawOrigin = new Vector2(projectile.width / 2, projectile.height / 2);

            Rectangle drawRect = new Rectangle(
                (int)Math.Round(projectile.Center.X - Main.screenPosition.X),
                (int)Math.Round(projectile.Center.Y - Main.screenPosition.Y),
                (int)Math.Round((endPoint - projectile.Center).Length()),
                projectile.width);
                
            spriteBatch.Draw(mod.GetTexture("Projectiles/Laser"), drawRect, null, Color.Red, drawRotation, Vector2.Zero, SpriteEffects.None, 0);

            return false;
        }


        private Vector2 GetEndPoint() {
            Vector2 pos = projectile.Center;
            List<int> boosted = new List<int>(){};
            while ((pos - projectile.Center).Length() < Main.screenWidth && Collision.CanHit(projectile.Center, 1, 1, pos, 1, 1)) {
                pos += projectile.velocity.SafeNormalize(Vector2.One);
                Lighting.AddLight(pos, 0.1f, 0, 0);
                for(int i = 0; i < Main.npc.Length; i++){
                    NPC npc = Main.npc[i];
                    if (npc.active && npc.Hitbox.Contains((int)pos.X, (int)pos.Y)){
                        return pos;
                    }
                }
                for(int i = 0; i < Main.player.Length; i++){
                    Player player = Main.player[i];
                    if (player.active && player.whoAmI != projectile.owner && player.Hitbox.Contains((int)pos.X, (int)pos.Y)) {
                        return pos;
                    }
                }
                for(int i = 0; i < Main.projectile.Length; i++){
                    Projectile proj = Main.projectile[i];
                    if (proj.active && (proj.type == ProjectileID.CultistBossLightningOrbArc || proj.type == ProjectileID.VortexLightning)) {
                        Rectangle box = proj.Hitbox;
                        box.Inflate(32,32);
                        if(box.Contains((int)pos.X, (int)pos.Y)){
                            proj.ai[0] = ((pos+projectile.velocity*3)-proj.Center).ToRotation();// - (Main.rand.NextFloat((float)Math.PI/2)-((float)Math.PI/4));
                            proj.velocity = Vector2.Lerp(proj.velocity,projectile.velocity.SafeNormalize(proj.velocity) * 6, 0.01f);//*=1.0001f;
                            proj.friendly = true;
                            proj.timeLeft+=3;
                            if(!boosted.Contains(i)){
                                boosted.Add(i);
                                proj.damage+=3;
                                if(Main.rand.NextBool(6))proj.penetrate++;
                            }
                        }
                    }
                }
            }
            return pos;
        }
    }
}