using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Graphics.VertexFormats;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;

namespace RLEnvs.D3.Components
{
    [DataContract]
    public class TerrainMeshComponent : MeshComponent
    {
        [DataMember]
        private int rows;

        [DataMember]
        private int cols;

        [DataMember]
        private float terrainWidth;

        [DataMember]
        private float terrainHeight;

        [DataMember]
        private float landingArea;

        [DataMember]
        private float landingSmoothMargin;

        [DataMember]
        private float amplitude;

        private PerlinNoise perlinNoise;

        private VertexPositionNormalTangentTexture[] vertices;
        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;

        public float TerrainWidth
        {
            get
            {
                return this.terrainWidth;
            }

            set
            {
                this.terrainWidth = value;
                this.Reset();
            }
        }

        public float TerrainHeight
        {
            get
            {
                return this.terrainHeight;
            }

            set
            {
                this.terrainHeight = value;
                this.Reset();
            }
        }

        public int Rows
        {
            get
            {
                return this.rows;
            }

            set
            {
                this.rows = value;
                this.Reset();
            }
        }

        public int Cols
        {
            get
            {
                return this.cols;
            }

            set
            {
                this.cols = value;
                this.Reset();
            }
        }

        public float LandingArea
        {
            get
            {
                return this.landingArea;
            }

            set
            {
                this.landingArea = value;
                this.Reset();
            }
        }

        public float LandingSmoothMargin
        {
            get
            {
                return this.landingSmoothMargin;
            }

            set
            {
                this.landingSmoothMargin = value;
                this.Reset();
            }
        }

        public float Amplitude
        {
            get
            {
                return this.amplitude;
            }

            set
            {
                this.amplitude = value;
                this.Reset();
            }
        }

        public Vector3 LandingPosition;

        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.terrainWidth = 100;
            this.terrainHeight = 100;
            this.rows = 100;
            this.cols = 100;
            this.amplitude = 10f;
            this.landingArea = 10f;
            this.landingSmoothMargin = 10f;
        }

        protected override void Initialize()
        {
            base.Initialize();
            this.Reset();
        }

        public void Reset()
        {
            if (this.InternalModel != null)
            {
                this.InternalModel.Unload();
                this.InternalModel = null;
            }

            this.perlinNoise = new PerlinNoise(WaveServices.Random.Next(1000));

            float centerLevel = this.GetTerrainLevel(0.5f, 0.5f);

            Vector3 positionOffset = -new Vector3(this.terrainWidth * 0.5f, centerLevel, this.terrainHeight * 0.5f);

            this.vertices = new VertexPositionNormalTangentTexture[this.rows * this.cols];

            int index = 0;
            for (int i = 0; i < this.Rows; i++)
            {
                float uCoord = i / (float)(this.rows - 1);
                float x = this.terrainWidth * (uCoord - 0.5f);

                for (int j = 0; j < this.Cols; j++, index++)
                {
                    float vCoord = j / (float)(this.cols - 1);
                    float z = this.terrainHeight * (vCoord - 0.5f);

                    float y = this.GetTerrainLevel(uCoord, vCoord) - centerLevel;

                    var vPosition = new Vector3(x, y, z);

                    // Flats the landing area with a smooth margin
                    var absX = Math.Abs(vPosition.X - this.LandingPosition.X);
                    var absZ = Math.Abs(vPosition.Z - this.LandingPosition.Z);
                    var min = Math.Max(absX, absZ);

                    if (min < this.landingArea)
                    {
                        // The landing area terrain. We flat it
                        vPosition.Y = 0;
                    }
                    else if (min < (this.landingArea + this.landingSmoothMargin))
                    {
                        // The landing margin area. We smooth it
                        vPosition.Y *= MathHelper.SmoothStep(0, 1, (min - this.landingArea) / this.landingSmoothMargin);
                    }

                    vertices[index].Position = vPosition;
                    vertices[index].TexCoord = new Vector2(uCoord, vCoord);

                    if ((i > 0) && (j > 0))
                    {
                        var tangent = vPosition - (vertices[index - 1].Position);
                        var binormal = vPosition - (vertices[index - this.cols - 1].Position);
                        var normal = Vector3.Cross(tangent, binormal);
                        normal.Normalize();

                        vertices[index].Normal = normal;
                        vertices[index].Tangent = tangent;
                        vertices[index].Binormal = binormal;
                    }
                    else
                    {
                        vertices[index].Normal = Vector3.Up;
                        vertices[index].Tangent = Vector3.UnitX;
                        vertices[index].Binormal = Vector3.UnitZ;
                    }
                }
            }

            this.vertexBuffer = new VertexBuffer(VertexPositionNormalTangentTexture.VertexFormat);
            this.vertexBuffer.SetData(vertices);

            var ind = new List<int>();
            for (int i = 0; i < this.Rows - 1; i++)
            {
                var x = (i * this.Cols);
                var xOffset = (i + 1) * this.Cols;
                for (int j = 0; j < this.Cols - 1; j++)
                {
                    var y = x + j;
                    var yOffset = xOffset + j;
                    ind.AddRange(new List<int>() { y, yOffset , yOffset +1 ,
                                                   yOffset +1, y +1 , y});

                    //Debug.WriteLine($"[{string.Join(",", ind.Skip(ind.Count - 6))}]");
                }
            }

            this.indexBuffer = new IndexBuffer(ind.Select(x => (ushort)x).ToArray<ushort>());
            var primitiveCount = ind.Count / 3;
            var mesh = new Mesh(0, vertices.Length, 0, primitiveCount, vertexBuffer, this.indexBuffer, PrimitiveType.TriangleList);
            mesh.Name = "terrain";
            var boundingBox = new BoundingBox(new Vector3(-this.rows * 0.5f, -this.amplitude * 0.5f, -this.cols * 0.5f), new Vector3(this.rows * 0.5f, this.amplitude * 0.5f, this.cols * 0.5f));

            this.ModelMeshName = "terrain";

            this.InternalModel = new InternalModel();
            this.InternalModel.FromMesh(WaveServices.GraphicsDevice, mesh, boundingBox);

            this.ThrowRefreshEvent();
        }

        private float GetTerrainLevel(float u, float v)
        {
            float level = (float)(
              // First octave
              (this.perlinNoise.Noise(2 * v, 2 * u, -0.5) + 1) / 2 * 0.7 +
              // Second octave
              (this.perlinNoise.Noise(4 * v, 4 * u, 0) + 1) / 2 * 0.2 +
              // Third octave
              (this.perlinNoise.Noise(8 * v, 8 * u, +0.5) + 1) / 2 * 0.1);

            return this.amplitude * (Math.Min(1, Math.Max(0, level)) - 0.5f);
        }

        private VertexPositionNormalColorTexture GetPNCT(Vector3 position, Vector2 texture, Color color)
        {
            return new VertexPositionNormalColorTexture()
            {
                Position = position,
                Color = color,
                TexCoord = texture,
                Normal = Vector3.Up
            };
        }

        ////private void GenerateHeightMap()
        ////{
        ////    var stopwatch = new Stopwatch();
        ////    stopwatch.Start();
        ////    var randy = new System.Random();
        ////    var n = randy.Next(0, 1000);
        ////    PerlinNoise perlinNoise = new PerlinNoise(n);
        ////    double widthDivisor = 1 / (double)this.Cols;
        ////    double heightDivisor = 1 / (double)this.Rows;

        ////    this.data = new float[this.Rows, this.Cols];

        ////    for (int i = 0; i < this.Rows; i++)
        ////    {
        ////        for (int j = 0; j < this.Cols; j++)
        ////        {
        ////            double v =
        ////               // First octave
        ////               (perlinNoise.Noise(2 * j * widthDivisor, 2 * i * heightDivisor, -0.5) + 1) / 2 * 0.7 +
        ////               // Second octave
        ////               (perlinNoise.Noise(4 * j * widthDivisor, 4 * i * heightDivisor, 0) + 1) / 2 * 0.2 +
        ////               // Third octave
        ////               (perlinNoise.Noise(8 * j * widthDivisor, 8 * i * heightDivisor, +0.5) + 1) / 2 * 0.1;

        ////            v = Math.Min(1, Math.Max(0, v)) - 0.5f;
        ////            this.data[i, j] = (float)v;
        ////        }
        ////    }

        ////    // Make landing path in the center at middle heigth
        ////    var fromHeight = (int)((this.Rows * 0.5f) - (this.LandingSize.Y * 0.5f));
        ////    var fromWidth = (int)((this.Cols * 0.5f) - (this.LandingSize.X * 0.5f));

        ////    for (int i = fromHeight; i < fromHeight + this.LandingSize.Y; i++)
        ////    {
        ////        for (int j = fromWidth; j < fromWidth + this.LandingSize.X; j++)
        ////        {
        ////            this.data[i, j] = 0;
        ////        }
        ////    }

        ////    stopwatch.Stop();
        ////    //Debug.WriteLine($"Time spent creating heigtmap [{cols}x{rows}] is: {stopwatch.Elapsed}");
        ////}
    }
}
