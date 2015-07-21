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
using Moq;

namespace PSOTests
{
    [TestClass]
    public class ClassicSolutionTests
    {
        public ClassicSolution Solution;
        public Double[] randomItemsArray;

        [TestInitialize]
        //Sets up the test environment by building a Solution object with 4 random parameter values, SolTestFunc as the function,
        //an initial Fitness of 0, 0.0 as the minimum parameter value and 100.0 as the maximum parameter value.
        public void PrepareSolutionTests()
        {
            Random random = new Random();
            this.randomItemsArray = new Double[4];
            for (int i = 0; i < 4; i++)
            {
                this.randomItemsArray[i] = random.NextDouble();
            }
            List<Double> parameters = new List<double>(this.randomItemsArray);
            this.Solution = new ClassicSolution(parameters, this.SolTestFunc, new Object(), 0.0, 100.0);
            this.Solution.Fitness = 0.0;
        }

        public Double SolTestFunc(List<Double> parameters, Object aux)
        {
            Double result = 0;
            for (int i = 0; i < parameters.Count; i++)
            {
                result += parameters[i] * Math.Pow(10, parameters.Count - 1 - i);
            }
            return result;
        }
    
        [TestMethod]
        public void UpdateFitnessTest()
        {
            this.Solution.UpdateFitness();
            Assert.AreEqual(this.SolTestFunc(new List<Double>(this.randomItemsArray), new Object()), this.Solution.Fitness);
        }

        [TestMethod]
        public void UpdateParametersTest()
        {
            //Tests if the update value is correctly added and if parameter limits are respected
            this.Solution.Parameters = new List<double>(new Double[4] {0.0, 2.0, 0.0, 100.0});
            List<Double> speeds = new List<double>(new Double[4] { 1.0, -1.0, -1.0, 1.0 });
            this.Solution.UpdateParameters(speeds);
            CollectionAssert.AreEqual(new List<Double>(new Double[4] { 1.0, 1.0, 0.0, 100.0 }), this.Solution.Parameters);
        }

        [TestMethod]
        [ExpectedException (typeof(InvalidOperationException))]
        public void UpdateParameterAndThrowInvalidOperationException()
        {
            List<Double> speeds = new List<double>(new Double[3] { 1.0, 1.0, 1.0});
            this.Solution.UpdateParameters(speeds);
        }

        [TestMethod]
        public void CopyTest()
        {
            this.Solution.UpdateFitness();
            ISolution copySolution = this.Solution.Copy();
            //Test if properties were copied correctly.
            CollectionAssert.Equals(copySolution.Parameters, this.Solution.Parameters);
            Assert.AreEqual(copySolution.AuxData, this.Solution.AuxData);
            Assert.AreEqual(copySolution.Fitness, Solution.Fitness);

            //Test if UpdateFitness produces the same result.
            copySolution.Fitness = 0.0;
            copySolution.UpdateFitness();
            Assert.AreEqual(copySolution.Fitness, Solution.Fitness);

            //Test if Parameters are indeed independent.
            this.Solution.UpdateParameters(new List<double>(new Double[4] { 1.0, 0.0, 0.0, 0.0 }));
            CollectionAssert.AreNotEqual(copySolution.Parameters, this.Solution.Parameters);

            //Test if UpdateFitness modifies only the solution called.
            Double copySolutionCurrentFitness = copySolution.Fitness;
            this.Solution.UpdateFitness();
            Assert.AreNotEqual(this.Solution.Fitness, copySolution.Fitness);
            Assert.AreEqual(copySolution.Fitness, copySolutionCurrentFitness);
        }

        [TestMethod]
        public void BetterThanTest()
        {
            ISolution fakeSolution = Mock.Of<ISolution>(s => s.Fitness == 1.0);
            this.Solution.Fitness = 2.0;
            Assert.IsTrue(this.Solution.BetterThan(fakeSolution));
        }
    }
}
