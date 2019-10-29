cbuffer Matrices : register(b0)
{
    float4x4	WorldViewProj						: packoffset(c0);
    float4x4	World								: packoffset(c4);
    float4x4	WorldInverseTranspose				: packoffset(c8);
};

Texture2D DiffuseTexture 			: register(t0);
Texture2D BumpTexture				: register(t1);
SamplerState Sampler 	: register(s0);

cbuffer Parameters : register(b1)
{
	float				Amplitude : packoffset(c0.x);
};

struct VS_IN
{
	float4 Position : POSITION;
	float3 Normal	: NORMAL0;
	float2 TexCoord : TEXCOORD0;
};

struct VS_OUT
{
	float4 Position : SV_POSITION;
	float2 TexCoord : TEXCOORD0;
};


VS_OUT vsTerrain( VS_IN input )
{
    VS_OUT output = (VS_OUT)0;
	
	float bump = BumpTexture.SampleLevel(Sampler, input.TexCoord, 0);

	float4 position = input.Position;
	position.z += bump * Amplitude;

    output.Position = mul(position, WorldViewProj);
	output.TexCoord = input.TexCoord;

    return output;
}

float4 psTerrain( VS_OUT input ) : SV_Target0
{
	return DiffuseTexture.Sample(Sampler, input.TexCoord);	
}