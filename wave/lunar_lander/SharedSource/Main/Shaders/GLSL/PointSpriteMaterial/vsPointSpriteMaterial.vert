//-----------------------------------------------------------------------------
// PointSpriteMaterial.fx
//
// Copyright � 2019 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------

uniform mat4  WorldViewProj;


uniform vec3 CameraUp;
uniform float  Bias;
uniform vec3 CameraRight;
uniform float  Time;
uniform vec3 NoiseScale;
uniform float  VelocityTrailFactor;
uniform vec2 TimeFactor;
uniform vec4 TintColor;
uniform vec3 CameraPos;

uniform vec3 AnimationCenter;
uniform float AnimationPlaybackTime;
uniform float AnimationDisplace;
uniform float AnimationScale;  
uniform float AnimationPlaybackRate;
uniform float AnimationRotationFactor;
uniform float DepthBias;

uniform vec3 ProjectionOrigin;
uniform float ProjectionRange;		

uniform float Scale;
uniform vec3 Padding;

uniform sampler2D Texture;
uniform sampler2D NoiseTexture;
uniform sampler2D TimeShiftTexture;
uniform sampler2D ColorTexture;

//vec3 ComputeNoise(sampler2D noiseTexture, vec3 position, vec2 texCoord, vec3 noiseScale, float time, vec2 timeFactor)
//{
//	vec2 timeScale = texCoord + time * timeFactor;
//	vec3 noiseOffset = texture2D(noiseTexture, (texCoord + timeScale)).rgb - vec3(0.5, 0.5, 0.5);
//
//	noiseOffset *= noiseScale;
//
//	return noiseOffset;
//}
//
//vec4 ComputeUVShift(sampler2D uwShiftTexture, vec2 uw, float scale, float time, vec2 timeFactor)
//{
//	vec2 timeScale = time * timeFactor;
//	return  texture2D(uwShiftTexture, (uw + timeScale));
//}

mat4 CreateRotationY(float rotation)
{
	float s = sin(rotation);
	float c = cos(rotation);

	return mat4(
		c, 0, -s, 0,
		0, 1, 0, 0,
		s, 0, c, 0,
		0, 0, 0, 1);
}

//void ComputeAnimation(
//	sampler2D timeShiftTexture,
//	vec3 position,
//	vec4 color,
//	vec3 animationCenter,
//	float animationScale,
//	float animationPlaybackTime,
//	float animationPlaybackRate,
//	float animationRotationFactor,
//	float animationDisplace,
//	out vec3 newPosition,
//	out vec4 newColor
//)
//{
//	animationCenter.y = position.y;
//	vec3 centerDir = animationCenter - position;
//	float distance = length(centerDir) * animationScale;
//	float timeShift = texture2D(timeShiftTexture, vec2(distance, 0.0)).r;
//	float lerp = smoothstep(0.0, 1.0, animationPlaybackTime - timeShift * animationPlaybackRate);
//	float invLerp = 1.0 - lerp;
//	float distanceFactor = clamp(1.0 / (0.0001 + timeShift), 0.0, 40.0);
//	centerDir = centerDir * mat3(CreateRotationY(invLerp * animationRotationFactor * distanceFactor));
//	newPosition = animationCenter - (centerDir * (1.0 + (animationDisplace * clamp(invLerp * distanceFactor, 0.0, 1.0))));
//	newColor = color * lerp;
//}

attribute vec3  Position0;
attribute vec4  Color0;
attribute vec2  TextureCoordinate0; // Color
attribute vec4  TextureCoordinate1; // Velocity & Rotation
attribute vec2  TextureCoordinate2; // TexCoord
attribute vec2  TextureCoordinate3; // VertexUVs

varying vec4 outColor;
varying vec2 outTexCoord;

void main(void)
{
  vec3 position = Position0;
  vec4 color = (Color0 / 255.0) * TintColor;
  vec2 size = TextureCoordinate0;
  vec3 velocity  = TextureCoordinate1.xyz;
  float rotation = TextureCoordinate1.w;
  vec2 texCoord = TextureCoordinate2;
  vec2 vertexUVs = TextureCoordinate3;
  vec2 offsets = vertexUVs * 2.0 - 1.0;

//#ifdef NOISE
//	vec3 noiseOffset = ComputeNoise(NoiseTexture, NoiseTextureSampler, position, TexCoord, NoiseScale, Time, TimeFactor);
//	position += noiseOffset;
//#endif

//#ifdef APPEARING
//	ComputeAnimation(
//		TimeShiftTexture,
//		TimeShiftTextureSampler,
//		position,
//		color,
//		AnimationCenter,
//		AnimationScale,
//		AnimationPlaybackTime,
//		AnimationPlaybackRate,
//		AnimationRotationFactor,
//		AnimationDisplace,
//		position,
//		color);
//#endif

	vec3 center = position;
	size = size * Scale;
	vec2 halfSize = size * 0.5;

//#ifdef VELOCITY
//	float  halfVelocityLength = (length(velocity) * 0.5 * VelocityTrailFactor);
//
//	vec3 velocityDir = normalize(velocity);
//	vec3 cameraDir = CameraPos - center;
//	vec3 crossSpeedCameraDir = normalize(cross(velocityDir, cameraDir));
//
//	vec3 transformHalfRight = velocityDir * (halfSize.x + halfVelocityLength);
//	vec3 transformHalfUp = crossSpeedCameraDir * halfSize.y;
//#else
	float sinRotation = sin(rotation);
	float cosRotation = cos(rotation);

	vec3 halfRight = halfSize.x * CameraRight;
	vec3 halfUp = halfSize.y * CameraUp;

	vec3 transformHalfRight = halfRight * cosRotation + halfUp * sinRotation;
	vec3 transformHalfUp = halfRight * sinRotation - halfUp * cosRotation;
//#endif // VELOCITY

  vec4 v = vec4(center + (offsets.x * transformHalfRight) - (offsets.y * transformHalfUp), 1.0);
  vec4 positionCS = WorldViewProj * v;
  positionCS.z += DepthBias;

//#ifdef BIAS
//	positionCS.z += Bias;
//#endif // BIAS	
	outTexCoord = vertexUVs;

//#ifndef COLORTEXTURE	
	outColor = color;
//#elif RANGE
//	float coordV = distance(position.xyz, ProjectionOrigin) / ProjectionRange;
//	outColor = color * texture2D(ColorTexture, vec2(0.5, clamp(coordV, 0.0, 1.0)));
//#else
//	outColor = color * ColorTexture.SampleLevel(ColorTextureSampler, input.TexCoord, 0);
//#endif

	gl_Position = positionCS;
}
