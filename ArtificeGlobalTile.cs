using Artifice.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice {
    public class ArtificeGlobalTile : GlobalTile {
        public override bool Drop(int i, int j, int type){
            if(type == TileID.Stalactite && Main.rand.NextBool(7)){
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), new Vector2(i, j)*16, new Vector2(16, 16), ModContent.ItemType<Niter>());
                return true;
            }
            if(type == TileID.Ash && Main.rand.NextBool(19)){
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), new Vector2(i, j)*16, new Vector2(16, 16), ModContent.ItemType<Sulfur>());
                return true;
            }
            return true;
        }
    }
}