using System;
using System.Collections.Generic;
using System.Text;
using TwinCAT.Ads;
using System.Linq;
using TwinCAT.TypeSystem;
using TwinCAT.Ads.TypeSystem;

namespace TcMsTester
{
    class TcTestProvider : IDisposable
    {
        private AdsSession session;

        public TcTestProvider(string amsNetId, int port = 851)
        {
            session = new AdsSession(new AmsNetId(amsNetId), port);
            session.Connect();

            var test = GetTestInstances();
        }

        public IReadOnlyList<TcTestInstance> GetTestInstances(string testInterfaceName = "ITest")
        {
            if (!session.IsConnected)
                return Enumerable.Empty<TcTestInstance>().ToList();

            session.Settings.SymbolLoader.SymbolsLoadMode = TwinCAT.SymbolsLoadMode.Flat;
            var testSymbols = session.SymbolServer.Symbols
                .Where(s => s.Category == DataTypeCategory.Struct &&
                (s.DataType as StructType).InterfaceImplementationNames.Contains(testInterfaceName));

            if (testSymbols?.Count() > 0)
            {
                var client = new AdsClient(session, null);
                client.Connect(session.Address);

                return testSymbols.Select(s =>
                {
                    return new TcTestInstance()
                    {
                        Name = (string)(client.ReadSymbol(s.InstancePath + "._name") as IValueSymbol).ReadValue(),
                        Result = (bool)client.InvokeRpcMethod(s.InstancePath, "Execute", null),
                        Message = (string)(client.ReadSymbol(s.InstancePath + "._message") as IValueSymbol).ReadValue()
                    };
                })
                .ToList();
            }
            else
            {
                return Enumerable.Empty<TcTestInstance>().ToList();
            }   
        }

        public void Dispose()
        {
            session?.Disconnect();
            session?.Dispose();
        }
    }
}
