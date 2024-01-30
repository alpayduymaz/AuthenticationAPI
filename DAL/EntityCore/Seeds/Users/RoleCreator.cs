using Entity.Models;
using Entity.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EntityCore.Seeds.Users
{
    public static class RoleCreator
    {
        public static void Create(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(new Role[] {
                    new Role {
                        Id = 1,
                        Name = "Super Admin",
                        CreatedAt = new DateTime(2024, 01, 09),
                        DataStatus = DataStatus.Activated
                    }
            });
        }
    }
}
