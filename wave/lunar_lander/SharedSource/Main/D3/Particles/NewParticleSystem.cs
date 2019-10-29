#region File Description
//-----------------------------------------------------------------------------
// NewParticleSystem
// Copyright © 2016 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using GridTest;
using RLEnvs.D3.Particles.Emitters;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Attributes.Converters;
using WaveEngine.Common.Curves;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Common.Physics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
#endregion

namespace RLEnvs.D3.Particles
{
    /// <summary>
    /// Particle system class.
    /// </summary>
    [DataContract]

    public class NewParticleSystem : Component
    {
        public enum EmitType
        {
            Rate,
            Burst
        }

        public enum ScreenAlignType
        {
            Sprite,
            Velocity
        }

        public enum ValueType
        {
            Constant,
            Random
        }

        public enum SpaceEnum
        {
            World,
            Local
        }

        public enum ParticleCollisionType
        {
            Die,
            Bounce,
            Stop
        }

        private static FastRandom random;

        /// <summary>
        /// Gets or sets the Number of instances of this component created.
        /// </summary>
        private static int instances;

        public PointSpriteMaterial material;

        [RequiredComponent]
        public Transform3D Transform;

        [DataMember]
        private float emitDuration;

        [DataMember]
        private int layerId;

        [DataMember]
        private float emitRate;

        [DataMember]
        private bool emitAutomatically;

        private bool isEmitting;

        [DataMember]
        private AEmitter shape;

        [DataMember]
        private string texturePath;

        [DataMember]
        public bool sizeAnimated;

        [DataMember]
        public EmitType emitType;

        [DataMember]
        public bool velocityAnimated;

        [DataMember]
        private float depthBias;


        public float DepthBias
        {
            get
            {
                return this.depthBias;
            }

            set
            {
                this.depthBias = value;
                this.RefreshMaterial();
            }
        }

        #region Properties
        public bool IsEmitting
        {
            get
            {
                return this.isEmitting;
            }

            set
            {
                this.isEmitting = value;
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
                this.RefreshMaterial();
            }
        }

        [RenderPropertyAsLayer]
        public int LayerId
        {
            get
            {
                return this.layerId;
            }

            set
            {
                this.layerId = value;
                this.RefreshMaterial();
            }
        }

        [RenderProperty(Tag = 123)]
        public EmitType EmitMode
        {
            get
            {
                return this.emitType;
            }

            set
            {
                this.emitType = value;
            }
        }

        [RenderProperty(AttatchToTag = 123, AttachToValue = EmitType.Rate)]
        public float EmitDuration
        {
            get
            {
                return this.emitDuration;
            }

            set
            {
                this.emitDuration = value;
            }
        }

        [RenderPropertyAsFInput(AttatchToTag = 123, AttachToValue = EmitType.Rate, MinLimit = 0)]
        public float EmitRate
        {
            get
            {
                return this.emitRate;
            }

            set
            {
                this.emitRate = value;
            }
        }

        public bool EmitAutomatically
        {
            get
            {
                return this.emitAutomatically;
            }

            set
            {
                this.emitAutomatically = value;
            }
        }

        public ShapeType ShapeType
        {
            get
            {
                if (this.shape == null)
                {
                    this.shape = ShapeFactory.GetEmitter(ShapeType.Point);
                    this.shape.BaseInitialize(this);
                }

                return this.shape.ShapeType;
            }

            set
            {
                this.shape = ShapeFactory.GetEmitter(value);
                this.shape.BaseInitialize(this);
            }
        }

        [RenderPropertyAsObject]
        public AEmitter Shape
        {
            get
            {
                return this.shape;
            }

            set
            {
                this.shape = value;
                this.shape.BaseInitialize(this);
            }
        }

        [RenderProperty(Tag = 50)]
        [DataMember]
        public ValueType InitSpeedType;

        [RenderProperty(AttatchToTag = 50, AttachToValue = ValueType.Constant)]
        [DataMember]
        public float InitSpeed;

        [RenderProperty(AttatchToTag = 50, AttachToValue = ValueType.Random)]
        [DataMember]
        public float MinSpeed;

        [RenderProperty(AttatchToTag = 50, AttachToValue = ValueType.Random)]
        [DataMember]
        public float MaxSpeed;

        [RenderProperty(Tag = 1)]
        [DataMember]
        public ValueType InitColorType;

        [RenderProperty(AttatchToTag = 1, AttachToValue = ValueType.Constant)]
        [DataMember]
        public Color InitColor;

        [RenderProperty(AttatchToTag = 1, AttachToValue = ValueType.Random)]
        [DataMember]
        public Color MinColor;

        [RenderProperty(AttatchToTag = 1, AttachToValue = ValueType.Random)]
        [DataMember]
        public Color MaxColor;

        [RenderProperty(Tag = 2)]
        [DataMember]
        public ValueType InitLifeType;

        [RenderProperty(AttatchToTag = 2, AttachToValue = ValueType.Constant)]
        [DataMember]
        public float InitLife;

        [RenderProperty(AttatchToTag = 2, AttachToValue = ValueType.Random)]
        [DataMember]
        public float MinLife;

        [RenderProperty(AttatchToTag = 2, AttachToValue = ValueType.Random)]
        [DataMember]
        public float MaxLife;

        [RenderProperty(Tag = 3)]
        [DataMember]
        public ValueType InitRotationType;

        [RenderPropertyAsFInput(AttatchToTag = 3, AttachToValue = ValueType.Constant, ConverterType = typeof(FloatRadianToDegreeConverter))]
        [DataMember]
        public float InitRotation;

        [RenderPropertyAsFInput(AttatchToTag = 3, AttachToValue = ValueType.Random, ConverterType = typeof(FloatRadianToDegreeConverter))]
        [DataMember]
        public float MinRotation;

        [RenderPropertyAsFInput(AttatchToTag = 3, AttachToValue = ValueType.Random, ConverterType = typeof(FloatRadianToDegreeConverter))]
        [DataMember]
        public float MaxRotation;

        [RenderProperty(Tag = 4)]
        [DataMember]
        public ValueType InitSizeType;

        [RenderProperty(AttatchToTag = 4, AttachToValue = ValueType.Constant)]
        [DataMember]
        public float InitSize;

        [RenderProperty(AttatchToTag = 4, AttachToValue = ValueType.Random)]
        [DataMember]
        public float MinSize;

        [RenderProperty(AttatchToTag = 4, AttachToValue = ValueType.Random)]
        [DataMember]
        public float MaxSize;

        [DataMember(Name = "GravityVector")]
        public Vector3 Gravity;

        [RenderPropertyAsFInput(0, float.MaxValue)]
        [DataMember]
        public float LifeFactor;

        [RenderPropertyAsFInput(0, float.MaxValue)]
        [DataMember]
        public float TimeFactor;

        [RenderProperty(Tag = 5)]
        [DataMember]
        public bool ColorAnimated;

        [RenderProperty(AttatchToTag = 5, AttachToValue = true)]
        [DataMember]
        public ColorCurve ColorOverLife;

        [RenderProperty(Tag = 6)]
        [DataMember]
        public bool SizeAnimated
        {
            get
            {
                return this.sizeAnimated;
            }
            set
            {
                this.sizeAnimated = value;

                if (value && (SizeOverLife == null))
                {
                    this.SizeOverLife = new FloatCurve(1);
                }
            }
        }

        [RenderProperty(AttatchToTag = 6, AttachToValue = true)]
        [DataMember]
        public FloatCurve SizeOverLife;

        [RenderProperty(Tag = 7)]
        [DataMember]
        public bool VelocityAnimated
        {
            get
            {
                return this.velocityAnimated;
            }
            set
            {
                this.velocityAnimated = value;

                if (value && (SizeOverLife == null))
                {
                    this.VelocityOverLifeX = new FloatCurve(1);
                    this.VelocityOverLifeY = new FloatCurve(1);
                    this.VelocityOverLifeZ = new FloatCurve(1);
                }
            }
        }

        [RenderProperty(AttatchToTag = 7, AttachToValue = true)]
        [DataMember]
        public FloatCurve VelocityOverLifeX;

        [RenderProperty(AttatchToTag = 7, AttachToValue = true)]
        [DataMember]
        public FloatCurve VelocityOverLifeY;

        [RenderProperty(AttatchToTag = 7, AttachToValue = true)]
        [DataMember]
        public FloatCurve VelocityOverLifeZ;

        [RenderProperty(Tag = 8)]
        [DataMember]
        public bool RotationAnimated;

        [RenderProperty(Tag = 9)]
        [DataMember]
        public ValueType AngularVelocityType;

        [RenderPropertyAsFInput(AttatchToTag = 9, AttachToValue = ValueType.Constant, ConverterType = typeof(FloatRadianToDegreeConverter))]
        [DataMember]
        public float AngularVelocity;

        [RenderPropertyAsFInput(AttatchToTag = 9, AttachToValue = ValueType.Random, ConverterType = typeof(FloatRadianToDegreeConverter))]
        [DataMember]
        public float MinAngularVelocity;

        [RenderPropertyAsFInput(AttatchToTag = 9, AttachToValue = ValueType.Random, ConverterType = typeof(FloatRadianToDegreeConverter))]
        [DataMember]
        public float MaxAngularVelocity;

        [RenderProperty(Tag = 10)]
        [DataMember]
        public ScreenAlignType ScreenAlign;

        [RenderPropertyAsFInput(MinLimit = 0, AttatchToTag = 10, AttachToValue = ScreenAlignType.Velocity)]
        [DataMember]
        public float VelocityTrailFactor;

        [DataMember]
        public SpaceEnum Space;

        [RenderProperty(Tag = 11)]
        [DataMember]
        public bool ApplyForces;

        [RenderPropertyAsBitwise(AttatchToTag = 11, AttachToValue = true)]
        [DataMember]
        public ColliderCategory2D ForcesCategory { get; set; }

        [RenderProperty(Tag = 12)]
        [DataMember]
        public bool CollideWithFloor;

        [RenderProperty(AttatchToTag = 12, AttachToValue = true)]
        [DataMember]
        public ParticleCollisionType CollisionType;

        [RenderProperty(AttatchToTag = 12, AttachToValue = true)]
        [DataMember]
        public float FloorLevel;

        [RenderProperty(AttatchToTag = 12, AttachToValue = true)]
        [DataMember]
        public float FloorBounciness;

        [DataMember]
        public bool ZOrdering;

        [RenderPropertyAsInput(MinLimit = 0, MaxLimit = ushort.MaxValue)]
        [DataMember]
        public int MaxParticles;

        [DataMember]
        public bool PremultiplyAlpha;
        #endregion

        #region Initialize

        /// <summary>
        ///     Initializes a new instance of the <see cref="NewParticleSystem" /> class.
        /// </summary>
        public NewParticleSystem()
            : base("ParticleSystem" + instances++)
        {
        }

        /// <summary>
        /// Set default values
        /// </summary>
        protected override void DefaultValues()
        {
            base.DefaultValues();

            ////this.ShapeType = ShapeType.Point;

            this.LayerId = DefaultLayers.Additive;
            this.emitType = EmitType.Rate;
            this.emitDuration = 0;
            this.emitAutomatically = true;
            this.EmitRate = 100;
            this.MaxParticles = 1000;
            this.InitColor = Color.White;
            this.InitLife = 1;
            this.InitSpeed = 1;
            this.InitSize = 0.1f;
            this.ForcesCategory = ColliderCategory2D.Cat1;
            this.Space = SpaceEnum.World;
            this.LifeFactor = 1;
            this.TimeFactor = 1;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.RefreshMaterial();
            this.material.Initialize(this.Assets);

            if (random == null)
            {
                random = WaveServices.FastRandom;
            }

            this.isEmitting = this.emitAutomatically;

            if (this.shape == null)
            {
                this.ShapeType = ShapeType.Point;
            }

            if (!this.shape.IsInitialized)
            {
                this.shape.BaseInitialize(this);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void ResetParticle(ref Particle p)
        {
            Vector3 position;
            Vector3 direction;

            p.Generation++;

            if (this.shape != null)
            {
                this.Shape.NextParticle(out position, out direction);
            }
            else
            {
                position = Vector3.Zero;
                direction = Vector3.UnitY;
            }

            if (this.InitSpeedType == ValueType.Constant)
            {
                p.InitVelocity = this.InitSpeed * direction;
            }
            else
            {
                p.InitVelocity = MathHelper.Lerp(this.MinSpeed, this.MaxSpeed, (float)random.NextDouble()) * direction;
            }

            if (this.InitRotationType == ValueType.Constant)
            {
                p.Angle = this.InitRotation;
            }
            else
            {
                p.Angle = MathHelper.Lerp(this.MaxRotation, this.MinRotation, (float)random.NextDouble());
            }

            if (this.AngularVelocityType == ValueType.Constant)
            {
                p.AngularVelocity = this.AngularVelocity;

            }
            else
            {
                p.AngularVelocity = MathHelper.Lerp(this.MaxAngularVelocity, this.MinAngularVelocity, (float)random.NextDouble());
            }

            if (this.InitLifeType == ValueType.Constant)
            {
                p.LifeTime = this.InitLife;
            }
            else
            {
                p.LifeTime = MathHelper.Lerp(this.MinLife, this.MaxLife, (float)random.NextDouble());
            }

            if (this.InitSizeType == ValueType.Constant)
            {
                p.InitSize = this.InitSize;
            }
            else
            {
                p.InitSize = MathHelper.Lerp(this.MinSize, this.MaxSize, (float)random.NextDouble());
            }

            if (this.InitColorType == ValueType.Constant)
            {
                p.InitColor = this.InitColor;
            }
            else
            {
                var color1 = this.MinColor;
                var color2 = this.MaxColor;

                p.InitColor = Color.Lerp(ref color1, ref color2, (float)random.NextDouble());
            }

            // Init values
            p.Alive = true;
            p.Position = position;
            p.Velocity = p.InitVelocity;
            p.LifeLerp = 0;
            p.Direction = direction;
            p.Size = p.InitSize;
            p.Color = p.InitColor;
            p.Forces = Vector3.Zero;

            if (this.shape != null)
            {
                p.Velocity += this.shape.VelocityOffset;
            }
        }

        private void RefreshMaterial()
        {
            if (this.material == null)
            {
                this.material = new PointSpriteMaterial();
            }

            this.material.TexturePath = this.texturePath;
            this.material.DepthBias = this.depthBias;

            this.material.LayerId = this.layerId;
        }
        #endregion
    }
}