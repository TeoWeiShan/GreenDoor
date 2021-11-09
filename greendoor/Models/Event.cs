using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace greendoor.Models
{
    public class Event
    {
        [Display(Name = "Event ID")]
        public int EventID { get; set; }
        

        [Required(ErrorMessage = "Please enter a name!")]
        [Display(Name = "Name of competition")]
        [StringLength(255, ErrorMessage = "Name cannot exceed 255 characters")]
        public string EventName { get; set; }
        
        [Display(Name = "Event description")]
        public string EventDescription { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Date posted")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime DateTimePosted { get; set; }
    }
}
