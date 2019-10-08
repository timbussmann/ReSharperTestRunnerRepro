using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace TestRunnerRepro
{
    [TestFixture]
    public class TestClass
    {
        [SetUp]
        public void Setup()
        {
            if (AppDomainRunner.IsNotInTestAppDomain)
            {
                AppDomainRunner.DataStore.Set("TestDirectory", TestContext.CurrentContext.TestDirectory);

                var dirpath = Path.Combine(TestContext.CurrentContext.TestDirectory, "testdir");
                if (Directory.Exists(dirpath))
                {
                    Directory.Delete(dirpath, true);
                }

                Directory.CreateDirectory(dirpath);
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (AppDomainRunner.IsNotInTestAppDomain)
            {
                var dirpath = Path.Combine(TestContext.CurrentContext.TestDirectory, "testdir");
            }
        }

        [Test, RunInApplicationDomain]
        public void A_TestRunningInSeparateAppDomain()
        {
            AppDomain.CurrentDomain.Load("NServiceBus.Core");
            File.Move(
                Path.Combine(AppDomainRunner.DataStore.Get<string>("TestDirectory"), "NServiceBus.Core.dll"),
                Path.Combine(AppDomainRunner.DataStore.Get<string>("TestDirectory"), "testdir", "NServiceBus.Core.dll"));
        }

        [Test, RunInApplicationDomain]
        public void B_SecondTestInSeparateAppDomain()
        {
        }
    }
}
