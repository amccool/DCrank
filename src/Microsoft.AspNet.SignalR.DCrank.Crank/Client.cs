﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace Microsoft.AspNet.SignalR.DCrank.Crank
{
    public class Client
    {
        private Connection _connection;

        public Action OnClosed;
        public Action<string> OnMessage;

        public async Task CreateConnection(CrankArguments arguments)
        {
            _connection = new Connection(arguments.Url + "TestConnection");

            _connection.Received += OnMessage;
            _connection.Closed += OnClosed;

            for (int connectCount = 0; connectCount < 3; connectCount++)
            {
                try
                {
                    if (arguments.Transport == null)
                    {
                        await _connection.Start();
                    }
                    else
                    {
                        await _connection.Start(arguments.GetTransport());
                    }

                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Connection.Start Failed: {0}: {1}", ex.GetType(), ex.Message);

                    if (connectCount == 3)
                    {
                        throw;
                    }
                }

                await Task.Delay(1000);
            }
        }

        public async void StartTest(CrankArguments arguments)
        {
            var payload = (arguments.SendBytes == 0) ? String.Empty : new string('a', arguments.SendBytes);

            if (!String.IsNullOrEmpty(payload))
            {
                Task.Run(async () =>
                {
                    while (_connection.State == ConnectionState.Connected)
                    {
                        await _connection.Send(payload);

                        await Task.Delay(arguments.SendInterval);
                    }
                });
            }
        }

        public void StopConnection()
        {
            _connection.Stop();
        }
    }
}