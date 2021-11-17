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

       /* public IActionResult Index()
        {
            List<ForumPost> forumPostList = forumPostContext.GetForumComments();
            return View(forumPostList);
        }
        public ActionResult ViewComments(int ForumPostID)
        {

        }*/
    }
}

