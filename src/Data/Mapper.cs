namespace Data;

public static class Mapper
{
  public static UserInfoWithId ToInfoWithId(this UserInfo userInfo, string userId)
  {
    return new UserInfoWithId(userId, userInfo.RemoteIp, userInfo.Country, userInfo.OperatingSystem, userInfo.Hostname,
      userInfo.Username);
  }
}