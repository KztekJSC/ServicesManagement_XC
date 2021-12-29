using Kztek_Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kztek_Web.Components.MultiselectColumn
{
    public class MultiselectColumnViewComponent : ViewComponent
    {
        private IHttpContextAccessor HttpContextAccessor;

        public MultiselectColumnViewComponent(IHttpContextAccessor HttpContextAccessor)
        {
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync(SelectListModel_Multi model)
        {
            return View(await Task.FromResult(model));
        }
    }
}
