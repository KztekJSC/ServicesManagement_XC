using System;
using Kztek_Data.Infrastructure;
using Kztek_Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Kztek_Data.Repository
{
    public interface ItblPCRepository : IRepository<tblPC>
    {
    }
    public class tblPCRepository : Repository<tblPC>, ItblPCRepository
    {
        public tblPCRepository(DbContextOptions<Kztek_Entities> options) : base(options)
        {
        }
    }
}
