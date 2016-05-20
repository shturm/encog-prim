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
using Encog.Neural.ART;
using Encog.Util;
using NUnit.Framework;

namespace Encog.Persist
{
    [TestFixture]
    public class TestPersistART
    {
        public static readonly TempDir TEMP_DIR = new TempDir();
        public readonly FileInfo EG_FILENAME = TEMP_DIR.CreateFile("encogtest.eg");
        public readonly FileInfo SERIAL_FILENAME = TEMP_DIR.CreateFile("encogtest.ser");

        private ART1 Create()
        {
            var network = new ART1(6, 3);
            network.WeightsF1ToF2[1, 1] = 2.0;
            network.WeightsF2ToF1[2, 2] = 3.0;
            return network;
        }


        private void Validate(ART1 network)
        {
            Assert.AreEqual(6, network.F1Count);
            Assert.AreEqual(3, network.F2Count);
            Assert.AreEqual(18, network.WeightsF1ToF2.Size);
            Assert.AreEqual(18, network.WeightsF2ToF1.Size);
            Assert.AreEqual(2.0, network.WeightsF1ToF2[1, 1]);
            Assert.AreEqual(3.0, network.WeightsF2ToF1[2, 2]);
            Assert.AreEqual(1.0, network.A1);
            Assert.AreEqual(1.5, network.B1);
            Assert.AreEqual(5.0, network.C1);
            Assert.AreEqual(0.9, network.D1);
            Assert.AreEqual(0.9, network.Vigilance);
        }

        [Test]
        public void TestPersistEG()
        {
            ART1 network = Create();

            EncogDirectoryPersistence.SaveObject(EG_FILENAME, network);
            var network2 = (ART1) EncogDirectoryPersistence.LoadObject(EG_FILENAME);

            Validate(network2);
        }

        [Test]
        public void TestPersistSerial()
        {
            ART1 network = Create();

            SerializeObject.Save(SERIAL_FILENAME.ToString(), network);
            var network2 = (ART1) SerializeObject.Load(SERIAL_FILENAME.ToString());

            Validate(network2);
        }
    }
}
