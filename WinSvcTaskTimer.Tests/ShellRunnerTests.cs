namespace WinSvcTaskTimer.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ShellRunnerTests
    {
        [TestMethod]
        public void Verb()
        {
            string input = "cd";
            var start = ShellRunner.ShellStart.Create(input);
            Assert.AreEqual(0, start.ExpectedCode);
            Assert.AreEqual(null, start.Directory);
            Assert.AreEqual("cd", start.FileName);
            Assert.AreEqual(null, start.Arguments);
        }

        [TestMethod]
        public void VerbAndArg()
        {
            string input = "ping google.com";
            var start = ShellRunner.ShellStart.Create(input);
            Assert.AreEqual(0, start.ExpectedCode);
            Assert.AreEqual(null, start.Directory);
            Assert.AreEqual("ping", start.FileName);
            Assert.AreEqual("google.com", start.Arguments);
        }

        [TestMethod]
        public void Path()
        {
            string input = "c:\\test.exe";
            var start = ShellRunner.ShellStart.Create(input);
            Assert.AreEqual(0, start.ExpectedCode);
            Assert.AreEqual(null, start.Directory);
            Assert.AreEqual("c:\\test.exe", start.FileName);
            Assert.AreEqual(null, start.Arguments);
        }

        [TestMethod]
        public void PathAndArg()
        {
            string input = "c:\\test.exe bidou bida";
            var start = ShellRunner.ShellStart.Create(input);
            Assert.AreEqual(0, start.ExpectedCode);
            Assert.AreEqual(null, start.Directory);
            Assert.AreEqual("c:\\test.exe", start.FileName);
            Assert.AreEqual("bidou bida", start.Arguments);
        }

        [TestMethod]
        public void QuotedPath()
        {
            string input = "\"c:\\test.exe\"";
            var start = ShellRunner.ShellStart.Create(input);
            Assert.AreEqual(0, start.ExpectedCode);
            Assert.AreEqual(null, start.Directory);
            Assert.AreEqual("c:\\test.exe", start.FileName);
            Assert.AreEqual(null, start.Arguments);
        }

        [TestMethod]
        public void QuotedPathAndArg()
        {
            string input = "\"c:\\test.exe\" bidou bida";
            var start = ShellRunner.ShellStart.Create(input);
            Assert.AreEqual(0, start.ExpectedCode);
            Assert.AreEqual(null, start.Directory);
            Assert.AreEqual("c:\\test.exe", start.FileName);
            Assert.AreEqual("bidou bida", start.Arguments);
        }

        [TestMethod]
        public void VerbWithCode()
        {
            string input = "CODE:1 cd";
            var start = ShellRunner.ShellStart.Create(input);
            Assert.AreEqual(1, start.ExpectedCode);
            Assert.AreEqual(null, start.Directory);
            Assert.AreEqual("cd", start.FileName);
            Assert.AreEqual(null, start.Arguments);
        }

        [TestMethod]
        public void VerbAndArgWithCode()
        {
            string input = "CODE:1 ping google.com";
            var start = ShellRunner.ShellStart.Create(input);
            Assert.AreEqual(1, start.ExpectedCode);
            Assert.AreEqual(null, start.Directory);
            Assert.AreEqual("ping", start.FileName);
            Assert.AreEqual("google.com", start.Arguments);
        }

        [TestMethod]
        public void PathWithCode()
        {
            string input = "CODE:1 c:\\test.exe";
            var start = ShellRunner.ShellStart.Create(input);
            Assert.AreEqual(1, start.ExpectedCode);
            Assert.AreEqual(null, start.Directory);
            Assert.AreEqual("c:\\test.exe", start.FileName);
            Assert.AreEqual(null, start.Arguments);
        }

        [TestMethod]
        public void PathAndArgWithCode()
        {
            string input = "CODE:1 c:\\test.exe bidou bida";
            var start = ShellRunner.ShellStart.Create(input);
            Assert.AreEqual(1, start.ExpectedCode);
            Assert.AreEqual(null, start.Directory);
            Assert.AreEqual("c:\\test.exe", start.FileName);
            Assert.AreEqual("bidou bida", start.Arguments);
        }

        [TestMethod]
        public void QuotedPathWithCode()
        {
            string input = "CODE:1 \"c:\\test.exe\"";
            var start = ShellRunner.ShellStart.Create(input);
            Assert.AreEqual(1, start.ExpectedCode);
            Assert.AreEqual(null, start.Directory);
            Assert.AreEqual("c:\\test.exe", start.FileName);
            Assert.AreEqual(null, start.Arguments);
        }

        [TestMethod]
        public void QuotedPathAndArgWithCode()
        {
            string input = "CODE:1 \"c:\\test.exe\" bidou bida";
            var start = ShellRunner.ShellStart.Create(input);
            Assert.AreEqual(1, start.ExpectedCode);
            Assert.AreEqual(null, start.Directory);
            Assert.AreEqual("c:\\test.exe", start.FileName);
            Assert.AreEqual("bidou bida", start.Arguments);
        }

        [TestMethod]
        public void VerbWithDir()
        {
            string input = "DIR:c:\\superDir cd";
            var start = ShellRunner.ShellStart.Create(input);
            Assert.AreEqual(0, start.ExpectedCode);
            Assert.AreEqual("c:\\superDir", start.Directory);
            Assert.AreEqual("cd", start.FileName);
            Assert.AreEqual(null, start.Arguments);
        }

        [TestMethod]
        public void VerbAndArgWithDir()
        {
            string input = "DIR:c:\\superDir ping google.com";
            var start = ShellRunner.ShellStart.Create(input);
            Assert.AreEqual(0, start.ExpectedCode);
            Assert.AreEqual("c:\\superDir", start.Directory);
            Assert.AreEqual("ping", start.FileName);
            Assert.AreEqual("google.com", start.Arguments);
        }

        [TestMethod]
        public void PathWithDir()
        {
            string input = "DIR:c:\\superDir c:\\test.exe";
            var start = ShellRunner.ShellStart.Create(input);
            Assert.AreEqual(0, start.ExpectedCode);
            Assert.AreEqual("c:\\superDir", start.Directory);
            Assert.AreEqual("c:\\test.exe", start.FileName);
            Assert.AreEqual(null, start.Arguments);
        }

        [TestMethod]
        public void PathAndArgWithDir()
        {
            string input = "DIR:c:\\superDir c:\\test.exe bidou bida";
            var start = ShellRunner.ShellStart.Create(input);
            Assert.AreEqual(0, start.ExpectedCode);
            Assert.AreEqual("c:\\superDir", start.Directory);
            Assert.AreEqual("c:\\test.exe", start.FileName);
            Assert.AreEqual("bidou bida", start.Arguments);
        }

        [TestMethod]
        public void QuotedPathWithDir()
        {
            string input = "DIR:c:\\superDir \"c:\\test.exe\"";
            var start = ShellRunner.ShellStart.Create(input);
            Assert.AreEqual(0, start.ExpectedCode);
            Assert.AreEqual("c:\\superDir", start.Directory);
            Assert.AreEqual("c:\\test.exe", start.FileName);
            Assert.AreEqual(null, start.Arguments);
        }

        [TestMethod]
        public void QuotedPathAndArgWithDir()
        {
            string input = "DIR:c:\\superDir \"c:\\test.exe\" bidou bida";
            var start = ShellRunner.ShellStart.Create(input);
            Assert.AreEqual(0, start.ExpectedCode);
            Assert.AreEqual("c:\\superDir", start.Directory);
            Assert.AreEqual("c:\\test.exe", start.FileName);
            Assert.AreEqual("bidou bida", start.Arguments);
        }

        [TestMethod]
        public void VerbWithDirAndCode()
        {
            string input = "CODE:1 DIR:c:\\superDir cd";
            var start = ShellRunner.ShellStart.Create(input);
            Assert.AreEqual(1, start.ExpectedCode);
            Assert.AreEqual("c:\\superDir", start.Directory);
            Assert.AreEqual("cd", start.FileName);
            Assert.AreEqual(null, start.Arguments);
        }

        [TestMethod]
        public void VerbAndArgWithDirAndCode()
        {
            string input = "CODE:1 DIR:c:\\superDir ping google.com";
            var start = ShellRunner.ShellStart.Create(input);
            Assert.AreEqual(1, start.ExpectedCode);
            Assert.AreEqual("c:\\superDir", start.Directory);
            Assert.AreEqual("ping", start.FileName);
            Assert.AreEqual("google.com", start.Arguments);
        }

        [TestMethod]
        public void PathWithDirAndCode()
        {
            string input = "CODE:1 DIR:c:\\superDir c:\\test.exe";
            var start = ShellRunner.ShellStart.Create(input);
            Assert.AreEqual(1, start.ExpectedCode);
            Assert.AreEqual("c:\\superDir", start.Directory);
            Assert.AreEqual("c:\\test.exe", start.FileName);
            Assert.AreEqual(null, start.Arguments);
        }

        [TestMethod]
        public void PathAndArgWithDirAndCode()
        {
            string input = "CODE:1 DIR:c:\\superDir c:\\test.exe bidou bida";
            var start = ShellRunner.ShellStart.Create(input);
            Assert.AreEqual(1, start.ExpectedCode);
            Assert.AreEqual("c:\\superDir", start.Directory);
            Assert.AreEqual("c:\\test.exe", start.FileName);
            Assert.AreEqual("bidou bida", start.Arguments);
        }

        [TestMethod]
        public void QuotedPathWithDirAndCode()
        {
            string input = "CODE:1 DIR:c:\\superDir \"c:\\test.exe\"";
            var start = ShellRunner.ShellStart.Create(input);
            Assert.AreEqual(1, start.ExpectedCode);
            Assert.AreEqual("c:\\superDir", start.Directory);
            Assert.AreEqual("c:\\test.exe", start.FileName);
            Assert.AreEqual(null, start.Arguments);
        }

        [TestMethod]
        public void QuotedPathAndArgWithDirAndCode()
        {
            string input = "CODE:1 DIR:c:\\superDir \"c:\\test.exe\" bidou bida";
            var start = ShellRunner.ShellStart.Create(input);
            Assert.AreEqual(1, start.ExpectedCode);
            Assert.AreEqual("c:\\superDir", start.Directory);
            Assert.AreEqual("c:\\test.exe", start.FileName);
            Assert.AreEqual("bidou bida", start.Arguments);
        }
    }
}
