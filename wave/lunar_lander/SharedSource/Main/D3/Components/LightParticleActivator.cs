using RLEnvs.D3.Model;
using RLEnvs.D3.Particles;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace RLEnvs.D3.Components
{
    [AllowMultipleInstances]
    [DataContract]
    public class LightParticleActivator : Behavior
    {
        [RequiredComponent(false)]
        protected LightProperties light;

        protected NewParticleRenderer particles;

        private float initIntensity;

        protected override void DefaultValues()
        {
            base.DefaultValues();
        }

        protected override void ResolveDependencies()
        {
            base.ResolveDependencies();

            this.particles = this.Owner.FindComponentInParents<NewParticleRenderer>();
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.initIntensity = this.light.Intensity;
        }

        protected override void Update(TimeSpan gameTime)
        {
            this.light.Intensity = this.initIntensity * this.particles.AliveParticles / (float) this.particles.NumParticles;
        }
    }
}