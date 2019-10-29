using RLEnvs.D3.Helper;
using RLEnvs.D3.Model;
using RLEnvs.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;

using WaveRandom = WaveEngine.Framework.Services.Random;

namespace RLEnvs.D3.Components
{
    [DataContract]
    public class ScenarioManager : Component
    {
        private WaveRandom randy;

        private ShipComponent shipComponent;
        private TerrainMeshComponent terrain;

        protected Lunar3DNetworkService networkService;

        [RenderPropertyAsEntity(new string[] { "WaveEngine.Framework.Graphics.Tranform3D" })]
        [DataMember]
        public string ShipEntityPath;

        [RenderPropertyAsEntity(new string[] { "RLEnvs.D3.Components.TerrainMeshComponent" })]
        [DataMember]
        public string TerrainEntityPath;

        [DontRenderProperty]
        public int CurrentEpisode;

        protected override void DefaultValues()
        {
            base.DefaultValues();
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.CurrentEpisode = 0;

            if (WaveServices.Platform.IsEditor
                || (string.IsNullOrEmpty(this.ShipEntityPath))
                || (string.IsNullOrEmpty(this.TerrainEntityPath)))
            {
                return;
            }

            this.randy = WaveServices.Random;

            this.shipComponent = this.Owner.Find(this.ShipEntityPath)?.FindComponent<ShipComponent>();
            this.terrain = this.Owner.Find(this.TerrainEntityPath)?.FindComponent<TerrainMeshComponent>();

            this.networkService = WaveServices.GetService<Lunar3DNetworkService>();

            if (this.networkService != null)
            {
                this.networkService.ActionReveiced += this.OnActionReceived;
                this.networkService.OnReset += this.OnReset;
                this.networkService.Updated += this.OnUpdated;
                this.networkService.Rendered += this.OnRendered;

                this.Owner.Scene.Pause();
                this.Owner.Scene.IsVisible = false;
            }
            else
            {
                this.Reset();
            }
        }

        protected override void Removed()
        {
            base.Removed();

            if (this.networkService != null)
            {
                this.networkService.ActionReveiced -= this.OnActionReceived;
                this.networkService.OnReset -= this.OnReset;
                this.networkService.Updated -= this.OnUpdated;
                this.networkService.Rendered -= this.OnRendered;
            }
        }

        private void OnActionReceived(object sender, EngineActions e)
        {
            this.shipComponent.ApplyEngineAction(e);
        }

        private void OnReset(object sender, EventArgs e)
        {
            this.Reset();
        }

        private void OnUpdated(object sender, TimeSpan e)
        {
            this.Owner.Scene.NextStep(e);

            if (this.networkService != null)
            {
                this.RefreshObservations();
            }
        }

        private void OnRendered(object sender, EventArgs e)
        {
            this.Owner.Scene.IsVisible = true;
            this.RenderManager.OnPostRender += this.OnPostRender;
        }

        private void OnPostRender(object sender, RenderEventArgs e)
        {
            e.RenderManager.OnPostRender -= this.OnPostRender;
            this.Owner.Scene.IsVisible = false;
        }

        public void Reset()
        {
            this.shipComponent.Reset();
            this.terrain.Reset();

            if (this.networkService != null)
            {
                this.RefreshObservations();
            }

            this.CurrentEpisode++;
        }

        private void RefreshObservations()
        {
            this.shipComponent.RefreshObservations(this.networkService);
            ////this.networkService.Distance = Vector3.Distance(this.shipComponent.Transform3D.Position, this.landingTransform.Position);
        }
    }
}
