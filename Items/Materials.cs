using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Items{
    public class Sulfur : ModItem {
        public override void SetDefaults(){
            Item.CloneDefaults(ItemID.ExplosivePowder);
        }
        public override void SetStaticDefaults(){
            DisplayName.SetDefault("Sulfur");
            Tooltip.SetDefault("Smells terrible");
            SacrificeTotal = 25;
        }
    }
    public class Charcoal : ModItem {
        public override void SetDefaults(){
            Item.CloneDefaults(ItemID.ExplosivePowder);
            Item.width = 16;
            Item.height = 14;
        }
        public override void SetStaticDefaults(){
            DisplayName.SetDefault("Charcoal");
            SacrificeTotal = 25;
        }
		public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("Wood");
			recipe.AddTile(TileID.Furnaces);
			recipe.Register();

            recipe = Recipe.Create(ItemID.Torch, 5);
            recipe.AddRecipeGroup("Wood");
			recipe.AddIngredient(ModContent.ItemType<Charcoal>());
			recipe.Register();

            recipe = Recipe.Create(ItemID.ExplosivePowder, 5);
            recipe.AddIngredient(ModContent.ItemType<Niter>(), 7);
			recipe.AddIngredient(ModContent.ItemType<Charcoal>(), 2);
			recipe.AddIngredient(ModContent.ItemType<Sulfur>(), 1);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddTile(TileID.Furnaces);
			recipe.Register();
		}
    }
    public class Niter : ModItem {
        public override void SetDefaults(){
            Item.CloneDefaults(ItemID.ExplosivePowder);
        }
        public override void SetStaticDefaults(){
            DisplayName.SetDefault("Niter");
            SacrificeTotal = 25;
        }
    }
    public class RedShroomite : ModItem {
        public override string Texture => "Terraria/Images/Item_1552";
        public override void SetDefaults(){
            Item.CloneDefaults(ItemID.ShroomiteBar);
            Item.color = Color.Red;
        }
        public override void SetStaticDefaults(){
            DisplayName.SetDefault("Red Shroomite");
            SacrificeTotal = 25;
        }
		public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ShroomiteBar);
            recipe.AddIngredient(ItemID.AdamantiteBar);
            recipe.AddIngredient(ItemID.Ruby);
			recipe.AddTile(TileID.Autohammer);
			recipe.AddTile(TileID.GlassKiln);
			recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ShroomiteBar);
            recipe.AddIngredient(ItemID.TitaniumBar);
            recipe.AddIngredient(ItemID.Ruby);
			recipe.AddTile(TileID.Autohammer);
			recipe.AddTile(TileID.GlassKiln);
			recipe.Register();
		}
    }
}