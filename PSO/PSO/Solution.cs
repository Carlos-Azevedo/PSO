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

namespace PSO
{
    /// <summary>
    /// The abstract for a Solution object.
    /// A Solution must contain a set of parameters, be able to update these parameters and be comparable to another solution.
    /// </summary>
    abstract class Solution
    {

        #region Properties
		/// <summary>
        /// The set of parameters that compose this solution.
        /// </summary>
        public List<Double> Parameters;

        /// <summary>
        /// This represents how thre result the solution is for its current set of parameters
        /// </summary>
        public Double Fitness;
	    #endregion


        #region Methods
        /// <summary>
        /// Compares this instance of Solution to another and returns if this is better.
        /// </summary>
        /// <param name="other">
        /// The solution this object is being compared to.
        /// </param>
        /// <returns>
        /// True if this Solution is considered better than other, False otherwise.
        /// </returns>
        public virtual bool BetterThan(Solution other)
        {
            return this.Fitness > other.Fitness;
        }

        /// <summary>
        /// Call to update the value of the solution's parameters for each iteration of the PSO.
        /// </summary>
        /// <param name="speedParameters">
        /// List of individual speedParameters for each parameter, used as the first parameter of the updateFunc call.
        /// </param>
        /// <param name="updateFunc">
        /// Updates each value of the Parameters property using a value of speeds. Returns the new value for the updated parameter.
        /// </param>
        public virtual void UpdateParameters(List<Object> speedParameters, Func<Object, Double, Double> updateFunc)
        {
            if(speedParameters.Count != Parameters.Count)
            {
                throw new InvalidOperationException("The number of elements in speeds must match the number of elements in Parameters.");
            }

            for (int index = 0; index < this.Parameters.Count; index++)
            {
                Parameters[index] = updateFunc(speedParameters[index], this.Parameters[index]);
            }
        } 
        #endregion

    }
}
