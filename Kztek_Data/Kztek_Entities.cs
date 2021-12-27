using System;
using System.Threading.Tasks;
using Kztek_Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
//using MySql.Data.MySqlClient;

namespace Kztek_Data
{
    public class Kztek_Entities : DbContext
    {
        public Kztek_Entities(DbContextOptions<Kztek_Entities> options) : base(options)
        {

        }

        //Main
        public DbSet<User> Users { get; set; }

        public DbSet<Role> SY_Roles { get; set; }

        public DbSet<MenuFunction> SY_MenuFunctions { get; set; }

        public DbSet<UserRole> SY_Map_User_Roles { get; set; }

        public DbSet<RoleMenu> SY_Map_Role_Menus { get; set; }

        public DbSet<tblSystemConfig> tblSystemConfigs { get; set; }

        public DbSet<MenuFunctionConfig> MenuFunctionConfigs { get; set; }

        public DbSet<tblLog> tblLogs { get; set; }

        public DbSet<User_AuthGroup> User_AuthGroups { get; set; } 
        public DbSet<ColumTable> ColumTables { get; set; }

        //Parking


        public DbSet<tblPC> tblPCs { get; set; }

        public DbSet<tblCamera> tblCameras { get; set; }

        public DbSet<tblLane> tblLanes { get; set; }

        public DbSet<Group> Groups { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<tblLED> tblLEDs { get; set; }

        public DbSet<tbl_Event> tbl_Events { get; set; }
      
        public DbSet<tbl_Lane_PC> tbl_Lane_PCs { get; set; }
        //Face

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<tblSystemConfig>(entity =>
            {
                entity.Ignore(e => e.SortOrder);
            }); 

            modelBuilder.Entity<tbl_Event>(entity =>
            {
               
            });
        }


    }
}
