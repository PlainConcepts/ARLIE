using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Graphics.VertexFormats;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Services;

namespace RLEnvs.Components
{
    [DataContract]
    public class TerrainComponent : MeshComponent
    {
        private const int VerticesPerPoint = 2;
        private const int IndicesPerPoint = 2;
        private List<Mesh> meshes;
        private List<TerrainPointInfo> linePoints;
        private BoundingBox boundingBox;

        [RequiredComponent]
        protected Transform2D transform;

        [RequiredComponent]
        protected EdgeCollider2D collider;

        [DataMember]
        [RenderPropertyAsList(
            CustomPropertyName = "Line Points",
            Tooltip = "List with the points that defines the terrain",
            AddItemAction = nameof(AddItem),
            UpdateItemAction = nameof(AddItem),
            RemoveItemAction = nameof(AddItem))]
        public List<TerrainPointInfo> LinePoints
        {
            get
            {
                return this.linePoints;
            }

            set
            {
                this.linePoints = value;

                if (this.isInitialized)
                {
                    this.RefreshMeshes();
                }
            }
        }

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.linePoints = new List<TerrainPointInfo>();
            this.meshes = new List<Mesh>();
            this.boundingBox = new BoundingBox();
            this.ModelMeshName = "Default";
        }

        protected override void Initialize()
        {
            this.RefreshMeshes();
        }

        public void AddItem(TerrainPointInfo point)
        {
            this.RefreshMeshes();
        }

        private void RefreshMeshes()
        {
            this.DisposeMeshes();

            if (this.linePoints == null || this.linePoints.Count < 2)
            {
                return;
            }

            this.ResetBoundingBox();
            this.GenerateMeshes();
            this.GenerateInternalModel();
        }

        private void ResetBoundingBox()
        {
            var points = this.linePoints.Select(x => x.Position.ToVector3(0)).ToList();
            points.Add(Vector3.Zero);
            this.boundingBox = WaveEngine.Common.Math.BoundingBox.CreateFromPoints(points);
        }

        private void GenerateMeshes()
        {
            var vertices = new VertexPosition[this.linePoints.Count * VerticesPerPoint];
            var colliderVertices = new Vector2[this.linePoints.Count];
            for (int i = 0, j = 0; i < this.linePoints.Count; i++)
            {
                var currentPoint = this.linePoints[i];
                var currentPosition = new Vector3(currentPoint.Position.X, -currentPoint.Position.Y, 0);
                colliderVertices[i] = currentPosition.ToVector2();
                vertices[j++] = new VertexPosition(currentPosition);
                currentPosition.Y = 0;
                vertices[j++] = new VertexPosition(currentPosition);
            }

            var center = this.boundingBox.Center.ToVector2();
            this.collider.Offset = new Vector2(-center.X, center.Y);
            this.collider.Vertices = colliderVertices;

            var indices = new ushort[this.linePoints.Count * IndicesPerPoint];

            for (int i = 1; i < indices.Length; i++)
            {
                indices[i] = (ushort)i;
            }

            this.AddMesh(vertices, indices, PrimitiveType.TriangleStrip);
        }

        private void AddMesh<T>(T[] vertices, ushort[] indices, PrimitiveType primitiveType)
            where T : struct, IBasicVertex
        {
            if (vertices.Length == 0 || indices.Length == 0)
            {
                return;
            }

            var vertexBuffer = new VertexBuffer(vertices[0].VertexFormat);
            vertexBuffer.SetData(vertices);
            var indexBuffer = new IndexBuffer(indices);

            var primitiveCount = primitiveType == PrimitiveType.TriangleStrip ? indices.Length - 2 : indices.Length / 3;
            var mesh = new Mesh(0, vertices.Length, 0, primitiveCount, vertexBuffer, indexBuffer, primitiveType);

            mesh.BoundingBox = this.boundingBox;
            this.ResetBoundingBox();

            this.meshes.Add(mesh);
        }

        private void GenerateInternalModel()
        {
            if (this.InternalModel != null)
            {
                this.InternalModel.Unload();
                this.InternalModel = null;
            }

            this.InternalModel = new InternalModel();
            this.InternalModel.FromMeshes(WaveServices.GraphicsDevice, this.meshes);

            this.ThrowRefreshEvent();

            this.RefreshTransformRectangle();
        }

        private void RefreshTransformRectangle()
        {
            var boundingBox = this.BoundingBox;
            if (boundingBox.HasValue)
            {
                var bbMin = boundingBox.Value.Min;
                var bbMax = boundingBox.Value.Max;
                var size = Vector3.Abs(boundingBox.Value.Max) + Vector3.Abs(bbMin);
                transform.Rectangle = new RectangleF(bbMin.X, bbMin.Y, size.X, size.Y);
            }
            else
            {
                transform.Rectangle = Rectangle.Empty;
            }
        }

        private void DisposeMeshes()
        {
            var graphicsDevice = WaveServices.GraphicsDevice;
            foreach (var mesh in this.meshes)
            {
                graphicsDevice.DestroyIndexBuffer(mesh.IndexBuffer);
                graphicsDevice.DestroyVertexBuffer(mesh.VertexBuffer);
            }

            this.meshes.Clear();
        }

        public override void Dispose()
        {
            base.Dispose();
            this.DisposeMeshes();
        }
    }
}
