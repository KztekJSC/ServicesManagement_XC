using Kztek_Data.Infrastructure;
using Kztek_Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Kztek_Data.Repository
{
    public interface IUserRoleRepository : IRepository<UserRole>
    {
    }

    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(DbContextOptions<Kztek_Entities> options) : base(options)
        {
        }
    }
}