using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Concoct_Builder.Controllers
{
    public class RunLayoutController : Controller
    {
        public IActionResult Index(string Id)
        {
            return View();
        }
    }
}
