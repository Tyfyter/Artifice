using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Artifice.Items.Dye;
using System;
using Artifice.Items;

namespace Artifice {
	/*ar·ti·fice
	/ˈärdəfəs/
	noun

	clever or cunning devices or expedients, especially as used to trick or deceive others.
	"the style is not free from the artifices of the period"
	*/
	public class Artifice : Mod {
		public static Mod instance;
        public static List<int> gores = new List<int>(){};
		public Artifice(){
			instance = this;
		}
        public override void Load(){
            gores.Add(ModGore.GoreCount);
            AddGore("Artifice/Items/ClF3_Gore1");
            gores.Add(ModGore.GoreCount);
            AddGore("Artifice/Items/ClF3_Gore2");
            //CustomEffect = GetEffect("Effects/CustomEffect");
            //LoadBasicColorDye(ItemID.RedDye, ModContent.ItemType<BlackandRedDye>(), ModContent.ItemType<RedandSilverDye>());
            //LoadBasicColorDye(ItemID.GreenDye, ModContent.ItemType<BlackandGreenDye>(), ModContent.ItemType<GreenandSilverDye>());
            //GameShaders.Armor.BindShader<ArmorShaderData>(ModContent.ItemType<WhiteandBlackDye>(), new SoftArmorShader(new Ref<Effect>(instance.GetEffect("Effects/ArmorShaders")), "ArmorInversePolarizedPass"));
            //GameShaders.Armor.BindShader<ArmorShaderData>(ModContent.ItemType<WhiteandBlackDye2>(), new SoftArmorShader(new Ref<Effect>(instance.GetEffect("Effects/ArmorShaders")), "ArmorInversePolarized2Pass"));
            base.Load();
        }
        public override void Unload(){
            instance = null;
            gores = new List<int>(){};
        }
        public static short SetGlowMask(string name)
        {
            if (!Main.dedServ)
            {
                Texture2D[] glowMasks = new Texture2D[Main.glowMaskTexture.Length + 1];
                for (int i = 0; i < Main.glowMaskTexture.Length; i++)
                {
                    glowMasks[i] = Main.glowMaskTexture[i];
                }
                glowMasks[glowMasks.Length - 1] = instance.GetTexture("Items/" + name);
                Main.glowMaskTexture = glowMasks;
                return (short)(glowMasks.Length - 1);
            }
            else return 0;
        }
        /*static void LoadBasicColorDye(int baseDyeItem, int blackDyeItem, int silverDyeItem, int oldShader = 1){
            Ref<Effect> pixelShaderRef = Main.PixelShaderRef;
            FieldInfo col = typeof(ArmorShaderData).GetField("_uColor", BindingFlags.NonPublic|BindingFlags.Instance);
            FieldInfo sat = typeof(ArmorShaderData).GetField("_uSaturation", BindingFlags.NonPublic|BindingFlags.Instance);
            ArmorShaderData bass = GameShaders.Armor.GetShaderFromItemId(baseDyeItem);
            //instance.Logger.Info(""+(col!=null));
            Vector3 c = (Vector3)col.GetValue(bass);
            float s = (float)sat.GetValue(bass);
            GameShaders.Armor.BindShader<ArmorShaderData>(blackDyeItem, new ArmorShaderData(new Ref<Effect>(instance.GetEffect("Effects/ArmorShaders")), "ColoredArmorInversePass")).UseColor(c).UseSaturation(s);
            GameShaders.Armor.BindShader<ArmorShaderData>(silverDyeItem, new ArmorShaderData(pixelShaderRef, "ArmorColoredAndSilverTrim")).UseColor(c).UseSaturation(s);
        }*/
	}
	public static class Extensions {
		public static Vector3 to3(this Vector2 input, float z = 0){
            return new Vector3(input.X,input.Y,z);
        }
        public static Vector2 to2(this Vector3 input){
            return new Vector2(input.X,input.Y);
        }
        public static Vector3 Rotate2D(this Vector3 input, double radians){
            return new Vector2(input.X,input.Y).RotatedBy(radians).to3(input.Z);
        }
        public static BoundingBox toBB(this Rectangle input, float thickness = 1){
            return BoundingBox.CreateFromPoints(new Vector3[]{new Vector3(input.Left,input.Top,-thickness),new Vector3(input.Right,input.Bottom,thickness)});
        }
	}
}
// always replace [\r\n\t\f\v]+\{ with {