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
using PSO.ClassicPSO;
using PSO.Parameters;
using PSO.Interfaces;
using PSO.Abstracts;

namespace PSO.InertiaPSO
{
    public class InertiaSwarm : ClassicSwarm
    {
        public Double InertiaMax;

        public Double InertiaMin;

        public UInt32 InertiaMaxTime;

        public InertiaSwarm(InertiaSwarmCreationParameters parameters)
        {
            this.InertiaMax = parameters.InertiaMax;
            this.InertiaMin = parameters.InertiaMin;
            this.InertiaMaxTime = parameters.InertiaMaxTime;
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
                this.CreateRandomsList(parameters.MaximumParameterValue, parameters.MinimumParameterValue, parameters.NumberOfParameters, ref newSpeedsList, ref newParameterList);
                ISolution newParticleSolution = new ClassicSolution(parameters.SolutionFunction, parameters.AuxData, parameters.MinimumParameterValue, parameters.MaximumParameterValue);
                newParticleSolution.Parameters = newParameterList;
                newParticleSolution.UpdateFitness();
                InertiaParticleCreationParameters creationParams = new InertiaParticleCreationParameters();
                creationParams.Speeds = newSpeedsList;
                creationParams.Solution = newParticleSolution;
                creationParams.InertiaMax = this.InertiaMax;
                creationParams.InertiaMin = this.InertiaMin;
                creationParams.InertiaMaxTime = this.InertiaMaxTime;
                particles.Add(new InertiaParticle(creationParams));
            }
            return particles;
        }
    }
}
