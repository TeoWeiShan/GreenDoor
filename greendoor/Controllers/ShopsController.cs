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
    public class ShopsController : Controller
    {
        private ShopDAL shopContext = new ShopDAL();
        private ReviewsDAL reviewContext = new ReviewsDAL();
        public IActionResult Index()
        {
            List<Shop> shopList = shopContext.GetAllShop();
            return View(shopList);
        }

        // GET: ShopController/Details/5
        public ActionResult Details(int id)
        {
            HttpContext.Session.SetString("ShopID", id.ToString());
            //HttpContext.Session.SetString("LoginID", id.ToString());

            int? customerID = HttpContext.Session.GetInt32("LoginID");
            string shopID = HttpContext.Session.GetString("ShopID");
            //int custID = Convert.ToInt32(customerID);
            if (id == null)
            { //Query string parameter not provided
              //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            ShopReviewViewModel shopreviewVM = new ShopReviewViewModel();
            shopreviewVM.reviewsList = reviewContext.GetAllReviews(id);
            //Get details of competition
            Shop shop = shopContext.GetDetails(id);
            shopreviewVM.ShopID = id;
            shopreviewVM.CustomerID = customerID;
            shopreviewVM.ShopPicture = shop.ShopPicture;
            shopreviewVM.ShopName = shop.ShopName;
            shopreviewVM.ShopDescription = shop.ShopDescription;
            shopreviewVM.Zone = shop.Zone;
            shopreviewVM.ContactNumber = shop.ContactNumber;
            shopreviewVM.Address = shop.Address;
            shopreviewVM.PostalCode = shop.PostalCode;
            shopreviewVM.SocialMediaLink = shop.SocialMediaLink;
            shopreviewVM.WebsiteLink = shop.WebsiteLink;
            //If query id not in db, redirect out
            if (shop.ShopName == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(shopreviewVM);
        }

        // GET: CompetitionController/Create
        public ActionResult _AddReviews(int id)
        {

            //If not admin, cannot access
            //if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Customer"))
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            //Populate dropdown list
            string customerID = HttpContext.Session.GetString("LoginID");
            string shop = HttpContext.Session.GetString("ShopID");

            return View();
        }

        // POST: CompetitionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _AddReviews(Reviews review)
        {
            //Populate list for drop-down list
            //in case of the need to return to Edit.cshtml view
            //ViewData["InterestList"] = GetAllAreaInterests();
            if (ModelState.IsValid)
            {
                //Update record to database
                review.ReviewsID = reviewContext.Add(review);
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the view
                //to display error message
                return View(review);
            }
        }

    }
}
