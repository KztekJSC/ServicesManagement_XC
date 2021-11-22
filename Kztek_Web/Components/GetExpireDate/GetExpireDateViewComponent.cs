using Kztek_Library.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Kztek_Web.Components.GetExpireDate
{

    public class GetExpireDateViewComponent : ViewComponent
    {
        private IHttpContextAccessor HttpContextAccessor;

        public GetExpireDateViewComponent(IHttpContextAccessor HttpContextAccessor)
        {
            this.HttpContextAccessor = HttpContextAccessor;
        }
        public async Task<string> InvokeAsync(string CardNumber)
        {
            DataTable dtCard = DatabaseHelper.ExcuteCommandToDataSet(string.Format("select ExpireDate from tblCard where CardNumber = '{0}'", CardNumber)).Tables[0];

            if (dtCard != null && dtCard.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dtCard.Rows[0]["ExpireDate"].ToString()))
                    return await Task.FromResult(DateTime.Parse(dtCard.Rows[0]["ExpireDate"].ToString()).ToString("dd/MM/yyyy"));
                else
                    return await Task.FromResult(dtCard.Rows[0]["ExpireDate"].ToString());
            }

            return await Task.FromResult("");
        }

       
    }
}
