using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using greendoor.DAL;
using greendoor.Models;

namespace greendoor.Controllers
{
    public class ShopController : Controller
    {
        private ShopDAL shopContext = new ShopDAL();
        public IActionResult Index()
        {
            List<Shop> shopList = shopContext.GetAllShop();
            return View(shopList);
        }

        // GET: ShopController/Details/5
        public ActionResult Details(int id)
        {
            if (id == null)
            { //Query string parameter not provided
              //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            //Get details of competition
            Shop shop = shopContext.GetDetails(id);
            //If query id not in db, redirect out
            if (shop.ShopName == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(shop);
        }

    }
}
