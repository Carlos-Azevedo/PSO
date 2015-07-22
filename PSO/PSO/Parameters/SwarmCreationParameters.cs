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
    public class SwarmCreationParameters
    {
        public UInt32 MaxIterations { get; set; }

        public Double FitnessThreshold { get; set; }

        public Double MinimumParameterValue { get; set; }

        public Double MaximumParameterValue { get; set; }

        public Random RandomNumberGenerator { get; set; }

        public UInt32 NumberOfParticles { get; set; }

        public UInt32 NumberOfParameters { get; set; }

        public Func<List<Double>, Object, Double> SolutionFunction {get; set;}

        public Object AuxData { get; set; }

        public Double GlobalBestBias { get; set; }

        public Double PersonalBestBias { get; set; }

        public int NumberOfParticleSets { get; set; }

        public void VerifyValues()
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

    public class StablwSwarmCreationParameters : SwarmCreationParameters
    {
        public Double ConstraintValue;
    }
}
