using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace MyBlog.Model
{
    public class Baseid
    {
        [SugarColumn(IsIdentity =true, IsPrimaryKey=true)]
        public int Id {get;set;}
    }
}
