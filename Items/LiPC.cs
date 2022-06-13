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
    //day 2
    public class LiPC : ModItem {
		protected override bool CloneNewInstances => true;
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
            SacrificeTotal = 1;
        }
		public override void SetDefaults(){
			Item.damage = 50;
			Item.DamageType = DamageClasses.Ranged_Magic;
			Item.noMelee = true;
			Item.width = 56;
			Item.height = 22;
			Item.useTime = 1;
			Item.useAnimation = 5;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item34;
			Item.shoot = ModContent.ProjectileType<LaserGuide>();
			//item.shoot = ProjectileID.DD2FlameBurstTowerT1Shot;
			Item.shootSpeed = 1f;
			Item.autoReuse = true;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(Mod, "ArtificerBonus", "Ranged/Magic");
            line.OverrideColor = new Color(179, 50, 0);
            tooltips.Insert(1, line);
        }

		public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ElectrosphereLauncher, 1);
			recipe.AddIngredient(ItemID.MartianConduitPlating, 20);
			recipe.AddIngredient(ItemID.MechanicalLens, 1);
			recipe.AddIngredient(ItemID.Lens, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
        public override void HoldItem(Player player){
            if(use>=5){
                switch (use){
                    case 14:
                    Item.glowMask = glowmasks[4];
                    break;
                    case 23:
                    Item.glowMask = glowmasks[3];
                    break;
                    case 32:
                    Item.glowMask = glowmasks[2];
                    break;
                    case 41:
                    Item.glowMask = glowmasks[1];
                    break;
                    case 50:
                    Item.glowMask = glowmasks[0];
                    break;
                    default:
                    break;
                }
                if(++use>59){
                    Item.glowMask = -1;
                    use = 0;
                }
            }
        }
        public override void HoldStyle(Player player, Rectangle heldItemFrame){
            if(use<5)use = 0;
        }
		public override Vector2? HoldoutOffset(){
			return new Vector2(-8, -2);
		}
		public override bool AltFunctionUse(Player player) => true;
		public override bool CanUseItem(Player player){
			//item.noUseGraphic = player.altFunctionUse==2;
			Item.UseSound = player.altFunctionUse==2?SoundID.Item34:null;
			return base.CanUseItem(player);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(!player.controlUseTile&&use<5)use = 0;
            Vector2 oofset = velocity.RotatedBy(Math.PI/2)*player.direction*-4.75f;
            position+=oofset;
            if (player.altFunctionUse != 2 && !player.controlUseTile) {
                return true;
            }
            if(use>5)return true;
            //Main.PlaySound(useSound, position);
            if(++use==5){
                int proj = Projectile.NewProjectile(source, position + velocity, velocity.RotatedByRandom(0.1), ProjectileID.CultistBossLightningOrbArc, damage, knockback, player.whoAmI, velocity.ToRotation(), Main.rand.NextFloat());
                Main.projectile[proj].friendly = true;
                Main.projectile[proj].hostile = false;
                Main.projectile[proj].timeLeft/=5;
                Main.projectile[proj].usesLocalNPCImmunity = true;
                Main.projectile[proj].localNPCHitCooldown = 4;
                Item.glowMask = glowmasks[5];
                SoundEngine.PlaySound(SoundID.Item38, position);
            }
            if(use<5)SoundEngine.PlaySound(SoundID.Item30.WithPitch(use/3f), position);//30 35
            return player.altFunctionUse!=2;
		}
        
    }
    public class LaserGuide : ModProjectile {
        public override string Texture => "Terraria/Images/Projectile_188";
        protected override bool CloneNewInstances => true;
        public bool notkil = true;
        /*public override void AI(){
            Player player = Main.player[projectile.owner];
            if(player.controlUseItem&&notkil)projectile.timeLeft = 2;
        }*/
        public override void SetDefaults()
        {
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.timeLeft = 3;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.netImportant = true;
        }
        public override bool PreKill(int timeLeft){
            notkil = false;
            return true;
        }
        public override bool ShouldUpdatePosition() => true;
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 endPoint = GetEndPoint();

            float drawRotation = Projectile.velocity.ToRotation();
            Vector2 drawOrigin = new Vector2(Projectile.width / 2, Projectile.height / 2);

            Main.EntitySpriteDraw(
                Mod.Assets.Request<Texture2D>("Projectiles/Laser").Value,
                Projectile.Center - Main.screenPosition,
                null,
                Color.Red,
                drawRotation,
                Vector2.Zero,
                new Vector2((endPoint - Projectile.Center).Length(), Projectile.width),
                SpriteEffects.None,
                0);

            return false;
        }


        private Vector2 GetEndPoint() {
            Vector2 pos = Projectile.Center;
            List<int> boosted = new List<int>(){};
            while ((pos - Projectile.Center).Length() < Main.screenWidth && Collision.CanHit(Projectile.Center, 1, 1, pos, 1, 1)) {
                pos += Projectile.velocity.SafeNormalize(Vector2.One);
                Lighting.AddLight(pos, 0.1f, 0, 0);
                for(int i = 0; i < Main.npc.Length; i++){
                    NPC npc = Main.npc[i];
                    if (npc.active && npc.Hitbox.Contains((int)pos.X, (int)pos.Y)){
                        return pos;
                    }
                }
                for(int i = 0; i < Main.player.Length; i++){
                    Player player = Main.player[i];
                    if (player.active && player.whoAmI != Projectile.owner && player.Hitbox.Contains((int)pos.X, (int)pos.Y)) {
                        return pos;
                    }
                }
                for(int i = 0; i < Main.projectile.Length; i++){
                    Projectile proj = Main.projectile[i];
                    if (proj.active && (proj.type == ProjectileID.CultistBossLightningOrbArc || proj.type == ProjectileID.VortexLightning)) {
                        Rectangle box = proj.Hitbox;
                        box.Inflate(32,32);
                        if(box.Contains((int)pos.X, (int)pos.Y)){
                            proj.ai[0] = ((pos+Projectile.velocity*3)-proj.Center).ToRotation();// - (Main.rand.NextFloat((float)Math.PI/2)-((float)Math.PI/4));
                            proj.velocity = Vector2.Lerp(proj.velocity,Projectile.velocity.SafeNormalize(proj.velocity) * 6, 0.01f);//*=1.0001f;
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