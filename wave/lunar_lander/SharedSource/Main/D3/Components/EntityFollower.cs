using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Threading;

namespace RLEnvs.D3.Components
{
    [DataContract]
    public class EntityFollower : Component
    {
        [RequiredComponent(false)]
        protected Transform3D transform;

        private string followEntityPath;

        private Entity followEntity;

        [DataMember]
        [RenderPropertyAsEntity(new string[] { "WaveEngine.Framework.Graphics.Transform3D" }, CustomPropertyName = "Follow Entity", Tooltip = "The target entity")]
        public string FollowEntityPath
        {
            get
            {
                return this.followEntityPath;
            }

            set
            {
                this.followEntityPath = value;

                if (this.isInitialized)
                {
                    this.followEntity = EntityManager.Find(this.followEntityPath);
                }
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

            if (!WaveServices.Platform.IsEditor)
            {
                if (this.followEntity == null && !string.IsNullOrEmpty(this.followEntityPath))
                {
                    this.followEntity = this.Owner.Find(this.followEntityPath);
                    this.Owner.Scene.AddSceneBehavior(new EntityFollowerSceneBehavior(
                            this.Owner,
                            this.followEntity/*,
                                this.Smooth*/),
                            SceneBehavior.Order.PostUpdate);
                }
            }
        }
    }
}
