using System;
using Kztek_Data;
using Kztek_Data.Infrastructure;
using Kztek_Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Kztek_Data.Repository
{
    public interface IUser_AuthGroupRepository : IRepository<User_AuthGroup>
    {
    }

    public class User_AuthGroupRepository : Repository<User_AuthGroup>, IUser_AuthGroupRepository
    {
        public User_AuthGroupRepository(DbContextOptions<Kztek_Entities> options) : base(options)
        {
        }
    }
}
