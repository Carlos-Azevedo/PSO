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

namespace PSO.Abstracts
{
    /// <summary>
    /// The abstract for a Solution object.
    /// A Solution must contain a set of parameters, be able to update these parameters and be comparable to another solution.
    /// </summary>
    public abstract class Solution : ISolution
    {

        #region Properties
		/// <summary>
        /// The set of parameters that compose this solution.
        /// </summary>
        public List<Double> Parameters { get; set; }

        /// <summary>
        /// This represents how thre result the solution is for its current set of parameters.
        /// </summary>
        public Double Fitness { get; set; }

        /// <summary>
        /// Private instance of AuxData.
        /// </summary>
        protected Object _AuxData;

        /// <summary>
        /// Auxiliary data needed to call RunSolution.
        /// </summary>
        public Object AuxData {
            get
            {
                return _AuxData;
            }
        }

        /// <summary>
        /// Method called to execute this solution. Must be thread safe.
        /// Takes the list of parameters and the AuxData properties as input and outputs the Fitness value.
        /// </summary>
        public Func<List<Double>, Object, Double> RunSolution;
	    #endregion

        #region Methods      
        /// <summary>
        /// Compares this Solution's Fitness score to another's and returns if this is better.
        /// </summary>
        /// <param name="other">
        /// The solution this object is being compared to.
        /// </param>
        /// <returns>
        /// True if this Solution is considered better than other, False otherwise.
        /// </returns>
        public virtual bool BetterThan(ISolution other)
        {
            return this.Fitness > other.Fitness;
        }

        /// <summary>
        /// Update the values of the solution's parameters.
        /// </summary>
        /// <param name="speeds">
        /// List of individual speeds for each parameter, used as the first parameter of the UpdateParametersFunction call.
        /// </param>
        public abstract void UpdateParameters(List<Double> speeds);               

        /// <summary>
        /// Updates this solution's Fitness score for the execution of SolutionExecution with the current set of Parameters.
        /// </summary>
        public virtual void UpdateFitness()
        {
            this.Fitness = this.RunSolution(this.Parameters, this.AuxData);
        }

        /// <summary>
        /// Returns a deap copy of this Solution.
        /// </summary>
        /// <returns>
        /// A copy of this Solution
        /// </returns>
        public abstract ISolution Copy();
        #endregion
    }
}
