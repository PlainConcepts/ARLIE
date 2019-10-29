using System;
using System.Runtime.Serialization;
using WaveEngine.Common.Math;

namespace RLEnvs.D3.Particles.Emitters
{
    [DataContract]
    internal class PointEmitter : AEmitter
    {
        public override ShapeType ShapeType
        {
            get
            {
                return ShapeType.Point;
            }
        }

        internal override void NextParticle(out Vector3 position, out Vector3 velocity)
        {
            position = Vector3.Zero;
            velocity = Vector3.UnitY;

            if (this.owner.Space == NewParticleSystem.SpaceEnum.World)
            {
                var transform = this.owner.Transform.WorldTransform;
                Vector3.Transform(ref position, ref transform, out position);
                Vector3.TransformNormal(ref velocity, ref transform, out velocity);
            }
        }
    }
}