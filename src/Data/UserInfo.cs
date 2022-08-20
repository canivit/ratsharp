using System.Text.Json.Serialization;

namespace Data;

/// <summary>
/// Modal class that represents the information of a UserClient
/// </summary>
public readonly struct UserInfo
{
  [JsonInclude] public string LocalIp { get; }
  [JsonInclude] public string RemoteIp { get; }
  [JsonInclude] public string Country { get; }
  [JsonInclude] public string OperatingSystem { get; }
  [JsonInclude] public string Username { get; }

  public UserInfo(string localIp, string remoteIp, string country, string operatingSystem, string username)
  {
    LocalIp = localIp;
    RemoteIp = remoteIp;
    Country = country;
    OperatingSystem = operatingSystem;
    Username = username;
  }
}