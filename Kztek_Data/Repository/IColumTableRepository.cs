using System;
using Kztek_Data;
using Kztek_Data.Infrastructure;
using Kztek_Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Kztek_Data.Repository
{
    public interface IColumTableRepository : IRepository<ColumTable>
    {
    }

    public class ColumTableRepository : Repository<ColumTable>, IColumTableRepository
    {
        public ColumTableRepository(DbContextOptions<Kztek_Entities> options) : base(options)
        {
        }
    }
}
