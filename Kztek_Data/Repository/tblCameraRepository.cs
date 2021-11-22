using System;
using Kztek_Data.Infrastructure;
using Kztek_Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Kztek_Data.Repository
{
    public interface ItblCameraRepository : IRepository<tblCamera>
    {
    }

    public class tblCameraRepository : Repository<tblCamera>, ItblCameraRepository
    {
        public tblCameraRepository(DbContextOptions<Kztek_Entities> options) : base(options)
        {
        }
    }
}
