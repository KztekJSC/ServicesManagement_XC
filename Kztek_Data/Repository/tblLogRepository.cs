using System;
using Kztek_Data;
using Kztek_Data.Infrastructure;
using Kztek_Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Kztek_Data.Repository
{
    public interface ItblLogRepository : IRepository<tblLog>
    {
    }

    public class tblLogRepository : Repository<tblLog>, ItblLogRepository
    {
        public tblLogRepository(DbContextOptions<Kztek_Entities> options) : base(options)
        {
        }
    }
}
