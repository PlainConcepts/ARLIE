using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework;

namespace RLEnvs.D3.Components
{
    [DataContract]
    public class ColorButtonComponent : Component
    {
        [RequiredComponent]
        private Sprite sprite;

        [DataMember]
        private Color color;

        [DataMember]
        private Color hoverColor;

        [DataMember]
        private Color pressedColor;

        public event EventHandler Clicked;

        protected override void Initialize()
        {
            base.Initialize();

            this.sprite.TintColor = this.color;
        }
    }
}
