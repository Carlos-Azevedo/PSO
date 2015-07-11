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
namespace PSO.Abstracts
{
    /// <summary>
    /// The abstract for a Solution object.
    /// A Solution must contain a set of parameters, be able to update these parameters and be comparable to another solution.
    /// </summary>
    public abstract class Solution
    {

        #region Properties
		/// <summary>
        /// The set of parameters that compose this solution.
        /// </summary>
        public List<Double> Parameters;

        /// <summary>
        /// This represents how thre result the solution is for its current set of parameters.
        /// </summary>
        public Double Fitness;

        /// <summary>
        /// This property represents the Solution's execution and should return it's fitness value.
        /// </summary>
        public Func<SolutionParameters, Double> SolutionExecution;

        /// <summary>
        /// Function used to update this Solution's Parameters property based on the list of speeds, one for each parameter.
        /// </summary>
        public Func<List<Double>, List<Double>, List<Double>> UpdateParametersFunction;
	    #endregion


        #region Methods
        /// <summary>
        /// Constructor for a solution object with the minimum information required to create a valid Solution object.
        /// </summary>
        /// <param name="parameters">
        /// The list of parameters that characterizes this Solution.
        /// </param>
        /// <param name="solutionExecution">
        /// The fuction that is used to test this instance of Solution.
        /// </param>
        /// <param name="updateParametersFunction">
        /// The function used to update this Solution's parameters.
        /// </param>
        public Solution(List<Double> parameters, Func<SolutionParameters, Double> solutionExecution, Func<List<Double>, List<Double>, List<Double>> updateParametersFunction)
        {
            this.Parameters = parameters;
            this.SolutionExecution = solutionExecution;
            this.UpdateParametersFunction = updateParametersFunction;
            this.UpdateFitness();
        }

        /// <summary>
        /// Compares this Solution's Fitness score to another's and returns if this is better.
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
        /// Update the values of the solution's parameters.
        /// </summary>
        /// <param name="speeds">
        /// List of individual speeds for each parameter, used as the first parameter of the UpdateParametersFunction call.
        /// </param>
        /// 
        public virtual void UpdateParameters(List<Double> speeds)
        {
            if (speeds.Count != Parameters.Count)
            {
                throw new InvalidOperationException("The number of elements in speeds must match the number of elements in Parameters.");
            }

            this.Parameters = this.UpdateParametersFunction(speeds, this.Parameters);
        }

        /// <summary>
        /// Updates this solution's Fitness score for the execution of SolutionExecution with the current set of Parameters.
        /// </summary>
        public virtual void UpdateFitness()
        {
            SolutionParameters solutionParameters = this.convertParameters(this.Parameters);
            if (solutionParameters.SolutionType != this.GetType())
            {
                throw new InvalidOperationException("The convertParameters function returned a type of ISolutionParameters different from this Solution Object's type.");
            }
            this.Fitness = this.SolutionExecution(solutionParameters);
        }

        /// <summary>
        /// Returns a deap copy of this Solution.
        /// </summary>
        /// <returns>
        /// A copy of this Solution
        /// </returns>
        public abstract Solution Copy();

        /// <summary>
        /// Takes the property Parameters and converts it into an object usable by SolutionExecution to generate the Fitness score.
        /// </summary>
        /// <param name="parameters">
        /// This objects Parameters property
        /// </param>
        /// <returns>
        /// A converted set of Parameters usable by SolutionExecution.
        /// </returns>
        public abstract SolutionParameters convertParameters(List<Double> parameters);
        #endregion
    }
}
