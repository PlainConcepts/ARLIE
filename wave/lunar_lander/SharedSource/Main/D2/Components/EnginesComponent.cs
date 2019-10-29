using RLEnvs.Helpers;
using RLEnvs.Model;
using System;
using System.Runtime.Serialization;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Services;
using Random = WaveEngine.Framework.Services.Random;

namespace RLEnvs.Components
{

    [DataContract]
    public class EnginesComponent : Component
    {
        [RequiredService]
        protected Random random;

        [RequiredComponent]
        protected RigidBody2D body;

        [RequiredComponent]
        protected Transform2D transform;

        [RequiredComponent]
        protected EngineParticlesEmitter particlesEmitter;

        protected override void Initialize()
        {
            base.Initialize();
        }

        public void ApplyInitialForce()
        {
            var initialForce = this.random.InsideUnitCircle() * LunarConstants.INITIAL_RANDOM;
            this.body.ApplyForceToCenter(initialForce, true);
        }

        public void ApplyEngineAction(EngineActions action)
        {
            var tip = new[] { Math.Sin(this.transform.Rotation), Math.Cos(this.transform.Rotation) };
            var side = new[] { -tip[1], tip[0] };
            var dispersion = new[] { ((this.random.NextDouble() * 2) - 1) / LunarConstants.SCALE, ((this.random.NextDouble() * 2) - 1) / LunarConstants.SCALE };

            if (action == EngineActions.Main)
            {
                var power = 1f;
                var ox = tip[0] * (4 / LunarConstants.SCALE + 2 * dispersion[0]) + side[0] * dispersion[1];
                var oy = -tip[1] * (4 / LunarConstants.SCALE + 2 * dispersion[0]) - side[1] * dispersion[1];
                var impulsePosition = new Vector2(this.transform.Position.X + (float)ox, this.transform.Position.Y + (float)oy);
                var impulsePower = new Vector2((float)-ox * LunarConstants.MAIN_ENGINE_POWER * power, (float)oy * LunarConstants.MAIN_ENGINE_POWER * power);
                this.body.ApplyLinearImpulse(impulsePower, impulsePosition, true);
                this.particlesEmitter.CreateParticle(3.5f, impulsePosition, power, impulsePower * -1f);
            }
            
            if (action == EngineActions.Left || action == EngineActions.Right)
            {
                var power = 1f;
                var direction = (int)action - 2;
                var ox = tip[0] * dispersion[0] + side[0] * (3 * dispersion[1] + direction * LunarConstants.SIDE_ENGINE_AWAY / LunarConstants.SCALE);
                var oy = -tip[1] * dispersion[0] - side[1] * (3 * dispersion[1] + direction * LunarConstants.SIDE_ENGINE_AWAY / LunarConstants.SCALE);
                var impulsePosition = new Vector2(this.transform.Position.X + (float)ox - (float)tip[0] * 17 / LunarConstants.SCALE, this.transform.Position.Y + (float)oy + (float)tip[1] * LunarConstants.SIDE_ENGINE_HEIGHT / LunarConstants.SCALE);
                var impulsePower = new Vector2((float)-ox * LunarConstants.SIDE_ENGINE_POWER * power, (float)oy * LunarConstants.SIDE_ENGINE_POWER * power);
                this.body.ApplyLinearImpulse(impulsePower, impulsePosition, true);
                this.particlesEmitter.CreateParticle(3.5f, impulsePosition, power, impulsePower * -1f);
            }
        }
    }
}
