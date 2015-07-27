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
using Moq;
using PSO.ClassicPSO;
using PSO.Interfaces;
using PSO.Parameters;

namespace PSOTests
{
    [TestClass]
    public class ClassicParticleTests
    {
        public ClassicParticle Particle;
        public List<Double> Speeds;
        public Mock<ISolution> MockedSolution;
        public Mock<ISolution> CopySolution;

        [TestInitialize]
        public void PrepareClassicParticleTests()
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

            ClassicParticleCreationParameters creationParams = new ClassicParticleCreationParameters();
            creationParams.Speeds = this.Speeds;
            creationParams.Solution = mockedSolution.Object;
            this.Particle = new ClassicParticle(creationParams);

            SpeedParameters speedParams = new SpeedParameters();
            speedParams.GlobalBestBias = 2.0;
            speedParams.PersonalBestBias = 4.0;
            speedParams.PersonalBestSolution = new Double[3] { 3.0, 4.0, 5.0 };
            speedParams.GlobalBestSolution = new Double[3] { 3.0, 4.0, 5.0 };
            speedParams.RandomListGlobal = new List<Double>(new Double[3] { 1.0, 1.0, 1.0 });
            speedParams.RandomListPersonal = new List<Double>(new Double[3] { 1.0, 1.0, 1.0 });
            this.Particle.SetSpeedParameters(speedParams);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            Assert.AreEqual(this.Particle.CurrentSolution.Fitness, 1.0);
            Assert.AreEqual(this.Particle.PersonalBestSolution.Fitness, 2.0);
            Assert.AreEqual(this.Particle.Id, 0);
        }

        [TestMethod]
        public void UpdateSolutionTest()
        {
            this.MockedSolution.Setup(s => s.BetterThan(this.CopySolution.Object)).Returns(true);
            this.MockedSolution.Setup(s => s.Copy()).Returns(this.MockedSolution.Object);
            this.Particle.CurrentSolution = this.MockedSolution.Object;
            this.Particle.UpdateSolution();
            this.MockedSolution.Verify(s => s.UpdateParameters(this.Speeds), Times.Once,"UpdateParameters needs to be called once before the Solution is updated.");
            this.MockedSolution.Verify(s => s.UpdateFitness(), Times.Once, "UpdateFitness must be called once to update the solution.");
            this.MockedSolution.Verify(s => s.BetterThan(this.CopySolution.Object), Times.Once, "The resulting Solution must be compared to the PersonalBestSolution during an UpdateSolution call.");
            Assert.AreEqual(this.Particle.PersonalBestSolution, this.Particle.CurrentSolution);
        }

        [TestMethod]
        public void UpdateSpeedsTest()
        {
            this.Particle.UpdateSpeeds(this.Particle.SpeedParameters);
            CollectionAssert.AreEqual(this.Particle.Speeds, new List<Double>(new Double[3] {7.0, 7.0, 7.0}));
        }

        [TestMethod]
        public void IterateTest()
        {
            this.MockedSolution.Setup(s => s.BetterThan(this.CopySolution.Object)).Returns(true);
            this.MockedSolution.Setup(s => s.Copy()).Returns(this.MockedSolution.Object);
            this.Particle.CurrentSolution = this.MockedSolution.Object;
            SpeedParameters speedParams = new SpeedParameters();
        }
    }
}
