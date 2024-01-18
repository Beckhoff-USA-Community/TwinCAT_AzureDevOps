using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace TcMsTester
{
    [TestClass]
    public class TcMsTests
    {
        static TcTestProvider testProvider;

        [ClassInitialize]
        public static void TestFixtureSetup(TestContext context)
        {
            // (AI): Load VS solution; build, activate and run PLC
            // (ADS): Establish ADS connection, get all tests
            testProvider = new TcTestProvider("39.87.174.29.1.1");
        }

        [ClassCleanup]
        public static void TestFixtureTearDown()
        {
            // (ADS): Close ADS connection
            testProvider?.Dispose();

            // (AI): Set to Config mode; close VS
        }

        public static IEnumerable<object[]> TcTestData
        {
            get
            {
                var tests = testProvider.GetTestInstances();
                foreach (var t in tests)
                    yield return new object[] { t };
            }
        }

        [TestMethod]
        [DynamicData(nameof(TcTestData), DynamicDataDisplayName = nameof(TcRpcTestName))]
        public void TcMsTest_FromRcp(object testData)
        {
            Assert.IsTrue((testData as TcTestInstance).Result, (testData as TcTestInstance).Message);
        }

        public static string TcRpcTestName(MethodInfo methodInfo, object[] data)
        {
            return string.Format("Test: {0} ran via RPC", (data[0] as TcTestInstance).Name);
        }
    }
}
