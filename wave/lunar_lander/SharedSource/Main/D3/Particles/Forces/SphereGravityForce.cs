using System;
using System.Collections.Generic;
using System.Text;
using RLEnvs.D3.Particles;
using WaveEngine.Common.Math;
using System.Runtime.Serialization;
using WaveEngine.Framework;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework.Services;
using WaveEngine.Common.Graphics;

namespace Batchers.Particles.Forces
{
    [DataContract]
    public class SphereGravityForce : AGravityForce
    {
        private Dictionary<NewParticleRenderer, int[]> particleCache;
        private NewParticleRenderer currentRenderer;
        private int[] currentParticleIndices;

        private Vector3[] sphereSamples;
        private Vector3[] transformedSphereSamples;

        [RequiredService]
        WaveEngine.Framework.Services.Random random;

        [DataMember]
        private int nSamples;

        private bool samplesDirty;

        public int NSamples
        {
            get
            {
                return this.nSamples;
            }

            set
            {
                if (this.nSamples != value)
                {
                    this.nSamples = value;
                    this.samplesDirty = true;
                }
            }
        }

        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.particleCache = new Dictionary<NewParticleRenderer, int[]>();
            this.nSamples = 200;
            this.samplesDirty = true;
        }
        
        protected override void Initialize()
        {
            base.Initialize();
            
            this.transform.TransformChanged += Transform_TransformChanged;
        }

        private void Transform_TransformChanged(object sender, EventArgs e)
        {
            this.TrasformSamples();
        }

        private void TrasformSamples()
        {
            if (this.sphereSamples != null)
            {
                Matrix w = this.transform.WorldTransform;

                for (int i = 0; i < this.nSamples; i++)
                {
                    Vector3.Transform(ref this.sphereSamples[i], ref w, out this.transformedSphereSamples[i]);
                } 
            }
        }

        internal override void ApplyForce(NewParticleRenderer particleRenderer, float time, ref Particle p)
        {
            if (this.currentRenderer != particleRenderer || this.samplesDirty)
            {
                this.currentRenderer = particleRenderer;
                this.RefreshCache();
            }
            
            if (this.currentParticleIndices == null)
            {
                return;
            }

            base.ApplyForce(particleRenderer, time, ref p);
        }

        private void RefreshDistribution()
        {
            Array.Resize(ref this.sphereSamples, this.nSamples);
            Array.Resize(ref this.transformedSphereSamples, this.nSamples);

            for (int i = 0; i < this.nSamples; i++)
            {
                this.sphereSamples[i] = this.random.InUnitSphere();
            }

            this.TrasformSamples(); 
        }

        private void RefreshCache()
        {
            if (this.currentRenderer != null)
            {
                if (this.samplesDirty)
                {
                    this.samplesDirty = false;
                    this.RefreshDistribution();
                }

                if (!this.particleCache.TryGetValue(this.currentRenderer, out this.currentParticleIndices)
                    || (this.currentParticleIndices.Length != this.currentRenderer.Particles.Length))
                {
                    this.currentParticleIndices = new int[this.currentRenderer.Particles.Length];
                    this.particleCache.Add(this.currentRenderer, this.currentParticleIndices);
                    
                    int nVertices = this.sphereSamples.Length;

                    for (int i = 0; i < this.currentParticleIndices.Length; i++)
                    {
                        this.currentParticleIndices[i] = i % nVertices;
                    }
                }
            }
        }
        
        protected override void GetForcePosition(NewParticleRenderer particleRenderer, float time, ref Particle p, out Vector3 pos)
        {            
            pos = this.transformedSphereSamples[this.currentParticleIndices[p.Id]];
        }
    }
}
