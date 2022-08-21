using System.Threading.Tasks;

namespace UserClient;

/// <summary>
/// Represents a middleware that connects the user client to the RatSharp server.
/// This middleware's job is to handle the commands that the command RatSharp server sends.
/// </summary>
internal interface IMiddleware
{
  /// <summary>
  /// Starts the middleware
  /// </summary>
  /// <param name="serverUrl">The url of the RatSharp HTTP(S) server</param>
  /// <returns>A task that will be completed when the middleware stop running</returns>
  public Task RunAsync(string serverUrl);
}