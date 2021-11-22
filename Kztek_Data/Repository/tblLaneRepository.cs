using System;
using Kztek_Data.Infrastructure;
using Kztek_Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Kztek_Data.Repository
{
    public interface ItblLaneRepository : IRepository<tblLane>
    {
    }

    public class tblLaneRepository : Repository<tblLane>, ItblLaneRepository
    {
        public tblLaneRepository(DbContextOptions<Kztek_Entities> options) : base(options)
        {
        }
    }
}
