using Microsoft.AspNetCore.Mvc;

namespace Concoct_Builder.ViewComponents
{
    public class TabViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
