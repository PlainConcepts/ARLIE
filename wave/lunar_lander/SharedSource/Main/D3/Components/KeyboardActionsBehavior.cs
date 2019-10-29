using RLEnvs.D3.Model;
using RLEnvs.Services;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Input;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;

namespace RLEnvs.D3.Components
{
    [DataContract]
    public class KeyboardActionsBehavior : Behavior
    {
        [RequiredComponent]
        protected ShipComponent engineComponent;

        private ScenarioManager scenarioManager;

        private KeyboardState lastKeyboardState;

        protected Input input;

        protected override void Initialize()
        {
            base.Initialize();

            this.input = WaveServices.Input;

            this.scenarioManager = this.EntityManager.FindFirstComponentOfType<ScenarioManager>();

            if (!WaveServices.Platform.IsEditor && (WaveServices.GetService<Lunar3DNetworkService>() != null))
            {
                this.IsActive = false;
            }
        }

        protected override void Update(TimeSpan gameTime)
        {
            var currentState = this.input.KeyboardState;

            if (this.lastKeyboardState.IsKeyPressed(Keys.R) && currentState.IsKeyReleased(Keys.R))
            {
                this.scenarioManager.Reset();

                this.lastKeyboardState = currentState;
                return;
            }

            bool applied = false;

            if (currentState.IsKeyPressed(Keys.Space))
            {
                applied = true;
                this.engineComponent.ApplyEngineAction(EngineActions.Main);
            }

            if (currentState.IsKeyPressed(Keys.Up))
            {
                applied = true;
                this.engineComponent.ApplyEngineAction(EngineActions.PitchPositive);
            }
            else if (currentState.IsKeyPressed(Keys.Down))
            {
                applied = true;
                this.engineComponent.ApplyEngineAction(EngineActions.PitchNegative);
            }

            if (currentState.IsKeyPressed(Keys.Left))
            {
                applied = true;
                this.engineComponent.ApplyEngineAction(EngineActions.RollPositive);
            }
            else if (currentState.IsKeyPressed(Keys.Right))
            {
                applied = true;
                this.engineComponent.ApplyEngineAction(EngineActions.RollNegative);
            }

            if (currentState.IsKeyPressed(Keys.Z))
            {
                applied = true;
                this.engineComponent.ApplyEngineAction(EngineActions.YawNegative);
            }
            else if (currentState.IsKeyPressed(Keys.X))
            {
                applied = true;
                this.engineComponent.ApplyEngineAction(EngineActions.YawPositive);
            }
            
            if (!applied)
            {
                this.engineComponent.ApplyEngineAction(EngineActions.None);
            }

            this.lastKeyboardState = currentState;
        }
    }
}
