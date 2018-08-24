#tool "nuget:?package=ReportGenerator"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var testArtifact = Directory("./tests") + Directory("Artifacts");

Task("Clean")
    .Does(() =>{       
        CleanDirectory(testArtifact) ;
        DotNetCoreClean("./", new DotNetCoreCleanSettings()
        {
            Configuration = configuration,
            Verbosity = DotNetCoreVerbosity.Minimal
        });
    });

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>{
        DotNetCoreRestore("./",new DotNetCoreRestoreSettings()
        {            
            Verbosity = DotNetCoreVerbosity.Minimal
        });
    });

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>{
        DotNetCoreBuild("./",new DotNetCoreBuildSettings()
        {
            Configuration = configuration,
            NoRestore = true,  
            Verbosity = DotNetCoreVerbosity.Minimal          
        });
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>{
        var output = MakeAbsolute(testArtifact).ToString();
        foreach(var project in GetFiles("./tests/**/*.csproj"))
        {
            DotNetCoreTest(
                project.GetDirectory().FullPath,
                new DotNetCoreTestSettings()
                {
                    Configuration = configuration,
                    NoBuild = true,
                    NoRestore = true,
                    Verbosity = DotNetCoreVerbosity.Minimal,
                    ArgumentCustomization = args => args
                        .Append("/p:CollectCoverage=true")
                        .Append("/p:CoverletOutputFormat=cobertura")
                        .Append($"/p:CoverletOutput=\"{output}/\""),
                });
        }
    });

Task("Publish-Test")
    .IsDependentOn("Test")
    .Does(() =>
    {
        var file = testArtifact.Path.GetFilePath("coverage.cobertura.xml");
        var report = testArtifact.Path.Combine("Report");

        ReportGenerator(file, report);
        
        if(TFBuild.IsRunningOnVSTS)
        {
            TFBuild.Commands.PublishCodeCoverage(new TFBuildPublishCodeCoverageData()
            {
                SummaryFileLocation = file.ToString(),
                CodeCoverageTool = TFCodeCoverageToolType.Cobertura,
                ReportDirectory = report.ToString()
            });
        }
    });

Task("Default")
    .IsDependentOn("Publish-Test")
  .Does(() =>
{
     
});

RunTarget(target);