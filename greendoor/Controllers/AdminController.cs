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
        private ShopPostDAL shopPostContext = new ShopPostDAL();
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
            ShopVM = adCtx.GetShopDetails(ShopVM.ShopID);
            ShopVM.reviewsList = reviewContext.GetLatestReview(ShopID);
            ShopVM.shopPostList = shopPostContext.GetLatestShopPost(ShopID);
            return View(ShopVM);
        }
        public ActionResult ViewPosts(int id) // customer and public
        {
            AdminShopViewModel shopPost = new AdminShopViewModel();
            shopPost.shopPostList = shopPostContext.GetAllShopPost(id);
            shopPost.ShopID = id;
            return View(shopPost);
        }

        public ActionResult ViewReviews(int id) // customer and public
        {
            AdminShopViewModel reviews = new AdminShopViewModel();
            reviews.reviewsList = reviewContext.GetAllReviews(id);
            reviews.ShopID = id;
            return View(reviews);
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

        public ActionResult EventDetails(int eventID)
        {
            AdminEventViewModel aeVM = new AdminEventViewModel();
            aeVM.EventID = eventID;
            aeVM = adCtx.GetEventDetails(aeVM.EventID);
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
            AdminForumViewModel afpVM = new AdminForumViewModel();
            afpVM.CustomerPostsList = adCtx.CustomerPostList();
            afpVM.ShopPostsList = adCtx.ShopPostList();
            return View(afpVM);
        }



        public ActionResult About()
        {
            return View();
        }
    }
}

