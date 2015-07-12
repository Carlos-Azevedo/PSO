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
using PSO.Enumerators;
using PSO.Abstracts.Parameters;

namespace PSO.Abstracts
{
    public abstract class SwarmCreationParameters
    {
        public UInt32 MaxIterations;

        public Double FitnessThreshold;

        public Double MinimumParameterValue;

        public Double MaximumParameterValue;

        public Random RandomNumberGenerator;

        public UInt32 NumberOfParticles;

        public UInt32 NumberOfParameters;

        public Solution Solution;
    }

    public abstract class Swarm
    {
        #region Properties
        /// <summary>
        /// The list of particles that compose this Swarm
        /// </summary>
        public List<Particle> Particles;

        /// <summary>
        /// The best instance of Solution found up to now.
        /// </summary>
        public Solution GlobalBestSolution;

        /// <summary>
        /// The maximum number of iterations the search for an optimal solution can take.
        /// </summary>
        public UInt32 MaxIterations;

        /// <summary>
        /// A threshold that determines when a Solution can be considered acceptable.
        /// </summary>
        public Double FitnessThreshold;

        /// <summary>
        /// A random number generator used to generate random values throughout execution.
        /// </summary>
        public Random RandomGenerator;

        /// <summary>
        /// The PSO variant this swarm belongs to.
        /// </summary>
        public EPSOVariants Variant;

        /// <summary>
        /// Each element is a different disjunct set of Particles from the Particles property. There should be a number os sets equal to the number of processors available in the environment.
        /// Each set is used to update the Speeds, Parameters and Fitness values of each Particle in parallel. 
        /// </summary>
        public List<List<Particle>> ParticleSets;
        #endregion

        #region Methods
        //public virtual Swarm(SwarmCreationParameters parameters)
        //{
        //    this.FitnessThreshold = parameters.FitnessThreshold;
        //    this.MaxIterations = parameters.MaxIterations;
        //    this.RandomGenerator = parameters.RandomNumberGenerator;
        //    this.FitnessThreshold = parameters.FitnessThreshold;
        //    Double parameterRange = parameters.MaximumParameterValue - parameters.MinimumParameterValue;
        //    Double speedRange = parameterRange * 2.0;
        //    for (UInt32 index = 0; index < parameters.NumberOfParticles; index++)
        //    {
        //        List<Double> newParameterList = new List<double>();
        //        List<Double> newSpeedsList = new List<double>();
        //        for (int parameterIndex = 0; parameterIndex < parameters.NumberOfParticles; parameterIndex++)
        //        {
        //            newParameterList.Add(RandomGenerator.NextDouble() * parameterRange - parameters.MinimumParameterValue);
        //            newSpeedsList.Add(RandomGenerator.NextDouble() * speedRange - parameterRange);
        //        }
        //        Solution newParticleSolution = parameters.Solution.Copy();
        //        newParticleSolution.Parameters = newParameterList;
        //        newParticleSolution.UpdateFitness();
        //        Particle newParticle = new Particle(newSpeedsList, newParticleSolution, Particle.CurrentId);
        //    }
        //}

        /// <summary>
        /// Creates a set of parameters used by the particle to update its Speeds property.
        /// </summary>
        /// <param name="particle">
        /// The particle for which the SpeedParameters object will be created.
        /// </param>
        /// <returns>
        /// A SpeedParameters object used by the Particle to update its speeds and create a new set of Parameters for its Solution.
        /// </returns>
        public abstract SpeedParameters createSpeedParameters(Particle particle);

        /// <summary>
        /// Runs a single iteration of the PSO algorithm.
        /// Will attempt to run each ParticleSet in a different thread.
        /// </summary>
        public virtual void Iterate()
        {
            //Update all particle's SpeedParameters.
            foreach(Particle particle in this.Particles)
            {
                particle.SpeedParameters = this.createSpeedParameters(particle);
            }
            //Run each set of ParticleSets in a separate thread.
            Parallel.For(0, this.ParticleSets.Count, index =>
            {
                foreach (Particle particle in this.ParticleSets[index])
                {
                    particle.Iterate();
                }
            });
            this.UpdateBestGlobalSolution();
        }

        /// <summary>
        /// Updates the GlobalBestSolution property.
        /// </summary>
        public void UpdateBestGlobalSolution()
        {
            //Find the best solution in this iteration.
            Solution iterationBest = this.GlobalBestSolution;
            foreach (Particle particle in this.Particles)
            {
                if (particle.CurrentSolution.BetterThan(iterationBest))
                {
                    iterationBest = particle.CurrentSolution;
                }
            }
            if (iterationBest.BetterThan(this.GlobalBestSolution))
            {
                this.GlobalBestSolution = iterationBest.Copy();
            }
        }
        #endregion
    }
}
