using System.Text.Json.Serialization;

namespace Data;

/// <summary>
/// Modal class that represents the information of a UserClient and its associated UserId
/// </summary>
public sealed class UserInfoWithId
{
  [JsonInclude] public string UserId { get; }
  [JsonInclude] public string RemoteIp { get; }
  [JsonInclude] public string Country { get; }
  [JsonInclude] public string OperatingSystem { get; }
  [JsonInclude] public string Hostname { get; }
  [JsonInclude] public string Username { get; }

  public UserInfoWithId(string userId, string remoteIp, string country, string operatingSystem, string hostname,
    string username)
  {
    UserId = userId;
    RemoteIp = remoteIp;
    Country = country;
    OperatingSystem = operatingSystem;
    Hostname = hostname;
    Username = username;
  }
}