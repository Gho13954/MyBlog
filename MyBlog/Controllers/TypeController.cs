using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.IService;
using MyBlog.Model;
using MyBlog.Model.DTO;
using MyBlog.Utility.ApiResult;
using System.Threading.Tasks;

namespace MyBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TypeController : ControllerBase
    {
        private readonly ITypeInfoService _typeInfoService;
        public TypeController(ITypeInfoService typeInfoService)
        {
            this._typeInfoService = typeInfoService;
        }
        [HttpGet("Types")]
        public async Task<ApiResult> Types()
        {
            var types = await _typeInfoService.QueryAsync();
            if (types.Count==0) return ApiResultHelper.Error("没有更多的类型");
            return ApiResultHelper.Success(types);
        }

        [HttpPost("Create")]
        public async Task<ApiResult> Create([FromBody]TypeDTO dto)
        {
            #region 数据验证
            if (string.IsNullOrWhiteSpace(dto.Name))
                return ApiResultHelper.Error("类型名不能为空");
            #endregion
            TypeInfo type = new TypeInfo
            {
                Name = dto.Name
            };
            bool b=await _typeInfoService.CreateAsync(type);
            if (!b) return ApiResultHelper.Error("添加失败");
            return ApiResultHelper.Success(b);
        }
        [HttpPut("Edit")]
        public async Task<ApiResult> Edit([FromBody] TypeDTO dto)
        {
            var type = await _typeInfoService.FindAsync(dto.ID);
            if (type == null) return ApiResultHelper.Error("没有找到该文章类型");
            type.Name = dto.Name;
            bool b=await _typeInfoService.EditAsync(type);
            if (!b) return ApiResultHelper.Error("修改失败");
            return ApiResultHelper.Success(type);  
        }

        [HttpDelete("Delete")]
        public async Task<ApiResult> Delete(int id)
        {
            bool b=await _typeInfoService.DeleteAsync(id);
            if (!b) return ApiResultHelper.Error("删除失败");
            return ApiResultHelper.Success(b);
        }
    }
}
