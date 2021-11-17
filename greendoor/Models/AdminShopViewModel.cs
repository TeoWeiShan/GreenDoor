using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace greendoor.Models
{
    public class AdminShopViewModel
    {
        [Display(Name = "Shop ID")]
        public int ShopID { get; set; }

        [Display(Name = "Profile Photo")]
        [StringLength(255, ErrorMessage = "File name cannot exceed 255 characters!")]
        public string ShopPicture { get; set; }

        [Display(Name = "Shop Name")]
        [Required(ErrorMessage = "Please enter a name!")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters!")]
        public string ShopName { get; set; }

        [Display(Name = "Description")]
        [StringLength(250, ErrorMessage = "Description cannot exceed 250 characters!")]
        public string ShopDescription { get; set; }
        public string Zone { get; set; }
        [Display(Name = "Contact Number")]
        [Required(ErrorMessage = "Contact Number must not be empty")]
        public int ContactNumber { get; set; }

        [Required(ErrorMessage = "Address must not be empty")]
        public string Address { get; set; }
        [Display(Name = "Postal Code")]
        [Required(ErrorMessage = "Please enter the postal code")]
        [RegularExpression(@"^[0-9]{6}$", ErrorMessage = "Please enter a 6-digit Postal Code")]
        public int PostalCode { get; set; }

        [Display(Name = "Website URL")]
        public string WebsiteLink { get; set; }

        [Display(Name = "Social Media URL")]
        public string SocialMediaLink { get; set; }


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

        public List<AdminShopViewModel> shopList { get; set; }
        public List<Reviews> reviewsList { get; set; }
        public AdminShopViewModel()
        {
            shopList = new List<AdminShopViewModel>();
            reviewsList = new List<Reviews>();
        }
    }
}