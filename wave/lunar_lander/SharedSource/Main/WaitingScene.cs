using WaveEngine.Components.Toolkit;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;

namespace RLEnvs
{
    public class WaitingScene : Scene
    {
        private string host;
        private int port;

        public WaitingScene(string host, int port)
        {
            this.host = host;
            this.port = port;

            WaveServices.ScreenContextManager.SetDiagnosticsActive(true);
        }

        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.WaitingScene);

            this.EntityManager.FindFirstComponentOfType<TextComponent>().Text = $"Wave Lunar Lander\nRunning on {this.host}:{this.port}";
        }
    }
}
