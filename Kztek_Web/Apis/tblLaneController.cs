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
    public class tblLaneController : Controller
    {
        private ItblLaneService _tblLaneService;

        public tblLaneController(ItblLaneService _tblLaneService)
        {
            this._tblLaneService = _tblLaneService;
        }

        /// <summary>
        /// Api lấy danh sách theo từ khóa, id máy tính
        /// </summary>
        /// Author          Date            Summary
        /// TrungNQ         14/12/2019      Thêm mới
        /// <param name="key"> Từ khóa </param>
        /// <param name="pcid"> Id bản ghi </param>
        /// <returns> IEnumerable<tblLane> </returns>
        [AllowAnonymous]
        [HttpGet("byList")]
        public async Task<IEnumerable<tblLane>> GetList(string key, string pcid)
        {
            var data = await _tblLaneService.GetAllByFirst(key, pcid);

            return data;
        }

        /// <summary>
        /// Api lấy một bản ghi
        /// </summary>
        /// Author          Date            Summary
        /// TrungNQ         14/12/2019      Thêm mới
        /// <param name="id"></param>
        /// <returns> tblLane </returns>
        [HttpGet("byId")]
        public async Task<tblLane> GetById(string id)
        {
            return await _tblLaneService.GetById(id);
        }

        /// <summary>
        /// Api thêm mới bản ghi
        /// </summary>
        /// Author          Date            Summary
        /// TrungNQ         14/12/2019      Thêm mới
        /// <param name="value"></param>
        /// <returns> MessageReport </returns>
        [HttpPost]
        public async Task<ActionResult<MessageReport>> Post([FromBody] tblLane_Submit value)
        {
            return await _tblLaneService.Create(value);
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
        public async Task<ActionResult<MessageReport>> Put([FromBody] tblLane_Submit value)
        {
            return await _tblLaneService.Update(value);
        }

        /// <summary>
        /// Api xóa bản ghi
        /// </summary>
        /// Author          Date            Summary
        /// TrungNQ         14/12/2019      Thêm mới
        /// <param name="id">Id bản ghi</param>
        /// <returns> MessageReport </returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<MessageReport>> Delete(string id)
        {
            return await _tblLaneService.Remove(id);
        }
    }
}
