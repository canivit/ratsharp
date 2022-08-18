using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using OneOf;
using OneOf.Types;

namespace Server;

internal static class UserRepository
{
  private static readonly Lazy<ConcurrentDictionary<string, string>> LazyInstance =
    new(() => new ConcurrentDictionary<string, string>());

  public static void AddOrUpdate(string userId, string userInfo)
  {
    LazyInstance.Value.AddOrUpdate(userId, _ => userInfo, (_, _) => userInfo);
  }

  public static void RemoveIfExists(string userId)
  {
    LazyInstance.Value.TryRemove(userId, out _);
  }

  public static OneOf<string, None> TryGetUserInfo(string userId)
  {
    var userInfo = LazyInstance.Value.GetValueOrDefault(userId);
    if (userInfo is null)
    {
      return new None();
    }

    return userInfo;
  }

  public static IEnumerable<string> GetAllUsersInfo()
  {
    return LazyInstance.Value.Values;
  }
}