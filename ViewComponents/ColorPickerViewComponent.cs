using Microsoft.AspNetCore.Mvc;

namespace Concoct_Builder.ViewComponents
{
    public class ColorPickerViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
