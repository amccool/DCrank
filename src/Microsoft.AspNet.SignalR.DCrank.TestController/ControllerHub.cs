﻿
using System.Threading.Tasks;
namespace Microsoft.AspNet.SignalR.DCrank.TestController
{
    public class ControllerHub : Hub
    {
        private readonly string _uiGroup = "Dashboard";

        public override async Task OnConnected()
        {
            var isDashboard = Context.Request.QueryString["UI"] == "1";
            if (isDashboard)
            {
                await Groups.Add(Context.ConnectionId, _uiGroup);
            }
        }

        // A test to see if there is end to end communication - see if the int you send is actually received
        public void PingAgent(int value)
        {
            Clients.All.pingAgent(value);
        }

        public void PingWorker(string agentid, int workerId, int value)
        {
            Clients.Client(agentid).pingWorker(workerId, value);
        }

        public void PongAgent(int value)
        {
            Clients.All.agentPongResponse(Context.ConnectionId, value);
        }

        public void PongWorker(int workerId, int value)
        {
            Clients.All.workerPongResponse(Context.ConnectionId, workerId, value);
        }

        public void AgentHeartbeat(object heartbeatInformation)
        {
            Clients.All.agentConnected(Context.ConnectionId, heartbeatInformation);
        }

        public void StartWorker(string connectionId, int numberOfWorkersToStart)
        {
            Clients.Client(connectionId).startWorker(numberOfWorkersToStart);
        }

        public void KillWorker(string agentId, int workerId)
        {
            Clients.Client(agentId).killWorker(workerId);
        }

        public void LogAgent(string message)
        {
            Clients.All.agentsLog(Context.ConnectionId, message);
        }

        public void LogWorker(int workerId, string message)
        {
            Clients.All.workersLog(Context.ConnectionId, workerId, message);
        }
    }
}