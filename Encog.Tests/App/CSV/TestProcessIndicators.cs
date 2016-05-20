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
using Encog.App.Quant.Indicators;
using Encog.App.Quant.Indicators.Predictive;
using Encog.Util;
using Encog.Util.CSV;
using NUnit.Framework;

namespace Encog.App.CSV
{
    [TestFixture]
    public class TestProcessIndicators
    {
        public static readonly TempDir TempDir = new TempDir();
        public readonly FileInfo InputName = TempDir.CreateFile("test.csv");
        public readonly FileInfo OutputName = TempDir.CreateFile("test2.csv");

        public void GenerateTestFileHeadings(bool header)
        {
            var tw = new StreamWriter(InputName.ToString());

            if (header)
            {
                tw.WriteLine("date,close");
            }
            tw.WriteLine("20100101,1");
            tw.WriteLine("20100102,2");
            tw.WriteLine("20100103,3");
            tw.WriteLine("20100104,4");
            tw.WriteLine("20100105,5");
            tw.WriteLine("20100106,6");
            tw.WriteLine("20100107,7");
            tw.WriteLine("20100108,8");
            tw.WriteLine("20100109,9");
            tw.WriteLine("20100110,10");

            // close the stream
            tw.Close();
        }

        [Test]
        public void TestIndicatorsHeaders()
        {
            GenerateTestFileHeadings(true);
            var norm = new ProcessIndicators();
            norm.Analyze(InputName, true, CSVFormat.English);
            norm.AddColumn(new MovingAverage(3, true));
            norm.AddColumn(new BestClose(3, true));
            norm.Columns[0].Output = true;
            norm.Process(OutputName);

            var tr = new StreamReader(OutputName.ToString());

            Assert.AreEqual("\"date\",\"close\",\"MovAvg\",\"PredictBestClose\"", tr.ReadLine());
            Assert.AreEqual("20100103,3,2,6", tr.ReadLine());
            Assert.AreEqual("20100104,4,3,7", tr.ReadLine());
            Assert.AreEqual("20100105,5,4,8", tr.ReadLine());
            Assert.AreEqual("20100106,6,5,9", tr.ReadLine());
            Assert.AreEqual("20100107,7,6,10", tr.ReadLine());

            tr.Close();

            InputName.Delete();
            OutputName.Delete();
        }

        [Test]
        public void TestIndicatorsNoHeaders()
        {
            GenerateTestFileHeadings(false);
            var norm = new ProcessIndicators();
            norm.Analyze(InputName, false, CSVFormat.English);
            norm.AddColumn(new MovingAverage(3, true));
            norm.AddColumn(new BestClose(3, true));
            norm.Columns[0].Output = true;
            norm.RenameColumn(1, "close");
            norm.Process(OutputName);

            var tr = new StreamReader(OutputName.ToString());

            Assert.AreEqual("20100103,3,2,6", tr.ReadLine());
            Assert.AreEqual("20100104,4,3,7", tr.ReadLine());
            Assert.AreEqual("20100105,5,4,8", tr.ReadLine());
            Assert.AreEqual("20100106,6,5,9", tr.ReadLine());
            Assert.AreEqual("20100107,7,6,10", tr.ReadLine());

            tr.Close();

            InputName.Delete();
            OutputName.Delete();
        }
    }
}
