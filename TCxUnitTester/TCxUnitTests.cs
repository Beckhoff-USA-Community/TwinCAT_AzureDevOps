using System;
using System.Collections.Generic;
using Xunit;

namespace TCxUnitTester
{
    public class TCxUnitTester : IClassFixture<TcTestFixture>
    {
        TcTestFixture testFixture;

        public TCxUnitTester(TcTestFixture fixture)
        {
            testFixture = fixture;
        }

        public static IEnumerable<object[]> GetTestInstances()
        {
            foreach (var t in TcTestFixture.GetTestInstances())
                yield return new object[] { t };
        }

        [Theory]
        [MemberData(nameof(GetTestInstances))]
        public void TcMsTest_FromRcp(object testData)
        {
            var res = testFixture.RpcExecute(testData as string);
            Assert.True(res.Result, res.Message);
        }
    }
}
