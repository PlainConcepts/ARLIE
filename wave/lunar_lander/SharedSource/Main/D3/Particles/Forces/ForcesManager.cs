using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Physics2D;
using WaveEngine.Framework;

namespace Batchers.Particles.Forces
{
    [DataContract]
    public class ForcesManager : Component
    {
        private List<AForce> forces;

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.forces = new List<AForce>();
        }

        public void RegisterForce(AForce force)
        {
            this.forces.Add(force);
        }

        public void UnRegisterForce(AForce force)
        {
            this.forces.Remove(force);
        }

        public void GetForces(ColliderCategory2D category, ref List<AForce> compatibleForces)
        {
            compatibleForces.Clear();

            foreach (var force in this.forces)
            {
                if ((force.IsEnabled) && ((force.Category & category) > 0))
                {
                    compatibleForces.Add(force);
                }
            }
        }
    }
}
