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
        private List<string> searchpostCatList = new List<string> { "-","Upcycling", "Discussion", "Lobang", "Exchange", "Sharing", "Request" };
        private List<SelectListItem> postCatDropDownList = new List<SelectListItem>();
        private List<SelectListItem> searchpostCatDropDownList = new List<SelectListItem>();
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
            foreach(string postCat in searchpostCatList)
            {
                searchpostCatDropDownList.Add(
                    new SelectListItem
                    {
                        Text = postCat,
                    });
            }
        }
        public ActionResult ViewForum(string searchString)
        {
            ForumPostCommentViewModel fpcVM = new ForumPostCommentViewModel();
            fpcVM.CustomerPostsList = forumCtx.CustomerPostList();
            fpcVM.ShopPostsList = forumCtx.ShopPostList();
            ViewData["searchCategory"] = searchpostCatDropDownList;
            return View(fpcVM);
        }
        public ActionResult ViewDiscussion(int ForumPostID)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ViewSearchResults(ForumPostCommentViewModel fpc)
        {
            ViewData["searchCategory"] = searchpostCatDropDownList;
            if (fpc.searchQuery != null && fpc.categorySelected!= "-")
            {
                ForumPostCommentViewModel fpcVM = new ForumPostCommentViewModel();
                fpcVM.searchCustPostList = forumCtx.searchCustPostAllList(fpc);
                fpcVM.searchShopPostList = forumCtx.searchShopPostAllList(fpc);
                fpcVM.categorySelected = fpc.categorySelected;
                fpcVM.searchQuery = fpc.searchQuery;
                return View(fpcVM);
            }
            else if(fpc.categorySelected != "-")
            {
                ForumPostCommentViewModel fpcVM = new ForumPostCommentViewModel();
                fpcVM.searchCustPostList = forumCtx.searchCustPostCatList(fpc);
                fpcVM.searchShopPostList = forumCtx.searchShopPostCatList(fpc);
                fpcVM.categorySelected = fpc.categorySelected;
                return View(fpcVM);
            }
            else if(fpc.searchQuery != null)
            {
                ForumPostCommentViewModel fpcVM = new ForumPostCommentViewModel();
                fpcVM.searchCustPostList = forumCtx.searchCustPostList(fpc);
                fpcVM.searchShopPostList = forumCtx.searchShopPostList(fpc);
                fpcVM.searchQuery = fpc.searchQuery;
                return View(fpcVM);
            } 
            else
            {
                return View();
            }
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
            return RedirectToAction("ViewForum", "Forum");
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
            return RedirectToAction("ViewForum", "Forum");
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

            return RedirectToAction("ViewDiscussion", "Forum", custComment);
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

            return RedirectToAction("ViewDiscussion", "Forum", shopComment);
        }
    }
}

