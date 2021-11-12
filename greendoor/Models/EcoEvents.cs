using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace greendoor.Models
{
    public class EcoEvents
    {
        [Display(Name ="Event ID")]
        public int EventID { get; set;  }

        [Display(Name ="Shop ID")]
        public int ShopID { get; set; }

        [Display(Name ="Event Name")]
        [Required(ErrorMessage = "Please enter a name!")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters!")]
        public string EventName { get; set; }

        [Display(Name = "Event Description")]
        [Required(ErrorMessage = "Please enter a description!")]
        [StringLength(50, ErrorMessage = "Description must not be longer than 3000 words")]
        public string EventName { get; set; }
    }
}