using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.IService;
using MyBlog.Model;
using MyBlog.Model.DTO;
using MyBlog.Utility._MD5;
using MyBlog.Utility.ApiResult;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WriterInfoController : ControllerBase
    {
        private readonly IWriterInfoService _iwriterInfoService;
        public WriterInfoController(IWriterInfoService writerInfoService)
        {
            _iwriterInfoService = writerInfoService;
        }
        [HttpPost("Create")]
        public async Task<ApiResult> Create(string name,string username,string userpwd)
        {
            //数据校验
            WriterInfo writer = new WriterInfo
            {
                Name = name,
                //加密密码
                UserPwd = MD5Helper.MD5Encrypt32(userpwd),
                UserName = username
            };
            //判断数据库中是否已经存在账号跟要添加的账号相同的数据
            var oldWriter = await _iwriterInfoService.FindAsync(c => c.UserName == username);
            if (oldWriter != null) return ApiResultHelper.Error("账号已经存在");
            bool b =await _iwriterInfoService.CreateAsync(writer);
            if (!b) return ApiResultHelper.Error("添加失败");
            return ApiResultHelper.Success(writer);
        }
        [HttpPut("EditName")]
        public async Task<ApiResult> EditName([FromBody] WriterDTO dto)
        {
            int id = Convert.ToInt32(this.User.FindFirst("Id").Value);
            var writer =await _iwriterInfoService.FindAsync(id);
            writer.Name = dto.Name;
            bool b=await _iwriterInfoService.EditAsync(writer);
            if (!b) return ApiResultHelper.Error("修改失败");
            return ApiResultHelper.Success("修改成功");
        }
        [HttpPut("EditPwd")]
        public async Task<ApiResult> EditPwd([FromBody] WriterDTO dto)
        {
            string md5Pwd = MD5Helper.MD5Encrypt32(dto.Password);
            int id = Convert.ToInt32(this.User.FindFirst("Id").Value);
            var writer = await _iwriterInfoService.FindAsync(id);
            writer.UserPwd = md5Pwd;
            bool b = await _iwriterInfoService.EditAsync(writer);
            if (!b) return ApiResultHelper.Error("修改失败");
            return ApiResultHelper.Success("修改成功");
        }
        [HttpGet("FindMe")]
        public async Task<ApiResult> FindWriter([FromServices] IMapper iMapper)
        {
            int id = Convert.ToInt32(this.User.FindFirst("Id").Value);
            var writer = await _iwriterInfoService.FindAsync(id);
            var writerDTO = iMapper.Map<WriterDTO>(writer);
            return ApiResultHelper.Success(writerDTO);
        }
        [AllowAnonymous]
        [HttpGet("FindWriter")]
        public async Task<ApiResult> FindWriter([FromServices]IMapper iMapper,int id)
        {
            var writer =await _iwriterInfoService.FindAsync(id);
            var writerDTO = iMapper.Map<WriterDTO>(writer);
            return ApiResultHelper.Success(writerDTO);
        }
    }
}
