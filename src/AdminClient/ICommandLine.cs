using System.Threading.Tasks;

namespace AdminClient;

/// <summary>
/// An interface that will run the admin client based on passed in command line arguments
/// </summary>
internal interface ICommandLine
{
  /// <summary>
  /// Parses the passed in command line arguments and runs the admin client
  /// </summary>
  /// <param name="args"></param>
  /// <returns>A task that runs the admin client operation</returns>
  public Task RunAsync(string[] args);
}