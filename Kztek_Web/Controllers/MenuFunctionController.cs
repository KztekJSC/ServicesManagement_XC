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
    public class MenuFunctionController : Controller
    {
        private IMenuFunctionService _MenuFunctionService;
        public MenuFunctionController(IMenuFunctionService _MenuFunctionService)
        {
            this._MenuFunctionService = _MenuFunctionService;
        }

        [CheckSessionCookie]
        public async Task<IActionResult> Index(string AreaCode = "")
        {
            var data = await _MenuFunctionService.GetAll(AreaCode);
            ViewBag.AreaCodeValue = AreaCode;

            return View(data.ToList());
        }

        // public PartialViewResult SubMenu(List<MenuFunction> listChild, List<MenuFunction> allFunction)
        // {
        //     var model = new MenuFunction_Tree()
        //     {
        //         Data_All = allFunction,
        //         Data_Child = listChild
        //     };

        //     return PartialView(model);
        // }

        [CheckSessionCookie]
        [HttpGet]
        public async Task<IActionResult> Create(MenuFunction_Submit model, string AreaCode = "")
        {

            model = model == null ? new MenuFunction_Submit() : model;
            model.Active = true;

            ViewBag.Data_MenuFunction = await GetMenuList(AreaCode);
            ViewBag.Data_MenuType = StaticList.MenuType();
            ViewBag.AreaCodeValue = AreaCode;

            return View(model);
        }

        [CheckSessionCookie]
        [HttpPost]
        public async Task<IActionResult> Create(MenuFunction_Submit model, bool SaveAndCountinue = false, string AreaCode = "")
        {
            ViewBag.Data_MenuFunction = await GetMenuList(AreaCode);
            ViewBag.Data_MenuType = StaticList.MenuType();
            ViewBag.AreaCodeValue = AreaCode;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.MenuName))
            {
                ModelState.AddModelError("MenuName", await LanguageHelper.GetLanguageText("MESSAGE:FEE:enter_name"));
                return View(model);
            }

            //Gán giá trị
            var id = Guid.NewGuid().ToString();

            model.ControllerName = !string.IsNullOrWhiteSpace(model.ControllerName) ? model.ControllerName.Trim() : string.Format("Controller_{0}", id);
            model.ActionName = !string.IsNullOrWhiteSpace(model.ActionName) ? model.ActionName.Trim() : string.Format("Action_{0}", id);

            var obj = new MenuFunction()
            {
                Id = id,
                MenuName = model.MenuName,
                ControllerName = model.ControllerName,
                ActionName = model.ActionName,
                Icon = model.Icon,
                MenuType = model.MenuType,
                ParentId = string.IsNullOrWhiteSpace(model.ParentId) ? "0" : model.ParentId,
                Active = model.Active,
                OrderNumber = model.OrderNumber,
                MenuGroupListId = "12878956"
            };

            //Thực hiện thêm mới
            var result = await _MenuFunctionService.Create(obj);
            if (result.isSuccess)
            {
                if (SaveAndCountinue)
                {
                    TempData["Success"] = "Thêm mới thành công";
                    return RedirectToAction("Create", new { ControllerName = obj.ControllerName, ParentId = obj.ParentId, MenuType = obj.MenuType, AreaCode = AreaCode });
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
            var model = await _MenuFunctionService.GetCustomById(id);

            ViewBag.Data_MenuFunction = await GetMenuList(AreaCode);
            ViewBag.Data_MenuType = StaticList.MenuType();
            ViewBag.AreaCodeValue = AreaCode;

            return View(model);
        }

        [CheckSessionCookie]
        [HttpPost]
        public async Task<IActionResult> Update(MenuFunction_Submit model, string AreaCode = "")
        {
            ViewBag.Data_MenuFunction = await GetMenuList(AreaCode);
            ViewBag.Data_MenuType = StaticList.MenuType();
            ViewBag.AreaCodeValue = AreaCode;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var oldObj = await _MenuFunctionService.GetById(model.Id);
            if (oldObj == null)
            {
                ModelState.AddModelError("", "Bản ghi không tồn tại");
                return View(model);
            }

            model.ControllerName = !string.IsNullOrWhiteSpace(model.ControllerName) ? model.ControllerName.Trim() : string.Format("Controller_{0}", model.Id);
            model.ActionName = !string.IsNullOrWhiteSpace(model.ActionName) ? model.ActionName.Trim() : string.Format("Action_{0}", model.Id);

            oldObj.MenuName = model.MenuName;
            oldObj.ControllerName = model.ControllerName.Trim();
            oldObj.ActionName = model.ActionName.Trim();
            oldObj.ParentId = string.IsNullOrWhiteSpace(model.ParentId) ? "0" : model.ParentId;
            oldObj.Active = model.Active;
            oldObj.Icon = model.Icon;
            oldObj.MenuType = model.MenuType;
            oldObj.OrderNumber = model.OrderNumber;
        

            var result = await _MenuFunctionService.Update(oldObj);
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
            var result = await _MenuFunctionService.Delete(id);

            return Json(result);
        }

        private async Task<List<MenuFunction_Submit>> GetMenuList(string area = "")
        {
            var list = new List<MenuFunction_Submit>
            {
                new MenuFunction_Submit {  Id = "", MenuName = await LanguageHelper.GetLanguageText("MENUFUNCTION:SelectMenu") }
            };

            var MenuList = await _MenuFunctionService.GetAllCustomActiveOrder(area);
            var parent = MenuList.Where(c => c.ParentId == "0").ToList();
            if (parent.Any())
            {
                foreach (var item in parent)
                {
                    //Nếu có thì duyệt tiếp để lưu vào list
                    list.Add(new MenuFunction_Submit { Id = item.Id, MenuName = item.MenuName });

                    var listChild = MenuList.Where(c => c.ParentId == item.Id).ToList();

                    //Gọi action để lấy danh sách submenu theo id
                    if (listChild.Any())
                    {
                        Children(listChild, MenuList, list, item);
                    }

                    list.Add(new MenuFunction_Submit { Id = "", MenuName = "-----" });

                }
            }
            return list;
        }

        private void Children(List<MenuFunction_Submit> listChild, List<MenuFunction_Submit> allFunction, List<MenuFunction_Submit> lst, MenuFunction_Submit itemParent)
        {
            //Kiểm tra có dữ liệu chưa
            if (listChild.Any())
            {
                foreach (var item in listChild)
                {
                    //Nếu có thì duyệt tiếp để lưu vào list
                    lst.Add(new MenuFunction_Submit { Id = item.Id, MenuName = itemParent.MenuName + " \\ " + item.MenuName });

                    //Gọi action để lấy danh sách submenu theo id
                    var child = allFunction.Where(c => c.ParentId == item.Id).ToList();

                    //Gọi action để lấy danh sách submenu theo id
                    if (child.Any())
                    {
                        item.MenuName = itemParent.MenuName + " \\ " + item.MenuName;
                        Children(child, allFunction, lst, item);
                    }
                }
            }
        }
    }
}
