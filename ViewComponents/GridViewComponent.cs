namespace Concoct_Builder.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;

    public class GridViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
