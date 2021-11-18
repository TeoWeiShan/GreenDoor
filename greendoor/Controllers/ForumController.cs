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

        public ActionResult PublicViewForum()
        {
            ForumPostCommentViewModel fpcVM = new ForumPostCommentViewModel();
            fpcVM.PostsList = forumCtx.GetAllForumPostVM();
            return View(fpcVM);
        }
        public ActionResult PublicViewDiscussion()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            ForumPostCommentViewModel fpcVM = new ForumPostCommentViewModel();
            /*fpcVM = */
            return View(fpcVM);
        }
        public ActionResult ShopViewForum()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Shop"))
            {
                return RedirectToAction("Index", "Home");
            }
            ForumPostCommentViewModel fpcVM = new ForumPostCommentViewModel();
            fpcVM.PostsList = forumCtx.GetAllForumPostVM();
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
            return View();
        }
        public ActionResult CustomerViewForum()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            ForumPostCommentViewModel fpcVM = new ForumPostCommentViewModel();
            fpcVM.PostsList = forumCtx.GetAllForumPostVM();
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
            fpcVM = forumCtx.CustomerPostDetails(ForumPostID);
            fpcVM.ShopCommentsList = forumCtx.ShopComments(ForumPostID);
            fpcVM.CustomerCommentsList = forumCtx.CustomerComments(ForumPostID);
            return View(fpcVM);
        }

        public ActionResult CustomerCreate()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
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

            return RedirectToAction("CustomerViewDiscussion", "Forum");
        }
    }
}

