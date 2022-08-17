namespace Data;

/// <summary>
/// Modal class that represents the information of a UserClient
/// </summary>
public readonly struct UserInfo
{
  public string LocalIp { get; }
  public string RemoteIp { get; }
  public string Country { get; }
  public string OperatingSystem { get; }
  public string Username { get; }

  public UserInfo(string localIp, string remoteIp, string country, string operatingSystem, string username)
  {
    LocalIp = localIp;
    RemoteIp = remoteIp;
    Country = country;
    OperatingSystem = operatingSystem;
    Username = username;
  }
}