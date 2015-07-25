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
using PSO.Parameters;
using PSO.Interfaces;

namespace PSO.ClassicPSO
{
    public class ClassicParticle : Particle
    {
        #region Methods
        /// <summary>
        /// Creates a new ClassicParticle.
        /// </summary>
        public ClassicParticle(ClassicParticleCreationParameters parameters)
        {
            this.Id = Particle.CurrentId;
        }

        protected virtual void FillParameters(ClassicParticleCreationParameters parameters)
        {
            this.Speeds = parameters.Speeds;
            this.CurrentSolution = parameters.Solution;
            this.PersonalBestSolution = parameters.Solution.Copy();
        }

        protected ClassicParticle()
        { }

        public override void UpdateSpeeds(SpeedParameters parameters)
        {
            for (int index = 0; index < this.CurrentSolution.Parameters.Count; index++)
            {
                Double newSpeed = this.Speeds[index];
                newSpeed = newSpeed + (parameters.PersonalBestSolution[index] - this.CurrentSolution.Parameters[index]) * parameters.PersonalBestBias * parameters.RandomListPersonal[index];
                newSpeed = newSpeed + (parameters.GlobalBestSolution[index] - this.CurrentSolution.Parameters[index]) * parameters.GlobalBestBias * parameters.RandomListGlobal[index];
                this.Speeds[index] = newSpeed;
            }
        }

        public override void Iterate()
        {
            this.UpdateSpeeds(this.SpeedParameters);
            this.UpdateSolution();
        }

        public override void SetSpeedParameters(SpeedParameters parameters)
        {
            this._SpeedParameters = parameters;
        }
        #endregion
    }
}
