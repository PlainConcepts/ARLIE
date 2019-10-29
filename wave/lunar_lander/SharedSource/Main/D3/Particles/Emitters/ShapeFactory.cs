using System;
using System.Collections.Generic;
using System.Text;

namespace RLEnvs.D3.Particles.Emitters
{
    internal class ShapeFactory
    {
        public static AEmitter GetEmitter(ShapeType type)
        {
            switch (type)
            {
                default:
                case ShapeType.Point:
                    return new PointEmitter();
                case ShapeType.Sphere:
                    return new SphereEmitter();
                case ShapeType.Box:
                    return new BoxEmitter();
                case ShapeType.Circle:
                    return new CircleEmitter();
                case ShapeType.Cone:
                    return new ConeEmitter();
                case ShapeType.Edge:
                    return new EdgeEmitter();
                case ShapeType.Model:
                    return new ModelEmitter();
            }
        }
    }
}
