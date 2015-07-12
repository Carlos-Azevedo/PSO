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
    /// <summary>
    /// A particle describes an object that contains a current Solution, a best Solution, current n-dimensional speed and 
    /// </summary>
    public abstract class Particle
    {
        #region Static Properties
        /// <summary>
        /// A static Id value incremented when a new particle is created so each one has an unique value.
        /// </summary>
        public static UInt32 CurrentId = 0;
        #endregion

        #region Properties
        /// <summary>
        /// The List of speeds used to search for the optimal Solution in the problem's n-dimensional Solution Space.
        /// </summary>
        public List<Double> Speeds;

        /// <summary>
        /// A set of parameters used to update the Speed values.
        /// Should be updated by Swarm for each iteration.
        /// </summary>
        public SpeedParameters SpeedParameters;

        /// <summary>
        /// The personal best Solution configuration this Particle has found. Used to guide the direction in which the optimal Solution is searched.
        /// </summary>
        public Solution PersonalBestSolution;

        /// <summary>
        /// The current Solution configuration for this particle.
        /// </summary>
        public Solution CurrentSolution;

        /// <summary>
        /// An Id value used to uniquely identify this Particle.
        /// </summary>
        public UInt32 Id;

        /// <summary>
        /// The PSO variant this Particle type belongs to.
        /// </summary>
        public EPSOVariants Variant;
        #endregion

        #region Methods
        //public virtual List<Double> UpdateSpeeds(Solution globalBestSolution, Double personalBestWeight, Double globalBestWeight, Double acceleration, ref Random randomGenerator)
        //{
        //    List<Double> updatedSpeeds = new List<double>(this.Speeds.Count);

        //    for (int index = 0; index < this.PersonalBestSolution.Parameters.Count; index++)
        //    {
        //        Double newSpeed = this.Speeds[index] * acceleration;
        //        newSpeed = newSpeed + (this.PersonalBestSolution.Parameters[index] - this.Speeds[index]) * personalBestWeight * randomGenerator.NextDouble();
        //        newSpeed = newSpeed + (globalBestSolution.Parameters[index] - this.Speeds[index]) * globalBestWeight * randomGenerator.NextDouble();
        //        updatedSpeeds[index] = newSpeed;
        //    }

        //    return updatedSpeeds;
        //}

        public virtual void Iterate()
        {
            this.UpdateSpeeds(this.SpeedParameters);
            this.UpdateSolution();
        }
        
        /// <summary>
        /// Updates this Particle's Speeds property values. Speed update parameters and operation vary with PSO implementations.
        /// </summary>
        /// <param name="parameters">
        /// An object with the necessary parameters requried to update the Speeds for thie PSO variant
        /// </param>
        public abstract void UpdateSpeeds(SpeedParameters parameters);

        /// <summary>
        /// Updates this Particle's parameters values and calculates the new Parameters' fitness.
        /// If the new Solution configuration has a better Fitness than the CurrentBestSolution, the new Solution becomes the CurrentBestSolution.
        /// </summary>
        public virtual void UpdateSolution()
        {
            this.CurrentSolution.UpdateParameters(this.Speeds);
            this.CurrentSolution.UpdateFitness();
            if(this.CurrentSolution.BetterThan(this.PersonalBestSolution))
            {
                this.PersonalBestSolution = this.CurrentSolution.Copy();
            }
        }        
        #endregion
    }
}
