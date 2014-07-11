#include "Macros.fxh"

DECLARE_TEXTURE(Texture, 0);

BEGIN_CONSTANTS
MATRIX_CONSTANTS

    float4x4 MatrixTransform    _vs(c0) _cb(c0);
	float4 Intense = 1;
	float AspectRatio = 1;

END_CONSTANTS


struct VSOutput
{
	float4 position		: SV_Position;
	float4 color		: COLOR0;
    float2 texCoord		: TEXCOORD0;
};

VSOutput SpriteVertexShader(	float4 position	: SV_Position,
								float4 color	: COLOR0,
								float2 texCoord	: TEXCOORD0)
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
	float xDistance = min(uv.x, 1 - uv.x);
	float yDistance = min(uv.y, 1 - uv.y) / AspectRatio;
	float border = clamp(min(xDistance, yDistance), 0, 1);
	float alpha = border * border * AspectRatio * 2 * Intense;
	
    return float4(0, 0, 0, alpha);
}

TECHNIQUE( SpriteBatch, SpriteVertexShader, SpritePixelShader );