#region File Description
//-----------------------------------------------------------------------------
// Particle
//
// Copyright © 2016 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
#endregion

namespace RLEnvs.D3.Particles
{
    /// <summary>
    /// Class that holds the information of a particle.
    /// </summary>
    public class Particle
    {
        public int Id;

        public bool Alive;

        public float LifeLerp;

        public float LifeTime;

        public float InitSize;

        public Vector3 InitVelocity;

        public Color InitColor;

        public float Size;

        public Vector3 Position;

        public Vector3 Direction;

        public Vector3 Velocity;

        public Vector3 Forces;

        public float AngularVelocity;

        public float Angle;

        public Color Color;

        public ulong Generation;
    }
}
