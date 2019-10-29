using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Attributes;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace RLEnvs.D3.Components
{
    [DataContract]
    public class DualTargetCamera : Component
    {
        [RequiredComponent]
        protected Transform3D transform;

        [RequiredComponent]
        protected Camera3D camera;

        private string entityPathA;
        private string entityPathB;

        private Entity entityA;
        private Entity entityB;

        [DataMember]
        [RenderPropertyAsEntity(new string[] { "WaveEngine.Framework.Graphics.Transform3D" }, CustomPropertyName = "Follow Entity B", Tooltip = "The target entity")]
        public string EntityPathA
        {
            get
            {
                return this.entityPathA;
            }

            set
            {
                this.entityPathA = value;

                if (this.isInitialized)
                {
                    this.entityA = EntityManager.Find(this.entityPathA);
                }
            }
        }

        [DataMember]
        [RenderPropertyAsEntity(new string[] { "WaveEngine.Framework.Graphics.Transform3D" }, CustomPropertyName = "Follow Entity A", Tooltip = "The target entity")]
        public string EntityPathB
        {
            get
            {
                return this.entityPathB;
            }

            set
            {
                this.entityPathB = value;

                if (this.isInitialized)
                {
                    this.entityB = EntityManager.Find(this.entityPathB);
                }
            }
        }

        public DualTargetCamera()
            : base("DualTargetCamera")
        {
        }

        protected override void Initialize()
        {
            base.Initialize();

            if (!WaveServices.Platform.IsEditor)
            {
                if (this.entityA == null
                    && this.entityB == null
                    && !string.IsNullOrEmpty(this.entityPathA)
                    && !string.IsNullOrEmpty(this.entityPathB))
                {
                    this.entityA = this.Owner.Find(this.entityPathA);
                    this.entityB = this.Owner.Find(this.entityPathB);

                    this.Owner.Scene.AddSceneBehavior(new DualTargetCameraSceneBehavior(
                            this.Owner,
                            this.entityA,
                            this.entityB),
                            SceneBehavior.Order.PostUpdate);
                }
            }
        }
    }
}
