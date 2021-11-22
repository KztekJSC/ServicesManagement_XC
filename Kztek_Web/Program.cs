using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Kztek_Data;
using Kztek_Library.Helpers;
using Kztek_Model.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Kztek_Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Thread riêng
            var sync = "0";

            try
            {
                sync = AppSettingHelper.GetStringFromAppSetting("Realtime:SqlTableDependency").Result;
            }
            catch
            {
                sync = "0"; //Luôn off
            }

            if (sync == "1") //on
            {
               // new Thread(() => SqlHelper.SqlTableDependency()).Start();
              
            }

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();


    }
}
