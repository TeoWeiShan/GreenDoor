using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace greendoor.Models
{
    public class Favourite
    {
        public int FavouriteID { get; set; }
        public int CustomerID { get; set; }
        public int ShopID { get; set; }
    }
}