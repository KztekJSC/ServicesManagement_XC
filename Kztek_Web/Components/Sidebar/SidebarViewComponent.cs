using System.Linq;
using System.Threading.Tasks;
using Kztek_Library.Helpers;
using Kztek_Library.Models;
using Kztek_Service.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kztek_Web.Components.Sidebar
{
    public class SidebarViewComponent : ViewComponent
    {
        private IMenuFunctionService _MenuFunctionService;
        private IHttpContextAccessor HttpContextAccessor;

        public SidebarViewComponent(IMenuFunctionService _MenuFunctionService, IHttpContextAccessor HttpContextAccessor)
        {
            this._MenuFunctionService = _MenuFunctionService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync(string controllername, string actionname, string area = "")
        {
            //
            var user = await SessionCookieHelper.CurrentUser(HttpContextAccessor.HttpContext);

            var model = new SidebarModel();
            model.ControllerName = controllername;
            model.ActionName = actionname;

            var data = await _MenuFunctionService.GetAllActiveByUserId(HttpContextAccessor.HttpContext, user, area);

            model.Data = data.ToList();

            model.CurrentView = model.Data.FirstOrDefault(n => n.ControllerName.Equals(model.ControllerName) && n.ActionName.Equals(model.ActionName));

            model.Breadcrumb = await _MenuFunctionService.GetBreadcrumb(model.CurrentView != null ? model.CurrentView.Id : "", model.CurrentView != null ? model.CurrentView.ParentId : "", "");
            model.AreaCode = area;

            return View(model);
        }
    }
}