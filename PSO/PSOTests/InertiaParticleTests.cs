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
using System.Collections.Generic;
using PSO.InertiaPSO;
using PSO.Interfaces;
using PSO.Parameters;
using Moq;

namespace PSOTests
{
    [TestClass]
    public class InertiaParticleTests
    {
        public InertiaParticle Particle;
        public List<Double> Speeds;
        public Mock<ISolution> MockedSolution;
        public Mock<ISolution> CopySolution;

        [TestInitialize]
        public void PrepareInertiaParticlesTests()
        {
            var mockedSolution = new Mock<ISolution>();
            mockedSolution.Name = "original";
            var copySolution = new Mock<ISolution>();
            copySolution.Name = "copy";
            mockedSolution.Setup(s => s.Fitness).Returns(1.0);
            copySolution.Setup(s => s.Fitness).Returns(2.0);
            mockedSolution.Setup(s => s.Copy()).Returns(copySolution.Object);
            mockedSolution.Setup(s => s.Parameters).Returns(new List<Double>(new Double[3] { 2.0, 3.0, 4.0 }));

            this.MockedSolution = mockedSolution;
            this.CopySolution = copySolution;
            this.Speeds = new List<Double>(new Double[3] { 1.0, 1.0, 1.0 });

            InertiaParticleCreationParameters creationParams = new InertiaParticleCreationParameters();
            creationParams.Speeds = this.Speeds;
            creationParams.Solution = mockedSolution.Object;
            creationParams.InertiaMax = 1.0;
            creationParams.InertiaMin = 0.0;
            creationParams.InertiaMaxTime = 10;

            this.Particle = new InertiaParticle(creationParams);
        }

        [TestMethod]
        public void CalculateInertiaTest()
        {
            for (int iteration = 0; iteration < 10; iteration++)
            {
                this.Particle.CurrentIteration = (UInt32)iteration;
                Double inertia = this.Particle.CalculateInertia();
                Double expected = (10.0 - (Double)iteration) / 10.0;
                Assert.AreEqual(inertia, expected);
            }
        }
    }
}
