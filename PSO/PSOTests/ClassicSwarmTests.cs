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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PSO.ClassicPSO;
using PSO.Interfaces;
using PSO.Parameters;

namespace PSOTests
{
    [TestClass]
    public class ClassicSwarmTests
    {
        public ClassicSwarm swarm;

        public Double SolTestFunc(List<Double> parameters, Object aux)
        {
            Double result = 0;
            for (int i = 0; i < parameters.Count; i++)
            {
                result += parameters[i] * Math.Pow(10, parameters.Count - 1 - i);
            }
            return result;
        }

        [TestInitialize]
        public void TestMethod1()
        {
            SwarmCreationParameters parameters = new SwarmCreationParameters();
            parameters.GlobalBestBias = 4.0;
            parameters.PersonalBestBias = 2.0;
            parameters.RandomNumberGenerator = new Random();
            parameters.NumberOfParameters = 3;
            parameters.MaximumParameterValue = 10.0;
            parameters.MinimumParameterValue = -10.0;
            parameters.MaxIterations = 1;
            parameters.NumberOfParticles = 15;
            parameters.NumberOfParticleSets = 8;
            parameters.FitnessThreshold = 1000000;
            parameters.SolutionFunction = this.SolTestFunc;
            parameters.AuxData = new object();
            this.swarm = new ClassicSwarm(parameters);
        }

        [TestMethod]
        public void RunPSOTest()
        {
            ISolution bestSolution = this.swarm.RunPSO();
        }
    }
}
