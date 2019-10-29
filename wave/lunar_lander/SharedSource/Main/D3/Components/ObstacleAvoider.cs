using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;

namespace RLEnvs.D3.Components
{
    [DataContract]
    public class ObstacleAvoider : Behavior
    {
        [RequiredComponent]
        private Transform3D transform;

        [RequiredComponent]
        private Transform3D parentTransform;

        private Vector3 initPosition;

        private Vector3 initDirection;

        private float initDistance;
        private float initDistanceSquared;

        protected override void DefaultValues()
        {
            base.DefaultValues();
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.parentTransform = this.Owner.Parent.FindComponent<Transform3D>();

            this.initPosition = this.transform.LocalPosition;
            this.initDirection = this.initPosition;
            this.initDirection.Normalize();

            this.initDistance = this.transform.LocalPosition.Length();
            this.initDistanceSquared = this.initDistance * this.initDistance;
        }

        protected override void Update(TimeSpan gameTime)
        {
            var targetPos = this.parentTransform.Position + this.initPosition;
            var rayStart = targetPos  + (Vector3.UnitY * 100);
            var rayDir = -Vector3.UnitY;

            // Create ray
            Ray ray = new Ray(rayStart, rayDir);

            // Checks collision
            var result = this.Owner.Scene.PhysicsManager.Simulation3D.RayCast(ref ray, 1000);

            if (result.Succeeded && (result.Point.Y + 1 > targetPos.Y))
            {
                this.transform.Position = result.Point + Vector3.UnitY;
            }
            else
            {
                this.transform.LocalPosition = this.initPosition;
            }

            this.transform.LookAt(this.parentTransform.Position);
        }
    }
}
