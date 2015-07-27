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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PSO.StablePSO;

namespace PSOTests
{
    [TestClass]
    public class StableSwarmTests
    {
        [TestMethod]
        public void CalculateConstraintTest()
        {
            Double globalBias = 1.0;
            Double personalBias = 1.0;
            Double constraint = 0.5;
            Double result = StableSwarm.CalculateConstraint(globalBias,personalBias,constraint);
            Assert.AreEqual(Math.Sqrt(0.5),result);
            globalBias = 3.0;
            personalBias = 2.0;
            result = StableSwarm.CalculateConstraint(globalBias, personalBias, constraint);
            Assert.AreEqual(Math.Sqrt(1 /(3+Math.Sqrt(5))), result);
        }
    }
}
