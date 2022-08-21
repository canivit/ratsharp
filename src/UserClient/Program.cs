using System;
using UserClient;

IUserClient userClient = new UserClient.UserClient();
var userInfo = await userClient.GetUserInfo();
Console.WriteLine($"RemoteIp: {userInfo.RemoteIp}");
Console.WriteLine($"Country: {userInfo.Country}");
Console.WriteLine($"OS: {userInfo.OperatingSystem}");
Console.WriteLine($"Hostname: {userInfo.Hostname}");
Console.WriteLine($"Username: {userInfo.Username}");