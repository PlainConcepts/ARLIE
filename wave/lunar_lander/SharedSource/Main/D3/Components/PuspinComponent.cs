using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Components.Toolkit;
using WaveEngine.Framework;

namespace RLEnvs.D3.Components
{
    [DataContract]
    public class PuspinComponent : Behavior
    {
        private TextComponent text;
        private string label = string.Empty;
        protected override void ResolveDependencies()
        {
            base.ResolveDependencies();
            this.text = this.Owner.FindComponentInChildren<TextComponent>();
        }

        public void SetText(string text)
        {
            this.label = text;
            this.IsActive = true;
        }

        protected override void Update(TimeSpan gameTime)
        {
            if (!label.Equals(this.text.Text))
            {
                this.text.Text = label;
            }

            this.IsActive = false;
        }
    }
}
