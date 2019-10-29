using System;
using System.Collections.Generic;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Services;

namespace RLEnvs.D3.Helper
{
    public static class BoundingBoxExtensions
    {
        public static Vector3 GetRandomPositionInside(this BoundingBox boundingBox)
        {
            var randy = WaveServices.Random;

            var size = boundingBox.HalfExtent * 2;
            Vector3 position;
            position.X = (float)(randy.NextDouble() * size.X);
            position.Y = (float)(randy.NextDouble() * size.Y);
            position.Z = (float)(randy.NextDouble() * size.Z);
            return position + boundingBox.Center - boundingBox.HalfExtent;            
        }
    }
}
