using System;
using Kztek_Data;
using Kztek_Data.Infrastructure;
using Kztek_Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Kztek_Data.Repository
{
    public interface IEventInRepository : IRepository<EventIn>
    {
    }

    public class EventInRepository : Repository<EventIn>, IEventInRepository
    {
        public EventInRepository(DbContextOptions<Kztek_Entities> options) : base(options)
        {
        }
    }
}
