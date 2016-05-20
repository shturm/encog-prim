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
using Encog.App.Analyst.CSV.Segregate;
using Encog.Util;
using Encog.Util.CSV;
using NUnit.Framework;

namespace Encog.App.CSV
{
    [TestFixture]
    public class TestSegregateCSV
    {
        public static readonly TempDir TempDir = new TempDir();
        public readonly FileInfo InputName = TempDir.CreateFile("test.csv");
        public readonly FileInfo OutputName1 = TempDir.CreateFile("test2.csv");
        public readonly FileInfo OutputName2 = TempDir.CreateFile("test3.csv");

        public void GenerateTestFileHeadings(bool header)
        {
            var tw = new StreamWriter(InputName.ToString());


            if (header)
            {
                tw.WriteLine("a,b");
            }
            tw.WriteLine("one,1");
            tw.WriteLine("two,2");
            tw.WriteLine("three,3");
            tw.WriteLine("four,4");

            // close the stream
            tw.Close();
        }

        [Test]
        public void TestFilterCSVHeaders()
        {
            GenerateTestFileHeadings(true);
            var norm = new SegregateCSV();
            norm.Targets.Add(new SegregateTargetPercent(OutputName1, 75));
            norm.Targets.Add(new SegregateTargetPercent(OutputName2, 25));
            norm.Analyze(InputName, true, CSVFormat.English);
            norm.Process();

            var tr = new StreamReader(OutputName1.ToString());
            Assert.AreEqual("\"a\",\"b\"", tr.ReadLine());
            Assert.AreEqual("one,1", tr.ReadLine());
            Assert.AreEqual("two,2", tr.ReadLine());
            Assert.AreEqual("three,3", tr.ReadLine());
            Assert.IsNull(tr.ReadLine());
            tr.Close();

            tr = new StreamReader(OutputName2.ToString());
            Assert.AreEqual("\"a\",\"b\"", tr.ReadLine());
            Assert.AreEqual("four,4", tr.ReadLine());
            Assert.IsNull(tr.ReadLine());
            tr.Close();

            InputName.Delete();
            OutputName1.Delete();
            OutputName1.Delete();
        }

        [Test]
        public void TestFilterCSVNoHeaders()
        {
            GenerateTestFileHeadings(false);
            var norm = new SegregateCSV();
            norm.Targets.Add(new SegregateTargetPercent(OutputName1, 75));
            norm.Targets.Add(new SegregateTargetPercent(OutputName2, 25));
            norm.Analyze(InputName, false, CSVFormat.English);
            norm.ProduceOutputHeaders = false;
            norm.Process();

            var tr = new StreamReader(OutputName1.ToString());
            Assert.AreEqual("one,1", tr.ReadLine());
            Assert.AreEqual("two,2", tr.ReadLine());
            Assert.AreEqual("three,3", tr.ReadLine());
            Assert.IsNull(tr.ReadLine());
            tr.Close();

            tr = new StreamReader(OutputName2.ToString());
            Assert.AreEqual("four,4", tr.ReadLine());
            Assert.IsNull(tr.ReadLine());
            tr.Close();

            InputName.Delete();
            OutputName1.Delete();
            OutputName2.Delete();
        }
    }
}
