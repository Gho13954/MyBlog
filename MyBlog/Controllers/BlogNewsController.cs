using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.IService;
using MyBlog.Model;
using MyBlog.Model.DTO;
using MyBlog.Utility.ApiResult;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BlogNewsController : ControllerBase
    {
        private readonly IBlogNewsService _iblogNewsService;
        public BlogNewsController(IBlogNewsService iBlogNewsService)
        {
            this._iblogNewsService = iBlogNewsService;
        }
        [HttpGet("BlogNews")]
        public async Task<ActionResult<ApiResult>> GetBlogNews()
        {
            int id = Convert.ToInt32(this.User.FindFirst("Id").Value);//2;
            var data=await _iblogNewsService.QueryAsync(c=>c.WriterId==id);
            if (data == null)
                return ApiResultHelper.Error("没有更多的文章");
            return ApiResultHelper.Success(data);
        }
        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost("Create")]//使用 [FromBody] 特性来告诉 ASP.NET Core 将请求的 JSON 数据反序列化为 CreateBlogNewsDTO 对象
        public async Task<ActionResult<ApiResult>> Create([FromBody] BlogNewsDTO dto)// string title,string content,int typeid
        {
            BlogNews blogNews = new BlogNews
            {
                BrowseCount = 0,
                Content = dto.Content,
                LikeCount = 0,
                Time = DateTime.Now,
                Title = dto.Title,
                TypeId = dto.TypeId,
                WriterId = Convert.ToInt32(this.User.FindFirst("Id").Value) //2
            };
            bool b= await _iblogNewsService.CreateAsync(blogNews);
            if (!b)
                return ApiResultHelper.Error("添加失败，服务器发生错误");
            return ApiResultHelper.Success(blogNews);
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult<ApiResult>> Delete(int id)
        {
            bool b=await _iblogNewsService.DeleteAsync(id);
            if (!b) return ApiResultHelper.Error("删除失败");
            return ApiResultHelper.Success(b);
        }
        [HttpPut("Edit")]
        public async Task<ActionResult<ApiResult>> Edit([FromBody] BlogNewsDTO dto)//int id,string title,string content,int typeid
        {
            var blogNews = await _iblogNewsService.FindAsync(dto.ID);
            if (blogNews == null) return ApiResultHelper.Error("没有找到该文章");
            blogNews.Title = dto.Title;
            blogNews.Content = dto.Content;
            blogNews.TypeId = dto.TypeId;
            bool b=await _iblogNewsService.EditAsync(blogNews);
            if (!b) return ApiResultHelper.Error("修改失败");
            return ApiResultHelper.Success(blogNews);
        }
        [HttpGet("BlogNewsPage")]
        public async Task<ApiResult> GetBlogNewsPage([FromServices] IMapper imapper,int page,int size)
        {
            RefAsync<int> total = 0;
            var blognews =await _iblogNewsService.QueryAsync(page, size, total);
            try
            {
                var blognewsDTO = imapper.Map<List<BlogNewsDTO>>(blognews);
                return ApiResultHelper.Success(blognewsDTO,total);
            }
            catch (Exception)
            {
                return ApiResultHelper.Error("AutoMapper映射错误");
            }
        }
    }
}
