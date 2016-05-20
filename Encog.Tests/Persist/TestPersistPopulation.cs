//
// Encog(tm) Core v3.3 - .Net Version (unit test)
// http://www.heatonresearch.com/encog/
//
// Copyright 2008-2014 Heaton Research, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//   
// For more information on Heaton Research copyrights, licenses 
// and trademarks visit:
// http://www.heatonresearch.com/copyright
//
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Encog.Util;
using System.IO;
using Encog.ML.Data;
using Encog.Neural.Networks.Training;
using Encog.ML.Data.Basic;
using Encog.Engine.Network.Activation;
using Encog.Neural.NEAT;
using Encog.ML.EA.Train;
using Encog.ML.EA.Population;

namespace Encog.Persist
{
    [TestFixture]
    public class TestPersistPopulation
    {
        public static readonly TempDir TEMP_DIR = new TempDir();
        public readonly FileInfo EG_FILENAME = TEMP_DIR.CreateFile("encogtest.eg");
        public readonly FileInfo SERIAL_FILENAME = TEMP_DIR.CreateFile("encogtest.ser");

        private NEATPopulation Generate()
        {
            IMLDataSet trainingSet = new BasicMLDataSet(XOR.XORInput, XOR.XORIdeal);

            ICalculateScore score = new TrainingSetScore(trainingSet);
            // train the neural network
            ActivationStep step = new ActivationStep();
            step.Center = 0.5;

            IEvolutionaryAlgorithm train = NEATUtil.ConstructNEATTrainer(
                    score, 2, 1, 10);

            return (NEATPopulation)train.Population;
        }

        [Test]
        public void TestPersistEG()
        {
            IPopulation pop = Generate();

            EncogDirectoryPersistence.SaveObject((EG_FILENAME), pop);
            NEATPopulation pop2 = (NEATPopulation)EncogDirectoryPersistence.LoadObject((EG_FILENAME));

            Validate(pop2);
        }

        [Test]
        public void TestPersistSerial()
        {
            NEATPopulation pop = Generate();
            Validate(pop);

            SerializeObject.Save(SERIAL_FILENAME.ToString(), pop);
            NEATPopulation pop2 = (NEATPopulation)SerializeObject.Load(SERIAL_FILENAME.ToString());

            Validate(pop2);
        }

        private void Validate(NEATPopulation pop)
        {
            Assert.AreEqual(10, pop.PopulationSize);
            Assert.AreEqual(0.2, pop.SurvivalRate);

            // see if the population can actually be used to train
            IMLDataSet trainingSet = new BasicMLDataSet(XOR.XORInput, XOR.XORIdeal);
            ICalculateScore score = new TrainingSetScore(trainingSet);
            IEvolutionaryAlgorithm train = NEATUtil.ConstructNEATTrainer(pop, score);
            train.Iteration();

        }


    }
}
