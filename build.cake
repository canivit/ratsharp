var target = Argument("target", "PublishAll");
var configuration = Argument("configuration", "Release");

var solutionDir = "./";
var serverProjectDir = "./Server";
var adminClientProjectDir = "./AdminClient";
var userClientProjectDir = "./UserClient";
var publishDir = "./out";

#region Helper methods

public void PublishProject(string projectDir, string runtime)
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

#endregion

#region Publish Server

Task("PublishServerToLinux")
   .Does(() => 
   {
      PublishProject(serverProjectDir, "linux-x64");
   });

Task("PublishServerToWindows")
   .Does(() => 
   {
      PublishProject(serverProjectDir, "win-x64");
   });

Task("PublishServerToMacOs")
   .Does(() => 
   {
      PublishProject(serverProjectDir, "osx-x64");
   });

Task("PublishServer")
   .IsDependentOn("PublishServerToLinux")
   .IsDependentOn("PublishServerToWindows")
   .IsDependentOn("PublishServerToMacOs");

#endregion

#region Publish AdminClient

Task("PublishAdminClientToLinux")
   .Does(() => 
   {
      PublishProject(adminClientProjectDir, "linux-x64");
   });

Task("PublishAdminClientToWindows")
   .Does(() => 
   {
      PublishProject(adminClientProjectDir, "win-x64");
   });

Task("PublishAdminClientToMacOs")
   .Does(() => 
   {
      PublishProject(adminClientProjectDir, "osx-x64");
   });

Task("PublishAdminClient")
   .IsDependentOn("PublishAdminClientToLinux")
   .IsDependentOn("PublishAdminClientToWindows")
   .IsDependentOn("PublishAdminClientToMacOs");

#endregion

#region Publish UserClient

Task("PublishUserClientToLinux")
   .Does(() => 
   {
      PublishProject(userClientProjectDir, "linux-x64");
   });

Task("PublishUserClientToWindows")
   .Does(() => 
   {
      PublishProject(userClientProjectDir, "win-x64");
   });

Task("PublishUserClientToMacOs")
   .Does(() => 
   {
      PublishProject(userClientProjectDir, "osx-x64");
   });

Task("PublishUserClient")
   .IsDependentOn("PublishUserClientToLinux")
   .IsDependentOn("PublishUserClientToWindows")
   .IsDependentOn("PublishUserClientToMacOs");

#endregion

#region Publish All

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
   .IsDependentOn("PublishToLinux")
   .IsDependentOn("PublishToWindows")
   .IsDependentOn("PublishToMacOs");

#endregion

RunTarget(target);