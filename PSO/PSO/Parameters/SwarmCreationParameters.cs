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

namespace PSO.Parameters
{
    /// <summary>
    /// Creation parameters utilized to create a ClassicPSO.
    /// </summary>
    public class SwarmCreationParameters
    {
        /// <summary>
        /// Final search iteration for the PSO algorithm.
        /// </summary>
        public UInt32 MaxIterations { get; set; }

        /// <summary>
        /// The lowest acceptable value for the Fitness of a solution.
        /// If a solution reaches this value, that solution will be returned.
        /// </summary>
        public Double FitnessThreshold { get; set; }

        /// <summary>
        /// The lowest value a parameter can reach as part of an ISolution parameter.
        /// </summary>
        public Double MinimumParameterValue { get; set; }

        /// <summary>
        /// The highest value a parameter can reach as part of an ISolution parameter.
        /// </summary>
        public Double MaximumParameterValue { get; set; }

        /// <summary>
        /// A random number generator used when updating speed values.
        /// Since this random generator is not thread safe, it is only used when the algorithm
        /// is operating synchronously.
        /// </summary>
        public Random RandomNumberGenerator { get; set; }

        /// <summary>
        /// The number of particles that make up this swarm.
        /// </summary>
        public UInt32 NumberOfParticles { get; set; }

        /// <summary>
        /// How many parameters a solution can contain.
        /// </summary>
        public UInt32 NumberOfParameters { get; set; }

        /// <summary>
        /// This function is used to evaluate the parameters and should represent the problem being solved.
        /// The fir parameter in it is the list of parameters that compose a solution.
        /// The secon parameter should contain any auxiliary data required to run the function.
        /// The return should be a fitness value measured by how high it is. The higher the better.
        /// If this function is not thread-safe, set the NumberOfParticleSets value to 1.
        /// </summary>
        public Func<List<Double>, Object, Double> SolutionFunction {get; set;}

        /// <summary>
        /// Auxiliar data required by SolutionFunction.
        /// Must be copiable.
        /// </summary>
        public Object AuxData { get; set; }

        /// <summary>
        /// The influence of the best solution found by the PSO in each speeds update.
        /// </summary>
        public Double GlobalBestBias { get; set; }

        /// <summary>
        /// The influence of the best solution found by the particle in each speeds update.
        /// </summary>
        public Double PersonalBestBias { get; set; }

        /// <summary>
        /// This value represents how many threads should be used in each iteration.
        /// If SolutionFunction is not thread-safe, this must be set to 1.
        /// </summary>
        public int NumberOfParticleSets { get; set; }

        /// <summary>
        /// A method used to validate that all parameters used to create the swarm are valid.
        /// </summary>
        public virtual void VerifyValues()
        {
            foreach (var prop in this.GetType().GetProperties())
            {
                if (prop.GetValue(this) == null)
                {
                    throw new InvalidOperationException("The value of " + prop.Name + " can't be null.");
                }
                else if (prop.PropertyType == typeof(int) && prop.Name == "NumberOfParticleSets" && (int)prop.GetValue(this) <= 0)
                {
                    throw new InvalidOperationException("The value of NumberOfParticleSets must be a positive number.");
                }
                else if (prop.PropertyType == typeof(Double) && prop.Name != "MinimumParameterValue" && prop.Name != "MaximumParameterValue" && (Double)prop.GetValue(this) < 0)
                {
                    throw new InvalidOperationException("The value of " + prop.Name + " must not be a negative number");
                }
                else if (prop.PropertyType == typeof(UInt32) && (UInt32)prop.GetValue(this) == 0)
                {
                    throw new InvalidOperationException("The value of " + prop.Name + " must be a positive number");
                }

            }
        }
    }

    /// <summary>
    /// Parameters used to create a StablePSO.
    /// </summary>
    public class StableSwarmCreationParameters : SwarmCreationParameters
    {
        /// <summary>
        /// A constraining value which stops the particle's speed from growing too much and making the PSO unstable.
        /// </summary>
        public Double ConstraintValue;

        public override void VerifyValues()
        {
            base.VerifyValues();
            if (this.ConstraintValue <= 0.0 || this.ConstraintValue >= 1.0)
            {
                throw new InvalidOperationException("The value of ConstraintValue must be within 0.0 and 1.0.");
            }
        }
    }

    /// <summary>
    /// Parameters used to create an InertiaPSO.
    /// </summary>
    public class InertiaSwarmCreationParameters : SwarmCreationParameters
    {
        /// <summary>
        /// The highest value of inertia. Should be close to 1.0 to allow an early exploratory algorithm.
        /// </summary>
        public Double InertiaMax;

        /// <summary>
        /// The lowest value of inertia. Must be lower than InertiaMax and is used to force a careful exploration of the Solution space by the alogrithm.
        /// </summary>
        public Double InertiaMin;

        /// <summary>
        /// After this iteration, inertia is equal to InertiaMin.
        /// </summary>
        public UInt32 InertiaMaxTime;

         public override void VerifyValues()
        {
            base.VerifyValues();
            if (this.InertiaMin <= 0.0 || this.InertiaMax <= 0.0)
            {
                throw new InvalidOperationException("The values of Inertia must behigher than 0.");
            }
            else if(this.InertiaMax < this.InertiaMin)
            {
                throw new InvalidOperationException("The value of InertiaMax must behigher than InertiaMin.");
            }
        }
    }

    /// <summary>
    /// Parameters used to create an FrankensteinPSO.
    /// </summary>
    public class FrankensteinSwarmCreationParameters : InertiaSwarmCreationParameters
    {
        /// <summary>
        /// After this iteration, the particles are only connected to their 2 immediate neighbors.
        /// </summary>
        public UInt32 FinalTopologyUpdate;
    }
}
