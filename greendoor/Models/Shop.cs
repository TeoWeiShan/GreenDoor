using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace greendoor.Models
{
    public class Shop
    {
        [Display(Name = "Shop ID")]
        public int ShopID { get; set; }
        [Display(Name = "Profile Picture")]
        [StringLength(255, ErrorMessage = "Filename cannot exceed 255 characters!")]
        public string ShopPicture { get; set; }

        [Display(Name = "Shop Name")]
        [Required(ErrorMessage = "Please enter a name!")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters!")]
        public string ShopName { get; set; }

        [Display(Name = "Description")] 
        [Required(ErrorMessage = "Please enter a description")]
        [StringLength(150, ErrorMessage = "Name cannot exceed 50 characters!")]
        public string ShopDescription { get; set; }
        public string Zone { get; set; }
        public int ContactNumber { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }


        [Display(Name = "E-mail Address")]
        [EmailAddress]
        // Custom Validation Attribute for checking email address exists
        //[ValidateJudgeEmail]
        [Required(ErrorMessage = "Please enter an email address!")]
        [StringLength(50, ErrorMessage = "Email cannot exceed 50 characters!")]
        public string EmailAddr { get; set; }
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter a password!")]
        [StringLength(255, ErrorMessage = "Password cannot exceed 255 characters!")]
        public string Password { get; set; }
        public bool IsVerified { get; set; }
    }
}