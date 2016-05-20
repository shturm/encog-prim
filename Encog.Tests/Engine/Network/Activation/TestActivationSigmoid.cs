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
using NUnit.Framework;

namespace Encog.Engine.Network.Activation
{
    [TestFixture]
    public class TestActivationSigmoid
    {
        [Test]
        public void TestSigmoid()
        {
            var activation = new ActivationSigmoid();
            Assert.IsTrue(activation.HasDerivative);

            var clone = (ActivationSigmoid) activation.Clone();
            Assert.IsNotNull(clone);

            double[] input = {0.0};

            activation.ActivationFunction(input, 0, 1);

            Assert.AreEqual(0.5, input[0], 0.1);

            // test derivative, should throw an error
            input[0] = activation.DerivativeFunction(input[0],input[0]);
            Assert.AreEqual(0.25, input[0], 0.1);
        }
    }
}