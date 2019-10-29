using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace RLEnvs.D3.Particles.Emitters
{
    [DataContract]
    public enum ShapeType
    {
        Point,
        Sphere,
        Box,
        Circle,
        Edge,
        Cone,
        Model,
    }
}
