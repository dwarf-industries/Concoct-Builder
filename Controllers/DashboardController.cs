using Concoct_Builder.Datalayer;
using Concoct_Builder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Concoct_Builder.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FileHandler handler;

        public DashboardController(ILogger<HomeController> logger, IFileHandler fileHandler)
        {
            _logger = logger;
            handler = (FileHandler)fileHandler;
        }
        public IActionResult Index()
        {
            ViewData["Organizations"] = handler.GetAllOrganizations();
            ViewData["Components"] = GetWorkItemComponents();
            return View();
        }

        private List<Widget> GetWorkItemComponents()
        {
            return handler.GetAllWidgets();
        }

    }
}
