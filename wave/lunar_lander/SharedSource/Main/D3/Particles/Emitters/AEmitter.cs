using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.IO;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Services;

namespace RLEnvs.D3.Particles.Emitters
{
    [DataContract]
    public abstract class AEmitter : SerializableObject
    {
        public abstract ShapeType ShapeType { get; }

        protected FastRandom random;

        protected NewParticleSystem owner;

        public bool IsInitialized { get; private set; }

        [DataMember]
        public Vector3 VelocityOffset { get; set; }

        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.random = WaveServices.FastRandom;
        }

        [RenderPropertyAsFInput(MinLimit = 0, MaxLimit = 1, RenderPosition = RenderPosition.End)]
        [DataMember]
        public float Randomize { get; set; }

        internal abstract void NextParticle(out Vector3 position, out Vector3 velocity);

        public void BaseInitialize(NewParticleSystem owner)
        {
            this.owner = owner;
            this.Initialize();
            this.IsInitialized = true;
        }

        protected virtual void Initialize()
        {
        }
    }
}
