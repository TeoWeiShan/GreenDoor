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
            if (ModelState.IsValid)
            {
                //Add ForumPost record to database 
                int custID = 1;

                forumPost.ForumPostID = forumPostContext.Add(forumPost, custID);

                //alert user that ForumPost record has been successfully addded
                TempData["SuccessDetail"] = "Topic has been posted successfully!";

                //return to the Sucess view to display success message
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the Create view to display error message 
                return View(forumPost);
            }
        }
    }
}

