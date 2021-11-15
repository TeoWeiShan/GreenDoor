using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace greendoor.Models
{
    public class AdminForumCommentsViewModel
    {
        [Display(Name = "Comment ID")]
        public int ForumCommentsID { get; set; }

        [Display(Name = "Customer ID")]
        public int CustomerID { get; set; }

        [Display(Name = "Shop ID")]
        public int ShopID { get; set; }

        [Display(Name = "Post ID")]
        public int ForumPostID { get; set; }

        [Display(Name = "Post Name")]
        [Required(ErrorMessage = "Please enter a name for this post!")]
        [StringLength(50, ErrorMessage = "Name must not exceed 50 characters")]
        public string PostName { get; set; }

        [Display(Name = "Post Description")]
        [Required(ErrorMessage = "Please enter a description for this post!")]
        [StringLength(4000, ErrorMessage = "Description must not exceed 4000 characters")]
        public string PostDescription { get; set; }

        [Display(Name = "Comments Description")]
        [Required(ErrorMessage = "Please enter a comment!")]
        [StringLength(2000, ErrorMessage = "Comments must not exceed 2000 characters")]
        public string CommentsDescription { get; set; }

        [Display(Name = "Date Posted")]
        public DateTime DateTimePosted { get; set; }

    }
}
