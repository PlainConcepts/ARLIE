using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WaveEngine.Common.Math;
using WaveEngine.Common.Physics2D;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;

namespace RLEnvs.Components
{
    [DataContract]
    public class EngineParticlesEmitter : Behavior
    {
        private List<Entity> particlesPool;
        private Dictionary<Entity, float> activeParticles;

        protected override void Initialize()
        {
            base.Initialize();

            this.particlesPool = new List<Entity>();
            this.activeParticles = new Dictionary<Entity, float>();
        }

        public void CreateParticle(float mass, Vector2 position, float ttl, Vector2 impulse)
        {
            var particleAndBody = GetOrCreateParticle(mass, position);
            this.activeParticles.Add(particleAndBody.Item1, ttl);
            particleAndBody.Item2.ApplyLinearImpulse(impulse, position, true);
        }

        private Tuple<Entity, RigidBody2D> GetOrCreateParticle(float mass, Vector2 position)
        {
            if (this.particlesPool.Count == 0)
            {
                var body = new RigidBody2D();
                var newParticle = new Entity()
                    .AddComponent(new Transform2D { Position = position, Scale = (Vector2.One / 2) })
                    .AddComponent(new Sprite { TexturePath = WaveContent.Assets.particle_png, MaterialPath = WaveContent.Assets.particleMaterial })
                    .AddComponent(new SpriteRenderer())
                    .AddComponent(new CircleCollider2D
                    {
                        CollisionCategories = ColliderCategory2D.Cat4,
                        CollidesWith = ColliderCategory2D.Cat1,
                        Friction = 0.1f,
                        Density = mass,
                        Restitution = 0.3f
                    })
                    .AddComponent(body);

                this.Owner.AddChild(newParticle);
                return Tuple.Create(newParticle, body);
            }
            else
            {
                var particle = this.particlesPool[0];
                this.particlesPool.RemoveAt(0);
                particle.IsVisible = true;
                particle.IsActive = true;
                particle.FindComponent<Transform2D>().Position = position;
                particle.FindComponent<CircleCollider2D>().Density = mass;
                var body = particle.FindComponent<RigidBody2D>();
                body.ResetPosition(position);
                return Tuple.Create(particle, body);
            }
        }

        protected override void Update(TimeSpan gameTime)
        {
            var currentActiveEntities = this.activeParticles.Keys.ToArray();
            foreach (var entity in currentActiveEntities)
            {
                var ttl = this.activeParticles[entity];
                ttl -= 0.15f;
                if (ttl < 0)
                {
                    entity.IsVisible = false;
                    entity.IsActive = false;
                    this.activeParticles.Remove(entity);
                    this.particlesPool.Add(entity);
                }
                else
                {
                    this.activeParticles[entity] = ttl;
                }
            }
        }
    }
}
