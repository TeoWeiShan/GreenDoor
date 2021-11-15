using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace greendoor.Models
{
    public class Reviews
    {
        [Display(Name = "Reviews ID")]
        public int ReviewsID { get; set; }

        public int? Rating { get; set; }

        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Display(Name = "Shop ID")]
        public int ShopID { get; set; }

        [Display(Name = "Post ID")]
        public int ForumPostID { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Please enter a comment!")]
        [StringLength(150, ErrorMessage = "Description must not exceed 150 characters")]
        public string Description { get; set; }

        [Display(Name = "Date Posted")]
        public DateTime DateTimePosted { get; set; }

    }
}