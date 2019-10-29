using RLEnvs.D3.Particles;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;

namespace RLEnvs.D3.Components
{
    [DataContract]
    public class DustBehavior : Behavior
    {
        [RequiredComponent]
        private Transform3D transform;

        private Transform3D gazeTransform;

        private NewParticleSystem particles;

        private float maxDistance;

        private float maxDistanceSquared;

        [RenderPropertyAsEntity(new string[] { "WaveEngine.Framework.Graphics.Transform3D" })]
        [DataMember]
        public string GazeEntity;

        internal bool MainEngine;

        [DataMember]
        public float MaxDistance
        {
            get
            {
                return this.maxDistance;
            }

            set
            {
                this.maxDistance = value;
                this.maxDistanceSquared = value * value;
            }
        }

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.MaxDistance = 6;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.gazeTransform = this.Owner.Find(this.GazeEntity)?.FindComponent<Transform3D>();

            this.particles = this.Owner.FindComponentInChildren<NewParticleSystem>();
        }

        protected override void Update(TimeSpan gameTime)
        {
            if(!this.MainEngine)
            {
                this.particles.IsEmitting = false;
                return;
            }
            
            if ((this.gazeTransform != null) && (this.Owner.Scene.PhysicsManager.Simulation3D.InternalWorld != null))
            {
                var position = this.transform.Position;

                // Create ray
                Ray ray = new Ray(position, this.transform.WorldTransform.Down);

                // Checks collision
                var result = this.Owner.Scene.PhysicsManager.Simulation3D.RayCast(ref ray, 1000);

                Vector3.DistanceSquared(ref result.Point, ref position, out float distanceSquared);
                var succeed = result.Succeeded && (distanceSquared < this.maxDistanceSquared);

                this.particles.IsEmitting = succeed;

                if (succeed)
                {
                    this.gazeTransform.Position = result.Point;
                    this.gazeTransform.Rotation = result.Normal; //.LookAt(result.Point + result.Normal);
                }
            }
        }
    }
}
