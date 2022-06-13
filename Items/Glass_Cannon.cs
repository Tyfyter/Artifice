using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Artifice.Items {
	//update 2, day 5
	public class Glass_Cannon : ModItem {
		protected override bool CloneNewInstances => true;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Glass Cannon");
			SacrificeTotal = 1;
		}
		public override void SetDefaults(){
			Item.CloneDefaults(ItemID.Handgun);
			Item.UseSound = null;
			Item.damage = 127;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.width = 34;
			Item.height = 16;
			Item.useTime = 22;
			Item.useAnimation = 22;
			//item.useStyle = 5;
			Item.knockBack*=2;
			Item.value*=2;
			Item.rare = ItemRarityID.Yellow;
			Item.shoot = ModContent.ProjectileType<Glass_Shot>();
			Item.useAmmo = ItemID.Glass;
			Item.autoReuse = false;
			//item.shoot = ProjectileID.DD2FlameBurstTowerT1Shot;
			//item.shootSpeed = 12.5f;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips){
            TooltipLine line = new TooltipLine(Mod, "ArtificerBonus", "Ranged:Glass");
            line.OverrideColor = new Color(179, 50, 0);
            tooltips.Insert(1, line);
        }
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			SoundEngine.PlaySound(SoundID.Item38.WithPitch(0.15f), position);
			SoundEngine.PlaySound(SoundID.Shatter.WithPitchRange(-0.15f, 0.15f), position);
			Vector2 speed = velocity;
			int t = ModContent.ProjectileType<Glass_Shot>();
			//Main.NewText("type:"+type+" amount:"+a);
			Projectile.NewProjectile(source, position, speed, t, damage, knockback, player.whoAmI, type);//damage/(a/8+1)
			if(player.GetModPlayer<ArtificerPlayer>().ShroomiteBoost!=0){
				int a = Main.rand.Next(3);
				for(int i = 1; i <= a; i++){
					Projectile.NewProjectileDirect(source, position, speed.RotatedBy(((a/2d+1)/a)*i/8d)/(i+1), t, damage/(a+1), knockback, player.whoAmI, type, i).timeLeft/=((a+i)/32+1);
					Projectile.NewProjectileDirect(source, position, speed.RotatedBy(-((a/2d+1)/a)*i/8d)/(i+1), t, damage/(a+1), knockback, player.whoAmI, type, i).timeLeft/=((a+i)/32+1);
				}
			}
			return false;
		}
		/*public override Vector2? HoldoutOffset(){
			return new Vector2(-6, 0);
		}*/
		public override void AddRecipes() {
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<RedShroomite>(), 18);
			recipe.AddTile(TileID.Autohammer);
			recipe.AddTile(TileID.GlassKiln);
			recipe.Register();
		}
		public override void Load() {
			if (ModLoader.HasMod("Luiafk") || ModLoader.HasMod("Infinity")) {
				Mod.Logger.Info("Loading Unlimited Glass");
				Mod.AddContent(new InfiniteGlass(ItemID.Glass, "InfiniteGlass"));
				Mod.AddContent(new InfiniteGlass(ItemID.AmethystGemsparkBlock, "InfiniteGemsparkAmethyst"));
				Mod.AddContent(new InfiniteGlass(ItemID.TopazGemsparkBlock, "InfiniteGemsparkTopaz"));
				Mod.AddContent(new InfiniteGlass(ItemID.SapphireGemsparkBlock, "InfiniteGemsparkSapphire"));
				Mod.AddContent(new InfiniteGlass(ItemID.EmeraldGemsparkBlock, "InfiniteGemsparkEmerald"));
				Mod.AddContent(new InfiniteGlass(ItemID.RubyGemsparkBlock, "InfiniteGemsparkRuby"));
				Mod.AddContent(new InfiniteGlass(ItemID.DiamondGemsparkBlock, "InfiniteGemsparkDiamond"));
				Mod.AddContent(new InfiniteGlass(ItemID.AmberGemsparkBlock, "InfiniteGemsparkAmber"));
				Mod.AddContent(new InfiniteGlass(ItemID.WaterfallBlock, "InfiniteWaterfall"));
				Mod.AddContent(new InfiniteGlass(ItemID.LavafallBlock, "InfiniteLavafall"));
				Mod.AddContent(new InfiniteGlass(ItemID.HoneyfallBlock, "InfiniteHoneyfall"));
				Mod.AddContent(new InfiniteGlass(ItemID.ConfettiBlock, "InfiniteConfetti"));
				Mod.AddContent(new InfiniteGlass(ItemID.ConfettiBlockBlack, "InfiniteConfettiBlack"));
				Mod.AddContent(new InfiniteGlass(ItemID.SandFallBlock, "InfiniteSandFall"));
				Mod.AddContent(new InfiniteGlass(ItemID.SnowFallBlock, "InfiniteSnowFall"));
			} else Mod.Logger.Info("Not loading Unlimited Glass");
		}
	}
	public class Glass_Shot : ModProjectile{
        public override string Texture => "Terraria/Images/Item_4";
		public override void SetDefaults(){
			Projectile.CloneDefaults(ProjectileID.Bullet);
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.extraUpdates+=4;
			Projectile.penetrate = -1;
			Projectile.aiStyle = 0;
		}
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Glass Cannon");
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
			if (target.type != NPCID.TargetDummy) switch ((int)Projectile.ai[0]){
				case 1:
				target.velocity+=Projectile.velocity*Math.Max(target.knockBackResist-0.1f, 0);
				break;
				case 2:
				target.AddBuff(BuffID.OnFire, 360);
				break;
				case 3:
				Main.player[Projectile.owner].AddBuff(BuffID.RapidHealing, 60);
				Main.player[Projectile.owner].AddBuff(BuffID.Honey, 240);
				break;
				case 5:
				target.AddBuff(BuffID.Oiled, 600);
				break;
				case 6:
				target.velocity+=Projectile.velocity*(target.knockBackResist+0.1f);
				break;
				case 7:
				target.AddBuff(BuffID.Frostburn, 600);
				break;
				default:
				break;
			}
		}
		public override void OnHitPlayer(Player target, int damage, bool crit){
			switch ((int)Projectile.ai[0]){
				case 1:
				target.velocity+=Projectile.velocity*0.5f;
				break;
				case 2:
				target.AddBuff(BuffID.OnFire, 180);
				break;
				case 3:
				Main.player[Projectile.owner].AddBuff(BuffID.RapidHealing, 30);
				Main.player[Projectile.owner].AddBuff(BuffID.Honey, 180);
				break;
				case 5:
				target.AddBuff(BuffID.Oiled, 300);
				break;
				case 6:
				target.velocity+=Projectile.velocity;
				break;
				case 7:
				target.AddBuff(BuffID.Frostburn, 300);
				break;
				default:
				break;
			}
		}
        public override bool PreDraw(ref Color lightColor){
			if(Projectile.ai[0]<InfiniteGlass.glassTypes.Length){
				Dust.NewDustDirect(Projectile.Center-Vector2.One,2,2, DustID.Glass, Projectile.velocity.X, Projectile.velocity.Y, Scale:0.75f).noGravity = true;
				Dust.NewDustDirect(Projectile.Center-(Projectile.velocity/2)-Vector2.One,2,2, Projectile.ai[0]==7?80:13, Projectile.velocity.X, Projectile.velocity.Y, Scale:0.75f).noGravity = true;
				int[] types = InfiniteGlass.glassTypes[(int)Projectile.ai[0]];
				foreach (int i in types){
					int t = i;
					Color c = default(Color);
					if(t==DustID.Confetti)t+=Main.rand.Next(5);
					else if(t==DustID.Smoke)c = Color.Black;
					Dust.NewDustDirect(Projectile.Center-Vector2.One,2,2, t, Projectile.velocity.X, Projectile.velocity.Y, newColor:c);
					Dust.NewDustDirect(Projectile.Center-(Projectile.velocity/2)-Vector2.One,2,2, t, Projectile.velocity.X, Projectile.velocity.Y);
				}
			}
            return false;
        }
	}
	[Autoload(false)]
	public class InfiniteGlass : ModItem {
        public override string Texture => "Terraria/Images/Item_" + type;
		public int type { get; internal set; } = ItemID.Glass;
		readonly string name;
		public override string Name => name;
		public InfiniteGlass(){}
		internal InfiniteGlass(int t, string name) : base(){
			type = t;
			this.name = name;
		}
		public static readonly int[][] glassTypes = new int[][]{
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
		protected override bool CloneNewInstances => true;
		public override void SetDefaults(){
			Item.CloneDefaults(type);
			Item.useStyle = ItemUseStyleID.None;
			Item.maxStack = 1;
			Item.createTile = -1;
			Item.consumable = false;
		}
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Unlimited "+Lang.GetItemNameValue(type).Replace(" Block",""));
			SacrificeTotal = 1;
		}
		public override void AddRecipes() {
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(type, 3996);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
		}
	}
}
