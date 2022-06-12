using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Artifice.Items.Dye;
using System;
using Artifice.Items;
using System.Diagnostics.CodeAnalysis;
using System.Collections;
using ReLogic.Content;
using Terraria.Audio;

namespace Artifice {
	/*
	ar·ti·fice
	/ˈärdəfəs/
	noun

	clever or cunning devices or expedients, especially as used to trick or deceive others.
	"the style is not free from the artifices of the period"
	*/
	public class Artifice : Mod {
		public static Mod instance;
        public static List<int> gores;
        internal static IDictionary<DamageClass, DamageClass> explosiveDamageClasses;
		public Artifice(){
			instance = this;
            explosiveDamageClasses = new MirrorDictionary<DamageClass>();
        }
        public override void Load(){
            gores = new();
            //gores.Add(ModGore.GoreCount);
            //AddGore("Artifice/Items/ClF3_Gore1");
            //gores.Add(ModGore.GoreCount);
            //AddGore("Artifice/Items/ClF3_Gore2");
            //CustomEffect = GetEffect("Effects/CustomEffect");
            //LoadBasicColorDye(ItemID.RedDye, ModContent.ItemType<BlackandRedDye>(), ModContent.ItemType<RedandSilverDye>());
            //LoadBasicColorDye(ItemID.GreenDye, ModContent.ItemType<BlackandGreenDye>(), ModContent.ItemType<GreenandSilverDye>());
            //GameShaders.Armor.BindShader<ArmorShaderData>(ModContent.ItemType<WhiteandBlackDye>(), new SoftArmorShader(new Ref<Effect>(instance.GetEffect("Effects/ArmorShaders")), "ArmorInversePolarizedPass"));
            //GameShaders.Armor.BindShader<ArmorShaderData>(ModContent.ItemType<WhiteandBlackDye2>(), new SoftArmorShader(new Ref<Effect>(instance.GetEffect("Effects/ArmorShaders")), "ArmorInversePolarized2Pass"));
            
        }
        public override void Unload(){
            instance = null;
            gores = null;
            explosiveDamageClasses = null;
        }
        public static short SetGlowMask(string name)
        {
            if (Main.netMode!=NetmodeID.Server)
            {
                Asset<Texture2D>[] glowMasks = new Asset<Texture2D>[TextureAssets.GlowMask.Length + 1];
                for (int i = 0; i < TextureAssets.GlowMask.Length; i++) {
                    glowMasks[i] = TextureAssets.GlowMask[i];
                }
                glowMasks[glowMasks.Length - 1] = instance.Assets.Request<Texture2D>("Items/" + name);
                TextureAssets.GlowMask = glowMasks;
                return (short)(glowMasks.Length - 1);
            }
            else return 0;
        }
        public override void PostAddRecipes() {
            try {
                if (ModLoader.TryGetMod("Origins", out Mod origins)) {
					if (origins.Call("get_explosive_classes_dict") is IDictionary<DamageClass, DamageClass> dict) {
                        explosiveDamageClasses = dict;
					}
                    //ArtificeXModCompat.OriginsExplosiveRegister();
                }
			} catch (Exception) { }
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
    public struct AutoCastingAsset<T> where T : class {
        public bool HasValue => asset is not null;
        public bool IsLoaded => asset?.IsLoaded ?? false;
        public T Value => asset.Value;

        readonly Asset<T> asset;
        AutoCastingAsset(Asset<T> asset) {
            this.asset = asset;
        }
        public static implicit operator AutoCastingAsset<T>(Asset<T> asset) => new(asset);
        public static implicit operator T(AutoCastingAsset<T> asset) => asset.Value;
    }
    public static class Extensions {
		public static Vector3 To3(this Vector2 input, float z = 0){
            return new Vector3(input.X,input.Y,z);
        }
        public static Vector2 To2(this Vector3 input){
            return new Vector2(input.X,input.Y);
        }
        public static Vector3 Rotate2D(this Vector3 input, double radians){
            return new Vector2(input.X,input.Y).RotatedBy(radians).To3(input.Z);
        }
        public static BoundingBox ToBB(this Rectangle input, float thickness = 1){
            return BoundingBox.CreateFromPoints(new Vector3[]{new Vector3(input.Left,input.Top,-thickness),new Vector3(input.Right,input.Bottom,thickness)});
        }
        public static void TryMakeExplosive(this Item item, DamageClass damageClass) {
			if (Artifice.explosiveDamageClasses.TryGetValue(damageClass, out DamageClass damageType)) {
                item.DamageType = damageType;
			} else {
                item.DamageType = damageClass;
			}
        }
        public static void TryMakeExplosive(this Projectile projectile, DamageClass damageClass) {
            if (Artifice.explosiveDamageClasses.TryGetValue(damageClass, out DamageClass damageType)) {
                projectile.DamageType = damageType;
            } else {
                projectile.DamageType = damageClass;
            }
        }
        public static AutoCastingAsset<Texture2D> RequestTexture(this Mod mod, string name) => mod.Assets.Request<Texture2D>(name);
        public static SoundStyle WithPitch(this SoundStyle soundStyle, float pitch) {
            soundStyle.Pitch = pitch;
            return soundStyle;
        }
        public static SoundStyle WithPitchVarience(this SoundStyle soundStyle, float pitchVarience) {
            soundStyle.PitchVariance = pitchVarience;
            return soundStyle;
        }
        public static SoundStyle WithPitchRange(this SoundStyle soundStyle, float min, float max) {
            soundStyle.PitchRange = (min, max);
            return soundStyle;
        }
        public static SoundStyle WithVolume(this SoundStyle soundStyle, float volume) {
            soundStyle.Volume = volume;
            return soundStyle;
        }
        public static StatModifier MultiplyBonuses(this StatModifier statModifier, float factor) {
            return new StatModifier(
                (statModifier.Additive - 1) * factor + 1,
                (statModifier.Multiplicative - 1) * factor + 1,
                statModifier.Flat * factor,
                statModifier.Base * factor
            );
        }
    }
	public class MirrorDictionary<T> : IDictionary<T, T> {
		public T this[T key] {
            get {
                return key;
            }
            set { }
        }

		public ICollection<T> Keys { get; }
		public ICollection<T> Values { get; }
		public int Count { get; }
        public bool IsReadOnly => true;
		public void Add(T key, T value) {
			throw new NotImplementedException();
		}

		public void Add(KeyValuePair<T, T> item) {
			throw new NotImplementedException();
		}

		public void Clear() {
			throw new NotImplementedException();
		}

        public bool Contains(KeyValuePair<T, T> item) => item.Value.Equals(item.Key);
        public bool ContainsKey(T key) => true;

		public void CopyTo(KeyValuePair<T, T>[] array, int arrayIndex) {
			throw new NotImplementedException();
		}
		public IEnumerator<KeyValuePair<T, T>> GetEnumerator() {
			throw new NotImplementedException();
		}
		public bool Remove(T key) {
			throw new NotImplementedException();
		}
		public bool Remove(KeyValuePair<T, T> item) {
			throw new NotImplementedException();
		}
		public bool TryGetValue(T key, [MaybeNullWhen(false)] out T value) {
            value = key;
            return true;
		}
		IEnumerator IEnumerable.GetEnumerator() {
			throw new NotImplementedException();
		}
	}
	internal static class ArtificeXModCompat {
        internal static void OriginsExplosiveRegister() {

        }
    }
}
// always replace [\r\n\t\f\v]+\{ with {