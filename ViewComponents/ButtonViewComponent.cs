namespace Concoct_Builder.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;

    public class ButtonViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
