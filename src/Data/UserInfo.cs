using System.Text.Json.Serialization;

namespace Data;

/// <summary>
/// Modal class that represents the information of a UserClient
/// </summary>
public sealed class UserInfo
{
  [JsonInclude] public string RemoteIp { get; }
  [JsonInclude] public string Country { get; }
  [JsonInclude] public string OperatingSystem { get; }
  [JsonInclude] public string Hostname { get; }
  [JsonInclude] public string Username { get; }

  public UserInfo(string remoteIp, string country, string operatingSystem, string hostname, string username)
  {
    RemoteIp = remoteIp;
    Country = country;
    OperatingSystem = operatingSystem;
    Hostname = hostname;
    Username = username;
  }
}