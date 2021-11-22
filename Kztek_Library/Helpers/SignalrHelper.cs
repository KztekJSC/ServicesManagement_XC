using Kztek_Library.Models;
using Kztek_Model.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base;

namespace Kztek_Library.Helpers
{
    public class SignalrHelper : IHostedService, IDisposable
    {
        public static IHubContext<SqlHub> SqlHub;
        public SignalrHelper(IHubContext<SqlHub> _SqlHub)
        {
            SqlHub = _SqlHub;
        }

        public void Dispose()
        {
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
