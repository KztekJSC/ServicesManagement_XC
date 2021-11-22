using System;
using Kztek_Data.Infrastructure;
using Kztek_Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Kztek_Data.Repository
{
    public interface IMenuFunctionConfigRepository : IRepository<MenuFunctionConfig>
    {
    }

    public class MenuFunctionConfigRepository : Repository<MenuFunctionConfig>, IMenuFunctionConfigRepository
    {
        public MenuFunctionConfigRepository(DbContextOptions<Kztek_Entities> options) : base(options)
        {
        }
    }
}
