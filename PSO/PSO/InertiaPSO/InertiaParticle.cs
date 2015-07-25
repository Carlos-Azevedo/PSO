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
using PSO.ClassicPSO;
using PSO.Interfaces;
using PSO.Abstracts;
using PSO.Parameters;

namespace PSO.InertiaPSO
{
    public class InertiaParticle : ClassicParticle
    {
        #region Properties
        public Double InertiaMax;

        public Double InertiaMin;

        public UInt32 InertiaMaxTime;

        private UInt32 CurrentIteration; 
        #endregion

        public InertiaParticle(InertiaParticleCreationParameters parameters)
        {
            this.Id = Particle.CurrentId;
            this.CurrentIteration = 0;
        }

        public virtual void FillParameters(InertiaParticleCreationParameters parameters)
        {
            base.FillParameters(parameters);
            this.InertiaMax = parameters.InertiaMax;
            this.InertiaMin = parameters.InertiaMin;
            this.InertiaMaxTime = parameters.InertiaMaxTime;
        }

        public override void UpdateSpeeds(Parameters.SpeedParameters parameters)
        {
            Double currentInertia = InertiaMax;
            if (CurrentIteration < InertiaMaxTime)
	        {
		          currentInertia = ((InertiaMaxTime -  CurrentIteration)/InertiaMaxTime) * (InertiaMax - InertiaMin) + InertiaMin;
            }
            for (int i = 0; i < this.Speeds.Count; i++)
            {
                this.Speeds[i] = this.Speeds[i] * currentInertia;
            }
            base.UpdateSpeeds(parameters);
        }

        public override void SetSpeedParameters(Parameters.SpeedParameters parameters)
        {
            base.SetSpeedParameters(parameters);
            this.CurrentIteration++;
        }
    }
}
