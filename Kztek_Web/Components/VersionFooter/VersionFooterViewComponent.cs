using Kztek_Library.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Kztek_Web.Components.VersionFooter
{
    public class VersionFooterViewComponent : ViewComponent
    {
        public VersionFooterViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await GetVersionInfo();

            return View(model);
        }

        public Task<VersionFooterModel> GetVersionInfo()
        {
            var model = new VersionFooterModel()
            {
                AssemblyVersion = Assembly.GetEntryAssembly().GetName().Version.ToString(),
                FileVersion = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version,
                Version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion
            };

            return Task.FromResult(model);
        }
    }
}
