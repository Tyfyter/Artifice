using Artifice.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice {
    public class ArtificeGlobalNPC : GlobalNPC {
        public override void NPCLoot(NPC npc){
            if(npc.Center.Y > (float)((Main.maxTilesY - 200) * 16) && Main.rand.Next(11) == 0){
                Item.NewItem(npc.position, new Vector2(npc.width, npc.height), ModContent.ItemType<Sulfur>());
            }
        }
    }
}