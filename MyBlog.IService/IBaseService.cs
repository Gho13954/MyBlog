using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.IService
{
    public interface IBaseService<TEntity> where TEntity:class,new()
    {
        Task<bool> CreateAsync(TEntity entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> EditAsync(TEntity entity);
        Task<TEntity> FindAsync(int id);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> func);

        Task<List<TEntity>> QueryAsync();//查询全部的数据
        Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> func);//自定义条件查询
        Task<List<TEntity>> QueryAsync(int page, int size, RefAsync<int> total);//分页查询
        Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> func, int page, int size, RefAsync<int> total);//自定义分页查询
    }
}
