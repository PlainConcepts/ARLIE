using RLEnvs.D3.Services;
using RLEnvs.Services;
using WaveEngine.Common;
using WaveEngine.Framework;
using WaveEngine.Framework.Managers;
using WaveEngine.Framework.Services;

namespace RLEnvs
{
    public class Game : WaveEngine.Framework.Game
    {
        public override void Initialize(IApplication application)
        {
            base.Initialize(application);

            if (Program.Arguments.IsServerMode)
            {
                if (Program.Arguments.Is3D)
                {
                    WaveServices.RegisterService(new Lunar3DNetworkService(Program.Arguments.Port));
                }
                else
                {
                    WaveServices.RegisterService(new LunarNetworkService(Program.Arguments.Port));
                }

                ScreenContext screenContext = new ScreenContext(new WaitingScene("localhost", Program.Arguments.Port));
                WaveServices.ScreenContextManager.To(screenContext);
            }
            else
            {
                Scene scene;

                if (Program.Arguments.Is3D)
                {
                    scene = new MyScene3D();
                    ////WaveServices.RegisterService(new Lunar3DNetworkService(Program.Arguments.Port));
                }
                else
                {
                    scene = new MyScene();
                    WaveServices.RegisterService(new KeyboardActionsService());
                }

                ScreenContext screenContext = new ScreenContext(scene);
                WaveServices.ScreenContextManager.To(screenContext);
            }

            this.Load(WaveContent.GameInfo);
        }
    }
}
