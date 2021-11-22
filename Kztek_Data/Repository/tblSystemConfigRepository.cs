using System;
using Kztek_Data.Infrastructure;
using Kztek_Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Kztek_Data.Repository
{
    public interface ItblSystemConfigRepository : IRepository<tblSystemConfig>
    {
    }

    public class tblSystemConfigRepository : Repository<tblSystemConfig>, ItblSystemConfigRepository
    {
        public tblSystemConfigRepository(DbContextOptions<Kztek_Entities> options) : base(options)
        {
        }
    }
}
