using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Components.Toolkit;
using WaveEngine.Framework;

namespace RLEnvs.D3.Components
{
    [DataContract]
    public class EpisodeUpdater : Behavior
    {
        [RequiredComponent]
        private TextComponent text = null;

        ScenarioManager scenarioManager;

        private int currentEpisode;

        protected override void Initialize()
        {
            base.Initialize();

            this.scenarioManager = this.EntityManager.FindFirstComponentOfType<ScenarioManager>();
        }

        protected override void Update(TimeSpan gameTime)
        {
            if (this.currentEpisode != this.scenarioManager.CurrentEpisode)
            {
                this.currentEpisode = this.scenarioManager.CurrentEpisode;
                this.text.Text = $"Episode {this.currentEpisode}";
            }
        }
    }
}
