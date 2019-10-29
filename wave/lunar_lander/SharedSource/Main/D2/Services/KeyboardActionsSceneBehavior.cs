using System;
using WaveEngine.Common;
using WaveEngine.Common.Input;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using RLEnvs.Components;
using RLEnvs.Model;

namespace RLEnvs.Services
{
    public class KeyboardActionsService : UpdatableService
    {
        private Scene scene;
        protected EnginesComponent enginesComponent;

        private KeyboardState lastKeyboardState;

        protected Input input;

        protected override void Initialize()
        {
            this.input = WaveServices.Input;
        }

        public void SetScene(Scene scene)
        {
            this.scene = scene;
            this.enginesComponent = this.scene.EntityManager.FindFirstComponentOfType<EnginesComponent>();
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
                WaveServices.ScreenContextManager.To(new ScreenContext(new MyScene()), true);
                this.lastKeyboardState = currentState;
                return;
            }

            var currentAction = EngineActions.None;
            if (this.lastKeyboardState.IsKeyPressed(Keys.Up) && currentState.IsKeyReleased(Keys.Up))
            {
                currentAction = EngineActions.Main;
            }
            else if (this.lastKeyboardState.IsKeyPressed(Keys.Left) && currentState.IsKeyReleased(Keys.Left))
            {
                currentAction = EngineActions.Left;
            }
            else if (this.lastKeyboardState.IsKeyPressed(Keys.Right) && currentState.IsKeyReleased(Keys.Right))
            {
                currentAction = EngineActions.Right;
            }

            this.enginesComponent.ApplyEngineAction(currentAction);

            if (this.lastKeyboardState.IsKeyPressed(Keys.D) && currentState.IsKeyReleased(Keys.D))
            {
                void RenderManager_OnPostRender(object sender, WaveEngine.Framework.Graphics.RenderEventArgs e)
                {
                    this.scene.IsVisible = false;
                }

                this.scene.IsVisible = true;
                this.scene.RenderManager.OnPostRender += RenderManager_OnPostRender;
            }
            else if (this.lastKeyboardState.IsKeyPressed(Keys.U) && currentState.IsKeyReleased(Keys.U))
            {
                this.scene.NextStep(TimeSpan.FromSeconds(1 / 60d));
            }

            this.lastKeyboardState = currentState;
        }
    }
}
