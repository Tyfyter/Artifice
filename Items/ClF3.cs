using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Items {
    //day 9
    public class ClF3 : ModItem {
        protected override bool CloneNewInstances => true;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("ClF3");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults(){
            Item.CloneDefaults(ItemID.ToxicFlask);
			Item.damage = 45;
            Item.TryMakeExplosive(DamageClass.Throwing);
            Item.noMelee = true;
			Item.width = 56;
			Item.height = 22;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.shoot = ModContent.ProjectileType<ClF3_P>();
			//item.shoot = ProjectileID.DD2FlameBurstTowerT1Shot;
            Item.shootSpeed*=1.5f;
			Item.autoReuse = true;
            Item.noUseGraphic = true;
			Item.maxStack = 999;
			Item.consumable = true;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(Mod, "ArtificerBonus", "Thrown");
            line.OverrideColor = new Color(179, 50, 0);
            tooltips.Insert(1, line);
        }
		public override void AddRecipes() {
            Recipe recipe = CreateRecipe(2);
            recipe.AddIngredient(ItemID.FragmentSolar);
			recipe.AddIngredient(ItemID.CopperBar);
            recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
    }
    public class ClF3_P : ModProjectile {
        public override string Texture => "Artifice/Items/ClF3";
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("ClF3");
		}
        public override void SetDefaults(){
            Projectile.CloneDefaults(ProjectileID.ToxicFlask);
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            AIType = ProjectileID.ToxicFlask;
        }
        public override bool PreKill(int timeLeft){
            if(Projectile.ai[1]==0){
                SoundEngine.PlaySound(SoundID.Item107, Projectile.Center);
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity/2, Artifice.gores[0], 1f);
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity/2, Artifice.gores[0], 1f);
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity/2, Artifice.gores[1], 1f);
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity/2, Artifice.gores[1], 1f);
                for (int j = 0; j < 10; j++){
                    if (!Main.rand.NextBool(6)){
                        Projectile dust4 = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, Projectile.type, Projectile.damage, 0, Projectile.owner, 0, 1);//226
                        dust4.velocity = Projectile.velocity.RotatedByRandom(Math.PI*0.75f) * (0.5f + Main.rand.NextFloat(0.5f));
                        dust4.aiStyle = 14;
                        dust4.ignoreWater = true;
                        dust4.wet = false;
                        dust4.penetrate = -1;
                        dust4.timeLeft/=6;
                        dust4.ModProjectile.AIType = ProjectileID.GreekFire3;
                    }
                }
            }
            return true;
        }
        public override bool PreDraw(ref Color lightColor){
            return Projectile.ai[1]==0;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
			damage+=(int)((target.defense-Math.Min(target.GetGlobalNPC<ArtificeGlobalNPC>().defreduc,target.defense)/2)*0.3f);
			target.GetGlobalNPC<ArtificeGlobalNPC>().defreduc+=10;
            target.buffImmune[BuffID.CursedInferno] = false;
            target.buffImmune[BuffID.Daybreak] = false;
            target.buffImmune[BuffID.OnFire] = false;
            target.AddBuff(BuffID.Daybreak, 600);
            target.AddBuff(BuffID.OnFire, 600);
            if(Main.rand.NextBool(4))Gore.NewGore(Projectile.GetSource_FromThis(), target.Center, Projectile.velocity/2, Artifice.gores[0], 0.5f);
        }
    }
}