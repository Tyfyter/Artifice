using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Artifice {
	/*ar·ti·fice
	/ˈärdəfəs/
	noun

	clever or cunning devices or expedients, especially as used to trick or deceive others.
	"the style is not free from the artifices of the period"
	*/
	public class Artifice : Mod {
		static Mod instance;
		public Artifice(){
			instance = this;
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
	}
	public static class Extensions {
		public static Vector3 to3(this Vector2 input, float z = 0){
            return new Vector3(input.X,input.Y,z);
        }
        public static Vector2 to2(this Vector3 input){
            return new Vector2(input.X,input.Y);
        }
        public static BoundingBox toBB(this Rectangle input){
            return BoundingBox.CreateFromPoints(new Vector3[]{new Vector3(input.Left,input.Top,-1),new Vector3(input.Right,input.Bottom,1)});
        }
	}
}
