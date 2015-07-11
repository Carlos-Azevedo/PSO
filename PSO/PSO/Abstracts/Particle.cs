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

namespace PSO.Abstracts
{
    /// <summary>
    /// A particle describes an object that contains a current Solution, a best Solution, current n-dimensional speed and 
    /// </summary>
    public abstract class Particle
    {
        #region Static Properties
        public static UInt32 CurrentId = 0;
        #endregion

        #region Properties
        public List<Double> Speeds;

        public Solution PersonalBestSolution;

        public Solution CurrentSolution;

        public UInt32 Id;

        public EVariants Variant;

        private Func<SpeedParameters, List<Double>> SpeedUpdateFunction;
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

        public virtual void UpdateSpeeds()
        {
            SpeedParameters speedUpdateParameters = this.createSpeedParameters(this.Speeds);
            this.Speeds = this.SpeedUpdateFunction(speedUpdateParameters);

        }

        public virtual void UpdateSolution()
        {
            this.CurrentSolution.UpdateParameters(this.Speeds);
            this.CurrentSolution.UpdateFitness();
            if(this.CurrentSolution.BetterThan(this.PersonalBestSolution))
            {
                this.PersonalBestSolution = this.CurrentSolution.Copy();
            }
        }

        /// <summary>
        /// Used by each PSO variant to create their required parameters for updating the speed values.
        /// </summary>
        /// <returns>
        /// This PSO variant's set of parameters used to update the values of the Speeds List.
        /// </returns>
        public abstract SpeedParameters createSpeedParameters(List<Double> speeds);
        #endregion
    }
}
