using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using greendoor.DAL;
using greendoor.Models;

namespace greendoor.Controllers
{
    public class AdminController : Controller
    {
        private AdminDAL adCtx = new AdminDAL();
        private ReviewsDAL reviewContext = new ReviewsDAL();
        public IActionResult Index()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult ManageShops()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            AdminShopViewModel asVM = new AdminShopViewModel();
            asVM.shopList = adCtx.GetAllShop();
            if(asVM.shopList.Count == 0)
            {
                ViewData["noShop"] = "No Shop available";
            }
            return View(asVM);
        }

        public ActionResult ShopDetails(int ShopID)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            AdminShopViewModel ShopVM = new AdminShopViewModel();
            ShopVM.ShopID = ShopID;
            ShopVM.ShopName = (adCtx.GetShopDetails(ShopVM.ShopID)).ShopName;
            ShopVM.ShopDescription = (adCtx.GetShopDetails(ShopVM.ShopID)).ShopDescription;
            ShopVM.Zone = (adCtx.GetShopDetails(ShopVM.ShopID)).Zone;
            ShopVM.ContactNumber = (adCtx.GetShopDetails(ShopVM.ShopID)).ContactNumber;
            ShopVM.Address = (adCtx.GetShopDetails(ShopVM.ShopID)).Address;
            ShopVM.PostalCode = (adCtx.GetShopDetails(ShopVM.ShopID)).PostalCode;
            ShopVM.WebsiteLink = (adCtx.GetShopDetails(ShopVM.ShopID)).WebsiteLink;
            ShopVM.SocialMediaLink = (adCtx.GetShopDetails(ShopVM.ShopID)).SocialMediaLink;
            ShopVM.EmailAddr = (adCtx.GetShopDetails(ShopVM.ShopID)).EmailAddr;
            ShopVM.reviewsList = reviewContext.GetAllReviews(ShopID);
            return View(ShopVM);
        }

        public ActionResult ShopUpdate(int ShopID)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            AdminShopViewModel ShopVM = new AdminShopViewModel();
            ShopVM.ShopID = ShopID;
            ShopVM = adCtx.GetShopDetails(ShopVM.ShopID);
            return View(ShopVM);
        }

        public ActionResult ShopDelete(int ShopID)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            AdminShopViewModel ShopVM = new AdminShopViewModel();
            ShopVM.ShopID = ShopID;
            ShopVM = adCtx.GetShopDetails(ShopVM.ShopID);
            return View(ShopVM);
        }

        public ActionResult ManageEvents()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            AdminEventViewModel aeVM = new AdminEventViewModel();
            aeVM.eventsList = adCtx.GetAllEvents();
            if (aeVM.eventsList.Count == 0)
            {
                ViewData["noEvents"] = "No Event available";
            }
            return View(aeVM);
        }

        public ActionResult EventUpdate(int eventID)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            AdminEventViewModel eventVM = new AdminEventViewModel();
            eventVM.EventID = eventID;
            eventVM = adCtx.GetEventDetails(eventVM.EventID);
            return View(eventVM);
        }

        public ActionResult EventDelete(int eventID)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            AdminEventViewModel eventVM = new AdminEventViewModel();
            eventVM.EventID = eventID;
            eventVM = adCtx.GetEventDetails(eventVM.EventID);
            return View(eventVM);
        }

        public ActionResult ManageForum()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}

