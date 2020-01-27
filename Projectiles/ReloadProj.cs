using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Projectiles
{

    public class ReloadProj : ModProjectile
    {
        public override bool CloneNewInstances => true;
        public Action Reload = ()=>{};
        public Action<Projectile,ReloadTick> Tick = (p,t)=>{};
        ///<summary>
        ///an ordered list of integers
        ///</summary>
        public List<ReloadTick> ticks = new List<ReloadTick>(){};
        public override void SetDefaults()
        {
            //projectile.name = "Wind Shot";
            projectile.width = 64;
            projectile.height = 12;
            projectile.penetrate = -1;
            projectile.timeLeft = (int)projectile.ai[0];
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
        }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("\"If you see this I made a mistake\"");
		}
        public override void AI()
        {
            projectile.velocity = new Vector2();
            Player player = Main.player[projectile.owner];
            projectile.Center = player.MountedCenter - new Vector2(0, player.height*0.75f);
            if(projectile.timeLeft<=ticks[ticks.Count-1].pos){
                if(ticks[ticks.Count-1].alpha==1){
                    Tick(projectile, ticks[ticks.Count-1]);
                }
                ticks[ticks.Count-1].alpha-=1f/(ticks[ticks.Count-1].pos-ticks[ticks.Count-2].pos);
                if(ticks[ticks.Count-1].alpha<=0)ticks.RemoveAt(ticks.Count-1);
            }
            if(projectile.timeLeft <= 1){
                Reload();
                projectile.Kill();
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            projectile.rotation = 0;
            for(int i = 0; i < ticks.Count; i++){
                ReloadTick tick = ticks[i];
                spriteBatch.Draw(mod.GetTexture("Projectiles/ReloadProjTick"), new Vector2(projectile.position.X - Main.screenPosition.X+64-((tick.pos/(projectile.ai[0])*0.9f)*64), projectile.Center.Y - Main.screenPosition.Y),
					new Rectangle(0, 0, 2, 12), new Color(255,255,255,(int)(255*tick.alpha)), 0,
					new Vector2(1, 6), 2-tick.alpha, SpriteEffects.None, 0f);
            }
            lightColor = Color.White;
            return true;
        }
    }
    public class ReloadTick {
        public int pos;
        public float alpha = 1;
        public static implicit operator ReloadTick(int input){
            return new ReloadTick(){pos=input};
        }
    }
}