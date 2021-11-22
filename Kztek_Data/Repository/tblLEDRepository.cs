using System;
using Kztek_Data;
using Kztek_Data.Infrastructure;
using Kztek_Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Kztek_Data.Repository
{
    public interface ItblLEDRepository : IRepository<tblLED>
    {
    }

    public class tblLEDRepository : Repository<tblLED>, ItblLEDRepository
    {
        public tblLEDRepository(DbContextOptions<Kztek_Entities> options) : base(options)
        {
        }
    }
}
