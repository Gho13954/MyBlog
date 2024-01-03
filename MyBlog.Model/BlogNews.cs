using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace MyBlog.Model
{
    public class BlogNews : Baseid
    {
        [SugarColumn(ColumnDataType = "nvarchar(30)")]//nvarchar带中文比较好
        public String Title { get; set; }
        [SugarColumn(ColumnDataType = "text")]
        public string Content { get; set; }
        public DateTime Time { get; set; }

        public int TypeId { get; set; }
        public int BrowseCount { get; set; }

        public int LikeCount { get; set; }
        public int WriterId { get; set; }
        [SugarColumn(IsIgnore =true)]
        public TypeInfo TypeInfo { get; set; }
        [SugarColumn(IsIgnore = true)]
        public WriterInfo WriteInfo { get; set; }

    }
}
