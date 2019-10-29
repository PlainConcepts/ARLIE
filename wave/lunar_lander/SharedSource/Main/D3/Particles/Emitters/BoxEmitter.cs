using System;
using System.Runtime.Serialization;
using WaveEngine.Common.Math;

namespace RLEnvs.D3.Particles.Emitters
{
    [DataContract]
    internal class BoxEmitter : AEmitter
    {
        public override ShapeType ShapeType
        {
            get
            {
                return ShapeType.Box;
            }
        }

        internal override void NextParticle(out Vector3 position, out Vector3 velocity)
        {
            position = this.owner.Transform.Position;
            velocity = Vector3.UnitY;
        }
    }
}