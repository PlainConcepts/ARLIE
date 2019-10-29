using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;

namespace GridTest
{
    [StructLayout(LayoutKind.Explicit, Size = 56)]
    public struct PointSpriteVertexFormat : IBasicVertex
    {
        /// <summary>
        /// Vertex position.
        /// </summary>
        [FieldOffset(0)]
        public Vector3 Position;

        /// <summary>
        /// Vertex color.
        /// </summary>
        [FieldOffset(12)]
        public Color Color;

        /// <summary>
        /// Size of the sprite
        /// </summary>
        [FieldOffset(16)]
        public Vector2 Size;

        /// <summary>
        /// Speed of the sprite
        /// </summary>
        [FieldOffset(24)]
        public Vector4 VelocityAndRotation;

        /// <summary>
        /// Size of the sprite
        /// </summary>
        [FieldOffset(40)]
        public Vector2 TexCoord;

        [FieldOffset(48)]
        public Vector2 VertexTexCoords;

        /// <summary>
        /// Vertex format.
        /// </summary>
        public static readonly VertexBufferFormat VertexFormat;

        #region Properties
        /// <summary>
        /// Gets the vertex format.
        /// </summary>
        VertexBufferFormat IBasicVertex.VertexFormat
        {
            get
            {
                return VertexFormat;
            }
        }
        #endregion

        #region Initialize

        /// <summary>
        /// Initializes static members of the <see cref="PointSpriteVertexFormat"/> struct.
        /// </summary>
        static PointSpriteVertexFormat()
        {
            VertexFormat = new VertexBufferFormat(new VertexElementProperties[]
                {
                    new VertexElementProperties(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElementProperties(12, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                    new VertexElementProperties(16, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
                    new VertexElementProperties(24, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 1),
                    new VertexElementProperties(40, VertexElementFormat.Vector2,  VertexElementUsage.TextureCoordinate, 2),
                    new VertexElementProperties(48, VertexElementFormat.Vector2,  VertexElementUsage.TextureCoordinate, 3),
                });
        }
        #endregion
    }
}
