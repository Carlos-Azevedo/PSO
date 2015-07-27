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
    public class ClassicParticleCreationParameters
    {
        public List<Double> Speeds;

        public ISolution Solution;
    }

    public class StableParticleCreationParameters : ClassicParticleCreationParameters
    {
        public Double Constraint;
    }

    public class InertiaParticleCreationParameters : ClassicParticleCreationParameters
    {
        public Double InertiaMax;

        public Double InertiaMin;

        public UInt32 InertiaMaxTime;
    }

    public class FrankensteinParticleCreationParameters : InertiaParticleCreationParameters
    {
        public UInt32 FinalTopologyUpdate;

        public List<IParticle> Particles;

        public LinkedList<int> ConnectedIds;

        public Random RandomGenerator;
    }
}
