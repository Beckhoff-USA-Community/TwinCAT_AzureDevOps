## Automated PLC build and run
## Dependencies: 
##  - Visual Studio 2019
##  - TwinCAT XAE

# instantiate DTE object - this is the Visual Studio automation COM library
$dte = new-object -com "VisualStudio.DTE.16.0"
Start-Sleep 1

# suppress VS interface
$dte.SuppressUI = $true

# open solution file
$slnPath = "$pwd\TC_ADO_Demo_PLC.sln"
$sln = $dte.Solution

echo "Opening PLC solution at $slnPath (in background)"
$sln.Open($slnPath)
Start-Sleep 5

#### build options ###

# get base TwinCAT project
$systemProject = $sln.Projects.Item(1)

# set build configuration
$buildConfig = "Release|TwinCAT RT (x64)"

# build specific project
echo "Building TwinCAT project"
$sln.SolutionBuild.BuildProject($buildConfig, $systemProject.FullName, $true)

# set run mode
$systemManager = $systemProject.Object

echo "Activating Configuration"
$systemManager.ActivateConfiguration()

echo "Restarting TwinCAT runtime"
$systemManager.StartRestartTwinCAT()

# close VS
echo "Exiting VS..."
$dte.Quit()