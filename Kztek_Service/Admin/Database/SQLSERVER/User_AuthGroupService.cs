using System;
using System.Linq;
using System.Threading.Tasks;
using Kztek_Data.Repository;
using Kztek_Library.Helpers;
using Kztek_Model.Models;
using Microsoft.AspNetCore.Http;

namespace Kztek_Service.Admin.Database.SQLSERVER
{
    public class User_AuthGroupService : IUser_AuthGroupService
    {
        private ItblSystemConfigService _tblSystemConfigService;
        private IUser_AuthGroupRepository _User_AuthGroupRepository;

        public User_AuthGroupService(ItblSystemConfigService _tblSystemConfigService, IUser_AuthGroupRepository _User_AuthGroupRepository)
        {
            this._tblSystemConfigService = _tblSystemConfigService;
            this._User_AuthGroupRepository = _User_AuthGroupRepository;
        }

        public async Task<string> GetAuthCardGroupIds(HttpContext context)
        {
            var user = await SessionCookieHelper.CurrentUser(context);
            var userid = user != null ? user.UserId : "";

            string cardgroupids = "";

            if (user != null)
            {
                var objsystem = await _tblSystemConfigService.GetDefault();

                if (!string.IsNullOrEmpty(userid) && objsystem != null && objsystem.isAuthInView)
                {
                    var user_auth = await GetByUserId(userid);

                    if (user_auth != null && !string.IsNullOrEmpty(user_auth.CardGroupIds))
                    {
                        cardgroupids = user_auth.CardGroupIds;
                    }
                    else
                    {
                        cardgroupids = "NULL";
                    }
                }
            }

            return cardgroupids;
        }

        public async Task<User_AuthGroup> GetByUserId(string userid)
        {
            var query = from n in _User_AuthGroupRepository.Table
                        where n.UserId == userid
                        select n;

            return await Task.FromResult(query.FirstOrDefault());
        }
    }
}
