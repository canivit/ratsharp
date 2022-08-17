namespace Data;

/// <summary>
/// Modal class that represents the information of a UserClient and its associated UserId
/// </summary>
public readonly struct UserInfoWithId
{
  public string UserId { get; }
  public string LocalIp { get; }
  public string RemoteIp { get; }
  public string Country { get; }
  public string OperatingSystem { get; }
  public string Username { get; }

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