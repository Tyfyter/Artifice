using System.Collections.Generic;
using Artifice.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Artifice {
    public class ArtificerPlayer : ModPlayer {
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