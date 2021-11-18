using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace greendoor.Models
{
    public class EditPhotoViewModel
    {
        [Display(Name = "Profile Photo")]
        [StringLength(255, ErrorMessage = "File name cannot exceed 255 characters!")]
        public string ShopPicture { get; set; }

        [Required(ErrorMessage = "Please input a shop photo!")]
        public IFormFile PhotoFile { get; set; }

        [Display(Name = "Shop Name")]
        [Required(ErrorMessage = "Please enter a name!")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters!")]
        public string ShopName { get; set; }
    }
}
