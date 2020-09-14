using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AdventureWorks.Mvc.Controllers
{
    public class PurchaseOrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}