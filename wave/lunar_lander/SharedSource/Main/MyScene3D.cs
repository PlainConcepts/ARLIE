using RLEnvs.D3.Components;
using RLEnvs.D3.Services;
using RLEnvs.Helpers;
using RLEnvs.Services;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;

namespace RLEnvs
{
    public class MyScene3D : Scene
    {
        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene3D);

            WaveServices.ScreenContextManager.SetDiagnosticsActive(true);

            ////this.RenderManager.DebugLines = true;
        }
    }
}
