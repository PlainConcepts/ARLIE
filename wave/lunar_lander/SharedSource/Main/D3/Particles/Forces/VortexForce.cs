using System;
using System.Collections.Generic;
using System.Text;
using RLEnvs.D3.Particles;
using WaveEngine.Common.Math;
using System.Runtime.Serialization;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework;

namespace Batchers.Particles.Forces
{
    [DataContract]
    public class VortexForce : AForce
    {
        [DataMember]
        public float Force { get; set; }

        [RequiredComponent]
        public Transform3D transform = null;
        
        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.Force = 1;
        }

        internal override void ApplyForce(NewParticleRenderer particleRenderer, float time, ref Particle p)
        {
            var distance = Vector3.DistanceSquared(this.transform.Position, p.Position);
            if (this.IsRanged && distance > this.rangeSquared)
            {
                return;
            }

            var up = this.transform.WorldTransform.Up;
            var r = this.transform.Position - p.Position;
            var v = Vector3.Cross(up, r);
            v.Normalize();

            float factor = 0;

            if (this.Decay)
            {
                ////if (this.IsRanged)
                ////{
                ////    factor = 1 / (1 + (distance / this.Force));
                ////}
                ////else
                ////{
                    
                ////}
                factor = 1 / (1 + (distance / this.Force));
            }
            else
            {
                factor = this.Force;
            }

            var f = v * factor;
            p.Forces += f;

            ////p.Velocity = Vector3.Lerp(p.Velocity, f, 0.03f);
        }
    }
}
