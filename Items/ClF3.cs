using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Items {
    //day 9
    public class ClF3 : ModItem {
		public override bool CloneNewInstances => true;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("ClF3");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
            item.CloneDefaults(ItemID.ToxicFlask);
			item.damage = 45;
			item.thrown = true;
			item.noMelee = true;
			item.width = 56;
			item.height = 22;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 2;
			item.shoot = ModContent.ProjectileType<ClF3_P>();
			//item.shoot = ProjectileID.DD2FlameBurstTowerT1Shot;
            item.shootSpeed*=1.5f;
			item.autoReuse = true;
            item.noUseGraphic = true;
			item.maxStack = 999;
			item.consumable = true;
		}
		public override void AddRecipes(){
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FragmentSolar);
			recipe.AddIngredient(ItemID.CopperBar);
            recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this,2);
			recipe.AddRecipe();
		}
    }
    public class ClF3_P : ModProjectile {
        public override string Texture => "Artifice/Items/ClF3";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("ClF3");
		}
        public override void SetDefaults(){
            projectile.CloneDefaults(ProjectileID.ToxicFlask);
            projectile.width = 10;
            projectile.height = 10;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 15;
            aiType = ProjectileID.ToxicFlask;
        }
        public override bool PreKill(int timeLeft){
            if(projectile.ai[1]==0){
                Main.PlaySound(SoundID.Item107, projectile.Center);
                Gore.NewGore(projectile.Center, projectile.velocity/2, Artifice.gores[0], 1f);
                Gore.NewGore(projectile.Center, projectile.velocity/2, Artifice.gores[0], 1f);
                Gore.NewGore(projectile.Center, projectile.velocity/2, Artifice.gores[1], 1f);
                Gore.NewGore(projectile.Center, projectile.velocity/2, Artifice.gores[1], 1f);
                for (int j = 0; j < 10; j++){
                    if (Main.rand.Next(6) != 0){
                        Projectile dust4 = Projectile.NewProjectileDirect(projectile.Center, Vector2.Zero, projectile.type, projectile.damage, 0, projectile.owner, 0, 1);//226
                        dust4.velocity = projectile.velocity.RotatedByRandom(Math.PI*0.75f) * (0.5f + Main.rand.NextFloat(0.5f));
                        dust4.aiStyle = 14;
                        dust4.ignoreWater = true;
                        dust4.wet = false;
                        dust4.penetrate = -1;
                        dust4.timeLeft/=6;
                        dust4.modProjectile.aiType = ProjectileID.GreekFire3;
                    }
                }
            }
            return true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            return projectile.ai[1]==0;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
			damage+=(int)((target.defense-Math.Min(target.GetGlobalNPC<ArtificeGlobalNPC>().defreduc,target.defense)/2)*0.3f);
			target.GetGlobalNPC<ArtificeGlobalNPC>().defreduc+=10;
            target.buffImmune[BuffID.CursedInferno] = false;
            target.buffImmune[BuffID.Daybreak] = false;
            target.buffImmune[BuffID.OnFire] = false;
            target.AddBuff(BuffID.Daybreak, 600);
            target.AddBuff(BuffID.OnFire, 600);
            if(Main.rand.NextBool(4))Gore.NewGore(target.Center, projectile.velocity/2, Artifice.gores[0], 0.5f);
        }
    }
}