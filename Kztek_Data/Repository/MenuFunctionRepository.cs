using Kztek_Data.Infrastructure;
using Kztek_Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Kztek_Data.Repository
{
    public interface IMenuFunctionRepository : IRepository<MenuFunction>
    {
    }

    public class MenuFunctionRepository : Repository<MenuFunction>, IMenuFunctionRepository
    {
        public MenuFunctionRepository(DbContextOptions<Kztek_Entities> options) : base(options)
        {
        }
    }
}