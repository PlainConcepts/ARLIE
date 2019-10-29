using RLEnvs.D3.Components;
using RLEnvs.D3.Model;
using System;
using System.Collections.Generic;
using System.Text;
using WaveEngine.Common;
using WaveEngine.Common.Input;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;

namespace RLEnvs.D3.Services
{
    public class KeyboardActionsService3D: UpdatableService
    {
        private Scene scene;
        protected ShipComponent enginesComponent;

        private KeyboardState lastKeyboardState;

        protected Input input;

        protected override void Initialize()
        {
            this.input = WaveServices.Input;
        }

        public void SetScene(Scene scene)
        {
            this.scene = scene;
            this.enginesComponent = this.scene.EntityManager.FindFirstComponentOfType<ShipComponent>();
        }

        public override void Update(TimeSpan gameTime)
        {
            if (this.scene == null)
            {
                return;
            }

            var currentState = this.input.KeyboardState;
            if (this.lastKeyboardState.IsKeyPressed(Keys.R) && currentState.IsKeyReleased(Keys.R))
            {
                this.lastKeyboardState = currentState;
                return;
            }

            var currentAction = EngineActions.None;
            if (currentState.IsKeyPressed(Keys.Space))
            {
                currentAction = EngineActions.Main;
            }
            else if(currentState.IsKeyPressed(Keys.Up))
            {
                currentAction = EngineActions.PitchPositive;
            }
            else if(currentState.IsKeyPressed(Keys.Down))
            {
                currentAction = EngineActions.PitchNegative;
            }
            else if (currentState.IsKeyPressed(Keys.Left) )
            {
                currentAction = EngineActions.YawNegative;
            }
            else if (currentState.IsKeyPressed(Keys.Right))
            {
                currentAction = EngineActions.YawPositive;
            }
            else if (currentState.IsKeyPressed(Keys.Z)) 
            {
                currentAction = EngineActions.RollPositive;
            }
            else if (currentState.IsKeyPressed(Keys.X))
            {
                currentAction = EngineActions.RollNegative;
            }

            this.enginesComponent.ApplyEngineAction(currentAction);

            this.lastKeyboardState = currentState;
        }
    }
}
