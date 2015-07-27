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
using PSO.FrankensteinPSO;
using PSO.Interfaces;
using PSO.Parameters;
using Moq;

namespace PSOTests
{
    [TestClass]
    public class FrankensteinParticleTests
    {
        public FrankensteinParticle Particle;
        public List<Double> Speeds;
        public Mock<ISolution> MockedSolution;
        public Mock<ISolution> CopySolution;

        [TestInitialize]
        public void PrepareFrankensteinParticlesTests()
        {
            var mockedSolution = new Mock<ISolution>();
            mockedSolution.Name = "original";
            var copySolution = new Mock<ISolution>();
            copySolution.Name = "copy";
            mockedSolution.Setup(s => s.Fitness).Returns(1.0);
            copySolution.Setup(s => s.Fitness).Returns(2.0);
            mockedSolution.Setup(s => s.Copy()).Returns(copySolution.Object);
            mockedSolution.Setup(s => s.Parameters).Returns(new List<Double>(new Double[3] { 1.0, 1.0, 1.0 }));
            copySolution.Setup(s => s.Parameters).Returns(new List<Double>(new Double[3] { 1.0, 1.0, 1.0 }));

            this.MockedSolution = mockedSolution;
            this.CopySolution = copySolution;
            this.Speeds = new List<Double>(new Double[3] { 1.0, 1.0, 1.0 });

            FrankensteinParticleCreationParameters creationParams = new FrankensteinParticleCreationParameters();
            creationParams.Speeds = this.Speeds;
            creationParams.Solution = mockedSolution.Object;
            creationParams.InertiaMax = 1.0;
            creationParams.InertiaMin = 1.0;
            creationParams.InertiaMaxTime = 0;
            creationParams.Particles = this.CreateMockedConnectedParticles();
            int[] ids = new int[7];
            for (int i = 0; i < 7; i++)
            {
                ids[i] = i;
            }
            Random gen = new Random();
            creationParams.RandomGenerator = gen;
            creationParams.FinalTopologyUpdate = 8;
            creationParams.ConnectedIds = new LinkedList<int>(ids);
            this.Particle = new FrankensteinParticle(creationParams);
            this.Particle.Id = 0;
            creationParams.Particles.Insert(0, this.Particle);
        }

        public List<IParticle> CreateMockedConnectedParticles()
        {
            List<IParticle> fakeParticles = new List<IParticle>();
            for (int i = 1; i < 7; i++)
            {
                var fake = new Mock<IConnectedParticles>();
                List<Double> parameters = new List<Double>(new Double[3] { 2.0, 2.0, 2.0 });
                var fakeSolution = new Mock<ISolution>();
                fakeSolution.Setup(s => s.Parameters).Returns(parameters);
                fake.Setup(p => p.Id).Returns(i);
                fake.Setup(p => p.PersonalBestSolution).Returns(fakeSolution.Object);
                LinkedList<int> fakeLinks = new LinkedList<int>();
                fakeLinks.AddFirst(0);
                fake.Setup(p => p.ConnectedIds).Returns(fakeLinks);
                fakeParticles.Add(fake.Object);
            }
            return fakeParticles;
        }

        [TestMethod]
        public void UpdateConnectedParticlesTest()
        {
            for (int i = 0; i < 8; i++)
            {
                this.Particle.UpdateConnectedParticles(2);
                if (i % 2 == 1)
                {                    
                    CollectionAssert.DoesNotContain(this.Particle.ConnectedIds, (i / 2) + 2);
                    CollectionAssert.DoesNotContain(((IConnectedParticles)this.Particle.Particles[(i / 2) + 2]).ConnectedIds, 0);
                }
                this.Particle.CurrentIteration++;
            }
        }

        [TestMethod]
        public void CreateConnectedParticlesParametersTest()
        {
            this.Particle.CreateConnectedParticlesParameters();
            int i = 0;
            foreach(IParticle particle in this.Particle.Particles)
            {
                CollectionAssert.AreEqual(this.Particle.ConnectedParticlesParameters[i], this.Particle.Particles[i].PersonalBestSolution.Parameters);
                i++;
            }
        }

        [TestMethod]
        public void UpdateSpeedsFrankensteinTest()
        {
            SpeedParameters testSpeedParameters= new SpeedParameters();
            testSpeedParameters.RandomListPersonal = new List<double>(new Double[3] { 1.0, 1.0, 1.0 });
            testSpeedParameters.PersonalBestBias = 1.0;
            this.Particle.Speeds = new List<double>(new Double[3] { 0.0, 0.0, 0.0 });
            this.Particle.SetSpeedParameters(testSpeedParameters);
            this.Particle.FinalTopologyUpdate = 4;
            for (int i = 0; i < 4; i++)
            {                
                this.Particle.UpdateSpeeds(this.Particle.SpeedParameters);
                foreach(double speed in this.Particle.Speeds)
                {
                    Assert.AreEqual(Math.Round(1.0 - (1.0 / (7.0 - i)), 12), Math.Round(speed,12));
                }
                this.Particle.UpdateConnectedParticles(2);
                this.Particle.CreateConnectedParticlesParameters();
                this.Particle.Speeds = new List<double>(new Double[3] { 0.0, 0.0, 0.0 });
            }
        }
    }
}
