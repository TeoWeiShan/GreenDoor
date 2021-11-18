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

        public ActionResult ShopPostDelete(int ShopPostID)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            AdminShopViewModel asVM = new AdminShopViewModel();
            asVM = adCtx.InShopPostDetails(ShopPostID);
            return View(asVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShopPostDelete(AdminShopViewModel asVM)
        {
            //Add judge record to database
            adCtx.InShopPostDelete(asVM);
            //Redirect user to Judge/Create View
            return RedirectToAction("ShopDetails", "Admin",asVM);
        }

        public ActionResult ShopReviewDelete(int ReviewsID)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            AdminShopViewModel asVM = new AdminShopViewModel();
            asVM = adCtx.InShopReviewDetails(ReviewsID);
            return View(asVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShopReviewDelete(AdminShopViewModel asVM)
        {
            //Add judge record to database
            adCtx.InShopReviewsDelete(asVM);
            //Redirect user to Judge/Create View
            return RedirectToAction("ShopDetails", "Admin", asVM);
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

        public ActionResult EventDetails(int eventId)
        {
            AdminEventViewModel aeVM = new AdminEventViewModel();
            aeVM = adCtx.GetEventDetails(eventId);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EventUpdate(AdminEventViewModel aeVM)
        {
            //Add judge record to database
            adCtx.EventUpdate(aeVM);
            //Redirect user to Judge/Create View
            return RedirectToAction("EventDetails", "Admin", aeVM);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EventDelete(AdminEventViewModel aeVM)
        {
            //Add judge record to database
            adCtx.EventDelete(aeVM);
            //Redirect user to Judge/Create View
            return RedirectToAction("ManageEvents", "Admin");
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

        public ActionResult ViewDiscussion(int ForumPostID)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            AdminForumViewModel fpcVM = new AdminForumViewModel();
            fpcVM.ShopPostIDList = adCtx.ShopPostIDCheckList();
            fpcVM.CustomerPostIDList = adCtx.CustPostIDCheckList();
            if (fpcVM.ShopPostIDList.Count != 0)
            {
                foreach (var id in fpcVM.ShopPostIDList)
                {
                    if (ForumPostID == id.ForumPostID)
                    {   
                        fpcVM = adCtx.ShopPostDetails(ForumPostID);
                        fpcVM.ForumPostID = ForumPostID;
                        fpcVM.ShopCommentsList = adCtx.ShopComments(ForumPostID);
                        fpcVM.CustomerCommentsList = adCtx.CustomerComments(ForumPostID);
                        return View(fpcVM);
                    }
                    else
                    {
                        fpcVM = adCtx.CustomerPostDetails(ForumPostID);
                        fpcVM.ForumPostID = ForumPostID;
                        fpcVM.ShopCommentsList = adCtx.ShopComments(ForumPostID);
                        fpcVM.CustomerCommentsList = adCtx.CustomerComments(ForumPostID);
                        return View(fpcVM);
                    }
                }
            }
            else if (fpcVM.CustomerPostIDList.Count != 0)
            {
                fpcVM = adCtx.CustomerPostDetails(ForumPostID);
                fpcVM.ForumPostID = ForumPostID;
                fpcVM.ShopCommentsList = adCtx.ShopComments(ForumPostID);
                fpcVM.CustomerCommentsList = adCtx.CustomerComments(ForumPostID);
                return View(fpcVM);
            }
            return View();
        }

        public ActionResult PostDelete(int ForumPostID)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            AdminForumViewModel fpcVM = new AdminForumViewModel();
            fpcVM.ShopPostIDList = adCtx.ShopPostIDCheckList();
            fpcVM.CustomerPostIDList = adCtx.CustPostIDCheckList();
            if (fpcVM.ShopPostIDList.Count != 0)
            {
                foreach (var id in fpcVM.ShopPostIDList)
                {
                    if (ForumPostID == id.ForumPostID)
                    {
                        fpcVM = adCtx.ShopPostDetails(ForumPostID);
                        fpcVM.ForumPostID = ForumPostID;
                        fpcVM.ShopCheck = true;
                        return View(fpcVM);
                    }
                    else
                    {
                        fpcVM = adCtx.CustomerPostDetails(ForumPostID);
                        fpcVM.ForumPostID = ForumPostID;
                        fpcVM.ShopCheck = false;
                        return View(fpcVM);
                    }
                }
            }
            else if (fpcVM.CustomerPostIDList.Count != 0)
            {
                fpcVM = adCtx.CustomerPostDetails(ForumPostID);
                fpcVM.ForumPostID = ForumPostID;
                return View(fpcVM);
            }
            return View();
        }

        public ActionResult CommentDelete(int ForumCommentID)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            AdminForumViewModel fpcVM = new AdminForumViewModel();
            fpcVM.ShopCommentIDList = adCtx.ShopCommentIDCheckList(ForumCommentID);
            fpcVM.CustomerCommentIDList = adCtx.CustCommentIDCheckList(ForumCommentID);
            if (fpcVM.ShopCommentIDList.Count != 0)
            {
                foreach (var id in fpcVM.ShopCommentIDList)
                {
                    if (ForumCommentID == id.ForumCommentsID)
                    {
                        fpcVM = adCtx.ShopCommentsDetails(ForumCommentID);
                        fpcVM.ForumPostID = id.ForumPostID;
                        fpcVM.ForumCommentsID = ForumCommentID;
                        fpcVM.ShopCheck = true;
                        return View(fpcVM);
                    }
                }
            }
            else if (fpcVM.CustomerCommentIDList.Count != 0)
            {
                foreach(var id in fpcVM.CustomerCommentIDList)
                {
                    fpcVM = adCtx.CustomerCommentsDetails(ForumCommentID);
                    fpcVM.ForumCommentsID = ForumCommentID;
                    fpcVM.ForumPostID = id.ForumPostID;
                    fpcVM.ShopCheck = false;
                    return View(fpcVM);
                }
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CommentDelete(AdminForumViewModel afVM)
        {
            //Add judge record to database
            adCtx.CommentDelete(afVM);
            //Redirect user to Judge/Create View
            return RedirectToAction("ViewDiscussion", "Admin",afVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostDelete(AdminForumViewModel afVM)
        {
            //Add judge record to database
            adCtx.PostDelete(afVM);
            //Redirect user to Judge/Create View
            return RedirectToAction("ManageForum", "Admin");
        }


        public ActionResult About()
        {
            return View();
        }
    }
}

