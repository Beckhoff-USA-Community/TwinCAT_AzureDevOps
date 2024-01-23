using System;
using System.Collections.Generic;
using System.Linq;
using TwinCAT.Ads;
using TwinCAT.Ads.TypeSystem;

namespace TCxUnitTester
{
    public class TcTestFixture : IDisposable
    {
        private AdsClient client;

        static readonly string amsNetId = "127.0.0.1.1.1";
        static readonly int port = 851;

        public TcTestFixture()
        {
            client = new AdsClient();
            client.Connect(new AmsNetId(amsNetId), port);
        }

        public TcTestResult RpcExecute(string symbolPath)
        {
            var rcpRes = (byte[])client.InvokeRpcMethod(symbolPath, "Execute", null);
            return Helpers.BytesToStructure<TcTestResult>(rcpRes);
        }

        public static IReadOnlyCollection<string> GetTestInstances(string testInterfaceName = "ITest")
        {
            var session = new AdsSession(new AmsNetId(amsNetId), port);
            session.Connect();

            if (!session.IsConnected)
                return Enumerable.Empty<string>().ToList();

            session.Settings.SymbolLoader.SymbolsLoadMode = TwinCAT.SymbolsLoadMode.Flat;

            return session.SymbolServer.Symbols
                .Where(s => s.DataType is StructType &&
                    (s.DataType as StructType).InterfaceImplementationNames.Contains(testInterfaceName))
                .Select(x => x.InstancePath)
                .ToList();
        }

        public void Dispose()
        {
            client?.Dispose();
        }
    }
}
