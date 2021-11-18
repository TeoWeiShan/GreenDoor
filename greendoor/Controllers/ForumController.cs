﻿using Microsoft.AspNetCore.Http;
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
        private ForumDAL forumPostContext = new ForumDAL();
        private CustomerDAL custCtx = new CustomerDAL();

        public IActionResult ShopViewForum()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Owner"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<ForumPost> forumPostList = forumPostContext.GetAllForumPost();
            return View(forumPostList);
        }

        // GET: ForumController/Create
        public ActionResult ShopCreate()
        {
            // Stop accessing the action if not logged in or account not Admin role
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Owner"))
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
            fpcVM.PostsList = forumPostContext.GetAllForumPostVM();
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

        // POST: ForumController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShopCreate(ForumPostCommentViewModel forumPost)
        {
            forumPost.ShopID = (int)HttpContext.Session.GetInt32("LoginID");
            forumPost.DateTimePosted = DateTime.Now;
            //Add ForumPost record to database 
            forumPost.ForumPostID = forumPostContext.ShopAddPost(forumPost);

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
            forumPost.ForumPostID = forumPostContext.CustomerAddPost(forumPost);

            //return to the Sucess view to display success message
            return RedirectToAction("CustomerViewForum", "Forum");
        }
    }
}

