using System;
using Kztek_Data;
using Kztek_Data.Infrastructure;
using Kztek_Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Kztek_Data.Repository
{
    public interface Itbl_Lane_PCRepository : IRepository<tbl_Lane_PC>
    {
    }

    public class tbl_Lane_PCRepository : Repository<tbl_Lane_PC>, Itbl_Lane_PCRepository
    {
        public tbl_Lane_PCRepository(DbContextOptions<Kztek_Entities> options) : base(options)
        {
        }
    }
}
