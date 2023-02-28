using Microsoft.AspNetCore.Mvc;
using MyNoteSampleApp.Business;

namespace MyNoteSampleApp.ViewComponents
{
    //[ViewComponent(Name ="categories-menu-list")]
    public class CategoryMenuViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(bool useBadge)
        {
            CategoryService categoryService = new CategoryService();
            if (useBadge)
            {
                return View("BadgeList", categoryService.List().Data);

            }
            else
            {             
                return View("Default",categoryService.List().Data);
            }


        }
    }
}
