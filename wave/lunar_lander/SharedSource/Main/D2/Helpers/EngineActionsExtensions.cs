using System;
using RLEnvs.Model;
using static RLEnvs.Lunar2D.Action.Types;

namespace RLEnvs.Helpers
{
    public static class EngineActionsExtensions
    {
        public static EngineActions ToEngineAction(this Engine action)
        {
            switch (action)
            {
                case Engine.None:
                    return EngineActions.None;
                case Engine.Left:
                    return EngineActions.Left;
                case Engine.Main:
                    return EngineActions.Main;
                case Engine.Right:
                    return EngineActions.Right;
                default:
                    throw new ArgumentOutOfRangeException($"The argument value can't be converteds to {nameof(EngineActions)}. Value: {action}");
            }
        }
    }
}
