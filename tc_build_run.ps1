## Automated PLC build and run
## Dependencies: 
##  - Visual Studio 2019
##  - TwinCAT XAE

# instantiate DTE object - this is the Visual Studio automation COM library
$dte = new-object -com "VisualStudio.DTE.16.0"
# suppress VS interface
$dte.SuppressUI = $true

# open solution file
$sln = $dte.Solution
echo "Opening solution in VS (background)..."
$sln.Open("$pwd\TC_ADO_Demo_PLC.sln")

#### build options ###

# get base TwinCAT project
$systemProject = $sln.Projects.Item(1)

# set build configuration
$buildConfig = "Release|TwinCAT RT (x64)"

# build specific project
$sln.SolutionBuild.BuildProject($buildConfig, $systemProject.FullName, $true)

# set run mode
$systemManager = $systemProject.Object
$systemManager.ActivateConfiguration()
$systemManager.StartRestartTwinCAT()

# close VS
echo "Quitting VS (background)..."
$dte.Quit()