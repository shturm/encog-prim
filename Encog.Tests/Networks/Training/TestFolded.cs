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
using Encog.ML;
using Encog.ML.Data;
using Encog.ML.Data.Folded;
using Encog.ML.Train;
using Encog.Neural.Networks.Training.Cross;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using Encog.Util;
using Encog.Util.Simple;
using NUnit.Framework;

namespace Encog.Neural.Networks.Training
{
    [TestFixture]
    public class TestFolded
    {
        [Test]
        public void TestMethod1()
        {
        }


        public void TestRPROPFolded()
        {
            IMLDataSet trainingData = XOR.CreateNoisyXORDataSet(10);

            BasicNetwork network = NetworkUtil.CreateXORNetworkUntrained();

            var folded = new FoldedDataSet(trainingData);
            IMLTrain train = new ResilientPropagation(network, folded);
            var trainFolded = new CrossValidationKFold(train, 4);

            EncogUtility.TrainToError(trainFolded, 0.2);

            XOR.VerifyXOR((IMLRegression) trainFolded.Method, 0.2);
        }
    }
}
