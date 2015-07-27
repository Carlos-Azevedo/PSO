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
using PSO.Parameters;
using PSO.Interfaces;

namespace PSO.Abstracts
{
    /// <summary>
    /// A particle describes an object that contains a current Solution, a best Solution, current n-dimensional speed and 
    /// </summary>
    public abstract class Particle : IParticle
    {
        #region Static Properties
        private static int _CurrentId;

        /// <summary>
        /// A static Id value incremented when a new particle is created so each one has an unique value.
        /// </summary>
        public static int CurrentId
        {
            get { return _CurrentId++; }
        }
        #endregion

        #region Properties
        /// <summary>
        /// An object used to update the speed values.
        /// </summary>
        public SpeedParameters SpeedParameters { get { return _SpeedParameters; } }

        protected SpeedParameters _SpeedParameters;

        /// <summary>
        /// The List of speeds used to search for the optimal Solution in the problem's n-dimensional Solution Space.
        /// </summary>
        public List<Double> Speeds { get; set; }

        /// <summary>
        /// The personal best Solution configuration this Particle has found. Used to guide the direction in which the optimal Solution is searched.
        /// </summary>
        public ISolution PersonalBestSolution { get; set; }

        /// <summary>
        /// The current Solution configuration for this particle.
        /// </summary>
        public ISolution CurrentSolution { get; set; }

        /// <summary>
        /// An Id value used to uniquely identify this Particle.
        /// </summary>
        public int Id { get; set; }
        #endregion

        #region Methods

        public abstract void Iterate();

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

        public abstract void SetSpeedParameters(SpeedParameters parameters);
        #endregion
    }
}
