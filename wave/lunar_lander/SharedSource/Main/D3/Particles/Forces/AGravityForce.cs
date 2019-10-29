using System;
using System.Collections.Generic;
using System.Text;
using RLEnvs.D3.Particles;
using WaveEngine.Common.Math;
using System.Runtime.Serialization;
using WaveEngine.Common.Attributes;

namespace Batchers.Particles.Forces
{
    [DataContract]
    public abstract class AGravityForce : AForce
    {
        [DataMember]
        public float Gravity { get; set; }

        [RenderProperty(Tag = 2)]
        [DataMember]
        public bool IsSolid{ get; set; }

        [RenderProperty(AttatchToTag = 2, AttachToValue = true)]
        [DataMember]
        public float Cutout { get; set; }

        [RenderPropertyAsFInput(MaxLimit = 1.0f, MinLimit = 0, AttachToValue = true, AttatchToTag = 2)]
        [DataMember]
        public float CutoutLerp{ get; set; }


        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.Gravity = 1;
            this.Cutout = 0;
            this.CutoutLerp = 0.1f;
        }

        internal override void ApplyForce(NewParticleRenderer particleRenderer, float time, ref Particle p)
        {
            Vector3 forcePos;
            this.GetForcePosition(particleRenderer, time, ref p, out forcePos);
            
            var distance = Vector3.DistanceSquared(forcePos, p.Position);
            if (this.IsRanged && distance > this.rangeSquared)
            {
                return;
            }

            if (this.IsSolid && distance < this.Cutout)
            {
                p.Velocity = Vector3.Zero;
                p.Forces = Vector3.Zero;

                Vector3.Lerp(ref p.Position, ref forcePos, this.CutoutLerp, out p.Position);
                ////p.Position = forcePos;
            }
            else
            {
                if (this.IsRanged && distance > this.rangeSquared)
                {
                    return;
                }

                var distanceVector = (forcePos - p.Position);
                distanceVector.Normalize();

                var f = this.Gravity * time * distanceVector;

                if (this.Decay)
                {
                    float d;
                    if (this.IsRanged)
                    {
                        d = (1 - (distance / this.rangeSquared));
                        f *= (d * d);
                    }
                    else
                    {
                        d = (1 / distance);
                        f *= (d * d);
                    }
                    
                }

                p.Forces += f;
            }
        }

        protected abstract void GetForcePosition(NewParticleRenderer particleRenderer, float time, ref Particle p, out Vector3 pos);
    }
}
