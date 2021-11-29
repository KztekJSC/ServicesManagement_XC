using Kztek_Core.Models;
using Kztek_Library.Configs;
using Kztek_Model.Models;
using Kztek_Service.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Kztek_Web.Apis
{
    //[Authorize(Policy = ApiConfig.Auth_Bearer_Mobile)]
    [Route("api/[controller]")]
    public class tbl_EventController : Controller
    {
        private Itbl_EventService _tbl_EventService;

        public tbl_EventController(Itbl_EventService _tbl_EventService)
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
        public async Task<ActionResult<MessageReport>> Post([FromBody]tbl_Event_POST value)
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
        public async Task<ActionResult<MessageReport>> Put([FromBody]tbl_Event_POST value)
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
        [HttpDelete]
        public async Task<ActionResult<MessageReport>> Delete([FromBody]tbl_Event_POST value)
        {
            return await _tbl_EventService.Delete(value);
        }

        /// <summary>
        /// Api cập nhật thông tin xe vào
        /// </summary>
        /// Author          Date            Summary
        /// LamHN         29/11/2021      Thêm mới
        /// <returns></returns>
        [HttpPost("xevao")]
        public async Task<ActionResult<MessageReport>> VehicleIn([FromBody]API_VehicleStatus value)
        {
            return await _tbl_EventService.VehicleStatusIn(value);
        }

        /// <summary>
        /// Api cập nhật thông tin xe ra
        /// </summary>
        /// Author          Date            Summary
        /// LamHN         29/11/2021      Thêm mới
        /// <returns></returns>
        [HttpPost("xera")]
        public async Task<ActionResult<MessageReport>> VehicleOut([FromBody]API_VehicleStatus value)
        {
            return await _tbl_EventService.VehicleStatusOut(value);
        }
    }
}
