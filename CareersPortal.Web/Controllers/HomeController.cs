using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CareersPortal.Web.controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            this._configuration = configuration;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            string loggedUser = _configuration.GetValue<string>("authClientId");
            ViewBag.loggedUser = loggedUser;
            _logger.LogWarning("I am warning");
            _logger.LogError("I am Error");
            return View();
        }
    }
}
