﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace Tasker.WebApp.Jobs
{
    public class PingExternalDependencyJob : Job
    {
        private readonly ILogger<CheckSystemHealthJob> _logger;
        public PingExternalDependencyJob(IServiceProvider sp)
        {
            _logger = sp.GetService<ILogger<CheckSystemHealthJob>>();
        }

        public async override void Execute(object payload)
        {
            if (this.Payload != null && string.IsNullOrWhiteSpace(this.Payload.ToString()))
                throw new ArgumentNullException(nameof(payload), $"service url to be pinged must be passed as a string argument inside the Job payload property");

            var client = new HttpClient();
            var response = await client.GetAsync(this.Payload.ToString());

            if(response.IsSuccessStatusCode)
                _logger.LogInformation($"Ping to service '{this.Payload.ToString()}' is SUCCESSFUL");
            else
                _logger.LogInformation($"Ping to service '{this.Payload.ToString()}' is Failed");

        }
    }
}
