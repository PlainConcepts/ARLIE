using Grpc.Core;
using RLEnvs.D3.Components;
using RLEnvs.Helpers;
using RLEnvs.Lunar3D;

using RLEnvs.D3.Model;
using System;
using System.Threading.Tasks;
using WaveEngine.Common;

using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Threading;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Diagnostic;
using System.Diagnostics;

/**
* Rocket trajectory optimization is a classic topic in Optimal Control.
*
* According to Pontryagin's maximum principle it's optimal to fire engine full throttle or
* turn it off. That's the reason this environment is OK to have discreet actions (engine on or off).
*
* Landing pad is always at coordinates (0,0). Coordinates are the first two numbers in state vector.
* Reward for moving from the top of the screen to landing pad and zero speed is about 100..140 points.
* If lander moves away from landing pad it loses reward back. Episode finishes if the lander crashes or
* comes to rest, receiving additional -100 or +100 points. Each leg ground contact is +10. Firing main
* engine is -0.3 points each frame. Firing side engine is -0.03 points each frame. Solved is 200 points.
*
* Landing outside landing pad is possible. Fuel is infinite, so an agent can learn to fly and then land
* on its first attempt. Please see source code for details.
*/

namespace RLEnvs.Services
{
    public class Lunar3DNetworkService : Service
    {
        private const float LandingReward = 100;
        private const float CrashReward = -100;
        private const float MainEnginePenalty = 0.3f;
        private const float SideEnginePenalty = 0.03f;

        private readonly int port;

        // public float Distance;

        public bool LanderAwake;

        public Vector3 LanderPosition;
        public Vector3 LanderVelocity;
        public Vector3 LanderAngle;
        public Vector3 LanderAngularVelocity;

        public bool LanderFootGroundedF;
        public bool LanderFootGroundedB;
        public bool LanderFootGroundedL;
        public bool LanderFootGroundedR;
        public bool LanderBodyCollided;


        public event EventHandler<EngineActions> ActionReveiced;
        public event EventHandler OnReset;
        public event EventHandler<TimeSpan> Updated;
        public event EventHandler Rendered;

        private bool sceneLoaded;

        public Lunar3DNetworkService(int port)
        {
            this.port = port;
        }

        protected override void Initialize()
        {
            Server server = new Server
            {
                Services = { Lunar3DService.BindService(new Lunar(this)) },
                Ports = { new ServerPort("localhost", this.port, ServerCredentials.Insecure) }
            };

            server.Start();
        }

        private ActionResult GetActionResult(EngineActions action)
        {
            var observation = this.GetUpdatedObservation();

            var gameOver = this.LanderBodyCollided; // || !this.LanderAwake

            return new ActionResult
            {
                Done = gameOver,
                Observation = observation,
                Reward = 0f
            };
        }

        private Observation GetUpdatedObservation()
        {
            return new Observation
            {
                PosX = this.LanderPosition.X,
                PosY = this.LanderPosition.Y,
                PosZ = this.LanderPosition.Z,
                VelX = this.LanderVelocity.X,
                VelY = this.LanderVelocity.Y,
                VelZ = this.LanderVelocity.Z,
                AngleX = this.LanderAngle.X,
                AngleY = this.LanderAngle.Y,
                AngleZ = this.LanderAngle.Z,
                AngVelX = this.LanderAngularVelocity.X,
                AngVelY = this.LanderAngularVelocity.Y,
                AngVelZ = this.LanderAngularVelocity.Z,
                FootContactF = (this.LanderFootGroundedF) ? 1f : 0f,
                FootContactB = (this.LanderFootGroundedB) ? 1f : 0f,
                FootContactL = (this.LanderFootGroundedL) ? 1f : 0f,
                FootContactR = (this.LanderFootGroundedR) ? 1f : 0f,
            };
        }

        private class Lunar : Lunar3DService.Lunar3DServiceBase
        {
            private readonly Lunar3DNetworkService service;
            private static int actionDim;
            private static int observationDim;

            static Lunar()
            {
                actionDim = Enum.GetValues(typeof(EngineActions)).Length;
                observationDim = typeof(Observation).GetProperties(
                    System.Reflection.BindingFlags.Instance
                    | System.Reflection.BindingFlags.Public
                    | System.Reflection.BindingFlags.SetProperty
                    | System.Reflection.BindingFlags.GetProperty).Length;
            }

            public Lunar(Lunar3DNetworkService service)
            {
                this.service = service;
            }

            public override Task<DimResult> GetActionDim(ServiceMessage request, ServerCallContext context)
            {
                return Task.FromResult(new DimResult { Value = actionDim });
            }

            public override Task<DimResult> GetObservationDim(ServiceMessage request, ServerCallContext context)
            {
                return Task.FromResult(new DimResult { Value = observationDim });
            }

            private Stopwatch test = new Stopwatch();
            private long count;

            public override Task<ActionResult> PerformAction(Lunar3D.Action request, ServerCallContext context)
            {
                // count++;
                // test.Start();
                return WaveForegroundTask.Run(() =>
                {
                    var action = request.EngineAction.ToEngineAction();
                    this.service.ActionReveiced?.Invoke(this, action);

                    // Next step
                    this.service.Updated?.Invoke(this, TimeSpan.FromSeconds(1 / LunarConstants.FPS));

                    return this.service.GetActionResult(action);
                });/*.ContinueWith(t =>
                {
                    test.Stop();
                    if (count % 1000 == 0)
                    {
                        Debug.WriteLine(test.ElapsedMilliseconds / (double)count);
                    }
                    return t.Result;
                });*/
            }

            public override Task<ServiceMessage> Render(ServiceMessage request, ServerCallContext context)
            {
                return WaveForegroundTask.Run(() =>
                {
                    this.service.Rendered?.Invoke(this, null);

                    return new ServiceMessage();
                });
            }

            public override Task<Observation> Reset(ServiceMessage request, ServerCallContext context)
            {
                return WaveForegroundTask.Run(() =>
                {
                    if (!this.service.sceneLoaded)
                    {
                        this.service.sceneLoaded = true;
                        var newScene = new MyScene3D();
                        WaveServices.ScreenContextManager.To(new ScreenContext(newScene), true);
                        newScene.Initialize(WaveServices.GraphicsDevice);
                    }

                    this.service.OnReset?.Invoke(this.service, null);

                    return this.service.GetUpdatedObservation();
                });
            }
        }
    }
}
