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
    public class RunLayoutController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FileHandler handler;


        public RunLayoutController(ILogger<HomeController> logger,IFileHandler fileHandler)
        {
            _logger = logger;
            handler = (FileHandler)fileHandler;
        }

        public IActionResult Index(string Id)
        {
            ViewData["Components"] = handler.GetAllWidgets();  
            ViewData["SelectedFile"] = Id;
            return View();
        }

        public List<PageElement> GetContent(string contentLayoutPath)
        {
            if(!System.IO.File.Exists(contentLayoutPath))
                return null;

            return handler.ReadFile(contentLayoutPath);
         }
    }
}
