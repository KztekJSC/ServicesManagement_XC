using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kztek_Web.Models;
using Kztek_Service.Admin;
using Kztek_Model.Models;
using Kztek_Library.Helpers;
using Kztek_Web.Attributes;

namespace Kztek_Web.Controllers
{
    public class RoleController : Controller
    {
        private IRoleService _RoleService;
        private IMenuFunctionService _MenuFunctionService;

        public RoleController(IRoleService _RoleService, IMenuFunctionService _MenuFunctionService)
        {
            this._RoleService = _RoleService;
            this._MenuFunctionService = _MenuFunctionService;
        }

        [CheckSessionCookie]
        public async Task<IActionResult> Index(string AreaCode = "")
        {
            var list = await _RoleService.GetAll();

            ViewBag.AuthValue = await AuthHelper.CheckAuthAction("Role", this.HttpContext);
            ViewBag.AreaCodeValue = AreaCode;

            return View(list.ToList());
        }

        [CheckSessionCookie]
        [HttpGet]
        public async Task<IActionResult> Create(Role_Submit model, string AreaCode = "")
        {
            model = model == null ? new Role_Submit() : model;
            model.Data_Tree = await _MenuFunctionService.GetAllActive(AreaCode);
            ViewBag.AreaCodeValue = AreaCode;

            return View(model);
        }

        [CheckSessionCookie]
        [HttpPost]
        public async Task<IActionResult> Create(Role_Submit model, bool SaveAndCountinue = false, string AreaCode = "")
        {
            model.Data_Tree = await _MenuFunctionService.GetAllActive(AreaCode);
            ViewBag.AreaCodeValue = AreaCode;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var obj = new Role()
            {
                RoleName = model.RoleName,
                Description = model.Description,
                Active = model.Active,
                Id = Guid.NewGuid().ToString()
            };

            if (!string.IsNullOrWhiteSpace(model.MenuFunctionIds))
            {
                var ks = model.MenuFunctionIds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                model.MenuFunctions = new List<string>();
                foreach (var item in ks)
                {
                    model.MenuFunctions.Add(item);
                }

                foreach (var item in model.MenuFunctions)
                {
                    var t = new RoleMenu()
                    {
                        Id = Guid.NewGuid().ToString(),
                        MenuId = item,
                        RoleId = obj.Id
                    };

                    await _MenuFunctionService.CreateMap(t);
                }
            }

            //Thực hiện thêm mới
            var result = await _RoleService.Create(obj);
            if (result.isSuccess)
            {
                if (SaveAndCountinue)
                {
                    TempData["Success"] = "Thêm mới thành công";
                    return RedirectToAction("Create", new { AreaCode = AreaCode });
                }

                return RedirectToAction("Index", new { AreaCode = AreaCode });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(obj);
            }
        }

        [CheckSessionCookie]
        [HttpGet]
        public async Task<IActionResult> Update(string id, string AreaCode = "")
        {
            var model = _RoleService.GetCustomById(id);
            model.Data_Tree = await _MenuFunctionService.GetAllActive(AreaCode);
            ViewBag.AreaCodeValue = AreaCode;

            return View(model);
        }

        [CheckSessionCookie]
        [HttpPost]
        public async Task<IActionResult> Update(Role_Submit model, string AreaCode = "")
        {
            model.Data_Tree = await _MenuFunctionService.GetAllActive(AreaCode);
            ViewBag.AreaCodeValue = AreaCode;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var oldObj = await _RoleService.GetById(model.Id);
            if (oldObj == null)
            {
                ModelState.AddModelError("", "Bản ghi không tồn tại");
                return View(model);
            }

            oldObj.Active = model.Active;
            oldObj.Description = model.Description;
            oldObj.RoleName = model.RoleName;

            //Cập nhật lại menu
            await _MenuFunctionService.DeleteMap(oldObj.Id, AreaCode);

            if (!string.IsNullOrWhiteSpace(model.MenuFunctionIds))
            {
                var ks = model.MenuFunctionIds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                model.MenuFunctions = new List<string>();

                foreach (var item in ks)
                {
                    model.MenuFunctions.Add(item);
                }

                foreach (var item in model.MenuFunctions)
                {
                    var t = new RoleMenu()
                    {
                        Id = Guid.NewGuid().ToString(),
                        MenuId = item,
                        RoleId = oldObj.Id
                    };

                    await _MenuFunctionService.CreateMap(t);
                }
            }

            var result = await _RoleService.Update(oldObj);
            if (result.isSuccess)
            {
                return RedirectToAction("Index", new { AreaCode = AreaCode });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }
        }

        [CheckSessionCookie]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _RoleService.Delete(id);

            return Json(result);
        }
    }
}
