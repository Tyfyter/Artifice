using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Artifice.Items.Dye {

    public class GraphicsDebugger : ModItem {
        float f = 0f;
        double h = 0;
        public override string Texture => "Artifice/Items/Dye/SlantTopHalf";
        public override void SetStaticDefaults() {
		    DisplayName.SetDefault("Graphics Debugger");
		}

        public override void SetDefaults() {
            Item.damage = 60;
            Item.width = 24;
            Item.height = 28;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7.5f;
            Item.value = 1000;
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.channel = true;
            Item.shoot = ProjectileID.HeatRay;
            Item.shootSpeed = 7.5f;
            //Item.dye = 119;
        }

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		    player.itemTime = 0;
            if (player.controlUseItem) {
                player.itemAnimation = 8;
            }
            return false;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) {
            Artifice.distortMiscShader.Shader.Parameters["uOffset"].SetValue(new Vector2(0.5f));
            Vector3 color = new Vector3((float)(Math.Sin(f) * 0.5) + 0.5f, (float)(Math.Cos(f) * 0.5) + 0.5f, 1f);
            Artifice.distortMiscShader.UseColor(color);
            //Artifice.distortMiscShader.UseNonVanillaImage
            f += 0.01f;
            if (Math.Sin(f) < h) {
                h = Math.Sin(f);
            }

            Matrix matrix = Main.GameViewMatrix.EffectMatrix;

            matrix.Translation = new Vector3(Main.MouseScreen, 0);
            matrix.Right = new Vector3(8, 0, 0);
            matrix.Up = new Vector3(0, 8, 0);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, Main.Rasterizer, GameShaders.Armor.GetSecondaryShader(Item.dye, Main.LocalPlayer).Shader, matrix);

            DrawData data = new(Mod.Assets.Request<Texture2D>("Textures/40x40").Value, default, null, Color.White, 0, new Vector2(20, 20), 1f, SpriteEffects.None, 0);
            GameShaders.Armor.Apply(Item.dye, Item, data);
            data.Draw(spriteBatch);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.EffectMatrix);
        }
    }
}