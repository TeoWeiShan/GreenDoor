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
        private ForumDAL forumPostContext = new ForumDAL();

        public IActionResult Index()
        {
            List<ForumPost> forumPostList = forumPostContext.GetAllForumPost();
            return View(forumPostList);
        }

        // GET: ForumController/Create
        public ActionResult Create()
        {
            // Stop accessing the action if not logged in or account not Admin role
            if ((HttpContext.Session.GetString("Role") == "Customer") ||
                    (HttpContext.Session.GetString("Role") == "Admin") ||
                        (HttpContext.Session.GetString("Role") == "Owner"))
            {
                return View();
            }

            return RedirectToAction("Login", "Home");
        }

        // POST: ForumController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ForumPostCommentViewModel forumPost)
        {
            forumPost.CustomerID = (int)HttpContext.Session.GetInt32("LoginID");
            forumPost.DateTimePosted = DateTime.Now;
            //Add ForumPost record to database 
            forumPost.ForumPostID = forumPostContext.Add(forumPost);

            //return to the Sucess view to display success message
            return RedirectToAction("Index", "Forum");
        }
    }
}

