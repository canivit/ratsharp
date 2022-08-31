using UserClient;

IClient client = new Client();
IMiddleware middleware = new Middleware(client);
await middleware.RunAsync("http://localhost:8080");
// var userInfo = await client.GetUserInfoAsync();
// Console.WriteLine($"RemoteIp: {userInfo.RemoteIp}");
// Console.WriteLine($"Country: {userInfo.Country}");
// Console.WriteLine($"OS: {userInfo.OperatingSystem}");
// Console.WriteLine($"Hostname: {userInfo.Hostname}");
// Console.WriteLine($"Username: {userInfo.Username}");