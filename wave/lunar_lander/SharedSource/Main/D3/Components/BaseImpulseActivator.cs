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
    public abstract class BaseImpulseActivator : Component
    {
        [DataMember]
        public EngineActions Action;

        public abstract bool ImpulseActive { get; set; }

        protected override void Initialize()
        {
            base.Initialize();

            if (!WaveServices.Platform.IsEditor)
            {
                this.ImpulseActive = false;
            }
        }
    }
}
