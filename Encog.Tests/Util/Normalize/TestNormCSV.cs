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
using System.IO;
using Encog.Util.Normalize.Input;
using Encog.Util.Normalize.Output;
using Encog.Util.Normalize.Target;
using NUnit.Framework;

namespace Encog.Util.Normalize
{
    [TestFixture]
    public class TestNormCSV
    {
        public static readonly double[][] ARRAY_2D = {
                                                         new[] {1.0, 2.0, 3.0, 4.0, 5.0},
                                                         new[] {6.0, 7.0, 8.0, 9.0, 10.0}
                                                     };

        public static TempDir TEMP_DIR = new TempDir();
        public static FileInfo FILENAME1 = TEMP_DIR.CreateFile("norm1.csv");
        public static FileInfo FILENAME2 = TEMP_DIR.CreateFile("norm2.csv");

        private void Generate(string filename)
        {
            IInputField a;
            IInputField b;
            IInputField c;
            IInputField d;
            IInputField e;

            var norm = new DataNormalization();
            norm.Report = new NullStatusReportable();
            norm.AddInputField(a = new InputFieldArray2D(false, ARRAY_2D, 0));
            norm.AddInputField(b = new InputFieldArray2D(false, ARRAY_2D, 1));
            norm.AddInputField(c = new InputFieldArray2D(false, ARRAY_2D, 2));
            norm.AddInputField(d = new InputFieldArray2D(false, ARRAY_2D, 3));
            norm.AddInputField(e = new InputFieldArray2D(false, ARRAY_2D, 4));
            norm.AddOutputField(new OutputFieldDirect(a));
            norm.AddOutputField(new OutputFieldDirect(b));
            norm.AddOutputField(new OutputFieldDirect(c));
            norm.AddOutputField(new OutputFieldDirect(d));
            norm.AddOutputField(new OutputFieldDirect(e));
            norm.Storage = new NormalizationStorageCSV(filename.ToString());
            norm.Process();
        }

        public DataNormalization Create(string filename, double[][] outputArray)
        {
            IInputField a;
            IInputField b;
            IInputField c;
            IInputField d;
            IInputField e;

            var norm = new DataNormalization();
            norm.Report = new NullStatusReportable();
            norm.Storage = new NormalizationStorageCSV(filename.ToString());
            norm.AddInputField(a = new InputFieldCSV(false, filename.ToString(), 0));
            norm.AddInputField(b = new InputFieldCSV(false, filename.ToString(), 1));
            norm.AddInputField(c = new InputFieldCSV(false, filename.ToString(), 2));
            norm.AddInputField(d = new InputFieldCSV(false, filename.ToString(), 3));
            norm.AddInputField(e = new InputFieldCSV(false, filename.ToString(), 4));
            norm.AddOutputField(new OutputFieldRangeMapped(a, 0.1, 0.9));
            norm.AddOutputField(new OutputFieldRangeMapped(b, 0.1, 0.9));
            norm.AddOutputField(new OutputFieldRangeMapped(c, 0.1, 0.9));
            norm.AddOutputField(new OutputFieldRangeMapped(d, 0.1, 0.9));
            norm.AddOutputField(new OutputFieldRangeMapped(e, 0.1, 0.9));
            norm.Storage = new NormalizationStorageArray2D(outputArray);
            return norm;
        }

        [Test]
        public void TestGenerateAndLoad()
        {
            var outputArray = EngineArray.AllocateDouble2D(2, 5);
            Generate(FILENAME1.ToString());
            DataNormalization norm = Create(FILENAME1.ToString(),outputArray);
            norm.Process();
            Check(norm);
        }

        [Test]
        public void TestGenerateAndLoadSerial()
        {
            double[][] outputArray = EngineArray.AllocateDouble2D(2, 5);
            Generate(FILENAME2.ToString());
            DataNormalization norm = Create(FILENAME2.ToString(),outputArray);
            norm = (DataNormalization) SerializeRoundTrip.RoundTrip(norm);
            norm.Process();
            Check(norm);
        }

        private void Check(DataNormalization norm)
        {
            IInputField a = norm.InputFields[0];
            IInputField b = norm.InputFields[1];

            Assert.AreEqual(1.0, a.Min, 0.1);
            Assert.AreEqual(6.0, a.Max, 0.1);
            Assert.AreEqual(2.0, b.Min, 0.1);
            Assert.AreEqual(7.0, b.Max, 0.1);

            double[][] outputArray = ((NormalizationStorageArray2D) norm.Storage).GetArray();
            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(0.1, outputArray[0][i], 0.1);
                Assert.AreEqual(0.9, outputArray[1][i], 0.1);
            }
        }
    }
}
