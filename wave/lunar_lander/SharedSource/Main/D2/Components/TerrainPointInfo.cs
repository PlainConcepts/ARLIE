using System.Runtime.Serialization;
using WaveEngine.Common.Math;

namespace RLEnvs.Components
{
    [DataContract]
    public class TerrainPointInfo
    {
        /// <summary>
        /// The position of the point.
        /// </summary>
        [DataMember]
        public Vector2 Position;

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Position: {this.Position}";
        }
    }
}
