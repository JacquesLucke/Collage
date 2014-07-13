#include "Macros.fxh"

float4x4 MatrixTransform	_vs(c0) _cb(c0);
float4 Intense = 1;
float AspectRatio = 1;
float BorderRadius = 0.1;

struct VSOutput
{
	float4 position		: SV_Position;
	float4 color		: COLOR0;
	float2 texCoord		: TEXCOORD0;
};

VSOutput SpriteVertexShader( float4 position : SV_Position, float4 color : COLOR0, float2 texCoord : TEXCOORD0)
{
	VSOutput output;
	output.position = mul(position, MatrixTransform);
	output.color = color;
	output.texCoord = texCoord;
	return output;
}


float4 SpritePixelShader(VSOutput input) : SV_Target0
{
	// calculate distance from boundary
	float2 dis;
	dis.x = min(input.texCoord.x, 1 - input.texCoord.x);
	dis.y = min(input.texCoord.y, 1 - input.texCoord.y) / AspectRatio;

	// calculate border (a bit like intensity of the shadow at this pixel)
	float border;
	if (dis.x < BorderRadius && dis.y < BorderRadius) 
		border = BorderRadius - distance(dis, float2(BorderRadius, BorderRadius));
	else border = min(dis.x, dis.y);
	border = max(border, 0);

	// calculate the alpha value
	float alpha = border * border * Intense;

	return float4(0, 0, 0, alpha);
}

TECHNIQUE( SpriteBatch, SpriteVertexShader, SpritePixelShader );