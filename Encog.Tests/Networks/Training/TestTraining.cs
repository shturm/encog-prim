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
using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.Util;
using Encog.ML.Train;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using Encog.Neural.Networks.Training.Lma;
using Encog.Neural.Networks.Training.Propagation.Back;
using Encog.Neural.Networks.Training.Propagation.Manhattan;
using Encog.Neural.Networks.Training.Propagation.SCG;
using Encog.Neural.Networks.Training.Anneal;
using Encog.MathUtil.Randomize;
using Encog.Neural.PNN;
using Encog.Neural.Networks.Training.PNN;
using Encog.ML.Genetic;
using Encog.ML;

namespace Encog.Neural.Networks.Training
{
    [TestFixture]
    public class TestTraining
    {
        [Test]
        public void TestRPROP()
        {
            IMLDataSet trainingData = new BasicMLDataSet(XOR.XORInput, XOR.XORIdeal);

            BasicNetwork network = NetworkUtil.CreateXORNetworkUntrained();
            IMLTrain rprop = new ResilientPropagation(network, trainingData);
            NetworkUtil.TestTraining(rprop, 0.03);
        }

        [Test]
        public void TestLMA()
        {
            IMLDataSet trainingData = new BasicMLDataSet(XOR.XORInput, XOR.XORIdeal);

            BasicNetwork network = NetworkUtil.CreateXORNetworkUntrained();
            IMLTrain rprop = new LevenbergMarquardtTraining(network, trainingData);
            NetworkUtil.TestTraining(rprop, 0.03);
        }

        [Test]
        public void TestBPROP()
        {
            IMLDataSet trainingData = new BasicMLDataSet(XOR.XORInput, XOR.XORIdeal);

            BasicNetwork network = NetworkUtil.CreateXORNetworkUntrained();

            IMLTrain bprop = new Backpropagation(network, trainingData, 0.7, 0.9);
            NetworkUtil.TestTraining(bprop, 0.01);
        }

        [Test]
        public void TestManhattan()
        {
            IMLDataSet trainingData = new BasicMLDataSet(XOR.XORInput, XOR.XORIdeal);

            BasicNetwork network = NetworkUtil.CreateXORNetworkUntrained();
            IMLTrain bprop = new ManhattanPropagation(network, trainingData, 0.01);
            NetworkUtil.TestTraining(bprop, 0.01);
        }

        [Test]
        public void TestSCG()
        {
            IMLDataSet trainingData = new BasicMLDataSet(XOR.XORInput, XOR.XORIdeal);

            BasicNetwork network = NetworkUtil.CreateXORNetworkUntrained();
            IMLTrain bprop = new ScaledConjugateGradient(network, trainingData);
            NetworkUtil.TestTraining(bprop, 0.04);
        }

        [Test]
        public void TestAnneal()
        {
            IMLDataSet trainingData = new BasicMLDataSet(XOR.XORInput, XOR.XORIdeal);
            BasicNetwork network = NetworkUtil.CreateXORNetworkUntrained();
            ICalculateScore score = new TrainingSetScore(trainingData);
            NeuralSimulatedAnnealing anneal = new NeuralSimulatedAnnealing(network, score, 10, 2, 100);
            NetworkUtil.TestTraining(anneal, 0.01);
        }

        [Test]
        public void TestGenetic()
        {
            IMLDataSet trainingData = new BasicMLDataSet(XOR.XORInput, XOR.XORIdeal);
            ICalculateScore score = new TrainingSetScore(trainingData);


            MLMethodGeneticAlgorithm genetic = new MLMethodGeneticAlgorithm(() =>
            {
                return NetworkUtil.CreateXORNetworkUntrained();
            }, score, 500);

            NetworkUtil.TestTraining(genetic, 0.00001);
        }

        [Test]
        public void TestRegPNN()
        {

            PNNOutputMode mode = PNNOutputMode.Regression;
            BasicPNN network = new BasicPNN(PNNKernelType.Gaussian, mode, 2, 1);

            IMLDataSet trainingData = new BasicMLDataSet(XOR.XORInput, XOR.XORIdeal);

            TrainBasicPNN train = new TrainBasicPNN(network, trainingData);
            train.Iteration();

            XOR.VerifyXOR(network, 0.01);
        }

        [Test]
        public void TestClassifyPNN()
        {

            PNNOutputMode mode = PNNOutputMode.Classification;
            BasicPNN network = new BasicPNN(PNNKernelType.Gaussian, mode, 2, 2);

            IMLDataSet trainingData = new BasicMLDataSet(XOR.XORInput, XOR.XORIdeal);

            TrainBasicPNN train = new TrainBasicPNN(network, trainingData);
            train.Iteration();

            XOR.VerifyXOR(network, 0.01);
        }
    }
}
