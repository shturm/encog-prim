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
using System.IO;
using Encog.App.Quant;
using Encog.App.Quant.Loader.Yahoo;
using Encog.Util;
using Encog.Util.CSV;
using NUnit.Framework;

namespace Encog.App.CSV
{
    [TestFixture]
    public class TestYahooDownload
    {
        public static readonly TempDir TempDir = new TempDir();
        public readonly FileInfo OutputName = TempDir.CreateFile("test2.csv");

        [Test]
        public void TestYahooDownloadError()
        {
            try
            {
                var yahoo = new YahooDownload();
                yahoo.Precision = 2;
                // load a non-sense ticker, should throw error
                yahoo.LoadAllData("sdfhusdhfuish", OutputName.ToString(), CSVFormat.English,
                                  new DateTime(2000, 01, 01),
                                  new DateTime(2000, 01, 10));

                // bad!
                Assert.IsTrue(false);
            }
            catch (QuantError)
            {
                // good!
            }
        }

        [Test]
        public void TestYahooDownloadCSV()
        {
            var yahoo = new YahooDownload();
            yahoo.Precision = 2;
            yahoo.LoadAllData("yhoo", OutputName.ToString(), CSVFormat.English,
                              new DateTime(2000, 01, 01),
                              new DateTime(2000, 01, 10));
            var tr = new StreamReader(OutputName.ToString());

            Assert.AreEqual(
                "date,time,open price,high price,low price,close price,volume,adjusted price",
                tr.ReadLine());
            Assert.AreEqual(
                "20000110,0,432.5,451.25,420,436.06,61022400,109.02",
                tr.ReadLine());
            Assert.AreEqual("20000107,0,366.75,408,363,407.25,48999600,101.81",
                            tr.ReadLine());
            Assert.AreEqual("20000106,0,406.25,413,361,368.19,71301200,92.05",
                            tr.ReadLine());
            Assert.AreEqual(
                "20000105,0,430.5,431.12,402,410.5,83194800,102.62",
                tr.ReadLine());
            tr.Close();
        }
    }
}
