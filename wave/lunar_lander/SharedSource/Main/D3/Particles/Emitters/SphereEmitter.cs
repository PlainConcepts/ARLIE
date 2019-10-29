using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Services;

namespace RLEnvs.D3.Particles.Emitters
{
    [DataContract]
    public class SphereEmitter : AEmitter
    {
        public override ShapeType ShapeType
        {
            get
            {
                return ShapeType.Sphere;
            }
        }

        [DataMember]
        public float Radius { get; set; }

        [DataMember]
        public bool FromSurface { get; set; }

        [DataMember]
        public float Whirl { get; set; }

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.FromSurface = false;
            this.Radius = 1;
        }

        internal override void NextParticle(out Vector3 position, out Vector3 velocity)
        {
            Vector3 randomVector;

            if (this.FromSurface)
            {
                randomVector = this.random.OnUnitSphere();                
            }
            else
            {
                randomVector = this.random.InUnitSphere();
            }

            Matrix world = this.owner.Transform.WorldTransform;
            position = randomVector * this.Radius;

            if (Math.Abs(this.Whirl) > MathHelper.Epsilon)
            {
                var up = Vector3.Up;                
                Vector3 cross;
                Vector3.Cross(ref up, ref position, out cross);

                var length = position.LengthSquared();

                cross *= this.Whirl / length;

                velocity = cross;
            }
            else
            {
                velocity = randomVector;
            }

            if (this.Randomize > 0f)
            {
                velocity.X = (velocity.X * (1 - this.Randomize)) + (float)((this.random.NextDouble() - 0.5f) * this.Randomize * 2);
                velocity.Y = (velocity.Y * (1 - this.Randomize)) + (float)((this.random.NextDouble() - 0.5f) * this.Randomize * 2);
                velocity.Z = (velocity.Z * (1 - this.Randomize)) + (float)((this.random.NextDouble() - 0.5f) * this.Randomize * 2);

                Vector3.TransformNormal(ref velocity, ref world, out velocity);

                velocity.Normalize();
            }
            
            if (this.owner.Space == NewParticleSystem.SpaceEnum.World)
            {
                var transform = this.owner.Transform.WorldTransform;

                Vector3.Transform(ref position, ref transform, out position);
                Vector3.TransformNormal(ref velocity, ref transform, out velocity);
            }
        }
    }
}
