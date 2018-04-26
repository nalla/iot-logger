var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

Task("Clean")
  .Does(() =>
{
  CleanDirectory("./artifacts");
});

Task("Publish")
  .IsDependentOn("Clean")
  .Does(() =>
{
  DotNetCorePublish("./src/IoTLogger/IoTLogger.csproj", new DotNetCorePublishSettings
  {
    Configuration = configuration,
    OutputDirectory = "./artifacts"
  });
});

Task("Default").IsDependentOn("Publish");

RunTarget(target);
