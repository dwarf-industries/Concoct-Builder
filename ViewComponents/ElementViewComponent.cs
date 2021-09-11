namespace Concoct_Builder.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;

    public class ElementViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke(string componentName)
        {
            ViewData["RenderComponent"] = componentName;
            switch (componentName)
            {
                case "SystemSetting":
                    return View("SystemSetting");
                case "Flow":
                    return View("FlowDiagram");
                default:
                    return View();
             }
         }
    }
}
