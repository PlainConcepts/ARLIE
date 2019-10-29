using RLEnvs.D3.Particles;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Curves;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Framework;

namespace RLEnvs.D3.Particles
{
    public enum CurveEnum
    {
        Size,
        Velocity,
        VelocityX,
        VeloxityY,
        VelocityZ,
    }

    [DataContract]
    [AllowMultipleInstances]
    public class CurveApplierComponent : Component
    {
        [RequiredComponent]
        protected NewParticleSystem particles;

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.MinValue = 0;
            this.MaxValue = 1;
            this.ParticlePropertyTarget = CurveEnum.Size;
            this.Curve = new ColorCurve();
            this.Curve.Keyframes.Clear();
            this.Curve.AddKey(0, Color.Black);
            this.Curve.AddKey(0.2f, Color.White);
            this.Curve.AddKey(0.8f, Color.White);
            this.Curve.AddKey(1, Color.Black);
        }

        private bool apply;

        public bool Apply
        {
            get { return this.apply; }

            set
            {
                if (this.apply != value)
                {
                    this.apply = value;

                    if (this.apply)
                    {
                        this.RefreshCurve();
                    }
                }
            }
        }

        [DataMember]
        public CurveEnum ParticlePropertyTarget { get; set; }

        [DataMember]
        public float MinValue { get; set; }

        [DataMember]
        public float MaxValue { get; set; }

        [DataMember]
        public ColorCurve Curve { get; set; }

        private void RefreshCurve()
        {
            FloatCurve fCurve = new FloatCurve();
            fCurve.Keyframes.Clear();

            foreach (var colorKey in this.Curve.Keyframes)
            {
                var color = colorKey.Value;
                var intensity = (color.R + color.G + color.B) / (3 * 255f);
                fCurve.AddKey(colorKey.Lerp, MathHelper.Lerp(this.MinValue, this.MaxValue, intensity));
            }

            switch (this.ParticlePropertyTarget)
            {
                case CurveEnum.Size:
                    this.particles.SizeAnimated = true;
                    this.particles.SizeOverLife = fCurve;
                    break;
                case CurveEnum.Velocity:
                    this.particles.VelocityAnimated = true;
                    this.particles.VelocityOverLifeX = fCurve;
                    this.particles.VelocityOverLifeY = fCurve;
                    this.particles.VelocityOverLifeZ = fCurve;
                    break;
                case CurveEnum.VelocityX:
                    this.particles.VelocityAnimated = true;
                    this.particles.VelocityOverLifeX = fCurve;
                    break;
                case CurveEnum.VeloxityY:
                    this.particles.VelocityAnimated = true;
                    this.particles.VelocityOverLifeY = fCurve;
                    break;
                case CurveEnum.VelocityZ:
                    this.particles.VelocityAnimated = true;
                    this.particles.VelocityOverLifeZ = fCurve;
                    break;
            }
        }
    }
}
