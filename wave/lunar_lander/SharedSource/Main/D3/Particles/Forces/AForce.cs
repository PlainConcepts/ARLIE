using RLEnvs.D3.Particles;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Math;
using WaveEngine.Common.Physics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;

namespace Batchers.Particles.Forces
{
    [DataContract]
    public abstract class AForce : Component
    {
        [RequiredComponent]
        protected Transform3D transform;

        [DataMember]
        private float range;

        [DataMember]
        protected float rangeSquared;

        private ForcesManager forcesManager;

        [DataMember]
        public bool IsEnabled { get; set; }

        [RenderPropertyAsBitwise]
        [DataMember]
        public ColliderCategory2D Category { get; set; }

        [RenderProperty(Tag = 1)]
        [DataMember]
        public bool IsRanged { get; set; }

        [RenderPropertyAsFInput(AttatchToTag = 1, AttachToValue = true, MinLimit = 0)]
        [DataMember]
        public float Range
        {
            get
            {
                return this.range;
            }

            set
            {
                this.range = value;
                this.rangeSquared = value * value;
            }
        }

        [DataMember]
        public bool Decay { get; set; }

        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.IsRanged = false;
            this.Category = ColliderCategory2D.All;
            this.range = 1;
            this.Decay = true;
            this.IsEnabled = true;
        }

        internal abstract void ApplyForce(NewParticleRenderer particleRenderer, float time, ref Particle p);

        protected override void Initialize()
        {
            base.Initialize();
            this.rangeSquared = this.range * this.range;

            SearchForceManager();

            this.RegisterForce();
        }

        private void SearchForceManager()
        {
            foreach (var e in this.EntityManager.AllEntities)
            {
                var f = e.FindComponent<ForcesManager>();
                if (f != null)
                {
                    this.forcesManager = f;
                    break;
                }
            }
        }

        protected override void Removed()
        {
            base.Removed();
            this.UnRegisterForce();
        }

        protected override void DeleteDependencies()
        {
            base.DeleteDependencies();
            this.UnRegisterForce();
        }

        private void RegisterForce()
        {
            if (this.forcesManager != null)
            {
                this.forcesManager.RegisterForce(this);
            }
        }

        private void UnRegisterForce()
        {
            if (this.forcesManager != null)
            {
                this.forcesManager.UnRegisterForce(this);
            }
        }
    }
}
