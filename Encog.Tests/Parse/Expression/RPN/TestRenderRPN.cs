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

namespace Encog.Parse.Expression.RPN
{
    [TestFixture]
    public class TestRenderRPN
    {
        [Test]
        public void TestRenderBasic()
        {
            EncogProgram expression = new EncogProgram("(2+6)");
            RenderRPN render = new RenderRPN();
            String result = render.Render(expression);
            Assert.AreEqual("2 6 [+]", result);
        }

        [Test]
        public void TestRenderComplex()
        {
            EncogProgram expression = new EncogProgram("((a+25)^3/25)-((a*3)^4/250)");
            RenderRPN render = new RenderRPN();
            String result = render.Render(expression);
            Assert.AreEqual("a 25 [+] 3 [^] 25 [/] a 3 [*] 4 [^] 250 [/] [-]", result);
        }

        [Test]
        public void TestRenderFunction()
        {
            EncogProgram expression = new EncogProgram("(sin(x)+cos(x))/2");
            RenderRPN render = new RenderRPN();
            String result = render.Render(expression);
            Assert.AreEqual("x [sin] x [cos] [+] 2 [/]", result);
        }

        [Test]
        public void TestKnownConst()
        {
            EncogProgram expression = new EncogProgram("x*2*PI");
            RenderRPN render = new RenderRPN();
            String result = render.Render(expression);
            Assert.AreEqual("x 2 [*] PI [*]", result);
        }
    }
}
