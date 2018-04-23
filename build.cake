var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

Task("Publish")
  .Does(() =>
{
  DotNetCorePublish("./src/IoTLogger/IoTLogger.csproj", new DotNetCorePublishSettings
  {
    Configuration = configuration
  });
});

Task("Default").IsDependentOn("Publish");

RunTarget(target);
