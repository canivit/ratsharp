using System.Text.Json.Serialization;

namespace Data;

/// <summary>
/// Modal class that represents the information of a UserClient and its associated UserId
/// </summary>
public readonly struct UserInfoWithId
{
  [JsonInclude] public string UserId { get; }
  [JsonInclude] public string LocalIp { get; }
  [JsonInclude] public string RemoteIp { get; }
  [JsonInclude] public string Country { get; }
  [JsonInclude] public string OperatingSystem { get; }
  [JsonInclude] public string Username { get; }

  public UserInfoWithId(string userId, string localIp, string remoteIp, string country, string operatingSystem,
    string username)
  {
    UserId = userId;
    LocalIp = localIp;
    RemoteIp = remoteIp;
    Country = country;
    OperatingSystem = operatingSystem;
    Username = username;
  }
}