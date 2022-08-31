using System;
using System.Threading;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.SignalR.Client;

namespace UserClient;

/// <summary>
/// An IMiddleware implementation that handles server commands by delegating them to an IClient implementation
/// </summary>
internal sealed class Middleware : IMiddleware
{
  private const int ReconnectDelaySeconds = 5;
  private readonly IClient _client;

  /// <summary>
  /// Initializes this middleware by using the given IClient dependency
  /// </summary>
  /// <param name="client">IClient dependency</param>
  public Middleware(IClient client)
  {
    _client = client;
  }

  public async Task RunAsync(string serverUrl)
  {
    HubConnection connection = BuildConnection(serverUrl);
    AddEventHandlers(connection);
    await ConnectWithRetryAsync(connection);
    await Task.Delay(Timeout.InfiniteTimeSpan);
  }

  #region connection helper methods

  private static HubConnection BuildConnection(string serverUrl)
  {
    HubConnection connection = new HubConnectionBuilder()
      .WithUrl($"{serverUrl}/hub")
      .Build();

    return connection;
  }

  private async Task ConnectWithRetryAsync(HubConnection connection)
  {
    while (true)
    {
      try
      {
        await Console.Out.WriteLineAsync();
        await Console.Out.WriteLineAsync("Trying to connect to server...");

        await connection.StartAsync();
        UserInfo userInfo = await _client.GetUserInfoAsync();
        await connection.InvokeAsync("IdentifyAsUser", userInfo);

        await Console.Out.WriteLineAsync("Successfully connected to server.");
        return;
      }
      catch (Exception ex)
      {
        await Console.Error.WriteLineAsync("Failed to connect to server.");
        await Console.Error.WriteLineAsync($"Exception: {ex.Message}");
        await Console.Out.WriteLineAsync($"Retrying to connect in {ReconnectDelaySeconds} seconds...");
        await Console.Out.WriteLineAsync();
        await Task.Delay(TimeSpan.FromSeconds(ReconnectDelaySeconds));
      }
    }
  }

  private void AddEventHandlers(HubConnection connection)
  {
    connection.Closed += async exception => await HandleConnectionClosed(connection, exception);
    connection.On<string>("ReceiveExecuteCommandRequest",
      async (command) => await ReceiveExecuteCommandRequest(connection, command));
  }

  #endregion

  #region event handlers

  private async Task HandleConnectionClosed(HubConnection connection, Exception? ex)
  {
    await Console.Error.WriteLineAsync("Connection closed.");
    await Console.Error.WriteLineAsync($"Exception: {ex?.Message ?? "N/A"}");
    await Console.Out.WriteLineAsync($"Retrying to connect in {ReconnectDelaySeconds} seconds...");

    await Task.Delay(TimeSpan.FromSeconds(ReconnectDelaySeconds));
    await ConnectWithRetryAsync(connection);
  }

  private async Task ReceiveExecuteCommandRequest(HubConnection connection, string command)
  {
    string output = await _client.ExecuteCommandAsync(command);
    await connection.InvokeAsync("SendExecuteCommandResponse", output);
  }

  #endregion
}