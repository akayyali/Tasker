using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Tasker.WebApp.Jobs;
using Tasker.WebApp.Models;
using Tasker.WebApp.SchedulerServices;

namespace Tasker.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationJobSchedulerService _applicationJobSchedulerService;
        private readonly PingExternalDependencyJob _pingExternalDependencyJob;

        public HomeController(ILogger<HomeController> logger, ApplicationJobSchedulerService applicationJobSchedulerService, PingExternalDependencyJob pingExternalDependencyJob)
        {
            _logger = logger;
            _applicationJobSchedulerService = applicationJobSchedulerService;
            _pingExternalDependencyJob = pingExternalDependencyJob;
        }

        public IActionResult Index()
        {
            _applicationJobSchedulerService.AddJob(_pingExternalDependencyJob);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
