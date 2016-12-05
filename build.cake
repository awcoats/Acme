//#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#tool "nuget:?package=xunit.runner.console"
#tool "nuget:?package=OpenCover&version=4.6.519"
#tool "nuget:?package=ReportGenerator&version=2.4.5"
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory("./") ;

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    //CleanDirectory("/Acme/bin/"+configuration);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore("./Acme.sln");
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
      // Use MSBuild
      MSBuild("./Acme.sln", settings => settings.SetConfiguration(configuration).SetMaxCpuCount(0).SetVerbosity(Verbosity.Minimal));
    }
    else
    {
      // Use XBuild
      XBuild("./Acme.sln", settings =>
        settings.SetConfiguration(configuration));
    }
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    XUnit2("./**/bin/" + configuration + "/Acme.UnitTests.dll", new XUnit2Settings(){

    });
});

Task("FunctionalTests")      
    .Does(() =>
{
    //TODO - check to see if WinAppDriver is running.
    //TODO - start C:\Program Files (x86)\Windows Application Driver\WinAppDriver.EXECUTION
    //TODO http://127.0.0.1:4723/status - see if responds.
    XUnit2("./**/bin/" + configuration + "/Acme.FunctionalTests.dll", new XUnit2Settings(){
    });
});


Task("CodeCoverage")    
    //.IsDependentOn("Build")  
    .Does(() =>
{
    Information("Confirguation="+configuration);
	Action<ICakeContext> testAction = tool => {
       // tool.XUnit2("./**/bin/" + configuration + "/*.UnitTests.dll",new XUnit2Settings());
        tool.XUnit2("./Acme.UnitTests/bin/"+configuration+"/Acme.UnitTests.dll",new XUnit2Settings());
    };
    
    OpenCover(testAction,
        new FilePath("./coverageResults.xml"),
        new OpenCoverSettings(){
             //ReturnTargetCodeOffset = 0,
            // ArgumentCustomization = args => args.Append("-mergeoutput")
        }
        //.WithFilter("+[*]*")
        //.ExcludeByAttribute("*.ExcludeFromCodeCoverage*")
       // .ExcludeByFile("*/*Designer.cs;*/*.g.cs;*/*.g.i.cs")    
    );
    
    ReportGenerator("./coverageResults.xml", "./Reports/Coverage");
});



//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("CodeCoverage");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
