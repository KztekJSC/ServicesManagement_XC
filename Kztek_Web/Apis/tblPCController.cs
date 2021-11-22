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
    public class tblPCController : Controller
    {
        private ItblPCService _tblPCService;

        public tblPCController(ItblPCService _tblPCService)
        {
            this._tblPCService = _tblPCService;
        }

        /// <summary>
        /// Api lấy danh sách theo từ khóa, id cổng
        /// </summary>
        /// Author          Date            Summary
        /// TrungNQ         14/12/2019      Thêm mới
        /// <param name="key">Từ khóa</param>
        /// <param name="gateid">Id cổng</param>
        /// <returns> IEnumerable<tblPC> </returns>
        [AllowAnonymous]
        [HttpGet("byList")]
        public async Task<IEnumerable<tblPC>> GetList(string key, string gateid)
        {
            var data = await _tblPCService.GetAllByFirst(key, gateid);

            return data;
        }

        /// <summary>
        /// Api lấy bản ghi theo id
        /// </summary>
        /// Author          Date            Summary
        /// TrungNQ         14/12/2019      Thêm mới
        /// <param name="id">Id máy tính</param>
        /// <returns></returns>
        [HttpGet("byId")]
        public async Task<tblPC> GetById(string id)
        {
            return await _tblPCService.GetById(id);
        }

        /// <summary>
        /// Api thêm mới bản ghi
        /// </summary>
        /// Author          Date            Summary
        /// TrungNQ         14/12/2019      Thêm mới
        /// <param name="value"></param>
        /// <returns> MessageReport </returns>
        //[HttpPost("create")]
        //public async Task<ActionResult<MessageReport>> Post([FromBody] tblPC_POST value)
        //{
        //    return await _tblPCService.Create(value);
        //}
        [HttpPost]
        public async Task<ActionResult<MessageReport>> Post([FromBody] tblPC_POST value)
        {
            return await _tblPCService.Create(value);
        }
        /// <summary>
        /// Api cập nhật bản ghi
        /// </summary>
        /// Author          Date            Summary
        /// TrungNQ         14/12/2019      Thêm mới
        /// <param name="id">Id bản ghi</param>
        /// <param name="value"></param>
        /// <returns> MessageReport </returns>
        [HttpPut("update")]
        public async Task<ActionResult<MessageReport>> Put([FromBody] tblPC_POST value)
        {
            return await _tblPCService.Update(value);
        }

        /// <summary>
        /// Api lấy bản ghi
        /// </summary>
        /// Author          Date            Summary
        /// TrungNQ         14/12/2019      Thêm mới
        /// <param name="id">Id bản ghi</param>
        /// <returns> MessageReport </returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<MessageReport>> Delete(string id)
        {
            return await _tblPCService.Remove(id);
        }
    }
}
