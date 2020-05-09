using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Items {
	//update 2, day 5
	public class Glass_Cannon : ModItem {
		public override bool CloneNewInstances => true;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Glass Cannon");
		}
		public override void SetDefaults(){
			item.CloneDefaults(ItemID.Handgun);
			item.UseSound = null;
			item.damage = 67;
			item.ranged = true;
			item.noMelee = true;
			item.width = 34;
			item.height = 16;
			item.useTime = 22;
			item.useAnimation = 22;
			//item.useStyle = 5;
			item.knockBack*=2;
			item.value*=2;
			item.rare = ItemRarityID.Yellow;
			item.shoot = ModContent.ProjectileType<Glass_Shot>();
			item.useAmmo = ItemID.Glass;
			item.autoReuse = false;
			//item.shoot = ProjectileID.DD2FlameBurstTowerT1Shot;
			//item.shootSpeed = 12.5f;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(mod, "ArtificerBonus", "Ranged:Glass");
            line.overrideColor = new Color(179, 50, 0);
            tooltips.Insert(1, line);
        }
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 38, pitchOffset:0.15f);
			Main.PlaySound(13, (int)player.Center.X, (int)player.Center.Y, 0, pitchOffset:Main.rand.NextFloat()*0.3f-0.15f);
			Vector2 speed = new Vector2(speedX,speedY);
			int t = ModContent.ProjectileType<Glass_Shot>();
			//Main.NewText("type:"+type+" amount:"+a);
			Projectile.NewProjectile(position, speed, t, damage, knockBack, item.owner, type);//damage/(a/8+1)
			if(player.GetModPlayer<ArtificerPlayer>().ShroomiteBoost!=0){
				int a = Main.rand.Next(3);
				for(int i = 1; i <= a; i++){
					Projectile.NewProjectileDirect(position, speed.RotatedBy(((a/2d+1)/a)*i/8d)/(i+1), t, damage/(a+1), knockBack, item.owner, type, i).timeLeft/=((a+i)/32+1);
					Projectile.NewProjectileDirect(position, speed.RotatedBy(-((a/2d+1)/a)*i/8d)/(i+1), t, damage/(a+1), knockBack, item.owner, type, i).timeLeft/=((a+i)/32+1);
				}
			}
			return false;
		}
		/*public override Vector2? HoldoutOffset(){
			return new Vector2(-6, 0);
		}*/
		public override void AddRecipes(){
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<RedShroomite>(), 18);
			recipe.AddTile(TileID.Autohammer);
			recipe.AddTile(TileID.GlassKiln);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class Glass_Shot : ModProjectile{
        public override string Texture => "Terraria/Item_4";
		public override void SetDefaults(){
			projectile.CloneDefaults(ProjectileID.Bullet);
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
			projectile.extraUpdates+=4;
			projectile.penetrate = -1;
			projectile.aiStyle = 0;
		}
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Glass Cannon");
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
			switch ((int)projectile.ai[0]){
				case 1:
				target.velocity+=projectile.velocity*Math.Max(target.knockBackResist-0.1f, 0);
				break;
				case 2:
				target.AddBuff(BuffID.OnFire, 360);
				break;
				case 3:
				Main.player[projectile.owner].AddBuff(BuffID.RapidHealing, 60);
				Main.player[projectile.owner].AddBuff(BuffID.Honey, 240);
				break;
				case 5:
				target.AddBuff(BuffID.Oiled, 600);
				break;
				case 6:
				target.velocity+=projectile.velocity*(target.knockBackResist+0.1f);
				break;
				case 7:
				target.AddBuff(BuffID.Frostburn, 600);
				break;
				default:
				break;
			}
		}
		public override void OnHitPlayer(Player target, int damage, bool crit){
			switch ((int)projectile.ai[0]){
				case 1:
				target.velocity+=projectile.velocity*0.5f;
				break;
				case 2:
				target.AddBuff(BuffID.OnFire, 180);
				break;
				case 3:
				Main.player[projectile.owner].AddBuff(BuffID.RapidHealing, 30);
				Main.player[projectile.owner].AddBuff(BuffID.Honey, 180);
				break;
				case 5:
				target.AddBuff(BuffID.Oiled, 300);
				break;
				case 6:
				target.velocity+=projectile.velocity;
				break;
				case 7:
				target.AddBuff(BuffID.Frostburn, 300);
				break;
				default:
				break;
			}
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
			if(projectile.ai[0]<InfiniteGlass.glasstypes.Length){
				Dust.NewDustDirect(projectile.Center-Vector2.One,2,2, 13, projectile.velocity.X, projectile.velocity.Y, Scale:0.75f).noGravity = true;
				Dust.NewDustDirect(projectile.Center-(projectile.velocity/2)-Vector2.One,2,2, projectile.ai[0]==7?80:13, projectile.velocity.X, projectile.velocity.Y, Scale:0.75f).noGravity = true;
				int[] types = InfiniteGlass.glasstypes[(int)projectile.ai[0]];
				foreach (int i in types){
					int t = i;
					Color c = default(Color);
					if(t==DustID.Confetti)t+=Main.rand.Next(5);
					else if(t==DustID.Smoke)c = Color.Black;
					Dust.NewDustDirect(projectile.Center-Vector2.One,2,2, t, projectile.velocity.X, projectile.velocity.Y, newColor:c);
					Dust.NewDustDirect(projectile.Center-(projectile.velocity/2)-Vector2.One,2,2, t, projectile.velocity.X, projectile.velocity.Y);
				}
			}
            return false;
        }
	}
	public class InfiniteGlass : ModItem {
        public override string Texture => "Terraria/Item_"+type;
		public int type = ItemID.Glass;
		public InfiniteGlass(){}
		InfiniteGlass(int t) : base(){
			type = t;
		}
		public static readonly int[][] glasstypes = new int[][]{
			new int[]{}, 
			new int[]{33,33}, 
			new int[]{6,6}, 
			new int[]{102,102},
			new int[]{DustID.Confetti,DustID.Confetti}, 
			new int[]{DustID.Confetti,DustID.Confetti,DustID.Smoke},
			new int[]{32,32},
			new int[]{92},
			new int[]{86},
			new int[]{87},
			new int[]{88},
			new int[]{89},
			new int[]{90},
			new int[]{91},
			new int[]{262}
		};
		public override bool CloneNewInstances => true;
        public override bool Autoload(ref string name){
			if(ModLoader.GetMod("Infinity")!=null){
				mod.Logger.Info("Loading Unlimited Glass");
                mod.AddItem("InfiniteGlass", new InfiniteGlass(ItemID.Glass));
                mod.AddItem("InfiniteGemsparkAmethyst", new InfiniteGlass(ItemID.AmethystGemsparkBlock));
                mod.AddItem("InfiniteGemsparkTopaz", new InfiniteGlass(ItemID.TopazGemsparkBlock));
                mod.AddItem("InfiniteGemsparkSapphire", new InfiniteGlass(ItemID.SapphireGemsparkBlock));
                mod.AddItem("InfiniteGemsparkEmerald", new InfiniteGlass(ItemID.EmeraldGemsparkBlock));
                mod.AddItem("InfiniteGemsparkRuby", new InfiniteGlass(ItemID.RubyGemsparkBlock));
                mod.AddItem("InfiniteGemsparkDiamond", new InfiniteGlass(ItemID.DiamondGemsparkBlock));
                mod.AddItem("InfiniteGemsparkAmber", new InfiniteGlass(ItemID.AmberGemsparkBlock));
                mod.AddItem("InfiniteWaterfall", new InfiniteGlass(ItemID.WaterfallBlock));
                mod.AddItem("InfiniteLavafall", new InfiniteGlass(ItemID.LavafallBlock));
                mod.AddItem("InfiniteHoneyfall", new InfiniteGlass(ItemID.HoneyfallBlock));
                mod.AddItem("InfiniteConfetti", new InfiniteGlass(ItemID.ConfettiBlock));
                mod.AddItem("InfiniteConfettiBlack", new InfiniteGlass(ItemID.ConfettiBlockBlack));
                mod.AddItem("InfiniteSandFall", new InfiniteGlass(ItemID.SandFallBlock));
                mod.AddItem("InfiniteSnowFall", new InfiniteGlass(ItemID.SnowFallBlock));
            } else mod.Logger.Info("Not loading Unlimited Glass");
            return false;
        }
		public override void SetDefaults(){
			item.CloneDefaults(type);
			item.useStyle = 0;
			item.maxStack = 1;
			item.createTile = -1;
			item.consumable = false;
		}
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Unlimited "+Lang.GetItemNameValue(type).Replace(" Block",""));
		}
		public override void AddRecipes(){
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(type, 3996);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
		}
	}
}
