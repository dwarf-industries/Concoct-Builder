using Concoct_Builder.Datalayer;
using Concoct_Builder.Models;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
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
            Program.ActiveUserSetting = handler.GetActiveOrganization();
            ViewData["Organizations"] = handler.GetAllOrganizations(); 
            ViewData["Components"] = GetWorkItemComponents();
            return View();
        }

        public IActionResult Layouts(int id)
        {
            Program.ActiveUserSetting = id;
            ViewData["Layouts"] = handler.GetAllLayoutsByOrganization(id);
            ViewData["GetAllProjectsForOrganization"] = handler.GetAllProjetsForOrganization(id);
            ViewData["OrganizationTags"] = handler.GetAllOrganizationTags(id);
            ViewData["HideComponents"] = "none";
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

            var window = windows.FirstOrDefault();
            var checkState = window.IsMaximizedAsync().Result;
            if (!checkState)
                window.Maximize();
            else
            {
                window.SetSize(900, 900, true);
                window.Unmaximize();
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
            var context = new ConcoctbuilderDbContext();
            var activeSetting = context.UserSettings.FirstOrDefault(x => x.IsActive == 1);
            var result = context.Layouts.Where(x => x.UserSetting == activeSetting.Id && x.UserSetting == 0).Select(x => new IncomingFileRequest
            {
                Name = x.Name,
                Path = x.Name
            }).ToList();
            return result;
        }

        [HttpPost]
        public string ConvertTobase([FromBody] IncomingFileRequest file)
        {
            return handler.ConvertTobase64(file.Path);
        }

        [HttpPost]
        public string CCAuthenicationRequest([FromBody] AuthenicationRequest request)
        {
            var result = Get($"{request.Instance}/OutboundDetails/AuthenicateCB?key={request.Token}&&username={request.Username}&&password={request.Password}");
            if (result == "failed")
                return result;

            var parseToObject = JsonSerializer.Deserialize<IncomingServerResponse>(result);
            handler.SyncContentDownStream(parseToObject, request);
            return parseToObject.item1 ? "Success" : "failed";
        }

        [HttpPost]
        public bool SetLayoutActive([FromBody] IncomingIdRequest request)
        {
            var result = handler.SetLayoutActive(request.Id);
            return result;
        }

        public string Get(string uri)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (System.Exception)
            {
                return "failed";
            }
           
        }

        [HttpPost]
        public IActionResult InitScreen([FromBody]IncomingFileRequest request)
        {
            var options = new BrowserWindowOptions();
            options.AutoHideMenuBar = false;
            options.DarkTheme = true;
            options.Fullscreen = false;
            options.Title = $"Concoct Builder Compiled {request.Path}";
            options.TitleBarStyle = TitleBarStyle.defaultStyle;
            options.Closable = true;
            options.Modal = true;
            //options.Frame = false;
            Startup.CreateBrowserWindow($"http://localhost:8001/RunLayout?Id={request.Path}", options);
            return Ok();
        }


        [HttpPost]
        public bool UpdateCompiledView([FromBody] SaveFileRequest request)
        {
            handler.UpdateCompiled(request.File.Name, request.LayoutDetail);
            var setting = handler.GetSettingByLayout(request.File.Name);
            var data = handler.PushUpStream((int)setting.Id);
            SyncData(data, setting.Endpoint);
            return true;
        }

        internal void SyncData(string currentData, string endpoint)
        {
            var request = (HttpWebRequest)WebRequest.Create($"{endpoint}/OutboundDetails/ConcoctBuilderSync");

      
            var data = Encoding.ASCII.GetBytes(currentData);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        }

        [HttpPost]
        public bool SaveFile([FromBody] SaveFileRequest request)
        {
            handler.WriteFile(request.File.Name, request.PageElements, request.LayoutDetail, int.Parse(request.ProjectId), int.Parse(request.WorkItemId));
            return true;
        }

        [HttpPost]
        public bool RemoveItemFromLayout([FromBody] RemoveRequest request)
        {
            handler.RemoveLayoutFile(request.Layout, request.File);
            return true;
        }

        
    }
}
