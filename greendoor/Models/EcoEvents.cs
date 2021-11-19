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

        public int CheckShopID { get; set; }

        [Display(Name ="Event Name")]
        [Required(ErrorMessage = "Please enter a name!")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters!")]
        public string EventName { get; set; }

        [Display(Name = "Event Description")]
        [Required(ErrorMessage = "Please enter a description!")]
        [StringLength(4000, ErrorMessage = "Description must not be longer than 4000 words")]
        public string EventDescription { get; set; }

        [Display(Name = "Date Posted")]
        public DateTime DateTimePosted { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name ="End Date")]
        public DateTime EndDate { get; set; }

        public bool isShop { get; set; }

        public string ShopName { get; set; }
    }
}