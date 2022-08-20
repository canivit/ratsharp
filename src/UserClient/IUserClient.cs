using System.Threading.Tasks;
using Data;

namespace UserClient;

/// <summary>
/// Represents operations that a UserClient must support
/// </summary>
internal interface IUserClient
{
  /// <summary>
  /// Gets the information of this UserClient
  /// </summary>
  /// <returns>This UserClient's info</returns>
  public Task<UserInfo> GetUserInfo();

  /// <summary>
  /// Executes the given command on the running operating system as a new process and returns
  /// back the standard output and standard error of the process
  /// </summary>
  /// <param name="command">The command to execute</param>
  /// <returns>A task that will be resolved into the standard output
  /// and standard error of the running process</returns>
  public Task<string> ExecuteCommand(string command);
}