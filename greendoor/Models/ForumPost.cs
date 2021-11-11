using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace greendoor.Models
{
    public class ForumPost
    {
        public int CustomerID { get; set; }
        public int ForumPostID { get; set; }
        public string PostName { get; set; }
        public string PostDescription { get; set; }
        public DateTime DateTimePosted { get; set; }

    }
}
