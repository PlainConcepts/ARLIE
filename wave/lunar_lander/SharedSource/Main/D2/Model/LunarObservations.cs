using WaveEngine.Common.Math;

namespace RLEnvs.Model
{
    public class LunarObservations
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Angle { get; set; }
        public float AngleVelocity { get; set; }
        public bool LeftContact { get; set; }
        public bool RightContact { get; set; }
    }
}
