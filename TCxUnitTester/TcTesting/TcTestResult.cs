using System.Runtime.InteropServices;

namespace TCxUnitTester
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct TcTestResult
    {
        [MarshalAs(UnmanagedType.I1)]
        public bool Result;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Message;
    }
}
