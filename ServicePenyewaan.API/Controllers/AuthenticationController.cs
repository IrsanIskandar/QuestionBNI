using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicePenyewaan.API.Models;
using ServicePenyewaan.API.Models.Requests;
using ServicePenyewaan.API.Models.Response;
using ServicePenyewaan.API.ServiceHelpers;
using System;

namespace ServicePenyewaan.API.Controllers
{
    [Route("api/v1/authenticate")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        [HttpPost]
        [Route("auth/login")]
        public IActionResult LoginAuth([FromBody]LoginViewModel request)
        {
            ResponLoginViewModel respon = new ResponLoginViewModel();
            Users user = new Users()
            {
                UserId = 1,
                UserName = "Admin",
                UserEmail = "admin@mail.com",
                UserPassword = "123456",
                Role = "Admin"
            };

            if (request == null)
                return BadRequest();

            if (!string.IsNullOrEmpty(request.Username) && (request.Username == "Admin" && request.Password == "123456"))
            {
                string writeTokenBased = WriteTokenBase.GenerateToken(user);
                TokenBased tokenBased = new TokenBased();
                tokenBased.Access_Token = writeTokenBased;
                tokenBased.Expired_Token = "3600";
                tokenBased.UserDetail = user;

                respon.StatusCode = 200;
                respon.Message = "Ok";
                respon.Data = tokenBased;

                return Ok(respon);
            }
            else
            {
                respon.StatusCode = 500;
                respon.Message = "Internal Error";
                respon.Data = null;

                return Ok(respon);
            }
        }
    }
}
