using System;
using Kztek_Data;
using Kztek_Data.Infrastructure;
using Kztek_Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Kztek_Data.Repository
{
    public interface IGroupRepository : IRepository<Group>
    {
    }

    public class GroupRepository : Repository<Group>, IGroupRepository
    {
        public GroupRepository(DbContextOptions<Kztek_Entities> options) : base(options)
        {
        }
    }
}
