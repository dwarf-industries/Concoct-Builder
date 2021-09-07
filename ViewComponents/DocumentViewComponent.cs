namespace Concoct_Builder.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;

    public class DocumentViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
