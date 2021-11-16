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
            List<AdminShopViewModel> shopList = adCtx.GetAllShop();
            return View(shopList);
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
            ShopVM = adCtx.GetShopDetails(ShopVM.ShopID);
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
            List<AdminEventViewModel> eventList = adCtx.GetAllEvents();
            return View(eventList);
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

