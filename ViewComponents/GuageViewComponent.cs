namespace Concoct_Builder.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;

    public class GuageViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
