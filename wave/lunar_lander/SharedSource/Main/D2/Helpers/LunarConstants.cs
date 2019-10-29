namespace RLEnvs.Helpers
{
    public static class LunarConstants
    {
        public const float SCALE = 30f; // affects how fast-paced the game is, forces should be adjusted as well

        public const float MAIN_ENGINE_POWER = 13f;
        public const float SIDE_ENGINE_POWER = 0.6f;
        public const float SIDE_ENGINE_AWAY = 12.0f;
        public const float SIDE_ENGINE_HEIGHT = 14.0f;

        public const float FPS = 60f;
        public const float INITIAL_RANDOM = 1000.0f;   // Set 1500 to make game harder

        public const float LEG_AWAY = 20;

        public const float LEG_DOWN = 18;

        public const float LEG_W = 2;
        public const float LLEG_H = 8;

        public const float LEG_SPRING_TORQUE = 40;

        public const int VIEWPORT_W = 1280;

        public const int VIEWPORT_H = 720;
    }
}
