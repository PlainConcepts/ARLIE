using RLEnvs.D3.Helper;
using RLEnvs.D3.Model;
using RLEnvs.Helpers;
using RLEnvs.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;
using Random = WaveEngine.Framework.Services.Random;

namespace RLEnvs.D3.Components
{

    [DataContract]
    public class ShipComponent : Behavior
    {
        private const int MaxActions = 10;

        [RequiredService]
        protected Random random;

        [RequiredComponent]
        protected RigidBody3D shipBody;

        [RequiredComponent]
        protected Transform3D transform;

        private RigidBody3D footFront;
        private RigidBody3D footBack;
        private RigidBody3D footLeft;
        private RigidBody3D footRight;

        private bool footFCollision;
        private bool footBCollision;
        private bool footLCollision;
        private bool footRCollision;
        private bool bodyCollision;

        private DustBehavior dust;

        private List<BaseImpulseActivator> allParticleActivators;

        private Dictionary<EngineActions, List<BaseImpulseActivator>> particleActivators;

        private EngineActions[] actionsQueue;

        private int numActions;

        private BoundingBox shipDeployBB;

        private float rotationImpulse = 0.5f;

        // Max empirical components, to perform data normalization
        private static float PositionNormFactor = 46f; //[-46 - 46]
        private static float VelocityNormFactor = 46f; //[-46 - 46]
        private static float AnguleNormFactor = (float)Math.PI; //[-PI - PI]
        private static float AngularVelocityNormFactor = 10f; //[-10 - 10]

        [RenderPropertyAsEntity(new string[] { "WaveEngine.Components.Graphics3D.MeshComponent" })]
        [DataMember]
        public string ShipDeployEntityPath;

        [RenderPropertyAsEntity(new string[] { "WaveEngine.Framework.Physics3D.RigidBody3D" })]
        [DataMember]
        public string FootFrontPath;

        [RenderPropertyAsEntity(new string[] { "WaveEngine.Framework.Physics3D.RigidBody3D" })]
        [DataMember]
        public string FootBackPath;

        [RenderPropertyAsEntity(new string[] { "WaveEngine.Framework.Physics3D.RigidBody3D" })]
        [DataMember]
        public string FootLeftPath;

        [RenderPropertyAsEntity(new string[] { "WaveEngine.Framework.Physics3D.RigidBody3D" })]
        [DataMember]
        public string FootRightPath;

        protected override void Initialize()
        {
            base.Initialize();

            this.actionsQueue = new EngineActions[MaxActions];
            this.numActions = 0;

            this.dust = this.Owner.FindComponentInChildren<DustBehavior>();

            this.particleActivators = new Dictionary<EngineActions, List<BaseImpulseActivator>>();
            this.allParticleActivators = new List<BaseImpulseActivator>();

            var components = this.EntityManager.FindComponentsOfType<BaseImpulseActivator>(false);
            foreach (var particleActivator in components)
            {
                if (!this.particleActivators.ContainsKey(particleActivator.Action))
                {
                    this.particleActivators[particleActivator.Action] = new List<BaseImpulseActivator>();
                }

                this.particleActivators[particleActivator.Action].Add(particleActivator);

                this.allParticleActivators.Add(particleActivator);
            }

            if (!string.IsNullOrEmpty(this.ShipDeployEntityPath)
                && !string.IsNullOrEmpty(this.FootFrontPath)
                && !string.IsNullOrEmpty(this.FootBackPath)
                && !string.IsNullOrEmpty(this.FootLeftPath)
                && !string.IsNullOrEmpty(this.FootRightPath))
            {
                var shipDeployEntity = this.Owner.Find(this.ShipDeployEntityPath);
                var shipDeployTransform = shipDeployEntity.FindComponent<Transform3D>();
                this.shipDeployBB = shipDeployEntity.FindComponent<MeshComponent>(false).BoundingBox.Value;
                this.shipDeployBB.Max = (this.shipDeployBB.Max * shipDeployTransform.Scale) + shipDeployTransform.Position;
                this.shipDeployBB.Min = (this.shipDeployBB.Min * shipDeployTransform.Scale) + shipDeployTransform.Position;


                this.footFront = this.Owner.Find(this.FootFrontPath).FindComponent<RigidBody3D>();
                this.footBack = this.Owner.Find(this.FootBackPath).FindComponent<RigidBody3D>();
                this.footLeft = this.Owner.Find(this.FootLeftPath).FindComponent<RigidBody3D>();
                this.footRight = this.Owner.Find(this.FootRightPath).FindComponent<RigidBody3D>();

                this.footFront.BeginCollision += FootFrontBody_BeginCollision;
                this.footFront.EndCollision += FootFrontBody_EndCollision;

                this.footBack.BeginCollision += FootBackBody_BeginCollision;
                this.footBack.EndCollision += FootBackBody_EndCollision;

                this.footLeft.BeginCollision += FootLeftBody_BeginCollision;
                this.footLeft.EndCollision += FootLeftBody_EndCollision;

                this.footRight.BeginCollision += FootRightBody_BeginCollision;
                this.footRight.EndCollision += FootRightBody_EndCollision;

                this.shipBody.BeginCollision += this.Body_BeginCollision;
                this.shipBody.EndCollision += this.Body_EndCollision;
            }
        }

        private void FootFrontBody_EndCollision(object sender, CollisionInfo3D e)
        {
            this.footFCollision = false;
        }

        private void FootFrontBody_BeginCollision(object sender, CollisionInfo3D e)
        {
            this.footFCollision = true;
        }

        private void FootBackBody_EndCollision(object sender, CollisionInfo3D e)
        {
            this.footBCollision = false;
        }

        private void FootBackBody_BeginCollision(object sender, CollisionInfo3D e)
        {
            this.footBCollision = true;
        }

        private void FootLeftBody_EndCollision(object sender, CollisionInfo3D e)
        {
            this.footLCollision = false;
        }

        private void FootLeftBody_BeginCollision(object sender, CollisionInfo3D e)
        {
            this.footLCollision = true;
        }

        private void FootRightBody_EndCollision(object sender, CollisionInfo3D e)
        {
            this.footRCollision = false;
        }

        private void FootRightBody_BeginCollision(object sender, CollisionInfo3D e)
        {
            this.footRCollision = true;
        }

        private void Body_EndCollision(object sender, CollisionInfo3D e)
        {
            this.bodyCollision = false;
        }

        private void Body_BeginCollision(object sender, CollisionInfo3D e)
        {
            this.bodyCollision = true;
        }

        protected override void Removed()
        {
            base.Removed();

            this.footFront.BeginCollision -= FootFrontBody_BeginCollision;
            this.footFront.EndCollision -= FootFrontBody_EndCollision;

            this.footBack.BeginCollision -= FootBackBody_BeginCollision;
            this.footBack.EndCollision -= FootBackBody_EndCollision;

            this.footLeft.BeginCollision -= FootLeftBody_BeginCollision;
            this.footLeft.EndCollision -= FootLeftBody_EndCollision;

            this.footRight.BeginCollision -= FootRightBody_BeginCollision;
            this.footRight.EndCollision -= FootRightBody_EndCollision;

            this.shipBody.BeginCollision -= Body_BeginCollision;
            this.shipBody.EndCollision -= Body_EndCollision;
        }

        private Vector3 GetInitialCenterPosition()
        {
            // Position XZ at the center, Y max
            return Vector3.UnitY * this.shipDeployBB.Max.Y;
        }

        private Vector3 GetInitialRandomPosition()
        {
            // Position XZ random between the center and 2/3 of max distance, Y max
            Vector3 position = this.GetInitialCenterPosition();

            var size = PositionNormFactor * 2;
            position.X = ((float)(this.random.NextDouble() * size) - PositionNormFactor) * (2f/3f);
            position.Z = ((float)(this.random.NextDouble() * size) - PositionNormFactor) * (2f/3f);
            return position;
        }

        private Vector3 GetInitialFarPosition()
        {
            // Position XZ 2/3 of max distance (the corner is random), Y max
            Vector3 position = this.GetInitialCenterPosition();

            float distance = PositionNormFactor * (2f/3f);
            uint corner = (uint)this.random.Next(4);

            switch (corner)
            {
                case 0:
                    position.X = distance;
                    position.Z = distance;
                    break;
                case 1:
                    position.X = -distance;
                    position.Z = distance;
                    break;
                case 2:
                    position.X = distance;
                    position.Z = -distance;
                    break;
                case 3:
                    position.X = -distance;
                    position.Z = -distance;
                    break;

            }
            return position;
        }

        public void Reset()
        {
            Vector3 shipPosition;

            switch(RLEnvs.Program.Arguments.ResetMode)
            {
                case RLEnvs.ProgramArguments.InitMode.far:
                    shipPosition = this.GetInitialFarPosition();
                    break;
                case RLEnvs.ProgramArguments.InitMode.random:
                    shipPosition = this.GetInitialRandomPosition();
                    break;
                case RLEnvs.ProgramArguments.InitMode.center:
                default:
                    shipPosition = this.GetInitialCenterPosition();
                    break;
            }

            var shipScale = this.transform.Scale;

            this.shipBody.InternalBody.SetTransform(shipPosition, Quaternion.Identity, this.shipBody.Transform3D.Scale);
            this.shipBody.AngularVelocity = Vector3.Zero;
            this.shipBody.LinearVelocity = Vector3.Zero;
            this.shipBody.ClearForces();

            this.footFront.InternalBody.SetTransform(shipPosition + (shipScale * this.footFront.Transform3D.LocalPosition), Quaternion.Identity, this.footFront.Transform3D.Scale);
            this.footFront.AngularVelocity = Vector3.Zero;
            this.footFront.LinearVelocity = Vector3.Zero;
            this.footFront.ClearForces();

            this.footBack.InternalBody.SetTransform(shipPosition + (shipScale * this.footBack.Transform3D.LocalPosition), Quaternion.Identity, this.footBack.Transform3D.Scale);
            this.footBack.AngularVelocity = Vector3.Zero;
            this.footBack.LinearVelocity = Vector3.Zero;
            this.footBack.ClearForces();

            this.footLeft.InternalBody.SetTransform(shipPosition + (shipScale * this.footLeft.Transform3D.LocalPosition), Quaternion.Identity, this.footLeft.Transform3D.Scale);
            this.footLeft.AngularVelocity = Vector3.Zero;
            this.footLeft.LinearVelocity = Vector3.Zero;
            this.footLeft.ClearForces();

            this.footRight.InternalBody.SetTransform(shipPosition + (shipScale * this.footRight.Transform3D.LocalPosition), Quaternion.Identity, this.footRight.Transform3D.Scale);
            this.footRight.AngularVelocity = Vector3.Zero;
            this.footRight.LinearVelocity = Vector3.Zero;
            this.footRight.ClearForces();

            this.ApplyInitialForce();
        }

        public void ApplyInitialForce()
        {
            var initialForce = this.random.InUnitSphere() * LunarConstants.INITIAL_RANDOM;
            this.shipBody.ApplyForce(initialForce * this.shipBody.Mass);
            this.footFront.ApplyForce(initialForce * this.footFront.Mass);
            this.footBack.ApplyForce(initialForce * this.footBack.Mass);
            this.footLeft.ApplyForce(initialForce * this.footLeft.Mass);
            this.footRight.ApplyForce(initialForce * this.footRight.Mass);
        }

        internal void RefreshObservations(Lunar3DNetworkService networkService)
        {
            networkService.LanderPosition = this.transform.Position / PositionNormFactor;
            networkService.LanderVelocity = this.shipBody.LinearVelocity / VelocityNormFactor;
            networkService.LanderAngle = this.transform.Rotation / AnguleNormFactor;
            networkService.LanderAngularVelocity = this.shipBody.AngularVelocity / AngularVelocityNormFactor;
            networkService.LanderBodyCollided = this.bodyCollision;
            networkService.LanderFootGroundedF = this.footFCollision;
            networkService.LanderFootGroundedB = this.footBCollision;
            networkService.LanderFootGroundedL = this.footLCollision;
            networkService.LanderFootGroundedR = this.footRCollision;
            networkService.LanderAwake = this.shipBody.InternalBody.IsActive;
        }

        public void ApplyEngineAction(EngineActions action)
        {
            var rotation = Quaternion.Identity;

            var enginePenalty = 0f;

            switch (action)
            {
                case EngineActions.None:
                    break;
                case EngineActions.YawPositive:
                    this.shipBody.ApplyTorqueImpulse(this.transform.WorldTransform.Up * rotationImpulse);
                    enginePenalty -= 0.03f;
                    break;
                case EngineActions.YawNegative:
                    this.shipBody.ApplyTorqueImpulse(this.transform.WorldTransform.Up * -rotationImpulse);
                    enginePenalty -= 0.03f;
                    break;
                case EngineActions.RollPositive:
                    this.shipBody.ApplyTorqueImpulse(this.transform.WorldTransform.Forward * -rotationImpulse);
                    enginePenalty -= 0.03f;
                    break;
                case EngineActions.RollNegative:
                    this.shipBody.ApplyTorqueImpulse(this.transform.WorldTransform.Forward * rotationImpulse);
                    enginePenalty -= 0.03f;
                    break;
                case EngineActions.Main:
                    var impulse = 0.7f * this.transform.LocalTransform.Up;
                    this.shipBody.ApplyImpulse(impulse);
                    this.dust.MainEngine = true;
                    enginePenalty -= 0.3f;
                    break;
                case EngineActions.PitchPositive:
                    this.shipBody.ApplyTorqueImpulse(this.transform.WorldTransform.Left * rotationImpulse);
                    enginePenalty -= 0.03f;

                    break;
                case EngineActions.PitchNegative:
                    this.shipBody.ApplyTorqueImpulse(this.transform.WorldTransform.Left * -rotationImpulse);
                    enginePenalty -= 0.03f;
                    break;
                default:
                    break;
            }

            if (this.numActions < MaxActions)
            {
                this.actionsQueue[this.numActions++] = action;
            }
        }

        protected override void Update(TimeSpan gameTime)
        {
            this.dust.MainEngine = false;

            foreach (var p in this.allParticleActivators)
            {
                p.ImpulseActive = false;
            }

            for (int i = 0; i < this.numActions; i++)
            {
                var action = this.actionsQueue[i];

                if (action == EngineActions.Main)
                {
                    this.dust.MainEngine = true;
                }

                if (this.particleActivators.ContainsKey(action))
                {
                    foreach (var p in this.particleActivators[action])
                    {
                        p.ImpulseActive = true;
                    }
                }
            }

            this.numActions = 0;
        }
    }
}
