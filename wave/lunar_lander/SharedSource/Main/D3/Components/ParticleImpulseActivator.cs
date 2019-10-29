using RLEnvs.D3.Model;
using RLEnvs.D3.Particles;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;

namespace RLEnvs.D3.Components
{
    [AllowMultipleInstances]
    [DataContract]
    public class ParticleImpulseActivator : BaseImpulseActivator
    {
        [RequiredComponent]
        protected NewParticleSystem particles;

        public override bool ImpulseActive
        {
            get
            {
                return this.particles.IsEmitting;
            }

            set
            {
                this.particles.IsEmitting = value;
            }
        }
    }
}
