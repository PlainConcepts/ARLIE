using System;
using System.Runtime.Serialization;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Attributes.Converters;
using WaveEngine.Common.Math;

namespace RLEnvs.D3.Particles.Emitters
{
    [DataContract]
    internal class ConeEmitter : AEmitter
    {
        public enum EmitConeType
        {
            Base,
            BaseSurface,
            Surface,
            Volume
        }

        public override ShapeType ShapeType
        {
            get
            {
                return ShapeType.Cone;
            }
        }

        [DataMember]
        private float radius;

        [DataMember]
        private float angle;

        [DataMember]
        private float length;

        private float gradient;

        [RenderPropertyAsFInput(MinLimit = 0)]
        public float Radius
        {
            get
            {
                return this.radius;
            }

            set
            {
                this.radius = value;
                this.RefreshGradient();
            }
        }

        [RenderPropertyAsFInput(MinLimit = 0, MaxLimit = 1.57f/*, ConverterType = typeof(FloatRadianToDegreeConverter)*/)]
        public float Angle
        {
            get
            {
                return this.angle;
            }
            set
            {
                this.angle = value;
                this.RefreshGradient();
            }
        }

        public float Length
        {
            get
            {
                return this.length;
            }

            set
            {
                this.length = value;
                this.RefreshGradient();
            }
        }

        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.length = 1;
            this.radius = 0.5f;
            this.angle = 0.5f;
            this.ConeType = EmitConeType.Base;
        }

        [DataMember]
        public EmitConeType ConeType { get; set; }

        internal override void NextParticle(out Vector3 position, out Vector3 velocity)
        {
            var circleRandom = this.random.InsideUnitCircle();

            switch (this.ConeType)
            {
                default:
                case EmitConeType.Base:
                    velocity.X = circleRandom.X * this.gradient;
                    velocity.Y = 1;
                    velocity.Z = circleRandom.Y * this.gradient;
                    velocity.Normalize();

                    position.X = circleRandom.X * this.radius;
                    position.Y = 0;
                    position.Z = circleRandom.Y * this.radius;
                    break;
                case EmitConeType.BaseSurface:
                    velocity.X = circleRandom.X * this.gradient;
                    velocity.Y = 1;
                    velocity.Z = circleRandom.Y * this.gradient;
                    velocity.Normalize();

                    circleRandom.Normalize();
                    position.X = circleRandom.X * this.radius;
                    position.Y = 0;
                    position.Z = circleRandom.Y * this.radius;
                    break;
                case EmitConeType.Surface:
                    circleRandom.Normalize();
                    velocity.X = circleRandom.X * this.gradient;
                    velocity.Y = 1;
                    velocity.Z = circleRandom.Y * this.gradient;
                    velocity.Normalize();
                    var r = (float)this.random.NextDouble();
                    var r2 = this.radius + (r * this.length * this.gradient);
                    position.X = circleRandom.X * r2;
                    position.Y = r * this.length;
                    position.Z = circleRandom.Y * r2;
                    break;
                case EmitConeType.Volume:
                    velocity.X = circleRandom.X * this.gradient;
                    velocity.Y = 1;
                    velocity.Z = circleRandom.Y * this.gradient;
                    velocity.Normalize();
                    r = (float)this.random.NextDouble();
                    r2 = this.radius + (r * this.length * this.gradient);
                    position.X = circleRandom.X * r2;
                    position.Y = r * this.length;
                    position.Z = circleRandom.Y * r2;
                    break;
            }

            if (this.owner.Space == NewParticleSystem.SpaceEnum.World)
            {
                var transform = this.owner.Transform.WorldTransform;
                Vector3.Transform(ref position, ref transform, out position);
                Vector3.TransformNormal(ref velocity, ref transform, out velocity);
            }

            if (this.Randomize > 0f)
            {
                velocity.X = (velocity.X * (1 - this.Randomize)) + (float)((this.random.NextDouble() - 0.5f) * this.Randomize * 2);
                velocity.Y = (velocity.Y * (1 - this.Randomize)) + (float)((this.random.NextDouble() - 0.5f) * this.Randomize * 2);
                velocity.Z = (velocity.Z * (1 - this.Randomize)) + (float)((this.random.NextDouble() - 0.5f) * this.Randomize * 2);

                velocity.Normalize();
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
            this.RefreshGradient();
        }

        private void RefreshGradient()
        {
            this.gradient = (float)(Math.Sin(this.angle) / Math.Cos(this.angle));
        }
    }
}