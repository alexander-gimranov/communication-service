using CommunicationUI.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.IO.Pipes;
using System.Security.Principal;
using System.Threading.Tasks;

namespace CommunicationUI
{
    public class CommunicationServiceProvider : ICommunicationServiceProvider
    {
        private readonly IConfiguration _config;
        private readonly ILogger<CommunicationServiceProvider> _logger;

        public CommunicationServiceProvider(
            IConfiguration config
            ,ILogger<CommunicationServiceProvider> logger
        )
        {
            _config = config;
            _logger = logger;
        }

        public async Task<DateTime> GetDateTime()
        {
            var host = _config["CommunicationService:host"];
            var pipeName = _config["CommunicationService:pipename"];
            var timeOut = int.TryParse(_config["CommunicationService:timeout"], out var time) ? time : 100;
            try
            {
                using (var client = new NamedPipeClientStream(host, pipeName, PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation))
                {
                    _logger.LogInformation("Try connect to Communication Service...");
                    await client.ConnectAsync(timeOut);
                    _logger.LogInformation("Connected to Communication Service");

                    using (var reader = new StreamReader(client))
                    {
                        var str = await reader.ReadLineAsync() ?? throw new Exception("Communication Service doesn't provide inforamtion");
                        var bytes = Convert.FromBase64String(str);
                        var ticks = BitConverter.ToInt64(bytes);
                        return new DateTime(ticks);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while receiving information from server: {@ex.Message}", ex);
                throw;
            }
        }
    }
}
