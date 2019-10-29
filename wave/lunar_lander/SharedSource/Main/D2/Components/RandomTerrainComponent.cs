using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Managers;
using WaveEngine.Framework.Services;
using Random = WaveEngine.Framework.Services.Random;

namespace RLEnvs.Components
{
    [DataContract]
    public class RandomTerrainComponent : Component, IDisposable
    {
        private VirtualScreenManager virtualScreenManager;
        private Vector2[] generatedPoints;

        [DataMember]
        private int chunks;

        [RequiredComponent]
        protected TerrainComponent terrain;

        [RequiredService]
        protected Random random;

        public int Chunks
        {
            get => this.chunks;
            set
            {
                this.chunks = Math.Max(6, value);
                this.GenerateRandom();
                this.RefreshTerrain();
            }
        }

        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.chunks = 11;
        }

        protected override void ResolveDependencies()
        {
            base.ResolveDependencies();
            this.virtualScreenManager = this.Owner.Scene.VirtualScreenManager;
            this.virtualScreenManager.OnRefresh += this.VirtualScreenManagerOnRefresh;
        }

        protected override void DeleteDependencies()
        {
            base.DeleteDependencies();
            this.virtualScreenManager.OnRefresh -= this.VirtualScreenManagerOnRefresh;
        }

        protected override void Initialize()
        {
            base.Initialize();
            this.GenerateRandom();
            this.RefreshTerrain();
        }

        private void GenerateRandom()
        {
            // Generate random points [0, 1]
            var randomPoints = Enumerable.Range(0, this.Chunks)
                                             .Select(x => new Vector2(x / (float)(this.Chunks - 1), (float)random.NextDouble() / 2f))
                                             .ToArray();
            // Set helipad
            var helipadInitialChunk = ((int)(this.Chunks / 2f) - 2);
            var helipadSize = 5;
            for (int i = helipadInitialChunk; i < helipadInitialChunk + helipadSize; i++)
            {
                randomPoints[i] = new Vector2(randomPoints[i].X, 0.25f);
            }

            // Smooth result
            this.generatedPoints = new Vector2[randomPoints.Length];
            for (int i = 0; i < randomPoints.Length; i++)
            {
                var prev = i <= 0 ? (Vector2.One * .25f) : randomPoints[i - 1];
                var next = i >= randomPoints.Length - 1 ? (Vector2.One * .25f) : randomPoints[i + 1];
                var current = randomPoints[i];
                var smoothY = 0.33f * (prev.Y + current.Y + next.Y);
                this.generatedPoints[i] = new Vector2(current.X, smoothY);
            }
        }

        private void RefreshTerrain()
        {
            if (this.generatedPoints != null)
            {
                var size = new Vector2(this.virtualScreenManager.VirtualWidth, this.virtualScreenManager.VirtualHeight);

                var points = new List<TerrainPointInfo>();
                for (int i = 0; i < this.generatedPoints.Length; i++)
                {
                    points.Add(new TerrainPointInfo { Position = this.generatedPoints[i] * size });
                }

                this.terrain.LinePoints = points;
            }
        }

        private void VirtualScreenManagerOnRefresh(object sender, VirtualScreenManager e)
        {
            this.RefreshTerrain();
        }

        public void Dispose()
        {
            this.DeleteDependencies();
        }
    }
}
