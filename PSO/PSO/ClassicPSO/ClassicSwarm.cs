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
using PSO.Abstracts;
using PSO.Interfaces;
using PSO.Parameters;

namespace PSO.ClassicPSO
{
    public class ClassicSwarm : Swarm
    {
        protected ClassicSwarm()
        { }

        public ClassicSwarm(SwarmCreationParameters parameters)
        {
            this.FillSwarmParameters(parameters);
            this.Particles = this.CreateParticles(parameters);
            this.SplitParticlesInSets(parameters.NumberOfParticleSets);
        }

        protected override List<IParticle> CreateParticles(SwarmCreationParameters parameters)
        {
            
            List<IParticle> particles = new List<IParticle>();
            for (UInt32 index = 0; index < parameters.NumberOfParameters; index++)
            {
                List<Double> newParameterList = new List<double>();
                List<Double> newSpeedsList = new List<double>();
                this.CreateRandomsList(parameters.MaximumParameterValue, parameters.MinimumParameterValue, parameters.NumberOfParameters,ref newSpeedsList,ref newParameterList);
                ISolution newParticleSolution = new ClassicSolution(parameters.SolutionFunction, parameters.AuxData, parameters.MinimumParameterValue, parameters.MaximumParameterValue);
                newParticleSolution.Parameters = newParameterList;
                newParticleSolution.UpdateFitness();
                ClassicParticleCreationParameters creationParams = new ClassicParticleCreationParameters();
                creationParams.Speeds = newSpeedsList;
                creationParams.Solution = newParticleSolution;
                particles.Add(new ClassicParticle(creationParams));
            }
            return particles;
        }

        public override void createSpeedParameters()
        {
            foreach (IParticle particle in this.Particles)
            {
                SpeedParameters speedParams = new SpeedParameters();
                speedParams.GlobalBestBias = this.GlobalBestBias;
                speedParams.PersonalBestBias = this.PersonalBestBias;
                speedParams.GlobalBestSolution = new Double[particle.PersonalBestSolution.Parameters.Count];
                this.GlobalBestSolution.Parameters.CopyTo(speedParams.GlobalBestSolution);
                speedParams.PersonalBestSolution = new Double[particle.PersonalBestSolution.Parameters.Count];
                particle.PersonalBestSolution.Parameters.CopyTo(speedParams.PersonalBestSolution);
                speedParams.RandomListGlobal = new List<double>(particle.Speeds.Count);
                speedParams.RandomListPersonal = new List<double>(particle.Speeds.Count);
                for (int i = 0; i < particle.Speeds.Count; i++)
                {
                    speedParams.RandomListGlobal.Add(this.RandomGenerator.NextDouble());
                    speedParams.RandomListPersonal.Add(this.RandomGenerator.NextDouble());
                }
                particle.SetSpeedParameters(speedParams);
            }
        }

        public ISolution RunPSO()
        {
            UInt32 currentIteration = 0;
            this.GlobalBestSolution = this.Particles[0].CurrentSolution;
            this.UpdateBestGlobalSolution();
            while(currentIteration < this.MaxIterations)
            {
                this.Iterate();
                if (this.GlobalBestSolution.Fitness > this.FitnessThreshold)
                {
                    break;
                }
                currentIteration++;
            }
            return this.GlobalBestSolution;
        }
    }
}
