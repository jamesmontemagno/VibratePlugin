#addin "Cake.FileHelpers"

var TARGET = Argument ("target", Argument ("t", "Default"));

var version = EnvironmentVariable ("APPVEYOR_BUILD_VERSION") ?? Argument("version", "0.0.9999");

var libraries = new Dictionary<string, string> {
 	{ "./../Vibrate.sln", "Any" },
};

var samples = new Dictionary<string, string> {
	{ "./../Sample/VibrateSample.sln", "Win" },
};

var BuildAction = new Action<Dictionary<string, string>> (solutions =>
{

	foreach (var sln in solutions) 
    {

		// If the platform is Any build regardless
		//  If the platform is Win and we are running on windows build
		//  If the platform is Mac and we are running on Mac, build
		if ((sln.Value == "Any")
				|| (sln.Value == "Win" && IsRunningOnWindows ())
				|| (sln.Value == "Mac" && IsRunningOnUnix ())) 
        {
			
			// Bit of a hack to use nuget3 to restore packages for project.json
			if (IsRunningOnWindows ()) 
            {
				
				Information ("RunningOn: {0}", "Windows");

				NuGetRestore (sln.Key, new NuGetRestoreSettings
                {
					ToolPath = "./tools/nuget3.exe"
				});

				// Windows Phone / Universal projects require not using the amd64 msbuild
				MSBuild (sln.Key, c => 
                { 
					c.Configuration = "Release";
					c.MSBuildPlatform = Cake.Common.Tools.MSBuild.MSBuildPlatform.x86;
				});
			} 
            else 
            {
                // Mac is easy ;)
				NuGetRestore (sln.Key);

				DotNetBuild (sln.Key, c => c.Configuration = "Release");
			}
		}
	}
});

Task("Libraries").Does(()=>
{
    BuildAction(libraries);
});

Task("Samples")
    .IsDependentOn("Libraries")
    .Does(()=>
{
    BuildAction(samples);
});

Task ("NuGet")
	.IsDependentOn ("Samples")
	.Does (() =>
{
    if(!DirectoryExists("./../Build/nuget/"))
        CreateDirectory("./../Build/nuget");
        
	NuGetPack ("./../Vibrate.nuspec", new NuGetPackSettings { 
		Version = version,
		Verbosity = NuGetVerbosity.Detailed,
		OutputDirectory = "./../",
		BasePath = "./../",
	});	
});

Task("Component")
    .IsDependentOn("Samples")
    .IsDependentOn("NuGet")
    .Does(()=>
{
    // Clear out xml files from build (they interfere with the component packaging)
	DeleteFiles ("./../Build/**/*.xml");

	// Generate component.yaml files from templates
	CopyFile ("./../Component/component.template.yaml", "./../Component/component.yaml");

	// Replace version in template files
	ReplaceTextInFiles ("./../**/component.yaml", "{VERSION}", version);

	var xamCompSettings = new XamarinComponentSettings { ToolPath = "./tools/xamarin-component.exe" };

	// Package both components
	PackageComponent ("./../Component/", xamCompSettings);
});

Task ("Default").IsDependentOn("Component")


Task ("Clean").Does (() => 
{
	CleanDirectory ("./../Component/tools/");

	CleanDirectories ("./../Build/");

	CleanDirectories ("./../**/bin");
	CleanDirectories ("./../**/obj");
});


RunTarget (TARGET);
