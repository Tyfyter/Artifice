using System.Collections.Generic;
using Artifice.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace Artifice {
    public class ArtificerPlayer : ModPlayer {
        public bool hasRC = false;
        public byte ShroomiteBoost = 0;
        public override void ResetEffects(){
            hasRC = false;
            ShroomiteBoost = 0;
        }
        public override void ModifyWeaponDamage(Item item, ref float add, ref float mult, ref float flat){
			if (ShroomiteBoost > 0 && (item.useAmmo != AmmoID.Bullet && item.useAmmo != AmmoID.Rocket && item.useAmmo != AmmoID.Arrow && (item.useAmmo != AmmoID.None || item.ranged))){
				if(ShroomiteBoost > 1)flat+=10;
                mult*=item.useAmmo != AmmoID.None?1.1f:1.20f;
			}
		}
        public override void ModifyDrawLayers(List<PlayerLayer> layers){
            if(player.HeldItem.type==ModContent.ItemType<AbSolution>())for(int i = 0; i < layers.Count; i++){
                if(layers[i].Name=="MiscEffectsBack"){
                    layers[i] = new PlayerLayer("Artifice", "AbsolutionLayer", (layer)=>{
                        Main.playerDrawData.Add(new DrawData(Main.BackPackTexture[2], new Vector2((float)((int)(layer.position.X - Main.screenPosition.X + (float)(layer.drawPlayer.width / 2) - (float)(9 * layer.drawPlayer.direction))) + -4 * (float)layer.drawPlayer.direction, (float)((int)(layer.position.Y - Main.screenPosition.Y + (float)(layer.drawPlayer.height / 2) + 2f * layer.drawPlayer.gravDir + -8 * layer.drawPlayer.gravDir))), new Rectangle?(new Rectangle(0, 0, Main.BackPackTexture[2].Width, Main.BackPackTexture[2].Height)), default(Color), layer.drawPlayer.bodyRotation, new Vector2((float)(Main.BackPackTexture[2].Width / 2), (float)(Main.BackPackTexture[2].Height / 2)), 1f, SpriteEffects.None, 0));
                    });
                    break;
                }
            }
        }
    }
}