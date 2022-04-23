using Kztek_Data.Infrastructure;
using Kztek_Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Data.Repository
{
    
    public interface ItblGroupServiceRepository : IRepository<tblGroupService>
    {
    }

    public class tblGroupServiceRepository : Repository<tblGroupService>, ItblGroupServiceRepository
    {
        public tblGroupServiceRepository(DbContextOptions<Kztek_Entities> options) : base(options)
        {
        }
    }
}
