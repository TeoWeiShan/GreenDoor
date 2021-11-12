using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace greendoor.Models
{
    public class ForumComments
    {
        [Display(Name = "Comment ID")]
        public int ForumCommentsID { get; set; }

        [Display(Name = "Customer ID")]
        public int CustomerID { get; set; }

        [Display(Name = "Shop ID")]
        public int ShopID { get; set; }

        [Display(Name = "Post ID")]
        public int ForumPostID { get; set; }

        [Display(Name = "Comments Description")]
        [Required(ErrorMessage = "Please enter a comment!")]
        [StringLength(2000, ErrorMessage = "Comments must not exceed 2000 characters")]
        public string CommentsDescription { get; set; }

        [Display(Name = "Date Posted")]
        public DateTime DateTimePosted { get; set; }

    }
}