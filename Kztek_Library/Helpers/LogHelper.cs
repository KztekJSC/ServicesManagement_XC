using Kztek_Core.Models;
using Kztek_Library.Models;
using Kztek_Model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Library.Helpers
{
    public class LogHelper
    {
        public static async Task WriteLog(string objId, string actions,string description,HttpContext httpContext)
        {
            SessionModel currentuser = SessionCookieHelper.CurrentUser(httpContext).Result;
            var computername = await FunctionHelper.GetComputerName(httpContext.Connection.RemoteIpAddress.ToString());
            var arr = httpContext.Request.Path.Value.Split('/');          
            var area = arr != null && arr.Length > 1 ? arr[1] : "";
            var classname = arr != null && arr.Length > 2 ? arr[2] : "";


            var t = new tblLog();
            t.LogID = Guid.NewGuid().ToString();
            t.Actions = actions;
            t.AppCode = area;
            t.ComputerName = computername;
            t.Date = DateTime.Now;
            t.Description = description;
            t.IPAddress = computername;
            t.ObjectName = objId;
            t.SubSystemCode = classname;
            t.UserName = currentuser.Username;

            var str = new StringBuilder();

            str.Append("INSERT INTO tblLog (LogID, Date, UserName, AppCode, SubSystemCode, ObjectName, Actions, Description, ComputerName)");

            str.AppendLine("VALUES (");

            str.AppendLine(string.Format("'{0}'", Guid.NewGuid()));
            str.AppendLine(string.Format(", '{0}'", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
            str.AppendLine(string.Format(", '{0}'", t.UserName));
            str.AppendLine(string.Format(", '{0}'", t.AppCode));
            str.AppendLine(string.Format(", '{0}'", t.SubSystemCode));
            str.AppendLine(string.Format(", N'{0}'", t.ObjectName));
            str.AppendLine(string.Format(", N'{0}'", t.Actions));
            str.AppendLine(string.Format(", N'{0}'", t.Description));
            str.AppendLine(string.Format(", '{0}'", t.ComputerName));

            str.AppendLine(")");

            DatabaseHelper.ExcuteCommandToBool(str.ToString());           
        }

        public static async Task WriteLog(string objId, string actions,string classname, string description, HttpContext httpContext)
        {
            SessionModel currentuser = SessionCookieHelper.CurrentUser(httpContext).Result;
            var computername = await FunctionHelper.GetComputerName(httpContext.Connection.RemoteIpAddress.ToString());
            var arr = httpContext.Request.Path.Value.Split('/');
            var area = arr != null && arr.Length > 1 ? arr[1] : "";
          


            var t = new tblLog();
            t.LogID = Guid.NewGuid().ToString();
            t.Actions = actions;
            t.AppCode = area;
            t.ComputerName = computername;
            t.Date = DateTime.Now;
            t.Description = description;
            t.IPAddress = computername;
            t.ObjectName = objId;
            t.SubSystemCode = classname;
            t.UserName = currentuser.Username;

            var str = new StringBuilder();

            str.Append("INSERT INTO tblLog (LogID, Date, UserName, AppCode, SubSystemCode, ObjectName, Actions, Description, ComputerName)");

            str.AppendLine("VALUES (");

            str.AppendLine(string.Format("'{0}'", Guid.NewGuid()));
            str.AppendLine(string.Format(", '{0}'", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
            str.AppendLine(string.Format(", '{0}'", t.UserName));
            str.AppendLine(string.Format(", '{0}'", t.AppCode));
            str.AppendLine(string.Format(", '{0}'", t.SubSystemCode));
            str.AppendLine(string.Format(", N'{0}'", t.ObjectName));
            str.AppendLine(string.Format(", N'{0}'", t.Actions));
            str.AppendLine(string.Format(", N'{0}'", t.Description));
            str.AppendLine(string.Format(", '{0}'", t.ComputerName));

            str.AppendLine(")");

            DatabaseHelper.ExcuteCommandToBool(str.ToString());
        }

    }
}
