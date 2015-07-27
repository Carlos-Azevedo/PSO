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

namespace PSO.StablePSO
{
    /// <summary>
    /// Based on the article The Particle Swarm—Explosion, Stability, and Convergence in a Multidimensional Complex Space
    /// by Maurice Clerc and James Kennedy, this variation of the PSO uses a constraining factor to impede the Speeds values
    /// from increasing infinitely, thus leading to a more stable PSO.
    /// </summary>
    public class StableSwarm : ClassicSwarm
    {
        public Double Constraint { get; set; }

        public StableSwarm(StableSwarmCreationParameters parameters)
        {
            this.FillSwarmParameters(parameters); 
            this.Constraint = StableSwarm.CalculateConstraint(parameters.GlobalBestBias, parameters.PersonalBestBias, parameters.ConstraintValue);
            this.Particles = this.CreateParticles(parameters);
            this.SplitParticlesInSets(parameters.NumberOfParticleSets);
        }

        protected new List<IParticle> CreateParticles(SwarmCreationParameters parameters)
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
                StableParticleCreationParameters creationParams = new StableParticleCreationParameters();
                creationParams.Speeds = newSpeedsList;
                creationParams.Solution = newParticleSolution;
                creationParams.Constraint = this.Constraint;
                particles.Add(new StableParticle(creationParams));
            }
            return particles;
        }

        public static Double CalculateConstraint(Double globalBias, Double personalBias, Double constraint)
        {
            Double bias = globalBias + personalBias;
            if (bias <= 4)
            {
                return Math.Sqrt(constraint);
            }
            else
            {
                Double numerator = 2 * constraint;
                Double denominator = Math.Sqrt(Math.Pow(bias, 2) - 4 * bias);
                denominator = Math.Abs(bias - 2 + denominator);
                return Math.Sqrt(numerator / denominator);
            }
        }
    }
}
