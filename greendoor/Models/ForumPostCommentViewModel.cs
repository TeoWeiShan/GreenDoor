using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace greendoor.Models
{
    public class ForumPostCommentViewModel
    {
        public int CustomerID { get; set; }
        public int ForumPostID { get; set; }

        public int ForumCommentsID { get; set; }

        public int ShopID { get; set; }

        [Display(Name = "Post Name")]
        [Required(ErrorMessage = "Please enter a name for this post!")]
        [StringLength(50, ErrorMessage = "Name must not exceed 50 characters")]
        public string PostName { get; set; }

        [Display(Name = "Post Description")]
        [Required(ErrorMessage = "Please enter a description for this post!")]
        [StringLength(4000, ErrorMessage = "Description must not exceed 4000 characters")]
        public string PostDescription { get; set; }
        public DateTime DateTimePosted { get; set; }
         
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter a name!")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters!")]
        public string CustomerName { get; set; }

        [Display(Name = "E-mail Address")]
        [EmailAddress]
        [ValidateEmailExists]
        [Required(ErrorMessage = "Please enter an email address!")]
        [StringLength(50, ErrorMessage = "Email cannot exceed 50 characters!")]
        public string EmailAddr { get; set; }

        [Display(Name = "Comments Description")]
        [Required(ErrorMessage = "Please enter a comment!")]
        [StringLength(2000, ErrorMessage = "Comments must not exceed 2000 characters")]
        public string CommentsDescription { get; set; }

        [Display(Name = "Shop Name")]
        [Required(ErrorMessage = "Please enter a name!")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters!")]
        public string ShopName { get; set; }

        public List<ForumPostCommentViewModel> CustomerCommentsList { get; set; }

        public List<ForumPostCommentViewModel> ShopCommentsList { get; set; }
        public List<ForumPostCommentViewModel> CustomerPostsList { get; set; }

        public List<ForumPostCommentViewModel> ShopPostsList { get; set; }

        public List<ForumPostCommentViewModel> ShopPostIDList { get; set; }

        public List<ForumPostCommentViewModel> CustomerPostIDList { get; set; }

        public ForumPostCommentViewModel()
        {
            CustomerCommentsList = new List<ForumPostCommentViewModel>();
            ShopCommentsList = new List<ForumPostCommentViewModel>();
            CustomerPostsList = new List<ForumPostCommentViewModel>();
            ShopPostsList = new List<ForumPostCommentViewModel>();
            ShopPostIDList = new List<ForumPostCommentViewModel>();
            CustomerPostIDList = new List<ForumPostCommentViewModel>();
        }
    }
}
