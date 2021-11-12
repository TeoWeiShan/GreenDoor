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
        private ForumPostDAL forumPostContext = new ForumPostDAL();

        public IActionResult Index()
        {
            List<ForumPost> forumPostList = forumPostContext.GetAllForumPost();
            return View(forumPostList);
        }
    }
}

