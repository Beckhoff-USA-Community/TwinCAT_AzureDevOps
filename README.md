Azure DevOps continuous integration pipeline using TwinCAT 3.1.4024.

The "MSTest" branch contains a C# xUnit project which queries the PLC runtime for all test function block instances, then executes them via RPC over ADS.

Contains:
- TwinCAT XAE project with a PLC runtime (simulated application)
- PowerShell build script using Automation Interface
- xUnit test project
- Azure DevOps yaml pipeline
