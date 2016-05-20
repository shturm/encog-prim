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
using Encog.ML.Prg;

namespace Encog.Parse.Expression.Common
{
    [TestFixture]
    public class TestString
    {
        [Test]
        public void TestSimple()
        {
            Assert.AreEqual("test", EncogProgram.ParseString("\"test\""));
            Assert.AreEqual("", EncogProgram.ParseString("\"\""));
        }

        [Test]
        public void TestConcat()
        {
            Assert.AreEqual("test:123", EncogProgram.ParseString("\"test:\"+123.0"));
            Assert.AreEqual("helloworld", EncogProgram.ParseString("\"hello\"+\"world\""));
            Assert.AreEqual(4, (int)EncogProgram.ParseFloat("length(\"test\")"));
            Assert.AreEqual("5.22", EncogProgram.ParseString("format(5.2222,2)"));
        }
    }
}
