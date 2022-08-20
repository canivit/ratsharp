using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.EventStream;
using Data;

namespace UserClient;

internal class UserClient : IUserClient
{
  public Task<UserInfo> GetUserInfo()
  {
    throw new NotImplementedException();
  }

  public async Task<string> ExecuteCommand(string command)
  {
    List<string> cmdTokens = command.Split(Array.Empty<char>()).ToList();
    if (cmdTokens.Count == 0)
    {
      return string.Empty;
    }

    string target = cmdTokens.First();
    List<string> arguments = cmdTokens.GetRange(1, cmdTokens.Count - 1);

    var cmd = Cli.Wrap(target)
      .WithArguments(arguments)
      .WithWorkingDirectory(Directory.GetCurrentDirectory())
      .WithValidation(CommandResultValidation.None);

    var cmdOutput = new StringBuilder();
    await foreach (var cmdEvent in cmd.ListenAsync())
    {
      switch (cmdEvent)
      {
        case StandardOutputCommandEvent stdOut:
          cmdOutput.AppendLine(stdOut.Text);
          break;
        case StandardErrorCommandEvent stdErr:
          cmdOutput.AppendLine(stdErr.Text);
          break;
      }
    }

    return cmdOutput.ToString();
  }
}