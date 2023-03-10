using greendoor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;
using System.Threading.Tasks;
using greendoor.DAL;

namespace greendoor.Controllers
{
    public class CustomerController : Controller
    {
        private CustomerDAL custCtx = new CustomerDAL();
        private ShopDAL shopContext = new ShopDAL();
        private ReviewsDAL reviewContext = new ReviewsDAL();
        private EventDAL eContext = new EventDAL();
        private FavouriteDAL favouriteContext = new FavouriteDAL();

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Profile()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            
            Customer cust = new Customer();
            cust.CustomerID = (int)HttpContext.Session.GetInt32("LoginID");
            cust = custCtx.GetDetails(cust.CustomerID);

            FavouriteShopViewModel favShopVM = new FavouriteShopViewModel();
            favShopVM.ShopList = favouriteContext.GetAllFavShop(cust.CustomerID);
            favShopVM.CustomerID = cust.CustomerID;
            favShopVM.CustomerName = cust.CustomerName;
            favShopVM.Credibility = cust.Credibility;
            favShopVM.EmailAddr = cust.EmailAddr;
            favShopVM.Password = cust.Password;


            return View(favShopVM);
        }

        public ActionResult UpdateProfile()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            Customer cust = new Customer();
            cust = custCtx.GetDetails((int)HttpContext.Session.GetInt32("LoginID"));
            return View(cust);
        }

        public ActionResult DeleteProfile()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            Customer cust = new Customer();
            cust = custCtx.GetDetails((int)HttpContext.Session.GetInt32("LoginID"));
            return View(cust);
        }

        public ActionResult ViewEvents()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<EcoEvents> eList = eContext.GetAllEvent();
            return View(eList);
        }

        public ActionResult EventDetails(int EventID)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            EcoEvents e = eContext.GetDetails(EventID);
            if (e.EventName == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(e);
        }

        public ActionResult About()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Customer cust)
        {
            //Add judge record to database
            custCtx.Update(cust);
            //Redirect user to Judge/Create View
            return RedirectToAction("Profile", "Customer");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Customer cust)
        {
            custCtx.Delete(cust);
            HttpContext.Session.Remove("Role");
            HttpContext.Session.Remove("LoginID");
            //Redirect user to Judge/ Create View
            return RedirectToAction("Index", "Home");
        }
    }
}
