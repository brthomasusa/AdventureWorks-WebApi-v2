using Microsoft.AspNetCore.Mvc;

namespace AdventureWorks.Mvc.Components
{
    public class NavMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            string[] menuItems = { "Home", "Department", "Employee", "Vendor" };

            ViewBag.SelectedMenuItem = RouteData?.Values["controller"];
            return View(menuItems);
        }
    }
}
