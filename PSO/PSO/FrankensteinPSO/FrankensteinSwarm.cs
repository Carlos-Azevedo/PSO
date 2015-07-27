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
using PSO.InertiaPSO;
using PSO.Interfaces;
using PSO.Parameters;
using PSO.ClassicPSO;

namespace PSO.FrankensteinPSO
{
    /// <summary>
    /// Proposed in "Frankenstein’s PSO: A Composite Particle Swarm Optimization Algorithm" written by Marco A. Montes de Oca, Thomas Stützle, Mauro Birattari, 
    /// Member, IEEE , and Marco Dorigo, Fellow, IEEE. In the article multiple variants of the PSO were evaluated and the Frankenstein PSO was proposed by using
    /// the InertiaPSO combined with a mixture of the Fully Informed PSO and Adaptive Hierarchical Particle Swarm Optimizer. This PSO doesn't use the global
    /// best solution, instead particles are affected by the best solution of other particles to which they are connected. These connections are then cut
    /// during iterations.
    /// </summary>
    public class FrankensteinSwarm : InertiaSwarm
    {
        public UInt32 FinalTopologyUpdate;

        public FrankensteinSwarm(FrankensteinSwarmCreationParameters parameters)
        {
            this._FillSwarmParameters(parameters);
            this.Particles = this.CreateParticles(parameters);
            this.SplitParticlesInSets(parameters.NumberOfParticleSets);
        }

        protected void _FillSwarmParameters(FrankensteinSwarmCreationParameters parameters)
        {
            parameters.VerifyValues();
            this.FinalTopologyUpdate = parameters.FinalTopologyUpdate;
            this._FillSwarmParameters((InertiaSwarmCreationParameters)parameters);
        }

        protected override List<IParticle> CreateParticles(SwarmCreationParameters parameters)
        {
            List<IParticle> particles = new List<IParticle>();
            for (UInt32 index = 0; index < parameters.NumberOfParameters; index++)
            {
                List<Double> newParameterList = new List<double>();
                List<Double> newSpeedsList = new List<double>();
                this.CreateRandomsList(parameters.MaximumParameterValue, parameters.MinimumParameterValue, parameters.NumberOfParameters, ref newSpeedsList, ref newParameterList);
                ISolution newParticleSolution = new ClassicSolution(parameters.SolutionFunction, parameters.AuxData, parameters.MinimumParameterValue, parameters.MaximumParameterValue);
                newParticleSolution.Parameters = newParameterList;
                newParticleSolution.UpdateFitness();
                FrankensteinParticleCreationParameters creationParams = new FrankensteinParticleCreationParameters();
                creationParams.Speeds = newSpeedsList;
                creationParams.Solution = newParticleSolution;
                creationParams.InertiaMax = this.InertiaMax;
                creationParams.InertiaMin = this.InertiaMin;
                creationParams.InertiaMaxTime = this.InertiaMaxTime;
                creationParams.FinalTopologyUpdate = this.FinalTopologyUpdate;
                creationParams.Particles = this.Particles;
                int[] connectedIds = new int[parameters.NumberOfParameters];
                for (int i = 0; i < parameters.NumberOfParticles; i++)
			    {
                    connectedIds[i] = i;			 
			    }
                creationParams.ConnectedIds = new LinkedList<int>(connectedIds);
                creationParams.RandomGenerator = this.RandomGenerator;
                particles.Add(new FrankensteinParticle(creationParams));
            }
            return particles;
        }
    }
}
