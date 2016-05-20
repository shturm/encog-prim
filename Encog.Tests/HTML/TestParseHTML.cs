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
using System.Text;
using Encog.Parse.Tags;
using Encog.Parse.Tags.Read;
using NUnit.Framework;

namespace Encog.Bot.HTML
{
    [TestFixture]
    public class TestParseHTML
    {
        [Test]
        public void TestAttributeLess()
        {
            const string html = "12<b>12</b>1";
            var enc = new ASCIIEncoding();
            var bis = new MemoryStream(enc.GetBytes(html));

            var parse = new ReadHTML(bis);
            Assert.IsTrue(parse.Read() == '1');
            Assert.IsTrue(parse.Read() == '2');
            Assert.IsTrue(parse.Read() == 0);
            Assert.IsTrue(parse.LastTag.Name.Equals("b"));
            Assert.IsTrue(parse.LastTag.TagType == Tag.Type.Begin);
            Assert.IsTrue(parse.Read() == '1');
            Assert.IsTrue(parse.Read() == '2');
            Assert.IsTrue(parse.Read() == 0);
            Tag tag = parse.LastTag;
            Assert.IsTrue(tag.Name.Equals("b"));
            Assert.IsTrue(tag.TagType == Tag.Type.End);
            Assert.AreEqual(tag.ToString(), "</b>");
            Assert.IsTrue(parse.Read() == '1');
        }

        [Test]
        public void TestAttributes()
        {
            const string html = "<img src=\"picture.gif\" alt=\"A Picture\">";
            var enc = new ASCIIEncoding();
            var bis = new MemoryStream(enc.GetBytes(html));
            var parse = new ReadHTML(bis);
            Assert.IsTrue(parse.Read() == 0);
            Tag tag = parse.LastTag;
            Assert.IsNotNull(tag);
            Assert.IsTrue(tag.Name.Equals("img"));
            //TestCase.assertTrue(html.equals(tag.toString()));
            Assert.IsTrue(tag.GetAttributeValue("src").Equals("picture.gif"));
            Assert.IsTrue(tag.GetAttributeValue("alt").Equals("A Picture"));
        }

        [Test]
        public void TestAttributesNoDelim()
        {
            const string html = "<img src=picture.gif alt=APicture>";
            var enc = new ASCIIEncoding();
            var bis = new MemoryStream(enc.GetBytes(html));
            var parse = new ReadHTML(bis);
            Assert.IsTrue(parse.Read() == 0);
            Tag tag = parse.LastTag;
            Assert.IsNotNull(tag);
            Assert.IsTrue(tag.Name.Equals("img"));
            Assert.IsTrue(tag.GetAttributeValue("src").Equals("picture.gif"));
            Assert.IsTrue(tag.GetAttributeValue("alt").Equals("APicture"));
        }

        [Test]
        public void TestBoth()
        {
            const string html = "<br/>";
            const string htmlName = "br";
            var enc = new ASCIIEncoding();
            var bis = new MemoryStream(enc.GetBytes(html));
            var parse = new ReadHTML(bis);
            Assert.IsTrue(parse.Read() == 0);
            Tag tag = parse.LastTag;
            Assert.IsNotNull(tag);
            Assert.IsTrue(tag.TagType == Tag.Type.Begin);
            Assert.IsTrue(tag.Name.Equals(htmlName));
            parse.ReadToTag();
            tag = parse.LastTag;
            Assert.IsNotNull(tag);
            Assert.IsTrue(tag.TagType == Tag.Type.End);
            Assert.IsTrue(tag.Name.Equals(htmlName));
        }

        [Test]
        public void TestBothWithAttributes()
        {
            const string html = "<img src=\"picture.gif\" alt=\"A Picture\"/>";
            var enc = new ASCIIEncoding();
            var bis = new MemoryStream(enc.GetBytes(html));
            var parse = new ReadHTML(bis);
            Assert.IsTrue(parse.Read() == 0);
        }

        [Test]
        public void TestComment()
        {
            const string html = "a<!-- Hello -->b";
            var enc = new ASCIIEncoding();
            var bis = new MemoryStream(enc.GetBytes(html));
            var parse = new ReadHTML(bis);
            Assert.IsTrue(parse.Read() == 'a');
            Assert.IsTrue(parse.Read() == 0);
            Assert.IsTrue(parse.Read() == 'b');
        }

        [Test]
        public void TestScript()
        {
            const string html = "a<script>12</script>b";
            var enc = new ASCIIEncoding();
            var bis = new MemoryStream(enc.GetBytes(html));
            var parse = new ReadHTML(bis);
            Assert.IsTrue(parse.Read() == 'a');
            Assert.IsTrue(parse.Read() == 0);
            Assert.IsTrue(parse.Read() == '1');
            Assert.IsTrue(parse.Read() == '2');
            Assert.IsTrue(parse.Read() == 0);
            Assert.IsTrue(parse.Read() == 'b');
        }

        [Test]
        public void TestScript2()
        {
            const string html = "a<script>1<2</script>b<br>";
            var enc = new ASCIIEncoding();
            var bis = new MemoryStream(enc.GetBytes(html));
            var parse = new ReadHTML(bis);
            Assert.IsTrue(parse.Read() == 'a');
            Assert.IsTrue(parse.Read() == 0);
            Assert.IsTrue(parse.Read() == '1');
            Assert.IsTrue(parse.Read() == '<');
            Assert.IsTrue(parse.Read() == '2');
            Assert.IsTrue(parse.Read() == 0);
            Assert.IsTrue(parse.Read() == 'b');
            Assert.IsTrue(parse.Read() == 0);
        }

        [Test]
        public void TestToString()
        {     
            const  string html = "a<img src=\"picture.gif\" alt=\"A Picture\">b";
            var enc = new ASCIIEncoding();
            var bis = new MemoryStream(enc.GetBytes(html));
            var parse = new ReadHTML(bis);
            parse.ReadToTag();
            Assert.IsTrue(parse.ToString().IndexOf("A Picture") != -1);
        }

        [Test]
        public void TestTagToString()
        {
            const string html = "<br/>";
            var enc = new ASCIIEncoding();
            var bis = new MemoryStream(enc.GetBytes(html));
            var parse = new ReadHTML(bis);
            Assert.IsTrue(parse.Read() == 0);
        }

        [Test]
        public void TestSpecialCharacter()
        {
            const string html = "&lt;&gt;&#65;";
            var enc = new ASCIIEncoding();
            var bis = new MemoryStream(enc.GetBytes(html));
            var parse = new ReadHTML(bis);
            Assert.IsTrue(parse.Read() == '<');
            Assert.IsTrue(parse.Read() == '>');
            Assert.IsTrue(parse.Read() == 'A');
        }

        [Test]
        public void TestSimpleAttribute()
        {
            const string html = "<!DOCTYPE \"test\">";
            var enc = new ASCIIEncoding();
            var bis = new MemoryStream(enc.GetBytes(html));
            var parse = new ReadHTML(bis);
            Assert.IsTrue(parse.Read() == 0);
            Tag tag = parse.LastTag;
            Assert.AreEqual(tag.ToString(), html);
        }
    }
}
