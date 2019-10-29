cbuffer Matrices : register(b0)
{
	float4x4 WorldViewProj	: packoffset(c0);
};

cbuffer Parameters : register(b1)
{
	float3 CameraUp					: packoffset(c0.x);
	float  Bias						: packoffset(c0.w);
	float3 CameraRight				: packoffset(c1.x);
	float  Time						: packoffset(c1.w);
	float3 NoiseScale				: packoffset(c2.x);
	float  VelocityTrailFactor		: packoffset(c2.w);
	float2 TimeFactor				: packoffset(c3.x);
	float4 TintColor				: packoffset(c4.x);
	float3 CameraPos				: packoffset(c5.x);

	float3 AnimationCenter			: packoffset(c6.x);
	float AnimationPlaybackTime		: packoffset(c6.w);
	float AnimationDisplace			: packoffset(c7.x);
	float AnimationScale			: packoffset(c7.y);
	float AnimationPlaybackRate		: packoffset(c7.z);
	float AnimationRotationFactor	: packoffset(c7.w);

	float3 ProjectionOrigin			: packoffset(c8.x);
	float ProjectionRange			: packoffset(c8.w);

	float Scale						: packoffset(c9.x);
	float DepthBias					: packoffset(c9.y);
	float2 Padding 					: packoffset(c9.z);
};

Texture2D Texture			: register(t0);
SamplerState TextureSampler	: register(s0);

Texture2D NoiseTexture				: register(t1);
SamplerState NoiseTextureSampler	: register(s1);

Texture2D TimeShiftTexture				: register(t2);
SamplerState TimeShiftTextureSampler	: register(s2);

Texture2D ColorTexture				: register(t3);
SamplerState ColorTextureSampler	: register(s3);

inline float3 ComputeNoise(Texture2D noiseTexture, SamplerState noiseTextureSampler, float3 position, float2 texCoord, float3 noiseScale, float time, float2 timeFactor)
{
	float2 timeScale = texCoord + time * timeFactor;
	float3 noiseOffset = noiseTexture.SampleLevel(noiseTextureSampler, (texCoord + timeScale), 0).rgb - float3(0.5f, 0.5f, 0.5f);

	noiseOffset *= noiseScale;

	return noiseOffset;
}

inline float4 ComputeUVShift(Texture2D uwShiftTexture, SamplerState uwShiftTextureSampler, float2 uw, float scale, float time, float2 timeFactor)
{
	float2 timeScale = time * timeFactor;
	return  uwShiftTexture.SampleLevel(uwShiftTextureSampler, (uw + timeScale), 0);
}

inline float4x4 CreateRotationY(float rotation)
{
	float s = sin(rotation);
	float c = cos(rotation);

	return float4x4(
		c, 0, -s, 0,
		0, 1, 0, 0,
		s, 0, c, 0,
		0, 0, 0, 1);
}

inline void ComputeAnimation(
	Texture2D timeShiftTexture,
	SamplerState timeShiftTextureSampler,
	float3 position,
	float4 color,
	float3 animationCenter,
	float animationScale,
	float animationPlaybackTime,
	float animationPlaybackRate,
	float animationRotationFactor,
	float animationDisplace,
	out float3 newPosition,
	out float4 newColor
)
{
	animationCenter.y = position.y;
	float3 centerDir = animationCenter - position;
	float distance = length(centerDir) * animationScale;
	float timeShift = timeShiftTexture.SampleLevel(timeShiftTextureSampler, distance, 0).r;
	float lerp = smoothstep(0, 1, animationPlaybackTime - timeShift * animationPlaybackRate);
	float invLerp = 1 - lerp;
	float distanceFactor = clamp(0, 40, 1.0f / (0.0001f + timeShift));

	centerDir = mul(centerDir, (float3x3)CreateRotationY(invLerp * animationRotationFactor * distanceFactor));
	newPosition = animationCenter - (centerDir * (1 + (animationDisplace * saturate(invLerp * distanceFactor))));
	newColor = color * lerp;
}

struct VS_IN_COLOR
{
	float3 Position				: POSITION;
	float4 Color				: COLOR;
	float2 Size					: TEXCOORD0;
	float4 VelocityAndRotation	: TEXCOORD1;	
	float2 TexCoord				: TEXCOORD2;
	float2 VertexUVs			: TEXCOORD3;
};

struct VS_OUT_COLOR
{
	float4 Position			: SV_POSITION;
	float4 Color 			: COLOR;
	float2 TexCoord			: TEXCOORD0;
};

VS_OUT_COLOR vsPointSprite(VS_IN_COLOR input)
{
	VS_OUT_COLOR output = (VS_OUT_COLOR)0;

	float3 position = input.Position;
	float4 color = input.Color * TintColor;

#if NOISE
	float3 noiseOffset = ComputeNoise(NoiseTexture, NoiseTextureSampler, position, input.TexCoord, NoiseScale, Time, TimeFactor);
	position += noiseOffset;
#endif

#if APPEARING
	ComputeAnimation(
		TimeShiftTexture,
		TimeShiftTextureSampler,
		position,
		color,
		AnimationCenter,
		AnimationScale,
		AnimationPlaybackTime,
		AnimationPlaybackRate,
		AnimationRotationFactor,
		AnimationDisplace,
		position,
		color);
#endif

	float3 center = position;
	float2 size = input.Size * Scale;
	float2 halfSize = size * 0.5f;
	float rotation = input.VelocityAndRotation.w;
	float3 velocity = input.VelocityAndRotation.xyz;

#if VELOCITY
	float  halfVelocityLength = (length(velocity) * 0.5f * VelocityTrailFactor);

	float3 velocityDir = normalize(velocity);
	float3 cameraDir = CameraPos - center;
	float3 crossSpeedCameraDir = normalize(cross(velocityDir, cameraDir));

	float3 transformHalfRight = velocityDir * (halfSize.x + halfVelocityLength);
	float3 transformHalfUp = crossSpeedCameraDir * halfSize.y;
#else
	float sinRotation = sin(rotation);
	float cosRotation = cos(rotation);

	float3 halfRight = halfSize.x * CameraRight;
	float3 halfUp = halfSize.y * CameraUp;

	float3 transformHalfRight = halfRight * cosRotation + halfUp * sinRotation;
	float3 transformHalfUp = halfRight * sinRotation - halfUp * cosRotation;
#endif // VELOCITY

	float2 offsets = input.VertexUVs * 2.0f - 1.0f;
	float4 v = float4(center + (offsets.x * transformHalfRight) - (offsets.y * transformHalfUp), 1.0f);

	output.Position = mul(v, WorldViewProj);
	output.Position.z += DepthBias;

#if BIAS
	output.Position.z += Bias;
#endif // BIAS	
	output.TexCoord = input.VertexUVs;

#if !COLORTEXTURE	
	output.Color = color;
#else
#if RANGE
	float coordV = distance(position.xyz, ProjectionOrigin) / ProjectionRange;
	output.Color = color * ColorTexture.SampleLevel(ColorTextureSampler, float2(0.5, saturate(coordV)), 0);
#else
	output.Color = color * ColorTexture.SampleLevel(ColorTextureSampler, input.TexCoord, 0);
#endif
#endif

	return output;
}

float4 psPointSprite(VS_OUT_COLOR input) : SV_Target0
{
	float4 texColor = Texture.Sample(TextureSampler, input.TexCoord);

	return texColor * input.Color;	
}