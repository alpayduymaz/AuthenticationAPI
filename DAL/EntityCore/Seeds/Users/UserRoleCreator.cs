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
    public static class UserRoleCreator
    {
        public static void Create(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>().HasData(new UserRole[] {
                    new UserRole { Id = 1,
                        RoleId = 1,
                        UserId = 1,
                        CreatedAt = new DateTime(2024, 01, 09),
                        DataStatus = DataStatus.Activated
                    }
            });
        }
    }
}
