using Kztek_Library.Helpers;
using Kztek_Library.Models;
using Kztek_Service.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kztek_Web.Components.ImageFPT
{
    public class ImageFPTViewComponent : ViewComponent
    {
        private IHttpContextAccessor HttpContextAccessor;
        private ItblSystemConfigService _tblSystemConfigService;
        public ImageFPTViewComponent(IHttpContextAccessor HttpContextAccessor, ItblSystemConfigService _tblSystemConfigService)
        {
            this.HttpContextAccessor = HttpContextAccessor;
            this._tblSystemConfigService = _tblSystemConfigService;
        }
        public async Task<IViewComponentResult> InvokeAsync(ImageFPTModel model)
        {
            var config = await _tblSystemConfigService.GetDefault();

            if (model.Filename.Contains("bienso"))
            {
                model.Type = "HOAPHAT";
            }

            model.Image = config.ImagePath + model.Filename;/* await FunctionHelper.FtpImage(model.Filename);*/

            return View(await Task.FromResult(model));
        }
    }
}
