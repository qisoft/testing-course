using System;
using NUnit.Framework;

namespace Kontur.Courses.Testing.Patterns.Exercise
{
	[TestFixture]
	public class TextDocumentTests
	{
        [TestFixture]
        public class Replace_should
        {
            [TestCase("hello world", 0, 1, "H", Result = "Hello world", TestName = "1 letter at the beginning")]
            [TestCase("Hello world", 6, 5, "all!", Result = "Hello all!", TestName = "5 letters in the end")]
            [TestCase("world", 0, 0, "Hello ", Result = "Hello world", TestName = "nothing and insert text at the beginning")]
            [TestCase("Helloworld", 5, 0, " ", Result = "Hello world", TestName = "nothing and insert text in the middle")]
            public string Change(string input, int index, int count, string replacement)
            {
                var doc = new TextDocument(input);
                doc.MakeReplaceCommand(index, count, replacement).Do();
                return doc.Text;
            }

            [Test]
            public void IncreementEditsCount()
            {
                var doc = new TextDocument("hello world");
                doc.MakeReplaceCommand(0, 1, "H").Do();
                doc.MakeReplaceCommand(0, 1, "H").Do();
                Assert.AreEqual(2, doc.EditsCount);
            }
        }
	    [TestFixture]
	    public class Undo_should
	    {
	        [TestCase("hello world", TestName = "on non-empty string")]
            [TestCase("", TestName = "on empty string")]
	        public void RestoreDocumentToPreviousState(string input)
	        {
	            var doc = new TextDocument(input);
	            var cmd = doc.MakeReplaceCommand(0, 1, "H");
	            cmd.Do();
	            cmd.Undo();
	            Assert.AreEqual(0, doc.EditsCount);
	            Assert.AreEqual(input, doc.Text);
	        }

            [Test]
            public void WhenCommandIsNotDone_Fail()
            {
                var doc = new TextDocument("hello world!");
                var cmd = doc.MakeReplaceCommand(0, 1, "H");
                Assert.Throws<InvalidOperationException>(() => cmd.Undo());
            }
        }
	}
}