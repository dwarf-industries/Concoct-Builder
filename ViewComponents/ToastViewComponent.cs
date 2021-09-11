using Microsoft.AspNetCore.Mvc;

namespace Concoct_Builder.ViewComponents
{
    public class ToastViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
