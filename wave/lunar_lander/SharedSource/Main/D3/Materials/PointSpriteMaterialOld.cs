using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Graphics.VertexFormats;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Managers;
using WaveEngine.Framework.Services;

namespace GridTest
{
    public enum ColorProjectionEnum
    {
        UV,
        PointRange
    }

    [DataContract]
    public class PointSpriteMaterialOld : Material
    {
        private const string vsPointSpriteMaterial = "Content/Assets/Shaders/vsPointSpriteMaterial";

        private const string psPointSpriteMaterial = "Content/Assets/Shaders/psPointSpriteMaterial";

        private const string gsPointSpriteMaterial = "Content/Assets/Shaders/gsPointSpriteMaterial";

        ////private const string vsPointSpriteMaterial = "vsPointSpriteMaterial";

        ////private const string psPointSpriteMaterial = "psPointSpriteMaterial";

        ////private const string gsPointSpriteMaterial = "gsPointSpriteMaterial";

        /// <summary>
        /// Gets or sets the sprite texture
        /// </summary>        
        public Texture2D Texture;

        public Texture2D AppearingTexture;

        public Texture2D NoiseTexture;

        public Texture2D ColorTexture;

        [DataMember]
        private string texturePath;

        [DataMember]
        private string noiseTexturePath;

        [DataMember]
        private string appearingTexturePath;

        [DataMember]
        private string colorTexturePath;

        /// <summary>
        /// The techniques
        /// </summary>
        private static ShaderTechnique[] techniques =
        {
            new ShaderTechnique("PointSprite",        string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { }),

            // Noise
            new ShaderTechnique("PointSpriteNoise",   string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "NOISE" }),

            // Velocity
            new ShaderTechnique("PointSpriteVelocity",      string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] {},             new string[] { }, new string[] { "VELOCITY"}),
            new ShaderTechnique("PointSpriteVelocityNoise", string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "NOISE" },    new string[] { }, new string[] { "VELOCITY"}),

            // APPEARING
            new ShaderTechnique("PointSpriteAppearing",             string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "APPEARING" }),
            new ShaderTechnique("PointSpriteAppearingNoise",             string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "APPEARING", "NOISE"}),

            new ShaderTechnique("PointSpriteVelocityAppearing",     string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "APPEARING" },    new string[] { }, new string[] { "VELOCITY"}),
            new ShaderTechnique("PointSpriteVelocityAppearingNoise",     string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "APPEARING", "NOISE" },    new string[] { }, new string[] { "VELOCITY"}),


            // BIAS
            new ShaderTechnique("PointSpriteBias",        string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { },                new string[] { }, new string[] { "BIAS"}),
            new ShaderTechnique("PointSpriteBiasNoise",   string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "NOISE" },        new string[] { }, new string[] { "BIAS"}),

            new ShaderTechnique("PointSpriteBiasVelocity",      string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] {},           new string[] { }, new string[] { "BIAS", "VELOCITY"}),
            new ShaderTechnique("PointSpriteBiasVelocityNoise", string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "NOISE" },  new string[] { }, new string[] { "BIAS", "VELOCITY" }),

            new ShaderTechnique("PointSpriteBiasAppearing",             string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "APPEARING" },                      new string[] { }, new string[] { "BIAS"}),
            new ShaderTechnique("PointSpriteBiasAppearingNoise",             string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "APPEARING", "NOISE"},         new string[] { }, new string[] { "BIAS"}),

            new ShaderTechnique("PointSpriteBiasVelocityAppearing",     string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "APPEARING" },    new string[] { }, new string[] { "BIAS", "VELOCITY"}),
            new ShaderTechnique("PointSpriteBiasVelocityAppearingNoise",     string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "APPEARING", "NOISE" },        new string[] { }, new string[] { "BIAS", "VELOCITY" }),

            // COLOR
            ////////////////////

            new ShaderTechnique("PointSpriteColor",        string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] {"COLORTEXTURE"}),

            // Noise
            new ShaderTechnique("PointSpriteNoiseColor",   string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "COLORTEXTURE", "NOISE" }),

            // Velocity
            new ShaderTechnique("PointSpriteVelocityColor",      string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "COLORTEXTURE"},             new string[] { }, new string[] { "VELOCITY"}),
            new ShaderTechnique("PointSpriteVelocityNoiseColor", string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "COLORTEXTURE", "NOISE" },   new string[] { }, new string[] { "VELOCITY"}),

            // APPEARING
            new ShaderTechnique("PointSpriteAppearingColor",             string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "COLORTEXTURE", "APPEARING" }),
            new ShaderTechnique("PointSpriteAppearingNoiseColor",             string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "COLORTEXTURE", "APPEARING", "NOISE"}),

            new ShaderTechnique("PointSpriteVelocityAppearingColor",     string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "COLORTEXTURE", "APPEARING" },                   new string[] { }, new string[] { "VELOCITY"}),
            new ShaderTechnique("PointSpriteVelocityAppearingNoiseColor",     string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "COLORTEXTURE", "APPEARING", "NOISE" },     new string[] { }, new string[] { "VELOCITY"}),


            // BIAS
            new ShaderTechnique("PointSpriteBiasColor",        string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "COLORTEXTURE", },                new string[] { }, new string[] {"BIAS"}),
            new ShaderTechnique("PointSpriteBiasNoiseColor",   string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "COLORTEXTURE", "NOISE" },        new string[] { }, new string[] {"BIAS"}),

            new ShaderTechnique("PointSpriteBiasVelocityColor",      string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] {"COLORTEXTURE", },           new string[] { }, new string[] {"BIAS", "VELOCITY"}),
            new ShaderTechnique("PointSpriteBiasVelocityNoiseColor", string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "COLORTEXTURE", "NOISE" },  new string[] { }, new string[] {"BIAS", "VELOCITY" }),

            new ShaderTechnique("PointSpriteBiasAppearingColor",             string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "COLORTEXTURE", "APPEARING" },                      new string[] { }, new string[] {"BIAS"}),
            new ShaderTechnique("PointSpriteBiasAppearingNoiseColor",             string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "COLORTEXTURE", "APPEARING", "NOISE"},         new string[] { }, new string[] {"BIAS"}),

            new ShaderTechnique("PointSpriteBiasVelocityAppearingColor",     string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "COLORTEXTURE", "APPEARING" },    new string[] { }, new string[] {"BIAS", "VELOCITY"}),
            new ShaderTechnique("PointSpriteBiasVelocityAppearingNoiseColor",     string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "COLORTEXTURE", "APPEARING", "NOISE" },        new string[] { }, new string[] { "BIAS", "VELOCITY" }),

            // RANGE
            ////////////////////

            new ShaderTechnique("PointSpriteColorRange",        string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] {"COLORTEXTURE", "RANGE"}),

            // Noise
            new ShaderTechnique("PointSpriteNoiseColorRange",   string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "COLORTEXTURE", "NOISE", "RANGE" }),

            // Velocity
            new ShaderTechnique("PointSpriteVelocityColorRange",      string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "COLORTEXTURE", "RANGE"}, new string[] { }, new string[] { "VELOCITY"}),
            new ShaderTechnique("PointSpriteVelocityNoiseColorRange", string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "COLORTEXTURE", "NOISE", "RANGE" },   new string[] { }, new string[] { "VELOCITY"}),

            // APPEARING
            new ShaderTechnique("PointSpriteAppearingColorRange",             string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "COLORTEXTURE", "APPEARING", "RANGE" }),
            new ShaderTechnique("PointSpriteAppearingNoiseColorRange",             string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "COLORTEXTURE", "APPEARING", "NOISE", "RANGE"}),

            new ShaderTechnique("PointSpriteVelocityAppearingColorRange",     string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "COLORTEXTURE", "APPEARING", "RANGE" },                   new string[] { }, new string[] { "VELOCITY"}),
            new ShaderTechnique("PointSpriteVelocityAppearingNoiseColorRange",     string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "COLORTEXTURE", "APPEARING", "NOISE", "RANGE" },     new string[] { }, new string[] { "VELOCITY"}),


            // BIAS
            new ShaderTechnique("PointSpriteBiasColorRange",        string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "COLORTEXTURE", "RANGE", },                new string[] { }, new string[] {"BIAS"}),
            new ShaderTechnique("PointSpriteBiasNoiseColorRange",   string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "COLORTEXTURE", "NOISE", "RANGE" },        new string[] { }, new string[] {"BIAS"}),

            new ShaderTechnique("PointSpriteBiasVelocityColorRange",      string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] {"COLORTEXTURE", "RANGE", },           new string[] { }, new string[] {"BIAS", "VELOCITY"}),
            new ShaderTechnique("PointSpriteBiasVelocityNoiseColorRange", string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "COLORTEXTURE", "NOISE", "RANGE" },  new string[] { }, new string[] {"BIAS", "VELOCITY" }),

            new ShaderTechnique("PointSpriteBiasAppearingColorRange",             string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "COLORTEXTURE", "APPEARING", "RANGE" },                      new string[] { }, new string[] {"BIAS"}),
            new ShaderTechnique("PointSpriteBiasAppearingNoiseColorRange",             string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "COLORTEXTURE", "APPEARING", "NOISE", "RANGE"},         new string[] { }, new string[] {"BIAS"}),

            new ShaderTechnique("PointSpriteBiasVelocityAppearingColorRange",     string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "COLORTEXTURE", "APPEARING", "RANGE" },    new string[] { }, new string[] {"BIAS", "VELOCITY"}),
            new ShaderTechnique("PointSpriteBiasVelocityAppearingNoiseColorRange",     string.Empty, string.Empty, vsPointSpriteMaterial, string.Empty, psPointSpriteMaterial, string.Empty, gsPointSpriteMaterial, PointSpriteVertexFormat.VertexFormat, new string[] { "COLORTEXTURE", "APPEARING", "NOISE", "RANGE" },        new string[] { }, new string[] { "BIAS", "VELOCITY" }),
        };

        public override string CurrentTechnique
        {
            get
            {
                string technique = "PointSprite";

                if (this.Bias != 0)
                {
                    technique += "Bias";
                }

                if (this.VelocityTrailFactor > 0)
                {
                    technique += "Velocity";
                }

                if (this.AppearingTexture != null)
                {
                    technique += "Appearing";
                }

                if (this.NoiseTexture != null)
                {
                    technique += "Noise";
                }

                if (this.ColorTexture != null)
                {
                    technique += "Color";

                    if (this.ColorProjection == ColorProjectionEnum.PointRange)
                    {
                        technique += "Range";
                    }
                }

                return technique;
            }
        }

        [RenderPropertyAsAsset(AssetType.Texture)]
        public string TexturePath
        {
            get
            {
                return this.texturePath;
            }

            set
            {
                this.texturePath = value;
                this.RefreshTexture(value, ref this.Texture);
            }
        }

        [RenderPropertyAsAsset(AssetType.Texture)]
        public string NoiseTexturePath
        {
            get
            {
                return this.noiseTexturePath;
            }

            set
            {
                this.noiseTexturePath = value;
                this.RefreshTexture(value, ref this.NoiseTexture);
            }
        }

        [RenderPropertyAsAsset(AssetType.Texture)]
        public string AppearingTexturePath
        {
            get
            {
                return this.appearingTexturePath;
            }

            set
            {
                this.appearingTexturePath = value;
                this.RefreshTexture(value, ref this.AppearingTexture);
            }
        }

        [RenderPropertyAsAsset(AssetType.Texture)]
        public string ColorTexturePath
        {
            get
            {
                return this.colorTexturePath;
            }

            set
            {
                this.colorTexturePath = value;
                this.RefreshTexture(value, ref this.ColorTexture);
            }
        }

        [RenderProperty(Tag = 1)]
        [DataMember]
        public ColorProjectionEnum ColorProjection { get; set; }

        [RenderProperty(AttatchToTag = 1, AttachToValue = ColorProjectionEnum.PointRange)]
        [DataMember]
        public Vector3 ProjectionOrigin { get; set; }

        [RenderProperty(AttatchToTag = 1, AttachToValue = ColorProjectionEnum.PointRange)]
        [DataMember]
        public float ProjectionRange { get; set; }

        [DataMember]
        public float Time { get; set; }

        [DataMember]
        public Vector3 NoiseScale { get; set; }

        [DataMember]
        public Vector2 TimeFactor { get; set; }

        [DataMember]
        public Color TintColor { get; set; }

        [DataMember]
        public float VelocityTrailFactor { get; set; }

        // Animation
        public float AnimationPlaybackTime { get; set; }

        public Vector3 AnimationCenter { get; set; }

        public float AnimationDisplace { get; set; }

        public float AnimationScale { get; set; }
        public float AnimationPlaybackRate { get; set; }

        public float AnimationRotationFactor { get; set; }

        public float Bias { get; set; }

        public float Scale { get; internal set; }

        #region Shader Parameters
        /// <summary>
        /// Parameters for PointSpriteMaterial.
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Size = 160)]
        private struct PointSpriteMaterialParameters
        {
            [FieldOffset(0)]
            public Vector3 CameraUp;

            [FieldOffset(12)]
            public float Bias;

            [FieldOffset(16)]
            public Vector3 CameraRight;

            [FieldOffset(28)]
            public float Time;

            [FieldOffset(32)]
            public Vector3 NoiseScale;

            [FieldOffset(44)]
            public float VelocityTrailFactor;

            [FieldOffset(48)]
            public Vector2 TimeFactor;

            [FieldOffset(56)]
            public Vector2 Dummy2;

            [FieldOffset(64)]
            public Vector4 TintColor;

            [FieldOffset(80)]
            public Vector3 CameraPos;

            [FieldOffset(92)]
            public float Dummy3;

            [FieldOffset(96)]
            public Vector3 AnimationCenter;

            [FieldOffset(108)]
            public float AnimationPlaybackTime;

            [FieldOffset(112)]
            public float AnimationDisplace;

            [FieldOffset(116)]
            public float AnimationScale;

            [FieldOffset(120)]
            public float AnimationPlaybackRate;

            [FieldOffset(124)]
            public float AnimationRotationFactor;

            [FieldOffset(128)]
            public Vector3 ProjectionOrigin;

            [FieldOffset(140)]
            public float ProjectionRange;

            [FieldOffset(144)]
            public float Scale;

            [FieldOffset(148)]
            public Vector3 Padding;
        }

        /// <summary>
        /// Parameters for this shader.
        /// </summary>
        private PointSpriteMaterialParameters shaderParameters;
        #endregion

        public PointSpriteMaterial()
            : base(DefaultLayers.Additive)
        {
        }

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.shaderParameters = new PointSpriteMaterialParameters();
            this.Parameters = this.shaderParameters;
            this.InitializeTechniques(techniques);
            this.NoiseScale = Vector3.One * 0.2f;
            this.TimeFactor = Vector2.One * 0.01f;
            this.TintColor = Color.White;
            this.VelocityTrailFactor = 0;
            this.Bias = 0;

            this.ColorProjection = ColorProjectionEnum.UV;
            this.ProjectionOrigin = Vector3.Zero;
            this.ProjectionRange = 1;
            this.Scale = 1;

            this.AnimationPlaybackTime = 0;
            this.AnimationCenter = Vector3.Zero;
            this.AnimationDisplace = -0.05f;
            this.AnimationScale = 0.4f;
            this.AnimationPlaybackRate = 4f;
            this.AnimationRotationFactor = 0.025f;
        }

        public override void Initialize(WaveEngine.Framework.Services.AssetsContainer assets)
        {
            base.Initialize(assets);

            if (this.Texture == null && !string.IsNullOrEmpty(this.TexturePath))
            {
                this.RefreshTexture(this.TexturePath, ref this.Texture);
            }

            if (this.NoiseTexture == null && !string.IsNullOrEmpty(this.NoiseTexturePath))
            {
                this.RefreshTexture(this.NoiseTexturePath, ref this.NoiseTexture);
            }

            if (this.AppearingTexture == null && !string.IsNullOrEmpty(this.AppearingTexturePath))
            {
                this.RefreshTexture(this.AppearingTexturePath, ref this.AppearingTexture);
            }

            if (this.ColorTexture == null && !string.IsNullOrEmpty(this.ColorTexturePath))
            {
                this.RefreshTexture(this.ColorTexturePath, ref this.ColorTexture);
            }
        }

        public override void SetParameters(bool cached)
        {
            base.SetParameters(cached);

            if (!cached)
            {
                Matrix worldInverse = Matrix.Invert(this.Matrices.World);

                var cameraTransform = this.renderManager.CurrentDrawingCamera3D.Transform;
                this.shaderParameters.CameraUp = cameraTransform.WorldTransform.Up;
                this.shaderParameters.CameraRight = cameraTransform.WorldTransform.Right;
                this.shaderParameters.CameraPos = cameraTransform.Position;
                this.shaderParameters.Bias = this.Bias;

                Vector3.TransformNormal(ref this.shaderParameters.CameraUp, ref worldInverse, out this.shaderParameters.CameraUp);
                Vector3.TransformNormal(ref this.shaderParameters.CameraRight, ref worldInverse, out this.shaderParameters.CameraRight);
                Vector3.Transform(ref this.shaderParameters.CameraPos, ref worldInverse, out this.shaderParameters.CameraPos);

                this.shaderParameters.NoiseScale = this.NoiseScale;
                this.shaderParameters.Time = (float)WaveServices.Clock.TotalTime.TotalSeconds;
                this.shaderParameters.TimeFactor = this.TimeFactor;
                this.shaderParameters.TintColor = this.TintColor.ToVector4();
                this.shaderParameters.VelocityTrailFactor = this.VelocityTrailFactor;
                this.shaderParameters.AnimationCenter = this.AnimationCenter;
                this.shaderParameters.AnimationDisplace = this.AnimationDisplace;
                this.shaderParameters.AnimationPlaybackTime = this.AnimationPlaybackTime;
                this.shaderParameters.AnimationScale = this.AnimationScale;
                this.shaderParameters.AnimationPlaybackRate = this.AnimationPlaybackRate;
                this.shaderParameters.AnimationRotationFactor = this.AnimationRotationFactor;

                this.shaderParameters.Scale = this.Matrices.World.Scale.X * this.Scale;
                
                this.shaderParameters.ProjectionRange = this.ProjectionRange / this.Matrices.World.Scale.X;
                this.shaderParameters.ProjectionOrigin = this.ProjectionOrigin;
                Vector3.Transform(ref this.shaderParameters.ProjectionOrigin, ref worldInverse, out this.shaderParameters.ProjectionOrigin);

                this.Parameters = this.shaderParameters;

                if (this.Texture != null)
                {
                    graphicsDevice.SetTexture(this.Texture, 0);
                }

                if (this.NoiseTexture != null)
                {
                    graphicsDevice.SetTexture(this.NoiseTexture, 1, TextureSlotUsage.VertexShader);
                }

                if (this.AppearingTexture != null)
                {
                    graphicsDevice.SetTexture(this.AppearingTexture, 2, TextureSlotUsage.VertexShader);
                }

                if (this.ColorTexture != null)
                {
                    graphicsDevice.SetTexture(this.ColorTexture, 3, TextureSlotUsage.VertexShader);
                }
            }
        }

        /// <summary>
        /// Refreshes the texture.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="texture">The texture.</param>
        private void RefreshTexture(string path, ref Texture2D texture)
        {
            if (this.assetsContainer != null && texture != null && !string.IsNullOrEmpty(texture.AssetPath))
            {
                this.assetsContainer.UnloadAsset(texture.AssetPath);
                texture = null;
            }

            if (this.assetsContainer != null && texture == null && !string.IsNullOrEmpty(path))
            {
                texture = this.assetsContainer.LoadAsset<Texture2D>(path);
            }
        }
    }
}
