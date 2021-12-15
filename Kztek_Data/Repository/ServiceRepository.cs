using System;
using Kztek_Data;
using Kztek_Data.Infrastructure;
using Kztek_Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Kztek_Data.Repository
{
    public interface IServiceRepository : IRepository<Service>
    {
    }

    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        public ServiceRepository(DbContextOptions<Kztek_Entities> options) : base(options)
        {
        }
    }
}
