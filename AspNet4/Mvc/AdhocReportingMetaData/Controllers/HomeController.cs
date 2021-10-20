using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EqDemo.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        [Authorize]
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}