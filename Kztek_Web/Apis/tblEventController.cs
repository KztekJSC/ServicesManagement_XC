using Kztek_Core.Models;
using Kztek_Library.Configs;
using Kztek_Model.Models;
using Kztek_Service.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Kztek_Web.Apis
{
    [Authorize(Policy = ApiConfig.Auth_Bearer_Mobile)]
    [Route("api/[controller]")]
    public class tblEventController : Controller
    {
        private Itbl_EventService _tbl_EventService;

        public tblEventController(Itbl_EventService _tbl_EventService)
        {
            this._tbl_EventService = _tbl_EventService;
        }

        /// <summary>
        /// Api thêm mới bản ghi
        /// </summary>
        /// Author          Date            Summary
        /// LamHN         23/11/2021      Thêm mới
        /// <param name="value"> Model thêm mới </param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<MessageReport>> Post([FromBody] tbl_Event_POST value)
        {
            return await _tbl_EventService.Create(value);
        }

        /// <summary>
        /// Api cập nhật bản ghi
        /// </summary>
        /// Author          Date            Summary
        /// LamHN         23/11/2021      Thêm mới
        /// <param name="id">Id bản ghi</param>
        /// <param name="value"> Model cập nhật </param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<ActionResult<MessageReport>> Put([FromBody] tbl_Event_POST value)
        {
            return await _tbl_EventService.Update(value);
        }

        /// <summary>
        /// Api xóa bản ghi
        /// </summary>
        /// Author          Date            Summary
        /// LamHN         23/11/2021      Thêm mới
        /// <param name="id">Id bản ghi</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<MessageReport>> Delete(string id)
        {
            return await _tbl_EventService.Delete(id);
        }
    }
}
