using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kztek_Core.Models;
using Kztek_Library.Models;
using Kztek_Service.Api;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Kztek_Web.Apis
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private IUserService _UserService;

        public UserController(IUserService _UserService)
        {
            this._UserService = _UserService;
        }

        /// <summary>
        /// Api đăng nhập
        /// </summary>
        /// Author          Date            Summary
        /// TrungNQ         12/12/2019      Thêm mới tính năng
        /// <param name="value">{ Username, Password }</param>
        /// <returns>{ isSuccess, Message }</returns>
        [HttpPost]
        public async Task<ActionResult<MessageReport>> Post([FromBody] AuthModel value)
        {
            return await _UserService.SignIn(value);
        }
    }
}
