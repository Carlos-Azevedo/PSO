/*
PSO.dll is a collection of different PSO implementations.
Copyright (C) 2015  Carlos Frederico Azevedo

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSO.Interfaces;
using PSO.InertiaPSO;
using PSO.Parameters;
using PSO.Abstracts;

namespace PSO.FrankensteinPSO
{
    public class FrankensteinParticle : InertiaParticle, IConnectedParticles
    {
        #region Properties
        public List<IParticle> Particles { get; set; }

        public LinkedList<int> ConnectedIds { get; set; }

        public UInt32 FinalTopologyUpdate { get; set; }

        public List<List<Double>> ConnectedParticlesParameters;

        public Random RandomGenerator;

        public UInt32 TrimmingIterations; 
        #endregion

        #region Methods
        public FrankensteinParticle(FrankensteinParticleCreationParameters parameters)
        {
            this.Id = Particle.CurrentId;
            this.CurrentIteration = 0;
            this.FillParameters(parameters);
        }

        protected void FillParameters(FrankensteinParticleCreationParameters parameters)
        {
            base.FillParameters((InertiaParticleCreationParameters)parameters);
            this.Particles = parameters.Particles;
            this.ConnectedIds = parameters.ConnectedIds;
            this.FinalTopologyUpdate = parameters.FinalTopologyUpdate;
            this.RandomGenerator = parameters.RandomGenerator;
        }

        public override void SetSpeedParameters(SpeedParameters parameters)
        {
            int removed = this.RandomGenerator.Next(this.ConnectedIds.Count - 3);
            removed = removed + 2;
            this.UpdateConnectedParticles(removed);
            this.CreateConnectedParticlesParameters();
            base.SetSpeedParameters(parameters);
        }

        public void CreateConnectedParticlesParameters()
        {
            this.ConnectedParticlesParameters = new List<List<double>>();
            foreach(int id in this.ConnectedIds)
            {
                IParticle connectedParticle = this.Particles.Find(p => p.Id == id);
                Double[] connecetedParameters = new Double[this.Speeds.Count];
                connectedParticle.PersonalBestSolution.Parameters.CopyTo(connecetedParameters);
                this.ConnectedParticlesParameters.Add(connecetedParameters.ToList());
            }
        }

        public void UpdateConnectedParticles(int stepsToRemoved)
        {
            UInt32 removalIteration = (UInt32)(this.FinalTopologyUpdate / (this.Particles.Count - 3));
            if (this.CurrentIteration <= this.FinalTopologyUpdate && this.CurrentIteration  % removalIteration == (removalIteration - 1))
            {
                if (this.Id < (Particles.Count - 3 - this.TrimmingIterations) &&
                    this.ConnectedIds.Count > 3)                
                    {
                        LinkedListNode<int> currentNode = this.ConnectedIds.Find(this.Id);
                        for (int i = stepsToRemoved; i > 0; i--)
                        {
                            currentNode = currentNode.Next;
                        }
                        int removedId = currentNode.Value;
                        IParticle removedParticle = this.Particles.Find(p => p.Id == removedId);
                        if (removedParticle.GetType().GetInterfaces().Contains(typeof(IConnectedParticles)))
                        {
                            ((IConnectedParticles)removedParticle).ConnectedIds.Remove(this.Id);
                        }
                        this.ConnectedIds.Remove(currentNode);
                    }                
                this.TrimmingIterations++;
            }
        }

        public new void UpdateSpeeds(SpeedParameters parameters)
        {
            Double currentInertia = this.CalculateInertia();
            for (int index = 0; index < this.CurrentSolution.Parameters.Count; index++)
            {
                Double newSpeed = this.Speeds[index];
                newSpeed = newSpeed * currentInertia;
                foreach(List<Double> connectedParameters in this.ConnectedParticlesParameters)
                {
                    newSpeed = newSpeed + this._CalculateInfluence(this.CurrentSolution.Parameters[index], connectedParameters[index], parameters.PersonalBestBias/(double)this.ConnectedIds.Count, parameters.RandomListPersonal[index]);
                }
                this.Speeds[index] = newSpeed;
            }
        }
        #endregion
    }
}
