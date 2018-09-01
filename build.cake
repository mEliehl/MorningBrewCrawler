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
    .Does(() =>{
        DotNetCoreRestore("./",new DotNetCoreRestoreSettings()
        {            
            Verbosity = DotNetCoreVerbosity.Minimal
        });
    });

Task("Build")    
    .Does(() =>{
        DotNetCoreBuild("./",new DotNetCoreBuildSettings()
        {
            Configuration = configuration,
            NoRestore = true,  
            Verbosity = DotNetCoreVerbosity.Minimal          
        });
    });

Task("Test")    
    .Does(() =>{
        var output = MakeAbsolute(testArtifact).ToString() + "/Unit";
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
                    Logger = "trx",
                    ResultsDirectory = testArtifact,
                    Filter = "Category=Unit",
                    ArgumentCustomization = args => args
                        .Append("/p:CollectCoverage=true")
                        .Append("/p:CoverletOutputFormat=cobertura")
                        .Append($"/p:CoverletOutput=\"{output}/\""),
                });
        }
    });

Task("Database-Migration")
    .Does(() =>
    {
        DotNetCoreRun("./src/SqlServer.Migration.Fluent", "",
        new DotNetCoreRunSettings()
        {
            Configuration = configuration,
            NoBuild = true,
            NoRestore = true,
            Verbosity = DotNetCoreVerbosity.Minimal,
        });
    });

Task("Integration-Test")      
    .Does(() =>{
        var output = MakeAbsolute(testArtifact).ToString() + "/Integration";
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
                    Logger = "trx",
                    ResultsDirectory = testArtifact,
                    Filter = "Category=Integration",
                    ArgumentCustomization = args => args
                        .Append("/p:CollectCoverage=true")
                        .Append("/p:CoverletOutputFormat=cobertura")
                        .Append($"/p:CoverletOutput=\"{output}/\""),
                });
        }
    });

Task("Publish-Test")   
    .Does(() =>
    {
        var file = testArtifact.Path.GetFilePath("coverage.cobertura.xml");
        var report = testArtifact.Path.Combine("Report");
        var files = GetFiles($"{testArtifact.Path}/**/*.cobertura.xml");

        ReportGenerator(files, report, new ReportGeneratorSettings()
        {            
            ArgumentCustomization = args => args
                .Append("-reportTypes:htmlInline")                        
        });
    });

Task("BuildAndUnitTest")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .IsDependentOn("Test");

Task("MigrationAndIntegrationTest")
    .IsDependentOn("Database-Migration")
    .IsDependentOn("Integration-Test");

Task("Default")
    .IsDependentOn("BuildAndUnitTest")
    .IsDependentOn("MigrationAndIntegrationTest")
    .IsDependentOn("Publish-Test");

RunTarget(target);