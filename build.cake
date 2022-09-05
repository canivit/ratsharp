var target = Argument("target", "CleanAndPublishAll");
var configuration = Argument("configuration", "Release");

var serverProjectDir = "./Server";
var adminClientProjectDir = "./AdminClient";
var userClientProjectDir = "./UserClient";
var publishDir = "./out";

#region Helper methods

public void PublishProjectToRuntime(string projectDir, string runtime)
{
   DotNetPublish(projectDir, new DotNetPublishSettings
   {
      Configuration = configuration,
      PublishSingleFile = true,
      PublishTrimmed = true,
      SelfContained = true,
      Runtime = runtime,
      OutputDirectory = $"{publishDir}/{runtime}",
   });
}

public void PublishProjectCrossPlatform(string projectDir)
{
   var projectName = System.IO.Path.GetFileName(projectDir);
   DotNetPublish(projectDir, new DotNetPublishSettings
   {
      Configuration = configuration,
      OutputDirectory = $"{publishDir}/cross-platform/{projectName}",
   });
}

#endregion

#region Clean 

Task("Clean")
   .Does(() => 
   {
      CleanDirectories("./**/obj");
      CleanDirectories("./**/bin");
      CleanDirectory("./out");
   });

#endregion

#region Publish Server

Task("PublishServerCrossPlatform")
   .Does(() => 
   {
      PublishProjectCrossPlatform(serverProjectDir);
   });

Task("PublishServerToLinux")
   .Does(() => 
   {
      PublishProjectToRuntime(serverProjectDir, "linux-x64");
   });

Task("PublishServerToWindows")
   .Does(() => 
   {
      PublishProjectToRuntime(serverProjectDir, "win-x64");
   });

Task("PublishServerToMacOs")
   .Does(() => 
   {
      PublishProjectToRuntime(serverProjectDir, "osx-x64");
   });

Task("PublishServer")
   .IsDependentOn("PublishServerCrossPlatform")
   .IsDependentOn("PublishServerToLinux")
   .IsDependentOn("PublishServerToWindows")
   .IsDependentOn("PublishServerToMacOs");   

#endregion

#region Publish AdminClient

Task("PublishAdminClientCrossPlatform")
   .Does(() => 
   {
      PublishProjectCrossPlatform(adminClientProjectDir);
   });

Task("PublishAdminClientToLinux")
   .Does(() => 
   {
      PublishProjectToRuntime(adminClientProjectDir, "linux-x64");
   });

Task("PublishAdminClientToWindows")
   .Does(() => 
   {
      PublishProjectToRuntime(adminClientProjectDir, "win-x64");
   });

Task("PublishAdminClientToMacOs")
   .Does(() => 
   {
      PublishProjectToRuntime(adminClientProjectDir, "osx-x64");
   });

Task("PublishAdminClient")
   .IsDependentOn("PublishAdminClientCrossPlatform")
   .IsDependentOn("PublishAdminClientToLinux")
   .IsDependentOn("PublishAdminClientToWindows")
   .IsDependentOn("PublishAdminClientToMacOs");   

#endregion

#region Publish UserClient

Task("PublishUserClientCrossPlatform")
   .Does(() => 
   {
      PublishProjectCrossPlatform(userClientProjectDir);
   });

Task("PublishUserClientToLinux")
   .Does(() => 
   {
      PublishProjectToRuntime(userClientProjectDir, "linux-x64");
   });

Task("PublishUserClientToWindows")
   .Does(() => 
   {
      PublishProjectToRuntime(userClientProjectDir, "win-x64");
   });

Task("PublishUserClientToMacOs")
   .Does(() => 
   {
      PublishProjectToRuntime(userClientProjectDir, "osx-x64");
   });

Task("PublishUserClient")
   .IsDependentOn("PublishUserClientCrossPlatform")
   .IsDependentOn("PublishUserClientToLinux")
   .IsDependentOn("PublishUserClientToWindows")
   .IsDependentOn("PublishUserClientToMacOs");

#endregion

#region Publish All

Task("PublishCrossPlatform")
   .IsDependentOn("PublishServerCrossPlatform")
   .IsDependentOn("PublishAdminClientCrossPlatform")
   .IsDependentOn("PublishUserClientCrossPlatform");

Task("PublishToLinux")
   .IsDependentOn("PublishServerToLinux")
   .IsDependentOn("PublishAdminClientToLinux")
   .IsDependentOn("PublishUserClientToLinux");

Task("PublishToWindows")
   .IsDependentOn("PublishServerToWindows")
   .IsDependentOn("PublishAdminClientToWindows")
   .IsDependentOn("PublishUserClientToWindows");

Task("PublishToMacOs")
   .IsDependentOn("PublishServerToMacOs")
   .IsDependentOn("PublishAdminClientToMacOs")
   .IsDependentOn("PublishUserClientToMacOs");

Task("PublishAll")
   .IsDependentOn("PublishCrossPlatform")
   .IsDependentOn("PublishToLinux")
   .IsDependentOn("PublishToWindows")
   .IsDependentOn("PublishToMacOs");   

Task("CleanAndPublishAll")
   .IsDependentOn("Clean")
   .IsDependentOn("PublishAll");

#endregion

RunTarget(target);