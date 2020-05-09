sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;

float4 ArmorInversePolarized(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0{
    float4 color = tex2D(uImage0, coords);
	float4 icolor = color;
	float4 invcolor = 1.2;
	color.xyz = invcolor.xyz - color.xyz;
	color.x = color.y + color.x;
	color.x = color.z + color.x;
	color.y = (color.x * -0.333333) + 0.4;
	color.z = color.x * 0.166667;
	color.x = (color.x * 0.166667) + 0.3;
	float4 color1 = color.z;
	if(color.y > 0){
		color1.xyz = color.x;
	}
	color = color.w * color1;
	color.w = icolor.w;
    return color * sampleColor;
}
float4 ArmorInversePolarized2(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0{
    float4 color = tex2D(uImage0, coords);
	float4 icolor = color;
	//float4 invcolor = 1.1;
	//color.xyz = invcolor.xyz - color.xyz;
	color.x = color.y + color.x;
	color.x = color.z + color.x;
	color.y = (color.x * -0.333333) + 0.6;
	color.z = color.x * 0.083333;
	color.x = (color.x * 0.166667) + 0.4;
	float4 color1 = color.z;
	if(color.y > 0){
		color1.xyz = color.x;
	}
	color = color.w * color1;
	color.w = icolor.w;
    return color * sampleColor;
}
float4 ArmorInverseColoredBlack(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0{
	float4 r = tex2D(uImage0, coords);
	float4 r1 = 0;
	float4 r2 = 0;
	float4 r3 = 0;
	r1.w = max(r.y, r.z);
	r2.w = max(r.x, r1.w);
	r1.x = min(r.z, r.y);
	r2.x = min(r1.x, r.x);
	r1.x = r2.w + r2.x;
	r1.y = r2.w + -r2.x;
	r1.z = (r1.x * -0.5) + 1.5;
	float c5z = r1.w;
	r2.xyz = (r1.z * -uColor) + r1.w;
	r2.w = (r1.x * -0.5) + 0.5;
	r3.xyz = r1.x * uSaturation;
	if(r2.w>=0){
		r2.xyz = r3;
	}
	r2.xyz = (r1.x * -0.5) + r2;
	r2.w = r1.x * 0.5;
	r1.x = r1.y * 2;
	r1.y = (r1.y * 0.66) + 0.33;
	float c1x = r3.x;
	//r2.xyz = r.xyz * uColor;
	r1.x, (r1.x * r3.x) + 0.5;
	r2.xyz = (r1.x * r2) + r2.w;// smooth brightness
	r2.xyz = r.w * r2;
	r2.xyz = r.xyz * uColor;
	r.xyz = r1.y * r2;
	//r.xyz = r.xyz * uColor;//apply color 
	return r * sampleColor;
}
float4 ArmorColoredUltradark(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0{
	float4 r = tex2D(uImage0, coords);
	float4 r1 = 0;
	float4 r2 = 0;
	float4 r3 = 0;
	r1.w = max(r.y, r.z);
	r2.w = max(r.x, r1.w);
	r1.x = min(r.z, r.y);
	r2.x = min(r1.x, r.x);
	r1.x = r2.w + r2.x;
	r1.y = r2.w + -r2.x;
	r1.z = (r1.x * -0.5) + 1.5;
	float c5z = r1.w;
	//r2.xyz = (r1.z * -uColor) + r1.w;
	r2.w = (r1.x * -0.5) + 0.5;
	//r3.xyz = r1.x * uSaturation
	if(r2.w>=0)r2.xyz = r3;
	r2.xyz = (r1.x * -0.5) + r2;
	r2.w = r1.x * 0.5;
	// skipped line 21:'mul r1.x, r1.y, c4.x', unknown variable
	r1.y = (r1.y * 0.66) + 0.33;
	//float c1x = r3.x;
	//r1.x, (r1.x * r3.x) + c2.x;
	r2.xyz = (r1.x * r2) + r2.w;
	r2.xyz = r.w * r2;
	r.xyz = r1.y * r2; 
	r.xyz = r.xyz * uColor;
	return r;
}
float4 ArmorInvertedColoredGrey(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0{
	float4 r = tex2D(uImage0, coords);
	float4 r1 = 0;
	float4 r2 = 0;
	float4 r3 = 0;
	r1.w = max(r.y, r.z);
	r2.w = max(r.x, r1.w);
	r1.x = min(r.z, r.y);
	r2.x = min(r1.x, r.x);
	r1.x = r2.w + r2.x;
	r1.y = r2.w + -r2.x;
	r1.z = (r1.x * -0.5) + 1.5;
	float c5z = r1.w;
	r2.xyz = (r1.z * -uColor) + r1.w;
	r2.w = (r1.x * -0.5) + 0.5;
	r3.xyz = r1.x * uSaturation;
	if(r2.w>=0)r2.xyz = r3;
	r2.xyz = (r1.x * -0.5) + r2;
	r2.w = r1.x * 0.5;
	// skipped line 21:'mul 
	r1.x = r1.y * 2;// * c4.x;//', unknown variable
	r1.y = (r1.y * 0.66) + 0.33;
	float c1x = r3.x;
	r1.x, (r1.x * r3.x) + uSecondaryColor.x;
	r2.xyz = (r1.x * r2) + r2.w;
	r2.xyz = r.w * r2;
	r.xyz = r1.y * r2; 
	//r.xyz = r.xyz * uColor;
	return r * sampleColor;
}

technique Technique1
{
    pass ArmorInversePolarizedPass
    {
        PixelShader = compile ps_2_0 ArmorInversePolarized();
    }
    pass ArmorInversePolarized2Pass
    {
        PixelShader = compile ps_2_0 ArmorInversePolarized2();
    }
	pass ColoredArmorInversePass
    {
        PixelShader = compile ps_2_0 ArmorInverseColoredBlack();
    }
}