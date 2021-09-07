using Microsoft.AspNetCore.Mvc;

namespace Concoct_Builder.ViewComponents
{
    public class PlaceholderViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
