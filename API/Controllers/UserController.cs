using BLL.EntityCore.Abstract;
using DAL.Middleware;
using DTO.Shared;
using DTO.Systems;
using Entity.Models;
using Entity.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _UserRepository;
        private readonly IUserRoleRepository _UserRoleRepository;
        private readonly IUserSessionRepository _userSessionRepository;
        private readonly ICustomHttpContextAccessor _customHttpContextAccessor;


        public UserController(IUserRepository UserRepository, IUserRoleRepository UserRoleRepository, IUserSessionRepository userSessionRepository, ICustomHttpContextAccessor customHttpContextAccessor)
        {
            _customHttpContextAccessor = customHttpContextAccessor;
            _UserRepository = UserRepository;
            _UserRoleRepository = UserRoleRepository;
            _userSessionRepository = userSessionRepository;
        }

        /// <summary>
        /// Tüm User verilerini getirir.
        /// </summary>
        [HttpGet, Route("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_UserRepository.FindBy(x => x.DataStatus == Entity.Shared.DataStatus.Activated).ToList());
        }

        /// <summary>
        /// Tekil User verisini getirir.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("GetById/{id:int}")]
        public IActionResult GetById(int id)
        {
            var res = new DTO.Shared.DataAccessResult<object>();

            if (id == null)
            {
                res.Result = false;
                res.ResultMessage = "Sistemde bir hata oluştu lütfen yönetinizle görüşünüz";
                return BadRequest(res);
            }
            return Ok(_UserRepository.FindBy(x => x.Id == id && x.DataStatus == Entity.Shared.DataStatus.Activated).FirstOrDefault());
        }

        /// <param name="login">Oluşturulacak User kaydının bilgisidir.</param>
        [HttpPost, Route("Authenticate")]
        public IActionResult Authenticate([FromBody] Login login)
        {
            var user = _UserRepository.FindBy(a =>
                                              a.Email == login.Email
                                           && a.Password == _UserRepository.PasswordHash(login.Password)
                                           && a.DataStatus == DataStatus.Activated)
                                      .Select(a => new
                                      {
                                          a.Id,
                                          a.Name,
                                          a.Surname,
                                          a.Photo,
                                          a.WorkYear,
                                          a.SicilId
                                      })
                                      .FirstOrDefault();

            if (user == null)
                return BadRequest(new Response(false, "Mevcut parolanız ile girdiğiniz parolanız eşleşmedi."));

            var userRoleId = _UserRoleRepository.FindBy(a => a.UserId == user.Id && a.DataStatus == DataStatus.Activated)
                    .Select(a => a.RoleId).Distinct().ToArray();
            var ipAdress = HttpContext.Connection.RemoteIpAddress.ToString();
            var responseLogin = new ResponseLogin
            {
                Token = _UserRepository.BuildToken(new Token { UserId = user.Id, UserRoleId = userRoleId, FullName = user.Name + " " + user.Surname }),
                LoginUser = new LoginUser
                {
                    Id = user.Id,
                    Name = user.Name,
                    Image = user.Photo,
                    Surname = user.Surname,
                    WorkYear = user.WorkYear,
                    IpAddress = ipAdress,
                    HostName = _UserRepository.GetHostName(ipAdress),
                    //FirstFireLink = _pagePermissionRepository.GetFisrtFireLink(userRoleId)
                    FirstFireLink = "/yonetim",
                    SicilId = user.SicilId
                }
            };

            _userSessionRepository.Add(new UserSession
            {
                UserId = user.Id,
                Token = responseLogin.Token,
                RemoteIpAddress = _customHttpContextAccessor.GetRemoteIpAddress(),
                RequestHeader = _customHttpContextAccessor.GetHeaders(),
                LoginAt = DateTime.Now
            });

            _userSessionRepository.Commit();

            return Ok(responseLogin);
        }


        [HttpGet, Route("LoginUser")]
        public LoginUser LoginUser()
        {
            var user = _UserRepository.FindBy(a => a.Id == _customHttpContextAccessor.GetUserId() && a.DataStatus != Entity.Shared.DataStatus.Deleted)
                                        .Select(a => new
                                        {
                                            a.Id,
                                            a.Name,
                                            a.Surname,
                                            a.Photo,
                                            a.WorkYear,
                                            SicilId = a.SicilId.Value,
                                        })
                                        .FirstOrDefault();

            var ipAdress = HttpContext.Connection.RemoteIpAddress.ToString();
            if (user != null)
            {
                var userRoleId = _UserRoleRepository.FindBy(a => a.UserId == user.Id && a.DataStatus == DataStatus.Activated)
                    .Select(a => a.RoleId).Distinct().ToArray();

                var destekEkipRolId = userRoleId.Where(m => m == 11).FirstOrDefault(); //destek ekiip rolId 11

                return new LoginUser
                {
                    Id = user.Id,
                    Name = user.Name,
                    Image = user.Photo,
                    Surname = user.Surname,
                    WorkYear = user.WorkYear,
                    IpAddress = ipAdress,
                    HostName = _UserRepository.GetHostName(ipAdress),
                    SicilId = user.SicilId,
                    UserRolId = destekEkipRolId != 0 ? destekEkipRolId : userRoleId.FirstOrDefault()
                };
            }

            return null;
        }


        /// <summary>
        /// Yeni User kaydını oluşturur.
        /// </summary>
        /// <param name="val">Oluşturulacak User kaydının bilgisidir.</param>
        [HttpPost, Route("Post")]
        public IActionResult Post([FromBody] User val)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            val.Password = _UserRepository.PasswordHash(val.Password);
            val.WorkYear = DateTime.Now.Year;
            val.FullName = val.Name + " " + val.Surname;

            var res = new DTO.Shared.DataAccessResult<object>();

            try
            {
                _UserRepository.Add(val);
                _UserRepository.Commit();
                res.Result = true;
                res.ResultMessage = "İşlem Başarılı";
                res.Object = val;
                return Ok(res);
            }
            catch (Exception ex)
            {
                res.Result = false;
                res.ResultMessage = "Sistemde bir hata oluştu lütfen yönetinizle görüşünüz";
                return BadRequest(res);
            }
        }

        /// <summary>
        /// User kaydını günceller.
        /// </summary>
        /// <param name="id">Guncellenecek User kaydının tekil bilgisidir.</param>
        /// <param name="val">Guncellenecek User kaydının bilgisidir.</param>
        [HttpPost, Route("Update/{id:int}")]
        public IActionResult Update(int id, [FromBody] User val)
        {
            var res = new DTO.Shared.DataAccessResult<object>();

            if (id == null)
            {
                res.Result = false;
                res.ResultMessage = "Sistemde bir hata oluştu lütfen yönetinizle görüşünüz";
                return BadRequest(res);
            }

            try
            {
                var entity = _UserRepository.FindBy(x => x.Id == id && x.DataStatus == Entity.Shared.DataStatus.Activated).FirstOrDefault();
                entity = val;
                _UserRepository.Update(entity);
                _UserRepository.Commit();
                res.Result = true;
                res.ResultMessage = "Kayıt Güncellendi";
                res.Object = entity;
                return Ok(res);
            }
            catch (Exception ex)
            {
                res.Result = false;
                res.ResultMessage = "Sistemde bir hata oluştu lütfen yönetinizle görüşünüz";
                return BadRequest(res);
            }

        }

        /// <summary>
        /// User kaydını siler.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete, Route("Delete/{id}")]
        [Produces("application/json")]
        public IActionResult Delete(int id)
        {

            var res = new DTO.Shared.DataAccessResult<object>();
            try
            {
                var entity = _UserRepository.GetSingle(x => x.Id == id);
                if (entity == null)
                {
                    res.Result = false;
                    res.ResultMessage = "Kayıt Bulunamadı";
                    return NotFound(res);
                }
                _UserRepository.Delete(entity);
                _UserRepository.Commit();
                res.Result = true;
                res.ResultMessage = "Kayıt Silindi";
                res.Object = entity;
                return NoContent();
            }
            catch (Exception ex)
            {
                res.Result = false;
                res.ResultMessage = "Sistemde bir hata oluştu lütfen yönetinizle görüşünüz";
                return BadRequest(res);
            }
        }

        /// <summary>
        /// Tüm DeActivated User verilerini getirir.
        /// </summary>
        [HttpGet, Route("GetAllDeactivatedUsers")]
        public IActionResult GetAllDeactivatedUsers()
        {
            return Ok(_UserRepository.FindBy(x => x.DataStatus == Entity.Shared.DataStatus.DeActivated).ToList());
        }
    }
}
