using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyBlog.IService;
using MyBlog.JWT.Utility._MD5;
using MyBlog.JWT.Utility.ApiResult;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using System.Threading.Tasks;
using MyBlog.Model.DTO;

namespace MyBlog.JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthoizeController : ControllerBase
    {
        private readonly IWriterInfoService _iwriterInfoService;
        public AuthoizeController(IWriterInfoService iwriterInfoService)
        {
            _iwriterInfoService = iwriterInfoService;
        }
        [HttpPost("Login")]
        public async Task<ApiResult> Login([FromBody] WriterDTO dto)//string UserName,string Password
        {
            //加密后的密码
            string pwd = MD5Helper.MD5Encrypt32(dto.Password);
            //数据校验
            var writer =await _iwriterInfoService.FindAsync(c => c.UserName == dto.UserName
            && c.UserPwd == pwd);
            if (writer != null) 
            {
                //登陆成功
                var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, writer.Name),
                new Claim("Id",writer.Id.ToString()),
                new Claim("UserName",writer.UserName)
                //不能放敏感信息
            };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SDMC-CJAS1-SAD-DFSFA-SADHJVF-VF"));
                //issuer代表颁发Token的Web应用程序，audience是Token的受理者
                var token = new JwtSecurityToken(
                    issuer: "http://localhost:6060",
                    audience: "http://localhost:5000",
                    claims: claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddHours(1),//过期时间
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                return ApiResultHelper.Success(jwtToken);
            }
            else
            {
                return ApiResultHelper.Error("账号或密码错误");
            }
        }
    }
}
