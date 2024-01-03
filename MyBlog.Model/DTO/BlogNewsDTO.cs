using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Model.DTO
{
    public class BlogNewsDTO
    {
        public int ID { get; set; }
        public String Title { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public int BrowseCount { get; set; }//浏览量
        public int LikeCount { get; set; }//点赞量
        public int TypeId { get; set; }
        public int WriterId { get; set; }
        public string TypeName {  get; set; }
        public string WriterName { get; set; }
    }
}
