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
    public class ForumController : Controller
    {
        private ForumDAL forumCtx = new ForumDAL();
        private CustomerDAL custCtx = new CustomerDAL();
        private ShopDAL shopCtx = new ShopDAL();
        private List<string> postCatList = new List<string> { "Upcycling", "Discussion", "Lobang", "Exchange", "Sharing", "Request" };
        private List<SelectListItem> postCatDropDownList = new List<SelectListItem>();
        public ForumController()
        {
            foreach(string postCat in postCatList)
            {
                postCatDropDownList.Add(
                    new SelectListItem
                    {
                        Text = postCat,
                    });
            }
        }
        public ActionResult PublicViewForum(string searchString)
        {
            ForumPostCommentViewModel fpcVM = new ForumPostCommentViewModel();
            fpcVM.CustomerPostsList = forumCtx.CustomerPostList();
            fpcVM.ShopPostsList = forumCtx.ShopPostList();
            return View(fpcVM);
        }
        public ActionResult PublicViewDiscussion(int ForumPostID)
        {
            ForumPostCommentViewModel fpcVM = new ForumPostCommentViewModel();
            fpcVM.ShopPostIDList = forumCtx.ShopPostIDCheckList();
            fpcVM.CustomerPostIDList = forumCtx.CustPostIDCheckList();
            if(fpcVM.ShopPostIDList.Count!=0)
            { 
                foreach (var id in fpcVM.ShopPostIDList)
                {
                    if (ForumPostID == id.ForumPostID)
                    {
                        fpcVM = forumCtx.ShopPostDetails(ForumPostID);
                        fpcVM.ForumPostID = ForumPostID;
                        fpcVM.ShopCommentsList = forumCtx.ShopComments(ForumPostID);
                        fpcVM.CustomerCommentsList = forumCtx.CustomerComments(ForumPostID);
                        return View(fpcVM);
                    }
                    else
                    {
                        fpcVM = forumCtx.CustomerPostDetails(ForumPostID);
                        fpcVM.ForumPostID = ForumPostID;
                        fpcVM.ShopCommentsList = forumCtx.ShopComments(ForumPostID);
                        fpcVM.CustomerCommentsList = forumCtx.CustomerComments(ForumPostID);
                        return View(fpcVM);
                    }
                }
            }
            else if(fpcVM.CustomerPostIDList.Count!=0)
            {
                fpcVM = forumCtx.CustomerPostDetails(ForumPostID);
                fpcVM.ForumPostID = ForumPostID;
                fpcVM.ShopCommentsList = forumCtx.ShopComments(ForumPostID);
                fpcVM.CustomerCommentsList = forumCtx.CustomerComments(ForumPostID);
                return View(fpcVM);
            }
            return View();
        }
        public ActionResult ShopViewForum()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Shop"))
            {
                return RedirectToAction("Index", "Home");
            }
            ForumPostCommentViewModel fpcVM = new ForumPostCommentViewModel();
            fpcVM.CustomerPostsList = forumCtx.CustomerPostList();
            fpcVM.ShopPostsList = forumCtx.ShopPostList();
            return View(fpcVM);
        }

        // GET: ForumController/Create
        public ActionResult ShopCreate()
        {
            // Stop accessing the action if not logged in or account not Admin role
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Shop"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["PostCategory"] = postCatDropDownList;
            ForumPostCommentViewModel fpcVM = new ForumPostCommentViewModel();
            fpcVM.ShopID = (int)HttpContext.Session.GetInt32("LoginID");
            fpcVM.ShopName = (shopCtx.GetDetails(fpcVM.ShopID)).ShopName;
            return View(fpcVM);
        }
        public ActionResult CustomerViewForum()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            ForumPostCommentViewModel fpcVM = new ForumPostCommentViewModel();
            fpcVM.CustomerPostsList = forumCtx.CustomerPostList();
            fpcVM.ShopPostsList = forumCtx.ShopPostList();
            return View(fpcVM);
        }

        public ActionResult CustomerViewDiscussion(int ForumPostID)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            ForumPostCommentViewModel fpcVM = new ForumPostCommentViewModel();
            fpcVM.ShopPostIDList = forumCtx.ShopPostIDCheckList();
            fpcVM.CustomerPostIDList = forumCtx.CustPostIDCheckList();
            if (fpcVM.ShopPostIDList.Count != 0)
            {
                foreach (var id in fpcVM.ShopPostIDList)
                {
                    if (ForumPostID == id.ForumPostID)
                    {
                        fpcVM = forumCtx.ShopPostDetails(ForumPostID);
                        fpcVM.ForumPostID = ForumPostID;
                        fpcVM.ShopCommentsList = forumCtx.ShopComments(ForumPostID);
                        fpcVM.CustomerCommentsList = forumCtx.CustomerComments(ForumPostID);
                        return View(fpcVM);
                    }
                    else
                    {
                        fpcVM = forumCtx.CustomerPostDetails(ForumPostID);
                        fpcVM.ForumPostID = ForumPostID;
                        fpcVM.ShopCommentsList = forumCtx.ShopComments(ForumPostID);
                        fpcVM.CustomerCommentsList = forumCtx.CustomerComments(ForumPostID);
                        return View(fpcVM);
                    }
                }
            }
            else if (fpcVM.CustomerPostIDList.Count != 0)
            {
                fpcVM = forumCtx.CustomerPostDetails(ForumPostID);
                fpcVM.ForumPostID = ForumPostID;
                fpcVM.ShopCommentsList = forumCtx.ShopComments(ForumPostID);
                fpcVM.CustomerCommentsList = forumCtx.CustomerComments(ForumPostID);
                return View(fpcVM);
            }
            return View();
        }

        public ActionResult ShopViewDiscussion(int ForumPostID)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Shop"))
            {
                return RedirectToAction("Index", "Home");
            }
            ForumPostCommentViewModel fpcVM = new ForumPostCommentViewModel();
            fpcVM.ShopPostIDList = forumCtx.ShopPostIDCheckList();
            fpcVM.CustomerPostIDList = forumCtx.CustPostIDCheckList();
            if (fpcVM.ShopPostIDList.Count != 0)
            {
                foreach (var id in fpcVM.ShopPostIDList)
                {
                    if (ForumPostID == id.ForumPostID)
                    {
                        fpcVM = forumCtx.ShopPostDetails(ForumPostID);
                        fpcVM.ForumPostID = ForumPostID;
                        fpcVM.ShopCommentsList = forumCtx.ShopComments(ForumPostID);
                        fpcVM.CustomerCommentsList = forumCtx.CustomerComments(ForumPostID);
                        return View(fpcVM);
                    }
                    else
                    {
                        fpcVM = forumCtx.CustomerPostDetails(ForumPostID);
                        fpcVM.ForumPostID = ForumPostID;
                        fpcVM.ShopCommentsList = forumCtx.ShopComments(ForumPostID);
                        fpcVM.CustomerCommentsList = forumCtx.CustomerComments(ForumPostID);
                        return View(fpcVM);
                    }
                }
            }
            else if (fpcVM.CustomerPostIDList.Count != 0)
            {
                fpcVM = forumCtx.CustomerPostDetails(ForumPostID);
                fpcVM.ForumPostID = ForumPostID;
                fpcVM.ShopCommentsList = forumCtx.ShopComments(ForumPostID);
                fpcVM.CustomerCommentsList = forumCtx.CustomerComments(ForumPostID);
                return View(fpcVM);
            }
            return View();
        }

        public ActionResult CustomerCreate()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["PostCategory"] = postCatDropDownList;
            ForumPostCommentViewModel fpcVM = new ForumPostCommentViewModel();
            fpcVM.CustomerID = (int)HttpContext.Session.GetInt32("LoginID");
            fpcVM.CustomerName = (custCtx.GetDetails(fpcVM.CustomerID)).CustomerName;
            return View(fpcVM);
        }

        public ActionResult CustomerCreateComment(int ForumPostID)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            ForumPostCommentViewModel fpcVM = new ForumPostCommentViewModel();
            fpcVM.CustomerID = (int)HttpContext.Session.GetInt32("LoginID");
            fpcVM.ForumPostID = ForumPostID;
            fpcVM.CustomerName = (custCtx.GetDetails(fpcVM.CustomerID)).CustomerName;
            return View(fpcVM);
        }

        public ActionResult ShopCreateComment(int ForumPostID)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Shop"))
            {
                return RedirectToAction("Index", "Home");
            }
            ForumPostCommentViewModel fpcVM = new ForumPostCommentViewModel();
            fpcVM.ShopID = (int)HttpContext.Session.GetInt32("LoginID");
            fpcVM.ForumPostID = ForumPostID;
            fpcVM.ShopName = (shopCtx.GetDetails(fpcVM.ShopID)).ShopName;
            return View(fpcVM);
        }
        public ActionResult PublicViewSearchResults(ForumPostCommentViewModel searchQuery)
        {
            ViewData["Keyword"] = searchQuery.searchQuery;
            ForumPostCommentViewModel fpcVM = new ForumPostCommentViewModel();
            if (!String.IsNullOrEmpty(searchQuery.searchQuery))
            {
                fpcVM.searchCustPostList = forumCtx.searchCustPostList(searchQuery);
                fpcVM.searchShopPostList = forumCtx.searchShopPostList(searchQuery);
            }
            return View(fpcVM);
        }

        // POST: ForumController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShopCreate(ForumPostCommentViewModel forumPost)
        {
            forumPost.ShopID = (int)HttpContext.Session.GetInt32("LoginID");
            forumPost.DateTimePosted = DateTime.Now;
            //Add ForumPost record to database 
            forumPost.ForumPostID = forumCtx.ShopAddPost(forumPost);

            //return to the Sucess view to display success message
            return RedirectToAction("ShopViewForum", "Forum");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerCreate(ForumPostCommentViewModel forumPost)
        {
            forumPost.CustomerID = (int)HttpContext.Session.GetInt32("LoginID");
            forumPost.DateTimePosted = DateTime.Now;
            //Add ForumPost record to database 
            forumPost.ForumPostID = forumCtx.CustomerAddPost(forumPost);

            //return to the Sucess view to display success message
            return RedirectToAction("CustomerViewForum", "Forum");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerCreateComment(ForumPostCommentViewModel custComment, int ForumPostID)
        {
            custComment.CustomerID = (int)HttpContext.Session.GetInt32("LoginID");
            custComment.DateTimePosted = DateTime.Now;
            custComment.ForumPostID = ForumPostID;
            //Add ForumPost record to database 
            custComment.ForumCommentsID = forumCtx.CustomerAddComment(custComment);

            return RedirectToAction("CustomerViewDiscussion", "Forum", custComment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShopCreateComment(ForumPostCommentViewModel shopComment, int ForumPostID)
        {
            shopComment.ShopID = (int)HttpContext.Session.GetInt32("LoginID");
            shopComment.DateTimePosted = DateTime.Now;
            shopComment.ForumPostID = ForumPostID;
            //Add ForumPost record to database 
            shopComment.ForumCommentsID = forumCtx.ShopAddComment(shopComment);

            return RedirectToAction("ShopViewDiscussion", "Forum", shopComment);
        }
/*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PublicViewSearchResults(ForumPostCommentViewModel forumPost)
        {
            ForumPostCommentViewModel fpcVM = new ForumPostCommentViewModel();
            fpcVM.searchCustPostList =  forumCtx.searchCustPostList(forumPost);
            fpcVM.searchShopPostList =  forumCtx.searchShopPostList(forumPost);
            return View(fpcVM);
        }*/
    }
}

