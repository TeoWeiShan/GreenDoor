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
        private ForumDAL forumPostContext = new ForumDAL();
        private EventDAL eContext = new EventDAL();

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
            cust = custCtx.GetDetails((int)HttpContext.Session.GetInt32("LoginID"));
            return View(cust);
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

        public ActionResult ViewShops()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<Shop> shopList = shopContext.GetAllShop();
            return View(shopList);
        }

        public ActionResult ShopDetails(int ShopID)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            ShopReviewViewModel shopreviewVM = new ShopReviewViewModel();
            //Get details of competition
            shopreviewVM = custCtx.ShopDetails(ShopID);
            shopreviewVM.reviewsList = reviewContext.GetAllReviews(ShopID);
            shopreviewVM.CustomerID = (int)HttpContext.Session.GetInt32("LoginID");
            shopreviewVM.CustomerName = (custCtx.GetDetails(shopreviewVM.CustomerID)).CustomerName;
            return View(shopreviewVM);
        }

        public ActionResult ViewForum()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            ForumPostCommentViewModel fpcVM = new ForumPostCommentViewModel();
            fpcVM.PostsList = forumPostContext.GetAllForumPostVM();
            return View(fpcVM);
        }

        public ActionResult CreatePost()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            ForumPostCommentViewModel fpcVM = new ForumPostCommentViewModel();
            fpcVM.CustomerID = (int)HttpContext.Session.GetInt32("LoginID");
            fpcVM.CustomerName =(custCtx.GetDetails(fpcVM.CustomerID)).CustomerName;
            return View(fpcVM);
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
            //Redirect user to Judge/ Create View
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePost(ForumPostCommentViewModel forumPost)
        {
            forumPost.CustomerID = (int)HttpContext.Session.GetInt32("LoginID");
            forumPost.DateTimePosted = DateTime.Now;
            //Add ForumPost record to database 
            forumPost.ForumPostID =  forumPostContext.Add(forumPost);

            //return to the Sucess view to display success message
            return RedirectToAction("ViewForum","Customer");
        }
    }
}
