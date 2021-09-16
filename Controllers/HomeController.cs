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
            return new List<Widget>{
                new Widget
                {
                    ComponentName = "AreaChart",
                    DisplayName = "Area Chart",
                    Icon = "fa-area-chart"
                },
                new Widget
                {
                    ComponentName = "BarChart",
                    DisplayName = "Bar Chart",
                    Icon = "fa-bar-chart"
                },
                new Widget
                {
                    ComponentName = "Barcode",
                    DisplayName = "Barcode",
                    Icon = "fa-barcode"
                },
                new Widget
                {
                    ComponentName = "Button",
                    DisplayName = "Button",
                    Icon = "fa-caret-square-o-right"
                },
                new Widget
                {
                    ComponentName = "Calendar",
                    DisplayName = "Calendar",
                    Icon = "fa-calendar"
                },
                new Widget
                {
                    ComponentName = "Chip",
                    DisplayName = "Chip",
                    Icon = "Example of id-badge fa-id-badge"
                },
                new Widget
                {
                    ComponentName = "DateRangePicker",
                    DisplayName = "Date Range Picker",
                    Icon = "fa-calendar-o"
                },
                new Widget
                {
                    ComponentName = "Diagram",
                    DisplayName = "Diagram",
                    Icon = "fa-paint-brush"
                },
                new Widget
                {
                    ComponentName = "Document",
                    DisplayName = "Document",
                    Icon = "fa-file"
                },
                new Widget
                {
                    ComponentName = "Grantt",
                    DisplayName = "Grantt Chart",
                    Icon = "fa-clipboard"
                },
                new Widget
                {
                    ComponentName = "Grid",
                    DisplayName = "Grid",
                    Icon = "fa-th"
                },
                new Widget
                {
                    ComponentName = "Guage",
                    DisplayName = "Guage",
                    Icon = "fa-circle-o-notch"
                },
                new Widget
                {
                    ComponentName = "HeatmapCalendar",
                    DisplayName = "Heatmap Calendar",
                    Icon = "fa-globe"
                },
                new Widget
                {
                    ComponentName = "Kanban",
                    DisplayName = "Kanban Board",
                    Icon = "fa-graduation-cap"
                },
                new Widget
                {
                    ComponentName = "LinearGuage",
                    DisplayName = "Linear Guage",
                    Icon = "fa-linode"
                },
                new Widget
                {
                    ComponentName = "LineChart",
                    DisplayName = "Line Chart",
                    Icon = "fa-line-chart"
                },
                new Widget
                {
                    ComponentName = "Map",
                    DisplayName = "Map",
                    Icon = "fa-map"
                },
                new Widget
                {
                    ComponentName = "PivotTable",
                    DisplayName = "Pivot Table",
                    Icon = "fa-table"
                },
                new Widget
                {
                    ComponentName = "ScatterChart",
                    DisplayName = "SCatter Chart",
                    Icon = "fa-circle"
                },
                new Widget
                {
                    ComponentName = "Scheduler",
                    DisplayName = "Scheduler",
                    Icon = "fa-calendar"
                },
                new Widget
                {
                    ComponentName = "Spreadsheet",
                    DisplayName = "Spreadsheet",
                    Icon = "fa-file-excel-o"
                },
                new Widget
                {
                    ComponentName = "StockChart",
                    DisplayName = "Stock Chart",
                    Icon = "fa-money"
                },
                new Widget
                {
                    ComponentName = "Temperature",
                    DisplayName = "Temperature Guage",
                    Icon = "fa-thermometer-empty"
                },
                new Widget
                {
                    ComponentName = "TreeGrid",
                    DisplayName = "Tree Grid",
                    Icon = "fa-th-large"
                },
                new Widget
                {
                   ComponentName = "TreeMap",
                   DisplayName ="Tree Map",
                   Icon = "fa-map-marker"
                },
                new Widget
                {
                   ComponentName = "Placeholder",
                   DisplayName = "Placeholder (Image/Custom)",
                   Icon = "fa-calendar-o"
                },
                new Widget
                {
                   ComponentName = "InPlaceEditor",
                   DisplayName = "In Place Editor",
                   Icon = "fa-edit"
                },
                new Widget
                {
                   ComponentName = "TimePicker",
                   DisplayName = "Time Picker",
                   Icon = "fa-history"
                },
                new Widget
                {
                   ComponentName = "Dropdown",
                   DisplayName = "Dropdown",
                   Icon = "fa-th-list"
                },
                new Widget
                {
                   ComponentName = "MultiSelect",
                   DisplayName = "Multi SElect",
                   Icon = "fa-list-alt"
                },
                new Widget
                {
                   ComponentName = "ListBox",
                   DisplayName = "List Box",
                   Icon = "fa-list-alt"
                },
                new Widget
                {
                   ComponentName = "Accordion",
                   DisplayName = "Accordion",
                   Icon = "fa-list-alt"
                },
                 new Widget
                {
                   ComponentName = "MenuBar",
                   DisplayName = "Menu Bar",
                   Icon = "fa-list-alt"
                },
                new Widget
                {
                   ComponentName = "Tab",
                   DisplayName = "Tab",
                   Icon = "fa-list-alt"
                },
                new Widget
                {
                   ComponentName = "FileExplorer",
                   DisplayName = "File Explorer",
                   Icon = "fa-list-alt"
                },
                new Widget
                {
                   ComponentName = "Badge",
                   DisplayName = "Badge",
                   Icon = "fa-list-alt"
                },
                 new Widget
                {
                   ComponentName = "Toast",
                   DisplayName = "Toast",
                   Icon = "fa-list-alt"
                },
                new Widget
                {
                   ComponentName = "FileUploader",
                   DisplayName = "File Uploader",
                   Icon = "fa-list-alt"
                },
                 new Widget
                {
                   ComponentName = "ColorPicker",
                   DisplayName = "Color Picker",
                   Icon = "fa-list-alt"
                },
                new Widget
                {
                   ComponentName = "Input",
                   DisplayName = "Text Input",
                   Icon = "fa-list-alt"
                },
            }; 
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
        public void InitScreen(string request)
        {
            var options = new BrowserWindowOptions();
            options.AutoHideMenuBar = true;
            options.DarkTheme = true;
            options.Fullscreen = true;
            options.Title = "Concoct Builder V 0.1";
            options.TitleBarStyle = TitleBarStyle.hidden;
 
            Task.Run(async () => await Electron.WindowManager.CreateWindowAsync(options, "http://localhost/RunLayout?Id="+ request));
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
            UpdateDirectoryContent(request);
            return true;
        }

        private void UpdateDirectoryContent(SaveFileRequest request)
        {
            var fileContent = handler.ReadFileRaw(Startup.Settings.AssocaitedFileLocation);
            var fileData = handler.ReadDirectoryFile(fileContent);
            var exists = fileData.FirstOrDefault(x => x.Path == $"{request.File.Path}{Startup.Settings.SystemFolderDelimiter}{request.File.Name}"&& x.Name == request.File.Name);
            var currentData = string.Empty;
            if (exists == null)
            {
                if(!string.IsNullOrEmpty(Startup.Settings.AssocaitedFileLocation))
                {
                    currentData = handler.ReadFileRaw(Startup.Settings.AssocaitedFileLocation);
                    handler.DeleteFile(Startup.Settings.AssocaitedFileLocation);
                }
           
                var currentFileData = handler.SaveDirectoryFile(currentData, request.File);
                handler.CreateFile(Startup.Settings.AssocaitedFileLocation, currentFileData);
            }
             
        }
    }
}
