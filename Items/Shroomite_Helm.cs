using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Items {
	//update 2, day 2
	[AutoloadEquip(EquipType.Head)]
    public class Shroomite_Helm : ModItem {
        public override void SetStaticDefaults(){
            DisplayName.SetDefault("Shroomite Helm");
            Tooltip.SetDefault("20% increased unique ranged damage");
            SacrificeTotal = 1;
        }
        public override void SetDefaults(){
            Item.width = 18;
            Item.height = 20;
            Item.value = 375000;
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 11;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(Mod, "ArtificerBonus", "Ranged");
            line.OverrideColor = new Color(179, 50, 0);
            tooltips.Insert(1, line);
            tooltips.RemoveAll((tl)=>tl.Name == "Material");
        }

        public override bool IsArmorSet(Item head, Item body, Item legs){
            return body.type == ItemID.ShroomiteBreastplate && legs.type == ItemID.ShroomiteLeggings;
        }

        public override void UpdateArmorSet(Player player){
            player.shroomiteStealth = true;
            player.GetModPlayer<ArtificerPlayer>().ShroomiteBoost|=2;
            player.setBonus = "+10 unique ranged damage\nNot moving puts you in stealth,\nincreasing ranged ability and reducing chance for enemies to target you";
        }

        public override void UpdateEquip(Player player){
            player.GetModPlayer<ArtificerPlayer>().ShroomiteBoost|=1;
        }

        public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ShroomiteBar, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
            /*recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Shroomite_Helm_NoVisor>(), 1);
            recipe.Register();*/
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Shroomite_Helm_BlackVisor>(), 1);
            recipe.Register();
        }
    }
    /*[AutoloadEquip(EquipType.Head)]
    public class Shroomite_Helm_NoVisor : Shroomite_Helm {
        public override void SetStaticDefaults(){
            DisplayName.SetDefault("Shroomite Helm (No Visor)");
            Tooltip.SetDefault("20% increased unique ranged damage");
        }
        public override void AddRecipes(){
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Shroomite_Helm>(), 1);
            recipe.Register();
			recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Shroomite_Helm_BlackVisor>(), 1);
            recipe.Register();
        }
    }*/
    [AutoloadEquip(EquipType.Head)]
    public class Shroomite_Helm_BlackVisor : Shroomite_Helm {
        public override void SetStaticDefaults(){
            DisplayName.SetDefault("Shroomite Helm (Black Visor)");
            Tooltip.SetDefault("20% increased unique ranged damage");
            SacrificeTotal = 1;
        }
        public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Shroomite_Helm>(), 1);
            recipe.Register();
            /*recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Shroomite_Helm_NoVisor>(), 1);
            recipe.Register();*/
        }
    }
}