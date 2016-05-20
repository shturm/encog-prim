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
using Encog.Util.Normalize.Input;
using Encog.Util.Normalize.Output.ZAxis;
using Encog.Util.Normalize.Target;
using NUnit.Framework;

namespace Encog.Util.Normalize
{
    [TestFixture]
    public class TestZAxis
    {
        private readonly double[][] SAMPLE1 = {new[] {-10.0, 5.0, 15.0}, new[] {-2.0, 1.0, 3.0}};

        public DataNormalization Create()
        {
            IInputField a;
            IInputField b;
            IInputField c;
            double[][] arrayOutput = EngineArray.AllocateDouble2D(2, 4);

            var target = new NormalizationStorageArray2D(arrayOutput);
            var group = new ZAxisGroup();
            var norm = new DataNormalization();
            norm.Report = new NullStatusReportable();
            norm.Storage = target;
            norm.AddInputField(a = new InputFieldArray2D(false, SAMPLE1, 0));
            norm.AddInputField(b = new InputFieldArray2D(false, SAMPLE1, 1));
            norm.AddInputField(c = new InputFieldArray2D(false, SAMPLE1, 2));
            norm.AddOutputField(new OutputFieldZAxis(group, a));
            norm.AddOutputField(new OutputFieldZAxis(group, b));
            norm.AddOutputField(new OutputFieldZAxis(group, c));
            norm.AddOutputField(new OutputFieldZAxisSynthetic(group));
            return norm;
        }

        private void Check(DataNormalization norm)
        {
            double[][] arrayOutput = ((NormalizationStorageArray2D) norm.Storage).GetArray();

            Assert.AreEqual(-5.0, arrayOutput[0][0]);
            Assert.AreEqual(2.5, arrayOutput[0][1]);
            Assert.AreEqual(7.5, arrayOutput[0][2]);
            Assert.AreEqual(0.0, arrayOutput[0][3]);
            Assert.AreEqual(-1.0, arrayOutput[1][0]);
            Assert.AreEqual(0.5, arrayOutput[1][1]);
            Assert.AreEqual(1.5, arrayOutput[1][2]);
            Assert.AreEqual(0.0, arrayOutput[1][3]);
        }

        [Test]
        public void TestAbsolute()
        {
            DataNormalization norm = Create();
            norm.Process();
            Check(norm);
        }

        [Test]
        public void TestAbsoluteSerial()
        {
            DataNormalization norm = Create();
            norm = (DataNormalization) SerializeRoundTrip.RoundTrip(norm);
            norm.Process();
            Check(norm);
        }
    }
}
