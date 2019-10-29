using System.Runtime.Serialization;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Services;

namespace RLEnvs.D3.Particles.Emitters
{
    [DataContract]
    internal class EdgeEmitter : AEmitter
    {
        public override ShapeType ShapeType
        {
            get
            {
                return ShapeType.Edge;
            }
        }

        [DataMember]
        public float Length { get; set; }

        internal override void NextParticle(out Vector3 position, out Vector3 velocity)
        {
            position = Vector3.UnitX * ((float)(this.random.NextDouble() - 0.5) * this.Length);


            if (this.Randomize == 0)
            {
                velocity = Vector3.UnitY;
            }
            else
            {
                velocity.X = (float)((this.random.NextDouble() - 0.5f) * this.Randomize * 2);
                velocity.Y = (1 - this.Randomize) + (float)((this.random.NextDouble() - 0.5f) * this.Randomize * 2);
                velocity.Z = (float)((this.random.NextDouble() - 0.5f) * this.Randomize * 2);
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