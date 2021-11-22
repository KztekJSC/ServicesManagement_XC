using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kztek_Core.Models;
using Kztek_Library.Configs;
using Kztek_Model.Models;
using Kztek_Service.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Kztek_Web.Apis
{
    [Authorize(Policy = ApiConfig.Auth_Bearer_Mobile)]
    [Route("api/[controller]")]
    public class tblSystemConfigController : Controller
    {
        private ItblSystemConfigService _tblSystemConfigService;

        public tblSystemConfigController(ItblSystemConfigService _tblSystemConfigService)
        {
            this._tblSystemConfigService = _tblSystemConfigService;
        }

        /// <summary>
        /// APi lấy cấu hình
        /// </summary>
        /// Author          Date            Summary
        /// TrungNQ         12/12/2019      Thêm mới
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<tblSystemConfig>> Get()
        {
            var result = await _tblSystemConfigService.GetDefault();

            return result.FirstOrDefault();
        }

        /// <summary>
        /// Api Thêm mới
        /// </summary>
        /// Author          Date            Summary
        /// TrungNQ         12/12/2019      Thêm mới
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<MessageReport>> Post([FromBody] tblSystemConfig_POST value)
        {
            return await _tblSystemConfigService.Create(value);
        }

        /// <summary>
        /// Api cập nhật cấu hình
        /// </summary>
        /// Author          Date            Summary
        /// TrungNQ         12/12/2019      Thêm mới
        /// <param name="id">Id bản ghi</param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<MessageReport>> Put(string id, [FromBody] tblSystemConfig_PUT value)
        {
            value.Id = id;
            return await _tblSystemConfigService.Update(value);
        }

        /// <summary>
        /// Api xóa bản ghi
        /// </summary>
        /// Author          Date            Summary
        /// TrungNQ         12/12/2019      Thêm mới
        /// <param name="id">Id bản ghi</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<MessageReport>> Delete(string id)
        {
            return await _tblSystemConfigService.Remove(id);
        }
    }
}
