using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace greendoor.Models
{
    public class AdminForumPostViewModel
    {
        public int CustomerID { get; set; }
        public int ForumPostID { get; set; }

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

        public List<AdminForumPostViewModel> forumPostList { get; set; }

        public AdminForumPostViewModel()
        {
            forumPostList = new List<AdminForumPostViewModel>();
        }

    }
}
