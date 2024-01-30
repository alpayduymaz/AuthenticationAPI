using BLL.EntityCore.Abstract;
using DAL;
using DAL.EntityCore.Concrete;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.EntityCore.Concrete
{
    public class UserRoleRepository : EntityBaseRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(EntityContext context) : base(context)
        {

        }
    }
}
