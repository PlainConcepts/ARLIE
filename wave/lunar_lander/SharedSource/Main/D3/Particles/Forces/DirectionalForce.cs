using System;
using System.Collections.Generic;
using System.Text;
using RLEnvs.D3.Particles;
using WaveEngine.Common.Math;
using System.Runtime.Serialization;

namespace Batchers.Particles.Forces
{
    [DataContract]
    public class DirectionalForce : AForce
    {
        [DataMember]
        public float Amount { get; set; }

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.Amount = 1;
        }

        internal override void ApplyForce(NewParticleRenderer particleRenderer, float time, ref Particle p)
        {
            var distance = Vector3.DistanceSquared(this.transform.Position, p.Position);
            if (this.IsRanged && distance > this.rangeSquared)
            {
                return;
            }

            var f = time * this.Amount * this.transform.WorldTransform.Forward;

            if (this.Decay)
            {
                var d = (1 - (distance / this.rangeSquared));
                f *= (d * d);
            }

            p.Forces += f;
        }
    }
}
