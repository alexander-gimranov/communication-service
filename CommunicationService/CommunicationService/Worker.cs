using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CommunicationService
{
    public class Worker : BackgroundService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<Worker> _logger;
        private readonly DateTime _dateTime;

        public Worker(
            IConfiguration config
            ,ILogger<Worker> logger
        )
        {
            _config = config;
            _logger = logger;
            _dateTime = DateTime.UtcNow;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!int.TryParse(_config["pipe:threadscount"], out var threadCount)) threadCount = 5;
            var pipeName = _config["pipe:pipename"];

            while (!stoppingToken.IsCancellationRequested)
            {
                await CreatePipeServer(pipeName, threadCount, stoppingToken);
            }
        }

        private async Task CreatePipeServer(string pipeName, int threadCount, CancellationToken stoppingToken)
        {
            try
            {
                using (NamedPipeServerStream pipe = new NamedPipeServerStream(pipeName, PipeDirection.InOut, threadCount))
                {
                    _logger.LogInformation("Waiting for client connection...");
                    await pipe.WaitForConnectionAsync(stoppingToken);

                    _logger.LogInformation("Client connected.");

                    using (var writer = new StreamWriter(pipe))
                    {
                        var buf = BitConverter.GetBytes(_dateTime.Ticks);
                        await writer.WriteLineAsync(Convert.ToBase64String(buf));
                        writer.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
