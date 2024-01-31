using DAL.EntityCore.Abstract;
using DTO.Shared;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.EntityCore.Abstract
{
    public interface IUserRepository : IEntityBaseRepository<User>
    {
        
        string BuildToken(Token userToken);

        string IndefiniteBuildToken(Token userToken);

        string GenerateSystemUserToken();

        string PasswordHash(string password);

        string GetHostName(string IpAdress);
    }
}
