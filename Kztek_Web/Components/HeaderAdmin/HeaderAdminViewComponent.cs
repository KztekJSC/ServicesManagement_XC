using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kztek_Library.Helpers;
using Kztek_Library.Models;
using Kztek_Service.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Kztek_Web.Components.HeaderAdmin
{
    public class HeaderAdminViewComponent : ViewComponent
    {
        public HeaderAdminViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await SessionCookieHelper.CurrentAdmin(HttpContext);

            return View(user);
        }
    }
}