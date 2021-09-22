using Concoct_Builder.Datalayer;
using Concoct_Builder.Models;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Concoct_Builder.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FileHandler handler;

        public HomeController(ILogger<HomeController> logger, IFileHandler fileHandler)
        {
            _logger = logger;
            handler = (FileHandler)fileHandler;
        }

        public IActionResult Index()
        {

            ViewData["Components"] = GetWorkItemComponents();
            return View();
        }

        private List<Widget> GetWorkItemComponents()
        {
            return handler.GetAllWidgets();
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

        [HttpGet]
        public IActionResult GetComponent(string componentName)
        {
            return ViewComponent(componentName);
        }
        [HttpGet]
        public IActionResult GetComponentPanel(string componentName, string args)
        {
            return ViewComponent(componentName, args);
        }

        [HttpGet]
        public IActionResult CloseApp()
        {
            var windows = Electron.WindowManager.BrowserWindows;
            foreach (var window in windows)
            {
                window.Close();
            }
            return View();
        }

        [HttpGet]
        public IActionResult MinimizeApp()
        {
            var windows = Electron.WindowManager.BrowserWindows;
            foreach (var window in windows)
            {
                window.Minimize();
            }
            return View();
        }

        [HttpGet]
        public IActionResult FullScreen(bool condition)
        {
            var windows = Electron.WindowManager.BrowserWindows;
            foreach (var window in windows)
            {
                window.SetFullScreen(condition);
            }
            return View();
        }

        [HttpPost]
        public List<PageElement> LoadFromFile([FromBody]IncomingFileRequest file)
        {
            return handler.ReadFile(file.Path);
        }


        [HttpGet]
        public List<IncomingFileRequest> LoadLayouts()
        {
            var fileContent = handler.ReadFileRaw(Startup.Settings.AssocaitedFileLocation);
            return handler.ReadDirectoryFile(fileContent);
        }

        [HttpPost]
        public string ConvertTobase([FromBody] IncomingFileRequest file)
        {
            return handler.ConvertTobase64(file.Path);
        }

        [HttpPost]
        public IActionResult InitScreen([FromBody]IncomingFileRequest request)
        {
            var options = new BrowserWindowOptions();
            options.AutoHideMenuBar = true;
            options.DarkTheme = true;
            options.Fullscreen = true;
            options.Title = "Concoct Builder V 0.1";
            options.TitleBarStyle = TitleBarStyle.hidden;
 
            Task.Run(async () => await Electron.WindowManager.CreateWindowAsync(options, $"http://localhost:8001/RunLayout?Id={request.Path}"));
            return Ok();
        }

        [HttpPost]
        public bool SaveFile([FromBody] SaveFileRequest request)
        {

            // This text is added only once to the file.
            if (!System.IO.File.Exists($"{request.File.Path}{request.File.Name}"))
            {
                // Create a file to write to.
                handler.WriteFile($"{request.File.Path}{request.File.Name}", request.PageElements);
            }
            else
            {
                handler.DeleteFile($"{request.File.Path}{request.File.Name}");
                handler.WriteFile($"{request.File.Path}{request.File.Name}", request.PageElements);

                //System.IO.File.WriteAllText($"{request.File.Path}{request.File.Name}", Newtonsoft.Json.JsonConvert.SerializeObject(request.PageElements));
            }
            return true;
        }

        
    }
}
