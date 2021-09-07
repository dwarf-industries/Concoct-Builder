namespace Concoct_Builder.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;

    public class ElementViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke(string componentName)
        {
            ViewData["RenderComponent"] = componentName;
            return View();
        }
    }
}
