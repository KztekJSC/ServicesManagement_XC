using Kztek_Library.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Kztek_Web.Components.GetDays
{
    public class GetDaysViewComponent : ViewComponent
    {
        private IHttpContextAccessor HttpContextAccessor;

        public GetDaysViewComponent(IHttpContextAccessor HttpContextAccessor)
        {
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public async Task<string> InvokeAsync(string CardNumber, string cardgroupId)
        {
            double days = 0;
            string endDay = "";
            string typeCard = "";

            DataTable dtCard = DatabaseHelper.ExcuteCommandToDataSet(string.Format("select ExpireDate from tblCard where CardNumber = '{0}'", CardNumber)).Tables[0];

            if (dtCard != null && dtCard.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dtCard.Rows[0]["ExpireDate"].ToString()))
                    endDay = DateTime.Parse(dtCard.Rows[0]["ExpireDate"].ToString()).ToString("dd/MM/yyyy");
                else
                    endDay = dtCard.Rows[0]["ExpireDate"].ToString();
            }


            if (!string.IsNullOrEmpty(endDay))
            {
                days = (Convert.ToDateTime(endDay).Date - DateTime.Now.Date).TotalDays;
            }

            if (days < 0)
                days = 0;

            //type card
            if (!string.IsNullOrEmpty(cardgroupId) && !cardgroupId.Equals("LOOP_D") && !cardgroupId.Equals("LOOP_M"))
            {
                DataTable dtCardGroup = DatabaseHelper.ExcuteCommandToDataSet(string.Format("select CardType from tblCardGroup where CardGroupID = CONVERT(nvarchar(50), '{0}')", cardgroupId)).Tables[0];
                if (dtCardGroup != null && dtCardGroup.Rows.Count > 0)
                {
                    typeCard = dtCardGroup.Rows[0]["CardType"].ToString();
                }
            }


            if (!string.IsNullOrEmpty(cardgroupId) && (cardgroupId.Equals("LOOP_D") || cardgroupId.Equals("LOOP_M")))
            {
                typeCard = cardgroupId;
            }


            if (typeCard.Equals("1") || string.IsNullOrEmpty(typeCard) || typeCard.Equals("LOOP_D"))
            {
                return await Task.FromResult("");
            }
            else
            {
                return await Task.FromResult(days.ToString());
            }


        }
    }
}
