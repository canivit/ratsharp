using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.EventStream;
using Data;

namespace UserClient;

internal sealed class UserClient : IUserClient
{
  private readonly HttpClient _httpClient;
  private const string IpLocationApi = "https://ipinfo.io";

  public UserClient()
  {
    _httpClient = new HttpClient();
  }

  #region public interface

  public async Task<UserInfo> GetUserInfo()
  {
    IpLocation ipLocation = await GetRemoteIpAndCountry();
    string remoteIp = ipLocation.Ip ?? string.Empty;
    string country = ipLocation.Country ?? string.Empty;
    string operatingSystem = GetOperatingSystem();
    string hostname = GetHostName();
    string username = GetUsername();
    return new UserInfo(remoteIp, country, operatingSystem, hostname, username);
  }

  public async Task<string> ExecuteCommand(string command)
  {
    List<string> cmdTokens = command.Split(Array.Empty<char>()).ToList();
    if (cmdTokens.Count == 0)
    {
      return string.Empty;
    }

    string target = cmdTokens.First();
    List<string> arguments = cmdTokens.GetRange(1, cmdTokens.Count - 1);

    var cmd = Cli.Wrap(target)
      .WithArguments(arguments)
      .WithWorkingDirectory(Directory.GetCurrentDirectory())
      .WithValidation(CommandResultValidation.None);

    var cmdOutput = new StringBuilder();
    await foreach (var cmdEvent in cmd.ListenAsync())
    {
      switch (cmdEvent)
      {
        case StandardOutputCommandEvent stdOut:
          cmdOutput.AppendLine(stdOut.Text);
          break;
        case StandardErrorCommandEvent stdErr:
          cmdOutput.AppendLine(stdErr.Text);
          break;
      }
    }

    return cmdOutput.ToString();
  }

  #endregion

  #region helper methods

  private async Task<IpLocation> GetRemoteIpAndCountry()
  {
    try
    {
      var request = new HttpRequestMessage(HttpMethod.Get, IpLocationApi);
      HttpResponseMessage response = await _httpClient.SendAsync(request);
      IpLocation? ipLocation = await response.Content.ReadFromJsonAsync<IpLocation>();
      return ipLocation ?? new IpLocation();
    }
    catch (HttpRequestException)
    {
      return new IpLocation();
    }
  }

  private static string GetOperatingSystem()
  {
    return System.Runtime.InteropServices.RuntimeInformation.OSDescription;
  }

  private static string GetUsername()
  {
    return Environment.UserName;
  }

  private static string GetHostName()
  {
    return Environment.MachineName;
  }

  #endregion

  #region helper classes

  private sealed class IpLocation
  {
    [JsonInclude] public string? Ip { get; init; }
    [JsonInclude] public string? Country { get; init; }
  }

  #endregion
}