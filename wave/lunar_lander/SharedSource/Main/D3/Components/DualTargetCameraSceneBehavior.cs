using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Threading;

namespace RLEnvs.D3.Components
{
    public class DualTargetCameraSceneBehavior : SceneBehavior
    {
        private Transform3D transform;
        private Camera3D camera;

        private Transform3D targetTransformA;
        private Transform3D targetTransformB;

        private float sizeA;
        private float sizeB;

        private Vector3 eyeDir;

        private float orbitDistanceA;
        private float orbitDistanceB;

        private MouseState lastMouseState;

        private float minPitch;
        private float maxPitch;

        private float rotationSpeed;
        private float obstacleMargin;

        public DualTargetCameraSceneBehavior(Entity cameraEntity, Entity targetA, Entity targetB)
            : base("EntityFollowerSceneBehavior")
        {
            this.targetTransformA = targetA.FindComponent<Transform3D>();
            this.targetTransformB = targetB.FindComponent<Transform3D>();

            var bbA = targetA.FindComponent<MeshComponent>(false).BoundingBox.Value;
            var bbB = targetB.FindComponent<MeshComponent>(false).BoundingBox.Value;

            this.sizeA = bbA.HalfExtent.Length() * this.targetTransformA.Scale.X;
            this.sizeB = bbB.HalfExtent.Length() * this.targetTransformB.Scale.X;

            this.transform = cameraEntity.FindComponent<Transform3D>();

            this.camera = cameraEntity.FindComponent<Camera3D>();
                
            this.eyeDir = this.transform.WorldTransform.Forward;

            this.transform.LookAt(this.transform.Position + this.eyeDir);
            this.maxPitch = MathHelper.ToRadians(0);
            this.minPitch = MathHelper.ToRadians(-70);
            this.rotationSpeed = 0.001f;
            this.obstacleMargin = 2;
        }

        protected override void ResolveDependencies()
        {
        }

        protected override void Update(TimeSpan gameTime)
        {
            this.UpdateCameraDirection(gameTime);

            BoundingFrustum frustum = this.camera.BoundingFrustum;

            // Calculate 4 planes
            Plane planeT, planeB, planeR, planeL;
            this.CheckPlane(frustum.Top, out planeT);
            this.CheckPlane(frustum.Bottom, out planeB);
            this.CheckPlane(frustum.Right, out planeR);
            this.CheckPlane(frustum.Left, out planeL);

            // Do magic to obtain position from 4 planes
            Ray rayTB, rayRL, rayTBRL;
            planeT.Intersects(ref planeB, out rayTB);
            planeR.Intersects(ref planeL, out rayRL);

            Plane planeTB = new Plane(rayTB.Position, rayTB.Position + rayTB.Direction, rayTB.Position + this.eyeDir);
            Plane planeRL = new Plane(rayRL.Position, rayRL.Position + rayRL.Direction, rayRL.Position + this.eyeDir);
            planeTB.Intersects(ref planeRL, out rayTBRL);

            Plane planeTBC = new Plane(rayTB.Position, rayTB.Position + rayTB.Direction, rayTB.Position + rayRL.Direction);
            Plane planeRLC = new Plane(rayRL.Position, rayRL.Position + rayRL.Direction, rayRL.Position + rayTB.Direction);

            float? distanceTBC = rayTBRL.Intersects(planeTBC, false);
            float? distanceRLC = rayTBRL.Intersects(planeRLC, false);
            float distance = Math.Min(distanceTBC.Value, distanceRLC.Value);

            var position = rayTBRL.Position + rayTBRL.Direction * distance;

            var rayStart = position + (Vector3.UnitY * 100);
            var rayDir = -Vector3.UnitY;

            // Create ray
            Ray ray = new Ray(rayStart, rayDir);

            // Checks collision
            var result = this.Scene.PhysicsManager.Simulation3D.RayCast(ref ray, 1000);

            if (result.Succeeded && (result.Point.Y + this.obstacleMargin > position.Y))
            {
                position = result.Point + Vector3.UnitY * this.obstacleMargin;
            }

            this.transform.Position = position; //Vector3.SmoothStep(this.transform.Position, position, this.smooth);
        }

        private void UpdateCameraDirection(TimeSpan gameTime)
        {
            var mouseState = WaveServices.Input.MouseState;

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                var deltaX = mouseState.X - this.lastMouseState.X;
                var deltaY = mouseState.Y - this.lastMouseState.Y;

                var rot = this.transform.Rotation;
                rot.Y += deltaX * this.rotationSpeed;
                rot.X -= deltaY * this.rotationSpeed;

                rot.X = Math.Min(Math.Max(rot.X, this.minPitch), this.maxPitch);

                this.transform.Rotation = rot;

                this.eyeDir = this.transform.WorldTransform.Forward;
            }

            this.lastMouseState = mouseState;
        }

        private void CheckPlane(Plane plane, out Plane result)
        {
            result.Normal = plane.Normal;

            float distanceToPlane;

            CheckTrackDistance(ref plane, this.targetTransformA, this.sizeA, out float distanceA);
            CheckTrackDistance(ref plane, this.targetTransformB, this.sizeB, out float distanceB);

            distanceToPlane = (distanceA < distanceB) ? distanceA : distanceB;

            result.D = distanceToPlane + plane.D;
        }

        private static void CheckTrackDistance(ref Plane plane, Transform3D transform, float size, out float distance)
        {
            distance = -plane.DotCoordinate(transform.Position);
            distance -= size;
        }
    }
}
