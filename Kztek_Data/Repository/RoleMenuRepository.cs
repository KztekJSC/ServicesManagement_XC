using Kztek_Data.Infrastructure;
using Kztek_Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Kztek_Data.Repository
{
    public interface IRoleMenuRepository : IRepository<RoleMenu>
    {
    }

    public class RoleMenuRepository : Repository<RoleMenu>, IRoleMenuRepository
    {
        public RoleMenuRepository(DbContextOptions<Kztek_Entities> options) : base(options)
        {
        }
    }
}