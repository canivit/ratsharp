using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Data;
using OneOf;
using OneOf.Types;

namespace Server;

internal static class UserRepository
{
  private static readonly Lazy<ConcurrentDictionary<string, UserInfo>> LazyInstance =
    new(() => new ConcurrentDictionary<string, UserInfo>());

  public static void AddOrUpdate(string userId, UserInfo userInfo)
  {
    LazyInstance.Value.AddOrUpdate(userId, _ => userInfo, (_, _) => userInfo);
  }

  public static void RemoveIfExists(string userId)
  {
    LazyInstance.Value.TryRemove(userId, out _);
  }

  public static OneOf<UserInfoWithId, None> TryGetUserInfo(string userId)
  {
    bool userExists = LazyInstance.Value.TryGetValue(userId, out var userInfo);
    if (userExists)
    {
      return userInfo.ToInfoWithId(userId);
    }

    return new None();
  }

  public static IEnumerable<UserInfoWithId> GetAllUsersInfo()
  {
    return LazyInstance.Value.Select(pair => pair.Value.ToInfoWithId(pair.Key));
  }
}