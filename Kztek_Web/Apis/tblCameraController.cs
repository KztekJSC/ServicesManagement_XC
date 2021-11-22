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
    public class tblCameraController : Controller
    {
        private ItblCameraService _tblCameraService;

        public tblCameraController(ItblCameraService _tblCameraService)
        {
            this._tblCameraService = _tblCameraService;
        }

        /// <summary>
        /// Api lấy danh sách camera
        /// </summary>
        /// Author          Date            Summary
        /// TrungNQ         14/12/2019      Thêm mới
        /// <param name="key">Từ khóa</param>
        /// <param name="pcid">Id máy tính</param>
        /// <returns></returns>
        [HttpGet("byList")]
        public async Task<IEnumerable<tblCamera_View>> GetList(string key, string pcid)
        {
            var data = await _tblCameraService.GetAllCustomByFirst(key, pcid);

            return data;
        }

        /// <summary>
        /// Api lấy một bản ghi
        /// </summary>
        /// Author          Date            Summary
        /// TrungNQ         14/12/2019      Thêm mới
        /// <param name="id">Id bản ghi camera</param>
        /// <returns></returns>
        [HttpGet("byId")]
        public async Task<tblCamera_View> GetById(string id)
        {
            return await _tblCameraService.GetCustomById(id);
        }

        /// <summary>
        /// Api thêm mới bản ghi
        /// </summary>
        /// Author          Date            Summary
        /// TrungNQ         14/12/2019      Thêm mới
        /// <param name="value"> Model camera thêm mới </param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<MessageReport>> Post([FromBody] tblCamera_POST value)
        {
            return await _tblCameraService.Create(value);
        }

        /// <summary>
        /// Api cập nhật bản ghi
        /// </summary>
        /// Author          Date            Summary
        /// TrungNQ         14/12/2019      Thêm mới
        /// <param name="id">Id bản ghi</param>
        /// <param name="value"> Model camera cập nhật </param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<ActionResult<MessageReport>> Put([FromBody] tblCamera_PUT value)
        {
            return await _tblCameraService.Update(value);
        }

        /// <summary>
        /// Api xóa bản ghi
        /// </summary>
        /// Author          Date            Summary
        /// TrungNQ         14/12/2019      Thêm mới
        /// <param name="id">Id bản ghi</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<MessageReport>> Delete(string id)
        {
            return await _tblCameraService.Remove(id);
        }
    }
}
