using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kztek_Core.Models;
using Kztek_Data.Repository;
using Kztek_Library.Helpers;
using Kztek_Model.Models;

namespace Kztek_Service.Admin.Database.SQLSERVER
{
    public class MenuFunctionConfigService : IMenuFunctionConfigService
    {
        private IMenuFunctionConfigRepository _MenuFunctionConfigRepository;

        public MenuFunctionConfigService(IMenuFunctionConfigRepository _MenuFunctionConfigRepository)
        {
            this._MenuFunctionConfigRepository = _MenuFunctionConfigRepository;
        }

        public async Task<MessageReport> Create(MenuFunctionConfig obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                var str = string.Format("INSERT INTO MenuFunctionConfig (Id, MenuFunctionId) VALUES ('{0}', '{1}')", obj.Id, obj.MenuFunctionId);

                var result = DatabaseHelper.ExcuteCommandToBool(str);
                re.isSuccess = result;

                if (result)
                {
                    re.Message = "Thêm mới thành công";
                }
                else
                {
                    re.Message = "Có lỗi xảy ra";
                }
            }
            catch (Exception ex)
            {
                re.Message = ex.Message;
                re.isSuccess = false;
            }

            return await Task.FromResult(re);
        }

        public async Task<bool> DeleteAll()
        {
            var str = "DELETE FROM MenuFunctionConfig";
            var result = DatabaseHelper.ExcuteCommandToBool(str);

            return await Task.FromResult(result);
        }

        public async Task<List<MenuFunctionConfig>> GetAll()
        {
            var data = _MenuFunctionConfigRepository.Table;
            return await Task.FromResult(data.ToList());
        }
    }
}
