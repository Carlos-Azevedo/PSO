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
    public class ClassicSwarmCreationParameters : SwarmCreationParameters
    { }

    public class ClassicSwarm : Swarm
    {
        public ClassicSwarm(ClassicSwarmCreationParameters parameters)
        {
            if (parameters.Solution.GetType() != typeof(ClassicSolution))
            {
                throw new InvalidOperationException("A classic PSO requires a ClassicSolution Solution to work");
            }
            this.FitnessThreshold = parameters.FitnessThreshold;
            this.MaxIterations = parameters.MaxIterations;
            this.RandomGenerator = parameters.RandomNumberGenerator;
            this.FitnessThreshold = parameters.FitnessThreshold;
            this.Acceleration = parameters.Acceleration;
            this.GlobalBestBias = parameters.GlobalBestBias;
            this.PersonalBestBias = parameters.PersonalBestBias;
            this.Particles = this.CreateParticles(parameters.NumberOfParticles, parameters.MaximumParameterValue, parameters.MinimumParameterValue, parameters.Solution);           
        }

        public List<IParticle> CreateParticles(UInt32 numberOfParticles, Double maxParameterValue, Double minParameterValue, ISolution solution)
        {
            Double parameterRange = maxParameterValue - minParameterValue;
            Double speedRange = parameterRange * 2.0;
            List<IParticle> particles = new List<IParticle>();
            for (UInt32 index = 0; index < numberOfParticles; index++)
            {
                List<Double> newParameterList = new List<double>();
                List<Double> newSpeedsList = new List<double>();
                for (int parameterIndex = 0; parameterIndex < numberOfParticles; parameterIndex++)
                {
                    newParameterList.Add(RandomGenerator.NextDouble() * parameterRange - minParameterValue);
                    newSpeedsList.Add(RandomGenerator.NextDouble() * speedRange - parameterRange);
                }
                ISolution newParticleSolution = solution.Copy();
                newParticleSolution.Parameters = newParameterList;
                newParticleSolution.UpdateFitness();
                particles.Add(new ClassicParticle(newSpeedsList, newParticleSolution, Particle.CurrentId));
            }
            return particles;
        }

        public override void createSpeedParameters()
        {
            foreach (IParticle particle in this.Particles)
            {
                SpeedParameters speedParams = new SpeedParameters();
                speedParams.Acceleration = this.Acceleration;
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
                    speedParams.RandomListGlobal[i] = this.RandomGenerator.NextDouble();
                    speedParams.RandomListPersonal[i] = this.RandomGenerator.NextDouble();
                }
                particle.SpeedParameters = speedParams;
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
            }
            return this.GlobalBestSolution;
        }
    }
}
