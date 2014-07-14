#include "Macros.fxh"

DECLARE_TEXTURE(Texture, 0);

float4x4 MatrixTransform    _vs(c0) _cb(c0);
float4 Size = 1;
float AspectRatio = 1;
float4 ColorMultiply = float4(1, 1, 1, 1);

struct VSOutput
{
	float4 position		: SV_Position;
	float4 color		: COLOR0;
    float2 texCoord		: TEXCOORD0;
};

VSOutput SpriteVertexShader(float4 position : SV_Position, float4 color : COLOR0, float2 texCoord : TEXCOORD0)
{
	VSOutput output;
    output.position = mul(position, MatrixTransform);
	output.color = color;
	output.texCoord = texCoord;
	return output;
}

float4 SpritePixelShader(VSOutput input) : SV_Target0
{
	float2 uv = input.texCoord;
	// this is for faking anti-aliasing
	float border = clamp(min(min(uv.x, uv.y / AspectRatio), min(1 - uv.x, 1 - uv.y)) * Size / 2, 0, 1);
	float4 color = SAMPLE_TEXTURE(Texture, input.texCoord);
	color.a *= border;
	color *= ColorMultiply;
    return color;
}

TECHNIQUE( SpriteBatch, SpriteVertexShader, SpritePixelShader );