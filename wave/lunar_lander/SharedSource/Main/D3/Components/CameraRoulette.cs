using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Attributes;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace RLEnvs.D3.Components
{
    [DataContract]
    public class CameraRoulette : Component
    {
        private int index;

        [RequiredComponent]
        protected SimpleButton button;

        private Camera3D[] cameras;

        [DontRenderProperty]
        public int Index
        {
            get
            {
                return this.index;
            }

            private set
            {
                this.index = value;

                if (this.isInitialized)
                {
                    this.RefreshCameras();
                }
            }
        }

        public void Next()
        {
            this.Index = (this.index + 1) % this.RenderManager.Camera3DList.Count();
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.cameras = this.EntityManager.FindComponentsOfType<Camera3D>().ToArray();
            this.index = this.cameras.Length - 1;

            if (!WaveServices.Platform.IsEditor)
            {
                this.RefreshCameras();
            }

            this.button.Tapped += this.OnTapped;
        }

        protected override void Removed()
        {
            base.Removed();
            this.button.Tapped -= this.OnTapped;
        }

        private void OnTapped(object sender, EventArgs e)
        {
            this.Next();
        }

        private void RefreshCameras()
        {
            for (int i = 0; i < this.cameras.Length; i++)
            {
                var active = i == this.index;
                var cam = this.cameras[i];

                cam.Owner.IsActive = active;
                cam.IsActive = active;

                if (active)
                {
                    this.RenderManager.SetActiveCamera3D(cam.Owner);
                }
            }
        }
    }
}
