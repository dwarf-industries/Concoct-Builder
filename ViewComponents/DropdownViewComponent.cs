
using Microsoft.AspNetCore.Mvc;

namespace Concoct_Builder.ViewComponents 
{
    public class DropdownViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
