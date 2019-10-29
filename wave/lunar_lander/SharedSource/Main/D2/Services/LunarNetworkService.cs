using Grpc.Core;
using RLEnvs.Components;
using RLEnvs.Helpers;
using RLEnvs.Lunar2D;
using RLEnvs.Model;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WaveEngine.Common;
using WaveEngine.Common.Physics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Threading;

namespace RLEnvs.Services
{
    public class LunarNetworkService : Service
    {
        private Scene scene;
        protected EnginesComponent enginesComponent;
        private RigidBody2D landerBody;
        private Collider2D bodyCollider;
        private Collider2D leftCollider;
        private Collider2D rightCollider;

        private bool leftCollided;
        private bool rightCollided;
        private bool gameOver;
        private double? previousShaping;

        private readonly int port;

        public LunarNetworkService(int port)
        {
            this.port = port;
        }

        protected override void Initialize()
        {
            Server server = new Server
            {
                Services = { LunarService.BindService(new Lunar(this)) },
                Ports = { new ServerPort("localhost", this.port, ServerCredentials.Insecure) }
            };
            server.Start();
        }

        public void SetScene(Scene scene)
        {
            if (this.leftCollider != null)
            {
                this.leftCollider.BeginCollision -= this.Collider_LeftBeginCollision;
                this.leftCollider.EndCollision -= this.Collider_LeftEndCollision;
            }

            if (this.rightCollider != null)
            {
                this.rightCollider.BeginCollision -= this.Collider_RightBeginCollision;
                this.rightCollider.EndCollision -= this.Collider_RightEndCollision;
            }

            if (this.bodyCollider != null)
            {
                this.bodyCollider.BeginCollision -= this.Collider_BodyBeginCollision;
            }

            this.leftCollided = false;
            this.rightCollided = false;
            this.gameOver = false;

            this.scene = scene;
            this.enginesComponent = this.scene.EntityManager.FindFirstComponentOfType<EnginesComponent>();
            this.landerBody = this.scene.EntityManager.Find("lander.body").FindComponent<RigidBody2D>();
            this.bodyCollider = this.scene.EntityManager.Find("lander.body").FindComponent<Collider2D>(false);
            this.leftCollider = this.scene.EntityManager.Find("lander.leftLeft").FindComponent<Collider2D>(false);
            this.rightCollider = this.scene.EntityManager.Find("lander.rightLeg").FindComponent<Collider2D>(false);

            this.bodyCollider.BeginCollision += this.Collider_BodyBeginCollision;

            this.leftCollider.BeginCollision += this.Collider_LeftBeginCollision;
            this.leftCollider.EndCollision += this.Collider_LeftEndCollision;
            this.rightCollider.BeginCollision += this.Collider_RightBeginCollision;
            this.rightCollider.EndCollision += this.Collider_RightEndCollision;
        }

        private void Collider_BodyBeginCollision(ICollisionInfo2D contact)
        {
            this.gameOver = true;
        }

        private void Collider_LeftBeginCollision(ICollisionInfo2D contact)
        {
            this.leftCollided = true;
        }

        private void Collider_LeftEndCollision(ICollisionInfo2D contact)
        {
            this.leftCollided = false;
        }

        private void Collider_RightBeginCollision(ICollisionInfo2D contact)
        {
            this.rightCollided = true;
        }

        private void Collider_RightEndCollision(ICollisionInfo2D contact)
        {
            this.rightCollided = false;
        }

        private ActionResult GetActionResult(EngineActions action)
        {
            var observation = new Observation
            {
                PosX = this.landerBody.Transform2D.Position.X / ((LunarConstants.VIEWPORT_W / LunarConstants.SCALE) / 2),
                PosY = (LunarConstants.VIEWPORT_H - this.landerBody.Transform2D.Position.Y) / (LunarConstants.VIEWPORT_H / LunarConstants.SCALE),
                VelX = this.landerBody.LinearVelocity.X * ((LunarConstants.VIEWPORT_W / LunarConstants.SCALE) / 2) / LunarConstants.FPS,
                VelY = this.landerBody.LinearVelocity.Y * ((LunarConstants.VIEWPORT_H / LunarConstants.SCALE) / 2) / LunarConstants.FPS,
                Angle = this.landerBody.RigidBody.Angle,
                AngVel = 20f * this.landerBody.AngularVelocity / LunarConstants.FPS,
                LeftContact = this.leftCollided ? 1f : 0f,
                RightContact = this.rightCollided ? 1f : 0f
            };

            var reward = 0d;
            var shaping = -100 * Math.Sqrt(observation.PosX * observation.PosX + observation.PosY * observation.PosY)
                              - 100 * Math.Sqrt(observation.VelX * observation.VelX + observation.VelY * observation.VelY)
                              - 100 * Math.Abs(observation.Angle) + 10 * observation.LeftContact + 10 * observation.LeftContact;

            if (this.previousShaping != null)
            {
                reward = shaping - this.previousShaping.Value;
            }

            this.previousShaping = shaping;

            reward -= (action == EngineActions.Main ? 1 : 0) * 0.3;
            reward -= (action == EngineActions.Left || action == EngineActions.Right ? 1 : 0) * 0.3;

            var done = false;
            if (this.gameOver || Math.Abs(observation.PosX) >= 1)
            {
                done = true;
                reward = -100;
            }

            if (!this.landerBody.Awake)
            {
                done = true;
                reward = 100;
            }

            return new ActionResult
            {
                Done = done,
                Observation = observation,
                Reward = (float)reward
            };
        }

        private class Lunar : LunarService.LunarServiceBase
        {
            private readonly LunarNetworkService service;

            public Lunar(LunarNetworkService service)
            {
                this.service = service;
            }

            public override Task<DimResult> GetActionDim(ServiceMessage request, ServerCallContext context)
            {
                return Task.FromResult(new DimResult { Value = Enum.GetValues(typeof(EngineActions)).Length });
            }

            public override Task<DimResult> GetObservationDim(ServiceMessage request, ServerCallContext context)
            {
                // TODO Reflection?
                return Task.FromResult(new DimResult { Value = 8 });
            }

            public override Task<ActionResult> PerformAction(Lunar2D.Action request, ServerCallContext context)
            {
                return WaveForegroundTask.Run(() =>
                {
                    var action = request.EngineAction.ToEngineAction();
                    this.service.enginesComponent.ApplyEngineAction(action);
                    this.service.scene.NextStep(TimeSpan.FromSeconds(1 / LunarConstants.FPS));
                    return this.service.GetActionResult(action);
                });
            }

            public override Task<ServiceMessage> Render(ServiceMessage request, ServerCallContext context)
            {
                return WaveForegroundTask.Run(() =>
                {
                    void RenderManagerOnPostRender(object sender, RenderEventArgs e)
                    {
                        this.service.scene.RenderManager.OnPostRender -= RenderManagerOnPostRender;
                        this.service.scene.IsVisible = false;
                    }

                    this.service.scene.IsVisible = true;
                    this.service.scene.RenderManager.OnPostRender += RenderManagerOnPostRender;

                    return new ServiceMessage();
                });
            }

            public override Task<Observation> Reset(ServiceMessage request, ServerCallContext context)
            {
                return WaveForegroundTask.Run(() =>
                {
                    var newScene = new MyScene();
                    WaveServices.ScreenContextManager.To(new ScreenContext(newScene), true);

                    var tcs = new TaskCompletionSource<Observation>();
                    void StartedHandler(object sender, EventArgs args)
                    {
                        newScene.Started -= StartedHandler;
                        WaveForegroundTask.Run(() =>
                        {
                            this.service.SetScene(newScene);
                            this.service.enginesComponent.ApplyInitialForce();
                            this.service.enginesComponent.ApplyEngineAction(EngineActions.None);
                            this.service.scene.NextStep(TimeSpan.FromSeconds(1 / LunarConstants.FPS));
                            var actionResult = this.service.GetActionResult(EngineActions.None);
                            tcs.SetResult(actionResult.Observation);
                        });
                    }
                    newScene.Started += StartedHandler;
                    return tcs.Task;
                });
            }
        }
    }
}
