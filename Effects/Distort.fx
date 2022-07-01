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
float2 uOffset;
float uScale;
float2 uTargetPosition;
float4 uLegacyArmorSourceRect;
float2 uLegacyArmorSheetSize;
float3 uTestValue;
/*float2 uMin;
float2 uMax;*/

float4 Distort(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0 {
	
	float2 diff = coords - uOffset;
	float2 dir = normalize(round(diff * uImageSize0) / uImageSize0);
	
	float2 convertedCoords = coords * uImageSize0;
	
	float4 distortion = tex2D(uImage1, coords);
	float r = max(distortion.r - uColor.r, 0);
	float g = max(distortion.g - uColor.g, 0);
	float b = max(distortion.b - uColor.b, 0);
	float sum = r + g + b;
	
	float2 change = sum * dir;
	
	if (change.x > 0) {
		change.x = min(round(change.x), diff.x);
	} else {
		change.x = max(round(change.x), diff.x);
	}
	if (change.y > 0) {
		change.y = min(round(change.y), diff.y);
	} else {
		change.y = max(round(change.y), diff.y);
	}
	
	convertedCoords -= change * 2;
	
	float4 color = tex2D(uImage0, convertedCoords / uImageSize0);
	//color.r = abs(change.x);
	//color.g = abs(change.y);
	//color.b = 0;
	//return float4(uColor.r, uColor.g, uColor.b, 0.5);
	//const float borderPoint = 0.3;
	//const float borderThickness = 0.1;
	//float2 diff = coords - uOffset;
	//diff = round(diff * uImageSize0);
	//float4 distortion = tex2D(uImage1, coords);
	//float r = max(distortion.r - uColor.r, 0);
	//float g = max(distortion.g - uColor.g, 0);
	//float b = max(distortion.b - uColor.b, 0);
	//float sum = round((r + g + b) * 16) / 16;
	//if (sum > borderPoint && sum < borderPoint+borderThickness) {
	//	return float4(0,0,0,1);
	//}
	//float4 color = tex2D(uImage0, coords - (diff * sum) / uImageSize0);
	//color.rgb *= uColor;
	return color;
}

technique Technique1{
	pass Distort {
		PixelShader = compile ps_2_0 Distort();
	}
}