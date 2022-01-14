using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace greendoor.Models
{
    public class ShopFav
    {
        [Display(Name = "Customer ID")]
        public int? CustomerID { get; set; }

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
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0-9]{8}$", ErrorMessage = "Please enter a 8-digit Phone Number")]
        [Required(ErrorMessage = "Contact Number must not be empty")]
        public int ContactNumber { get; set; }

        [StringLength(500, ErrorMessage = "Email cannot exceed 50 characters!")]
        [Required(ErrorMessage = "Address must not be empty")]
        public string Address { get; set; }

        [Display(Name = "Postal Code")]
        [Required(ErrorMessage = "Please enter the postal code")]
        [RegularExpression(@"^[0-9]{6}$", ErrorMessage = "Please enter a 6-digit Postal Code")]
        public int PostalCode { get; set; }

        [Display(Name = "Website URL")]
        [StringLength(300, ErrorMessage = "Email cannot exceed 300 characters!")]
        public string? WebsiteLink { get; set; }

        [Display(Name = "Social Media URL")]
        [StringLength(300, ErrorMessage = "Email cannot exceed 300 characters!")]
        public string? SocialMediaLink { get; set; }

        [Display(Name = "E-mail Address")]
        [EmailAddress]
        // Custom Validation Attribute for checking email address exists
        [ValidateEmailExists]
        [Required(ErrorMessage = "Please enter an email address!")]
        [StringLength(50, ErrorMessage = "Email cannot exceed 50 characters!")]
        public string EmailAddr { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter a password!")]
        [StringLength(255, ErrorMessage = "Password cannot exceed 255 characters!")]
        public string Password { get; set; }
        public bool IsVerified { get; set; }

        public bool FavBool { get; set; }
    }
}
