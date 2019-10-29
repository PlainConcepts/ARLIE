using System;
using System.Runtime.Serialization;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;

namespace RLEnvs.Renderers
{
    [DataContract]
    public class TerrainRenderer : Drawable2D
    {
        [RequiredComponent(false)]
        protected MeshComponent meshComponent;

        [RequiredComponent]
        protected MaterialComponent materialComponent;

        public override void Draw(TimeSpan gameTime)
        {
            if (this.meshComponent.InternalModel == null ||
                this.materialComponent.Material == null)
            {
                return;
            }

            this.materialComponent.Material.LayerId = this.LayerId;

            var meshGroup = this.meshComponent.Meshes;
            if (meshGroup != null)
            {
                var worldTransform = this.Transform2D.WorldTransform;

                for (int i = 0; i < meshGroup.Count; i++)
                {
                    var currentMesh = meshGroup[i];
                    this.RenderManager.DrawMesh(currentMesh, this.materialComponent.Material, ref worldTransform);
                }
            }
        }
    }
}
 