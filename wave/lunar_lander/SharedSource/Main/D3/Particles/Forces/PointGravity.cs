using System;
using System.Collections.Generic;
using System.Text;
using RLEnvs.D3.Particles;
using WaveEngine.Common.Math;
using System.Runtime.Serialization;

namespace Batchers.Particles.Forces
{
    [DataContract]
    public class PointGravity : AGravityForce
    {
        protected override void GetForcePosition(NewParticleRenderer particleRenderer, float time, ref Particle p, out Vector3 pos)
        {
            pos = this.transform.Position; 
        }
    }
}
