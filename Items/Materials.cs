using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Items{
    public class Sulfur : ModItem {
        public override void SetDefaults(){
            item.CloneDefaults(ItemID.ExplosivePowder);
        }
        public override void SetStaticDefaults(){
            DisplayName.SetDefault("Sulfur");
            Tooltip.SetDefault("Smells terrible");
        }
    }
    public class Charcoal : ModItem {
        public override void SetDefaults(){
            item.CloneDefaults(ItemID.ExplosivePowder);
            item.width = 16;
            item.height = 14;
        }
        public override void SetStaticDefaults(){
            DisplayName.SetDefault("Charcoal");
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("Wood");
			recipe.AddTile(TileID.Furnaces);
			recipe.SetResult(this);
			recipe.AddRecipe();
            
            recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("Wood");
			recipe.AddIngredient(ModContent.ItemType<Charcoal>());
			recipe.SetResult(ItemID.Torch, 5);
			recipe.AddRecipe();
            
            recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Niter>(), 7);
			recipe.AddIngredient(ModContent.ItemType<Charcoal>(), 2);
			recipe.AddIngredient(ModContent.ItemType<Sulfur>(), 1);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddTile(TileID.Furnaces);
			recipe.SetResult(ItemID.ExplosivePowder, 5);
			recipe.AddRecipe();
		}
    }
    public class Niter : ModItem {
        public override void SetDefaults(){
            item.CloneDefaults(ItemID.ExplosivePowder);
        }
        public override void SetStaticDefaults(){
            DisplayName.SetDefault("Niter");
        }
    }
}