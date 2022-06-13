using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Projectiles{

    public class ReloadProj : ModProjectile
    {
        protected override bool CloneNewInstances => true;
        public Action Reload = ()=>{};
        public Action<Projectile,ReloadTick> Tick = (p,t)=>{};
        ///<summary>
        ///an ordered list of integers
        ///</summary>
        public List<ReloadTick> ticks = new List<ReloadTick>(){};
        int lastTickPos = 0;
        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 12;
            Projectile.penetrate = -1;
            Projectile.aiStyle = 0;
            Projectile.MaxUpdates = 1;
            Projectile.extraUpdates = 0;
            Projectile.tileCollide = false;
        }
		public override void OnSpawn(IEntitySource source) {
            Projectile.timeLeft = (int)Projectile.ai[0];
        }
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("\"If you see this I made a mistake\"");
		}
        public override void AI()
        {
            Projectile.velocity = new Vector2();
            Player player = Main.player[Projectile.owner];
            Projectile.Center = player.MountedCenter - new Vector2(0, player.height*0.75f);
            if(ticks.Count > 0 && Projectile.timeLeft <= ticks[^1].pos){
                if(ticks[^1].alpha==1){
                    Tick(Projectile, ticks[^1]);
                }
                ticks[^1].alpha-=1f/(ticks[^1].pos - lastTickPos);
                lastTickPos = ticks[^1].pos;
                if (ticks[^1].alpha<=0)ticks.RemoveAt(ticks.Count-1);
            }
            if(Projectile.timeLeft <= 1){
                Reload();
                Projectile.Kill();
            }
            player.GetModPlayer<ArtificerPlayer>().reloadTime = 2;
        }
        public override bool PreDraw(ref Color lightColor){
            Projectile.rotation = 0;
            for(int i = 0; i < ticks.Count; i++){
                ReloadTick tick = ticks[i];
                Main.EntitySpriteDraw(Mod.Assets.Request<Texture2D>("Projectiles/ReloadProjTick").Value,
                    new Vector2(Projectile.position.X - Main.screenPosition.X+64-((tick.pos/(Projectile.ai[0])*0.9f)*64), Projectile.Center.Y - Main.screenPosition.Y),
					new Rectangle(0, 0, 2, 12), new Color(255,255,255,(int)(255*tick.alpha)), 0,
					new Vector2(1, 6), 2-tick.alpha, SpriteEffects.None, 0);
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