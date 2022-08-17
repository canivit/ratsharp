using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Server;

internal static class UserRepository
{
  private static readonly Lazy<ConcurrentDictionary<string, string>> LazyInstance =
    new(() => new ConcurrentDictionary<string, string>());

  public static void AddUser(string userId, string userInfo)
  {
    LazyInstance.Value.AddOrUpdate(userId, _ => userInfo, (_, _) => userInfo);
  }

  public static void RemoveUser(string userId)
  {
    LazyInstance.Value.TryRemove(userId, out _);
  }

  public static string GetUserInfo(string userId)
  {
    return LazyInstance.Value[userId];
  }

  public static IEnumerable<string> GetAllUsersInfo()
  {
    return LazyInstance.Value.Values;
  }
}