using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace greendoor.Models
{
    public class AdminForumViewModel
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

        [Display(Name = "Customer Name")]
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

        public bool ShopCheck { get; set; }

        public List<AdminForumViewModel> CustomerCommentsList { get; set; }

        public List<AdminForumViewModel> ShopCommentsList { get; set; }
        public List<AdminForumViewModel> CustomerPostsList { get; set; }

        public List<AdminForumViewModel> ShopPostsList { get; set; }

        public List<AdminForumViewModel> ShopPostIDList { get; set; }

        public List<AdminForumViewModel> CustomerPostIDList { get; set; }

        public List<AdminForumViewModel> ShopCommentIDList { get; set; }

        public List<AdminForumViewModel> CustomerCommentIDList { get; set; }
        public AdminForumViewModel()
        {
            CustomerCommentsList = new List<AdminForumViewModel>();
            ShopCommentsList = new List<AdminForumViewModel>();
            CustomerPostsList = new List<AdminForumViewModel>();
            ShopPostsList = new List<AdminForumViewModel>();
            ShopPostIDList = new List<AdminForumViewModel>();
            CustomerPostIDList = new List<AdminForumViewModel>();
            ShopCommentIDList = new List<AdminForumViewModel>();
            CustomerCommentIDList = new List<AdminForumViewModel>();
        }

    }
}
