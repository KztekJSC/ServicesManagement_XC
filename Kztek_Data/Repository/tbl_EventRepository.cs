using System;

using Kztek_Data;

using Kztek_Data.Infrastructure;
using Kztek_Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Kztek_Data.Repository
{
    public interface Itbl_EventRepository : IRepository<tbl_Event>
    {
    }

    public class tbl_EventRepository : Repository<tbl_Event>, Itbl_EventRepository
    {
        public tbl_EventRepository(DbContextOptions<Kztek_Entities> options) : base(options)
        {
        }
    }
}
