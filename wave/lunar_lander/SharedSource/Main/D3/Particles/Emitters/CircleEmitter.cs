using System.Runtime.Serialization;
using WaveEngine.Common.Math;

namespace RLEnvs.D3.Particles.Emitters
{
    [DataContract]
    internal class CircleEmitter : AEmitter
    {
        public override ShapeType ShapeType
        {
            get
            {
                return ShapeType.Circle;
            }
        }

        [DataMember]
        public float Radius { get; set; }

        [DataMember]
        public bool FromSurface { get; set; }

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.FromSurface = false;
            this.Radius = 1;
        }

        ////internal override void NextParticle(out Vector3 position, out Vector3 velocity)
        ////{
        ////    position = this.owner.Transform.Position;
        ////    velocity = Vector3.UnitY;
        ////}

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

            position = randomVector * this.Radius;
            position.Y = 0;
            velocity = Vector3.UnitY;

            if (this.Randomize > 0f)
            {
                velocity.X = (velocity.X * (1 - this.Randomize)) + (float)((this.random.NextDouble() - 0.5f) * this.Randomize * 2);
                velocity.Y = (velocity.Y * (1 - this.Randomize)) + (float)((this.random.NextDouble() - 0.5f) * this.Randomize * 2);
                velocity.Z = (velocity.Z * (1 - this.Randomize)) + (float)((this.random.NextDouble() - 0.5f) * this.Randomize * 2);

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