using System;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TcMsTester
{
    [TestClass]
    public class TcMsTests
    {
        [ClassInitialize]
        public static void TestFixtureSetup(TestContext context)
        {
            // (AI): Load VS solution; build, activate and run PLC
            // (ADS): Establish ADS connection, get all tests
        }

        [ClassCleanup]
        public static void TestFixtureTearDown()
        {
            // (ADS): Close ADS connection
            // (AI): Set to Config mode; close VS
        }

        [TestInitialize]
        public void Init()
        {
            Console.WriteLine("Before each test");
        }

        public static IEnumerable<object[]> AdditionData
        {
            get
            {
                return new[]
                {
                    new object[] { 1, 1, 2 },
                    new object[] { 2, 2, 4 },
                    new object[] { 3, 3, 6 }
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(AdditionData), DynamicDataDisplayName = nameof(GetCustomDynamicDataDisplayName))]
        public void AddIntegers_FromDynamicDataTest(int x, int y, int expected)
        {
            int actual = x + y;

            Assert.AreEqual(expected, actual,
                "x:<{0}> y:<{1}>",
                new object[] { x, y });

            Console.WriteLine("Test Results");
        }

        public static string GetCustomDynamicDataDisplayName(MethodInfo methodInfo, object[] data)
        {
            return string.Format("DynamicDataTestMethod {0} with {1} parameters", methodInfo.Name, data.Length);
        }
    }
}
