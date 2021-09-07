namespace Concoct_Builder.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;

    public class PivotTableViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
