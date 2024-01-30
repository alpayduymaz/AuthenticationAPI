using BLL.EntityCore.Abstract;
using DAL;
using DAL.EntityCore.Concrete;
using DAL.EntityCore.Seeds;
using DTO.Shared;
using Entity.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.EntityCore.Concrete
{
    public class UserRepository : EntityBaseRepository<User>, IUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRoleRepository _userRoleRepository;
        public UserRepository(EntityContext context, IConfiguration configuration,
                              IUserRoleRepository userRoleRepository) : base(context)
        {
            _configuration = configuration;
            _userRoleRepository = userRoleRepository;
        }

        /// <summary>
        /// Web kullanıcıları için süreli token oluşturur.
        /// </summary>
        /// <param name="userToken"></param>
        /// <returns></returns>
        public string BuildToken(Token userToken)
        {
            List<Claim> claims = new List<Claim> {
                    new Claim("userId", userToken.UserId.ToString()),
                    new Claim("userRoleId", string.Join(',', userToken.UserRoleId)),
                    new Claim("fullName", userToken.FullName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            DateTime dt = DateTime.UtcNow;

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
              _configuration["Jwt:Audience"],
              claims,
              expires: dt.AddMinutes(int.Parse(_configuration["Jwt:expMinute"])), // addminutes veya addDays olarak burdan süre verilebilir
              notBefore: dt,
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Sistem ve mobil kulllanıcılar için süresiz token oluşturur.
        /// </summary>
        /// <param name="userToken"></param>
        /// <returns></returns>
        public string IndefiniteBuildToken(Token userToken)
        {
            List<Claim> claims = new List<Claim> {
                    new Claim("userId", userToken.UserId.ToString()),
                    new Claim("userRoleId", string.Join(',', userToken.UserRoleId)),
                    new Claim("fullName", userToken.FullName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            DateTime dt = DateTime.UtcNow;

            var token = new JwtSecurityToken(
               _configuration["Jwt:Issuer"],
              _configuration["Jwt:Audience"],
              claims,
              expires: dt.AddYears(15),
              notBefore: dt,
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Servislerin birbirleriyle iletişim kurması için system kullanıcısı üzerinden token oluşturulmaktadır
        /// </summary>
        /// <returns></returns>
        public string GenerateSystemUserToken() => IndefiniteBuildToken(new Token
        {
            UserId = UserCreator.SystemUser.Id,
            UserRoleId = _userRoleRepository.FindBy(x => x.DataStatus == Entity.Shared.DataStatus.Activated && x.UserId == UserCreator.SystemUser.Id).Select(s => s.RoleId).ToArray(),
            FullName = UserCreator.SystemUser.FullName
        });

        /// <summary>
        /// Parolanýn kendisini yine kendisiyle 256-bit SHA1 ile hasler ve geriye hasli parolayý döndürür.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string PasswordHash(string password)
        {
            byte[] salt = Encoding.ASCII.GetBytes(password);
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8)
            );
        }

        /// <summary>
        /// Bilgisayar adını döndürür
        /// </summary>
        /// <param name="IpAdress"></param>
        /// <returns></returns>
        public string GetHostName(string IpAdress)
        {
            IPAddress myIP = IPAddress.Parse(IpAdress);
            IPHostEntry GetIPHost;
            try
            {
                GetIPHost = Dns.GetHostEntry(myIP);
            }
            catch (Exception)
            {
                return "";
            }
            List<string> compName = GetIPHost.HostName.ToString().Split('.').ToList();
            return compName.FirstOrDefault();
        }
    }
}
