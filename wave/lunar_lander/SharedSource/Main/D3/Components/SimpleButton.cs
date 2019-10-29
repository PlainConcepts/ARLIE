using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Components.Gestures;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;

namespace RLEnvs.D3.Components
{
    [DataContract]
    public class SimpleButton : Component
    {
        [RequiredComponent]
        protected Sprite sprite;

        [RequiredComponent]
        protected Transform2D transform;

        [RequiredComponent]
        protected TouchGestures touchGestures;

        public event EventHandler Tapped;

        [DataMember]
        public Color PressedColor;

        [DataMember]
        public float PressedOpacity;

        [DataMember]
        public Color ReleasedColor;

        [DataMember]
        public float ReleasedOpacity;

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.PressedColor = Color.White;
            this.PressedOpacity = 1;

            this.ReleasedColor = Color.White;
            this.ReleasedOpacity = 0.3f;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.touchGestures.TouchPressed += this.OnPressed;
            this.touchGestures.TouchReleased += this.OnReleased;
            this.touchGestures.TouchTap += this.OnTap;
        }

        protected override void Removed()
        {
            base.Removed();

            this.touchGestures.TouchPressed -= this.OnPressed;
            this.touchGestures.TouchReleased -= this.OnReleased;
            this.touchGestures.TouchTap -= this.OnTap;
        }

        private void OnPressed(object sender, GestureEventArgs e)
        {
            this.sprite.TintColor = this.PressedColor;
            this.transform.Opacity = this.PressedOpacity;
        }

        private void OnReleased(object sender, GestureEventArgs e)
        {
            this.sprite.TintColor = this.ReleasedColor;
            this.transform.Opacity = this.ReleasedOpacity;
        }

        private void OnTap(object sender, GestureEventArgs e)
        {
            this.Tapped?.Invoke(this, null);
        }
    }
}
