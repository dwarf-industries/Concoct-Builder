using Microsoft.AspNetCore.Mvc;

namespace Concoct_Builder.ViewComponents
{
    public class BadgeViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
