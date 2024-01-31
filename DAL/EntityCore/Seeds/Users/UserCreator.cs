using Entity.Models;
using Entity.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EntityCore.Seeds
{
    public static class UserCreator
    {
        
        public static readonly User SystemUser = new User
        {
            Id = 1,
            Name = "Super",
            Surname = "Admin",
            FullName = "Super Admin",
            Email = "alpayduymaz9@icloud.com",
            Password = "9K7Cwg3Qw/8FR/S9VvrNdgl8znxhPagMZ4QrajV/3AQ=", // admin
            CreatedAt = new DateTime(2024, 01, 09),
            DataStatus = DataStatus.Activated
        };

        public static void Create(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User[] { SystemUser });
        }
    }
}
