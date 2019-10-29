//-----------------------------------------------------------------------------
// PointSpriteMaterial.fx
//
// Copyright � 2018 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------

#ifdef GL_ES
precision mediump float;
#endif

uniform sampler2D Texture;

varying vec4 outColor;
varying vec2 outTexCoord;

void main(void)
{
	vec4 texColor = texture2D(Texture, outTexCoord);
	gl_FragColor = texColor * outColor;	
}
