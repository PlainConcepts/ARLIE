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
    public class EntityFollowerSceneBehavior : SceneBehavior
    {
        protected Transform3D followerTransform;

        private Transform3D targetTransform;
        private Vector3 positionOffset;
        private bool followCamera;

        public EntityFollowerSceneBehavior(Entity follower, Entity target)
            : base("EntityFollowerSceneBehavior")
        {
            this.followCamera = target == null;

            this.followerTransform = follower.FindComponent<Transform3D>();

            if (!this.followCamera)
            {
                this.targetTransform = target?.FindComponent<Transform3D>();
                this.positionOffset = this.targetTransform.Position - this.followerTransform.Position;
            }
        }

        protected override void ResolveDependencies()
        {
        }

        protected override void Update(TimeSpan gameTime)
        {
            if (this.followCamera)
            {
                this.followerTransform.Position = this.Scene.RenderManager.ActiveCamera3D.Position;
            }
            else
            {
                this.followerTransform.Position = this.targetTransform.Position - this.positionOffset; ;
            }
        }
    }
}
