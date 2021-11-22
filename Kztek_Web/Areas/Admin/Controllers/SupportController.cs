using Kztek_Core.Models;
using Kztek_Library.Configs;
using Kztek_Library.Models;
using Kztek_Model.Models;
using Kztek_Service.Admin;
using Kztek_Web.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kztek_Web.Areas.Admin.Controllers
{
    [Area(AreaConfig.Admin)]
    public class SupportController : Controller
    {
        private ItblSystemConfigService _tblSystemConfigService;
        private IMenuFunctionService _MenuFunctionService;
        private IMenuFunctionConfigService _MenuFunctionConfigService;

        public SupportController(ItblSystemConfigService _tblSystemConfigService, IMenuFunctionService _MenuFunctionService, IMenuFunctionConfigService _MenuFunctionConfigService)
        {
            this._tblSystemConfigService = _tblSystemConfigService;
            this._MenuFunctionService = _MenuFunctionService;
            this._MenuFunctionConfigService = _MenuFunctionConfigService;
        }

        [CheckSessionCookie(AreaConfig.Admin, true)]
        public async Task<IActionResult> Index()
        {
            var model = new AdminViewModel()
            {
                Config = await _tblSystemConfigService.GetDefault(),
                Menus = await _MenuFunctionService.GetAll(),
                Selecteds = await _MenuFunctionConfigService.GetAll(),
                Childs = new List<Kztek_Model.Models.MenuFunction>()
            };

            return View(model);
        }

        [CheckSessionCookie(AreaConfig.Admin, true)]
        public async Task<IActionResult> saveNewConfig(List<string> str)
        {
            var result = new MessageReport();

            try
            {
                if (str.Any())
                {
                    await _MenuFunctionConfigService.DeleteAll();

                    foreach (var item in str)
                    {
                        var obj = new MenuFunctionConfig()
                        {
                            Id = Guid.NewGuid().ToString(),
                            MenuFunctionId = item
                        };

                        await _MenuFunctionConfigService.Create(obj);
                    }
                }

                result.isSuccess = true;
                result.Message = "Cập nhật thành công";
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.Message = ex.Message;
            }

            return Json(await Task.FromResult(result));
        }

        /// <summary>
        /// kích hoạt/hủy phân quyền nhóm thẻ
        /// </summary>
        /// <param name="isAuthInView"></param>
        /// <returns></returns>
        [CheckSessionCookie(AreaConfig.Admin, true)]
        public async Task<IActionResult> AuthInView(bool isAuthInView)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            var objsystem = await _tblSystemConfigService.GetDefault();

            if (objsystem != null)
            {
                objsystem.isAuthInView = isAuthInView;
                result = await _tblSystemConfigService.Update(objsystem);

                if (result.isSuccess)
                {
                    result = new MessageReport(true, objsystem.isAuthInView ? "Đã kích hoạt tính năng phân quyền nhóm thẻ" : "Đã hủy tính năng phân quyền nhóm thẻ");
                }
            }

            return Json(await Task.FromResult(result));
        }

        [CheckSessionCookie(AreaConfig.Admin,true)]
        public async Task<IActionResult> AutoCapture(bool isAutoCapture)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            var objsystem = await _tblSystemConfigService.GetDefault();

            if (objsystem != null)
            {
                objsystem.IsAutoCapture = isAutoCapture;
                result = await _tblSystemConfigService.Update(objsystem);

                if (result.isSuccess)
                {
                    result = new MessageReport(true, objsystem.IsAutoCapture ? "Đã kích hoạt tính năng chụp ảnh tự động" : "Đã hủy tính năng chụp ảnh tự động");
                }
            }

            return Json(await Task.FromResult(result));
        }
    }
}
