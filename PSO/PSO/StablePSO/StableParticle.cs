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

namespace PSO.StablePSO
{
    public class StableParticle : ClassicParticle
    {
        public Double Constraint { get; set; }

        public StableParticle(StableParticleCreationParameters parameters)
        {
            this.Id = Particle.CurrentId;
            this.FillParameters(parameters);
        }

        protected virtual void FillParameters(StableParticleCreationParameters parameters)
        {
            base._FillParameters(parameters);
            this.Constraint = parameters.Constraint;
        }

        public override void UpdateSpeeds(Parameters.SpeedParameters parameters)
        {
            base.UpdateSpeeds(parameters);
            for (int i = 0; i < this.Speeds.Count; i++)
            {
                this.Speeds[i] = this.Speeds[i] * this.Constraint;
            }
        }
    }
}
