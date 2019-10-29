using RLEnvs.Helpers;
using RLEnvs.Services;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;

namespace RLEnvs
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            this.PhysicsManager.Simulation2D.VelocityIterations = 6 * 30;
            this.PhysicsManager.Simulation2D.PositionIterations = 2 * 30;
            this.PhysicsManager.Simulation2D.WorldScale = LunarConstants.SCALE;

            this.Load(WaveContent.Scenes.MyScene);
            
            this.Pause();
            this.IsVisible = false;
        }

        protected override void Start()
        {
            base.Start();
            var keyboardService = WaveServices.GetService<KeyboardActionsService>();
            keyboardService?.SetScene(this);
            this.EntityManager.FindFirstComponentOfType<Components.EnginesComponent>().ApplyInitialForce();
        }
    }
}
