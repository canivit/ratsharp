var target = Argument("target", "PublishAll");
var configuration = Argument("configuration", "Release");
var solutionDir = "./";
var serverProjectDir = "./Server";
var adminClientProjectDir = "./AdminClient";
var userClientProjectDir = "./UserClient";
var publishDir = "./out";

#region Helper methods

public void RestoreProject(string runtime) 
{
   DotNetRestore(solutionDir, new DotNetRestoreSettings 
   {
      Runtime = runtime,
   });
}

public void PublishProject(string projectDir, string runtime)
{
   DotNetPublish(projectDir, new DotNetPublishSettings
   {
      NoRestore = true,
      Configuration = configuration,
      PublishSingleFile = true,
      PublishTrimmed = true,
      SelfContained = true,
      Runtime = runtime,
      OutputDirectory = $"{publishDir}/{runtime}",
   });
}

#endregion

#region Restore     

Task("RestoreForLinux")
   .Does(() => 
   {
      RestoreProject("linux-x64");
   });

Task("RestoreForWindows")
   .Does(() => 
   {
      RestoreProject("win-x64");
   });

Task("RestoreForMacOs")
   .Does(() => 
   {
      RestoreProject("osx-x64");
   });

#endregion

#region Publish Server

Task("PublishServerToLinux")
   .IsDependentOn("RestoreForLinux")
   .Does(() => 
   {
      PublishProject(serverProjectDir, "linux-x64");
   });

Task("PublishServerToWindows")
   .IsDependentOn("RestoreForWindows")
   .Does(() => 
   {
      PublishProject(serverProjectDir, "win-x64");
   });

Task("PublishServerToMacOs")
   .IsDependentOn("RestoreForMacOs")
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
   .IsDependentOn("RestoreForLinux")
   .Does(() => 
   {
      PublishProject(adminClientProjectDir, "linux-x64");
   });

Task("PublishAdminClientToWindows")
   .IsDependentOn("RestoreForWindows")
   .Does(() => 
   {
      PublishProject(adminClientProjectDir, "win-x64");
   });

Task("PublishAdminClientToMacOs")
   .IsDependentOn("RestoreForMacOs")
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
   .IsDependentOn("RestoreForLinux")
   .Does(() => 
   {
      PublishProject(userClientProjectDir, "linux-x64");
   });

Task("PublishUserClientToWindows")
   .IsDependentOn("RestoreForWindows")
   .Does(() => 
   {
      PublishProject(userClientProjectDir, "win-x64");
   });

Task("PublishUserClientToMacOs")
   .IsDependentOn("RestoreForMacOs")
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