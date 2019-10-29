using RLEnvs.D3.Model;
using System;

using static RLEnvs.Lunar3D.Action.Types;

namespace RLEnvs.Helpers
{
    public static class EngineActions3DExtensions
    {
        public static EngineActions ToEngineAction(this Engine action)
        {
            switch (action)
            {
                case Engine.None:
                    return EngineActions.None;
                case Engine.Yawpositive:
                    return EngineActions.YawPositive;
                case Engine.Yawnegative:
                    return EngineActions.YawNegative;
                case Engine.Pitchpositive:
                    return EngineActions.PitchPositive;
                case Engine.Pitchnegative:
                    return EngineActions.PitchNegative;
                case Engine.Rollnegative:
                    return EngineActions.RollNegative;
                case Engine.Rollpositive:
                    return EngineActions.RollPositive;
                case Engine.Main:
                    return EngineActions.Main;
                default:
                    throw new ArgumentOutOfRangeException($"The argument value can't be converteds to {nameof(EngineActions)}. Value: {action}");
            }
        }
    }
}
