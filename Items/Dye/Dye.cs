using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Items.Dye {
	//update 2, days 2-3
    public class BlackandRedDye : SimpleColoredDye {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Black and Red Dye");
        }
    }
    public class RedandSilverDye : SimpleColoredDye {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Red and Silver Dye");
        }
    }
    public class BlackandGreenDye : SimpleColoredDye {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Black and Green Dye");
        }
    }
    public class GreenandSilverDye : SimpleColoredDye {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Green and Silver Dye");
        }
    }
    public class WhiteandBlackDye : SimpleColoredDye {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("White and Black Dye");
        }
    }
    public class WhiteandBlackDye2 : SimpleColoredDye {
        public override string Texture => "Artifice/Items/Dye/WhiteandBlackDye";
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("White and Black Dye 2");
        }
    }
    [Autoload(false)]
	public class SimpleColoredDye : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Dye");
        }
		public override void SetDefaults() {
			int dye = Item.dye;
			Item.CloneDefaults(ItemID.RedandBlackDye);
			Item.dye = dye;
		}
	}
    
    public class SoftArmorShader : ArmorShaderData {
        public bool thrown = false;
        public SoftArmorShader(Ref<Effect> shader, string passName) : base(shader, passName) {}
        public override void Apply(Entity entity, DrawData? drawData) {
            try {
                base.Apply(entity, drawData);
            }catch (System.Exception e) {
                if(thrown) {
                    Main.NewTextMultiline(e.Message);
                    Artifice.instance.Logger.Warn(e.ToString());
                    thrown = true;
                }
            }

        }
    }
}