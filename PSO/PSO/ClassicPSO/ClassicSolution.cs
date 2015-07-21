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

namespace PSO.ClassicPSO
{
    public class ClassicSolution : Solution
    {
        #region Properties
        public Double MaximumParameterThreshold;

        public Double MinimumParameterThreshold;
        #endregion

        #region Methods
        public ClassicSolution(Func<List<Double>, Object, Double> runSolution, Object auxData, Double minParameter, Double maxParameter)
        {
            this._AuxData = auxData;
            this.RunSolution = runSolution;
            this.MaximumParameterThreshold = maxParameter;
            this.MinimumParameterThreshold = minParameter;
        }

        public ClassicSolution(List<Double> parameters, Func<List<Double>, Object, Double> runSolution, Object auxData, Double minParameter, Double maxParameter)
        {
            this.Parameters = parameters;
            this._AuxData = auxData;
            this.RunSolution = runSolution;
            this.MaximumParameterThreshold = maxParameter;
            this.MinimumParameterThreshold = minParameter;
            this.UpdateFitness();
        }

        public override void UpdateParameters(List<double> speeds)
        {
            if (speeds.Count != Parameters.Count)
            {
                throw new InvalidOperationException("The number of elements in speeds must match the number of elements in Parameters.");
            }
            for (int index = 0; index < this.Parameters.Count; index++)
            {
                this.Parameters[index] = this.Parameters[index] + speeds[index];
                if (this.Parameters[index] > this.MaximumParameterThreshold)
                {
                    this.Parameters[index] = this.MaximumParameterThreshold;
                }
                else if (this.Parameters[index] < this.MinimumParameterThreshold)
                {
                    this.Parameters[index] = this.MinimumParameterThreshold;
                }
            }            
        }

        public override ISolution Copy()
        {
            Double[] copyParametersList = new Double[this.Parameters.Count];
            this.Parameters.CopyTo(copyParametersList);
            ISolution copy =  new ClassicSolution(this.RunSolution, this.AuxData, this.MinimumParameterThreshold, this.MaximumParameterThreshold);
            copy.Parameters = copyParametersList.ToList();
            copy.Fitness = this.Fitness;
            return copy;
        }
        #endregion
    }
}
