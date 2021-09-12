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
                    return View("~/Views/Shared/Components/SystemSetting/Default.cshtml");
                case "FlowDiagram":
                    return View("~/Views/Shared/Components/FlowDiagram/Default.cshtml");
                default:
                    return View();
             }
         }
    }
}
