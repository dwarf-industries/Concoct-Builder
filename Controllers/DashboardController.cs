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
        public IActionResult Index(string id)
        {
            ViewData["SelectedLayout"] = id;
            ViewData["Organizations"] = handler.GetAllOrganizations();
            ViewData["Components"] = GetWorkItemComponents();
            ViewData["HideComponents"] = "";
            var projects = handler.GetAllProjetsForOrganization(Program.ActiveUserSetting);
            ViewData["GetProjects"] = projects;
            if (projects != null)
            {
                ViewData["GetAllWorkItemsForProjects"] = handler.GetAllWorkItemsInProjects(projects.Select(x => x.Id).ToList());
            }
            else
            {
                ViewData["GetAllWorkItemsForProjects"] = null;
            }
            return View();
        }

        [HttpPost]
        public List<WorkItems> GetWorkItemsForProject([FromBody] IncomingIdRequest request)
        {
            return handler.GetAllWorkItemsInProjects(request.Id);
        }

        private List<Widget> GetWorkItemComponents()
        {
            return handler.GetAllWidgets();
        }

    }
}
