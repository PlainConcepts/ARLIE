#region File Description
//-----------------------------------------------------------------------------
// ParticleSystemRenderer3D
//
// Copyright © 2016 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements

using Batchers.Particles.Forces;
using GridTest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Graphics.VertexFormats;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;

#endregion

namespace RLEnvs.D3.Particles
{

    /// <summary>
    /// Renders a particle system on the screen.
    /// </summary>
    [DataContract]
    public class NewParticleRenderer : Drawable3D
    {
        private const int VerticesPerParticle = 4;
        private const int IndicesPerParticle = 6;
        private const int PrimitivesPerParticle = 2;
        private const int MaxPointsPerMesh = ushort.MaxValue;

        private static readonly ParallelOptions parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };

        private static readonly Vector2[] VertexUVs = new Vector2[]
        {
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(1.0f, 0.0f)
        };

        [RequiredComponent]
        internal protected NewParticleSystem ParticleSystem;

        [RequiredComponent]
        protected Transform3D transform;

        private Particle[] particles;

        private PointSpriteVertexFormat[] vertices;

        private int numParticles;

        private int aliveParticles;

        private Mesh particleMesh;

        private float timeFactor;

        private float emissionAccumulated;

        private Matrix identity;

        private Matrix world;

        private Matrix inverseWorld;

        private static VertexComparer vertexComparer;

        private List<AForce> forces;

        private bool emitting;

        private bool emissionCountDown;

        private double emissionTimeleft;

        private ForcesManager forcesManager;

        private FastRandom random;

        public Particle[] Particles
        {
            get
            {
                return this.particles;
            }
        }

        public int AliveParticles
        {
            get
            {
                return this.aliveParticles;
            }
        }

        public int NumParticles
        {
            get
            {
                return this.numParticles;
            }
        }

        public NewParticleRenderer()
        {
        }

        static NewParticleRenderer()
        {
            vertexComparer = new VertexComparer();
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.random = WaveServices.FastRandom;

            this.numParticles = -1;
            this.identity = Matrix.Identity;
            this.forces = new List<AForce>();

            this.SearchForceManager();
        }

        private void SearchForceManager()
        {
            foreach (var e in this.EntityManager.AllEntities)
            {
                var f = e.FindComponent<ForcesManager>();
                if (f != null)
                {
                    this.forcesManager = f;
                    break;
                }
            }
        }

        public override void Draw(TimeSpan gameTime)
        {
            if (this.ParticleSystem.Space == NewParticleSystem.SpaceEnum.Local)
            {
                this.world = this.transform.WorldTransform;
            }
            else
            {
                this.world = identity;
            }
            this.inverseWorld = identity;

            WaveEngine.Framework.Diagnostic.Timers.BeginAveragedTimer("New particle system");
            if (this.numParticles != this.ParticleSystem.MaxParticles)
            {
                this.Refresh();
                return;
            }

            this.UpdateParticles(gameTime);

            if (this.particleMesh != null)
            {
                this.RenderManager.DrawMesh(this.particleMesh, this.ParticleSystem.material, ref this.world);
            }

            WaveEngine.Framework.Diagnostic.Timers.EndAveragedTimer("New particle system");
            Labels.Add("Alive particles", this.aliveParticles);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateParticles(TimeSpan gameTime)
        {
            if (this.ParticleSystem.ZOrdering)
            {
                Vector3 cameraPosition = this.RenderManager.CurrentDrawingCamera3D.Position;
                Vector3.Transform(ref cameraPosition, ref this.inverseWorld, out vertexComparer.Camera);
            }

            var pS = this.ParticleSystem;

            bool hasForces = false;

            ////if (pS.ApplyForces)
            ////{
            ////    if (this.forcesManager != null)
            ////    {
            ////        this.forcesManager.GetForces(pS.ForcesCategory, ref this.forces);
            ////        hasForces = this.forces.Count > 0;

            ////        if (hasForces)
            ////        {
            ////            for (int i = 0; i < this.forces.Count; i++)
            ////            {
            ////                this.forces[i].Prepare(this);
            ////            }
            ////        }
            ////    }
            ////}

            pS.material.VelocityTrailFactor = (pS.ScreenAlign == NewParticleSystem.ScreenAlignType.Velocity) ? pS.VelocityTrailFactor : 0;

            // The amount of time
            this.timeFactor = (float)gameTime.TotalSeconds * this.ParticleSystem.TimeFactor;

            // Gets the number of new particles in this frame
            var newParticles = this.LaunchNewParticles();

            this.aliveParticles = 0;

            float correctedTime = this.timeFactor * this.ParticleSystem.LifeFactor;

            Timers.BeginAveragedTimer("Particles");

            ////this.ParticleSystem.Shape?.Prepare();

            Parallel.For(0, this.numParticles, (i) =>
            {

                ////for (var i = 0; i < this.numParticles; i++)
                ////{
                ref Particle p = ref this.particles[i];

                // We check the life cycle of the particle
                if (p.Alive)
                {
                    p.LifeLerp += correctedTime / p.LifeTime;

                    if (p.LifeLerp > 1)
                    {
                        p.Alive = false;
                        p.Size = 0;
                    }
                }
                else
                {
                    // If the particle is not alive we create a new one (if there are new particles to create)
                    if (Interlocked.Decrement(ref newParticles) >= 0)
                    {
                        this.ParticleSystem.ResetParticle(ref p);

                        var startTime = (float)(this.timeFactor * this.random.NextDouble());

                        p.LifeLerp = startTime * (this.ParticleSystem.LifeFactor) / p.LifeTime;
                        p.Position += startTime * p.Velocity;
                    }
                }

                if (p.Alive)
                {
                    // If the particle is alive after all, we update it
                    p.Forces.X -= this.timeFactor * pS.Gravity.X;
                    p.Forces.Y -= this.timeFactor * pS.Gravity.Y;
                    p.Forces.Z -= this.timeFactor * pS.Gravity.Z;

                    if (hasForces)
                    {
                        for (int fIdx = 0; fIdx < this.forces.Count; fIdx++)
                        {
                            this.forces[fIdx].ApplyForce(this, this.timeFactor, ref p);
                        }
                    }

                    if (pS.ColorAnimated && (pS.ColorOverLife != null))
                    {
                        p.Color = p.InitColor * pS.ColorOverLife.GetValue(p.LifeLerp);

                        if (pS.PremultiplyAlpha)
                        {
                            var alpha = p.Color.A / 255f;
                            p.Color.R = (byte)(p.Color.R * alpha);
                            p.Color.G = (byte)(p.Color.G * alpha);
                            p.Color.B = (byte)(p.Color.B * alpha);
                        }
                    }

                    if (pS.SizeAnimated && (pS.SizeOverLife != null))
                    {
                        p.Size = p.InitSize * pS.SizeOverLife.GetValue(p.LifeLerp);
                    }

                    if ((pS.VelocityAnimated) && (pS.VelocityOverLifeX != null) && (pS.VelocityOverLifeY != null) && (pS.VelocityOverLifeZ != null))
                    {
                        p.Velocity.X = p.InitVelocity.X * pS.VelocityOverLifeX.GetValue(p.LifeLerp);
                        p.Velocity.Y = p.InitVelocity.Y * pS.VelocityOverLifeY.GetValue(p.LifeLerp);
                        p.Velocity.Z = p.InitVelocity.Z * pS.VelocityOverLifeZ.GetValue(p.LifeLerp);
                    }

                    if ((pS.CollideWithFloor) && (p.Position.Y < pS.FloorLevel))
                    {
                        switch (pS.CollisionType)
                        {
                            case NewParticleSystem.ParticleCollisionType.Die:
                                p.Alive = false;
                                p.Size = 0;
                                break;
                            case NewParticleSystem.ParticleCollisionType.Bounce:

                                p.Position.Y = pS.FloorLevel;

                                p.Velocity.X = p.Velocity.X * pS.FloorBounciness;
                                p.Velocity.Y = (p.Velocity.Y + p.Forces.Y) * -pS.FloorBounciness;
                                p.Velocity.Z = p.Velocity.Z * pS.FloorBounciness;

                                p.Forces.Y = 0;

                                break;
                            case NewParticleSystem.ParticleCollisionType.Stop:
                            default:
                                p.Position.Y = pS.FloorLevel;
                                p.Velocity.X = 0;
                                p.Velocity.Y = 0;
                                p.Velocity.Z = 0;
                                break;
                        }
                    }

                    p.Angle += timeFactor * p.AngularVelocity;

                    p.Position.X += (p.Velocity.X + p.Forces.X) * timeFactor;
                    p.Position.Y += (p.Velocity.Y + p.Forces.Y) * timeFactor;
                    p.Position.Z += (p.Velocity.Z + p.Forces.Z) * timeFactor;

                    int particleIndex = (Interlocked.Increment(ref this.aliveParticles) - 1) * 4;

                    PointSpriteVertexFormat pointVertex = default(PointSpriteVertexFormat);
                    pointVertex.Position = p.Position;
                    pointVertex.Size.X = p.Size;
                    pointVertex.Size.Y = p.Size;
                    pointVertex.Color = p.Color;
                    pointVertex.VelocityAndRotation.X = p.Velocity.X;
                    pointVertex.VelocityAndRotation.Y = p.Velocity.Y;
                    pointVertex.VelocityAndRotation.Z = p.Velocity.Z;
                    pointVertex.VelocityAndRotation.W = p.Angle;

                    // Copy to Vertex Buffer

                    pointVertex.VertexTexCoords = VertexUVs[0];

                    this.vertices[particleIndex++] = pointVertex; // 0

                    pointVertex.VertexTexCoords = VertexUVs[1];

                    this.vertices[particleIndex++] = pointVertex; // 1

                    pointVertex.VertexTexCoords = VertexUVs[2];

                    this.vertices[particleIndex++] = pointVertex; // 2

                    pointVertex.VertexTexCoords = VertexUVs[3];

                    this.vertices[particleIndex++] = pointVertex; // 3
                }
            }
            );

            Timers.EndAveragedTimer("Particles");

            ////if (this.particleSystem.ZOrdering)
            ////{
            ////    Array.Sort(this.vertices, 0, this.aliveParticles, vertexComparer);
            ////}

            float zOrder = Vector3.DistanceSquared(this.RenderManager.CurrentDrawingCamera3D.Position, this.transform.Position);

            this.particleMesh.ZOrder = zOrder;
            this.particleMesh.VertexBuffer.SetData(this.vertices);
            this.particleMesh.NumVertices = this.aliveParticles * VerticesPerParticle;
            this.particleMesh.NumPrimitives = this.aliveParticles * PrimitivesPerParticle;
            this.GraphicsDevice.BindVertexBuffer(this.particleMesh.VertexBuffer);
        }

        private class VertexComparer : IComparer<PointSpriteVertexFormat>
        {
            public Vector3 Camera;

            public int Compare(PointSpriteVertexFormat x, PointSpriteVertexFormat y)
            {
                var dX = Vector3.DistanceSquared(x.Position, this.Camera);
                var dY = Vector3.DistanceSquared(y.Position, this.Camera);

                if (dX < dY)
                {
                    return 1;
                }
                else if (dX > dY)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
        }

        protected override void DrawDebugLines()
        {
            base.DrawDebugLines();

            for (int i = 0; i < this.aliveParticles; i++)
            {
                var p = this.particles[i];
                var p2 = p.Position + (p.Velocity * this.timeFactor);
                this.RenderManager.LineBatch3D.DrawLine(p.Position, p2, p.Color);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int LaunchNewParticles()
        {
            if (!this.ParticleSystem.IsEmitting)
            {
                this.emissionAccumulated = 0;
                return 0;
            }

            int newParticles = 0;

            if (ParticleSystem.emitType == NewParticleSystem.EmitType.Rate)
            {

                if (!this.emitting)
                {
                    // Check emit duration
                    if (this.ParticleSystem.EmitDuration > 0)
                    {
                        this.emissionTimeleft = this.ParticleSystem.EmitDuration;
                        this.emissionCountDown = true;
                    }
                }

                float newParticlesF = (float)((this.ParticleSystem.EmitRate * this.timeFactor) + this.emissionAccumulated);

                newParticles = (int)newParticlesF;
                this.emissionAccumulated = newParticlesF - newParticles;

                if (this.emissionCountDown)
                {
                    this.emissionTimeleft -= this.timeFactor;

                    if (this.emissionTimeleft < 0)
                    {
                        this.ParticleSystem.IsEmitting = false;
                    }
                }

            }
            else
            {
                if (this.emitting)
                {
                    newParticles = this.ParticleSystem.MaxParticles;
                    this.ParticleSystem.IsEmitting = false;
                }
            }

            this.emitting = this.ParticleSystem.IsEmitting;

            return newParticles;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Refresh()
        {
            this.DisposeMesh();

            this.numParticles = Math.Min(MaxPointsPerMesh, this.ParticleSystem.MaxParticles);
            this.numParticles = Math.Max(this.numParticles, 0);

            this.particles = new Particle[this.numParticles];

            for (int i = 0; i < this.numParticles; i++)
            {
                this.particles[i] = new Particle()
                {
                    Id = i
                };
            }

            this.vertices = new PointSpriteVertexFormat[this.numParticles * VerticesPerParticle];

            VertexBuffer vertexBuffer = new DynamicVertexBuffer(PointSpriteVertexFormat.VertexFormat);
            vertexBuffer.SetData(this.vertices);

            var indices = new ushort[this.numParticles * IndicesPerParticle];

            for (int i = 0; i < this.numParticles; i++)
            {
                ushort baseIndex = (ushort)(i * IndicesPerParticle);
                ushort baseVertex = (ushort)(i * VerticesPerParticle);

                // Triangle 0
                indices[baseIndex + 0] = (ushort)(baseVertex + 0);
                indices[baseIndex + 1] = (ushort)(baseVertex + 1);
                indices[baseIndex + 2] = (ushort)(baseVertex + 2);

                // Triangle 1
                indices[baseIndex + 3] = (ushort)(baseVertex + 2);
                indices[baseIndex + 4] = (ushort)(baseVertex + 3);
                indices[baseIndex + 5] = (ushort)(baseVertex + 0);
            }
            IndexBuffer indexBuffer = new IndexBuffer(indices);

            this.particleMesh = new Mesh(vertexBuffer, indexBuffer, PrimitiveType.TriangleList);
        }

        protected override void Dispose(bool disposing)
        {
            this.DisposeMesh();
        }

        private void DisposeMesh()
        {
            if (this.particleMesh != null)
            {

                this.GraphicsDevice.DestroyIndexBuffer(particleMesh.IndexBuffer);
                this.GraphicsDevice.DestroyVertexBuffer(particleMesh.VertexBuffer);
            }

            this.particleMesh = null;
        }

        public void Reset()
        {
            if (this.particles != null)
            {
                for (int i = 0; i < this.numParticles; i++)
                {
                    this.particles[i].Alive = false;
                    this.particles[i].LifeLerp = 0;
                    this.particles[i].LifeTime = 0;
                }
            }

            // Check emit duration
            if (this.ParticleSystem.EmitDuration > 0)
            {
                this.emissionTimeleft = this.ParticleSystem.EmitDuration;
                this.emissionCountDown = true;
            }
        }
    }
}

