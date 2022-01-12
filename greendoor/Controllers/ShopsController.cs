using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using greendoor.DAL;
using greendoor.Models;
using System.IO;

namespace greendoor.Controllers
{
    public class ShopsController : Controller
    {
        private ShopDAL shopContext = new ShopDAL();
        private ReviewsDAL reviewContext = new ReviewsDAL();
        private ShopPostDAL shopPostContext = new ShopPostDAL();

        public IActionResult ViewShops() // shop lsit for customer and public
        {
            List<Shop> shopList = shopContext.GetAllShop();
            return View(shopList);
        }

        public IActionResult Index()  // for shops login
        {
            return View();
        }


        // GET: ShopController/Details/5
        public ActionResult ShopDetails(int id)  // for customer and public
        {
            HttpContext.Session.SetString("ShopID", id.ToString());
            //HttpContext.Session.SetString("LoginID", id.ToString());

            int? customerID = HttpContext.Session.GetInt32("LoginID");
            string shopID = HttpContext.Session.GetString("ShopID");
            //int custID = Convert.ToInt32(customerID);
            ShopReviewViewModel shopreviewVM = new ShopReviewViewModel();
            shopreviewVM.reviewsList = reviewContext.GetLatestReview(id);
            shopreviewVM.shopPostList = shopPostContext.GetLatestShopPost(id);
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
            int count = 0;
            int totalScore = 0;
            foreach (Reviews r in reviewContext.GetLatestReview(id))
            {
                totalScore += (int)r.Rating;
                count++;
            }
            int averageScore = totalScore / count;
            if (count != 0)
            {
                ViewData["avgScore"] = "Average review score: " + averageScore;
            }
            return View(shopreviewVM);
        }

        public ActionResult ShopProfile()  // for shop
        {
            ShopReviewViewModel shopreviewVM = new ShopReviewViewModel();
            shopreviewVM.ShopID = (int)HttpContext.Session.GetInt32("LoginID");
            shopreviewVM = shopContext.GetDetailsVM(shopreviewVM.ShopID);
            shopreviewVM.reviewsList = reviewContext.GetAllReviews(shopreviewVM.ShopID);
            //Get details of competition
            //If query id not in db, redirect out
            if (shopreviewVM.ShopName == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(shopreviewVM);
        }

        public ActionResult Delete() //for shop
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Shop"))
            {
                return RedirectToAction("Index", "Home");
            }
            ShopReviewViewModel shopreviewVM = new ShopReviewViewModel();
            shopreviewVM.ShopID = (int)HttpContext.Session.GetInt32("LoginID");
            shopreviewVM = shopContext.GetDetailsVM(shopreviewVM.ShopID);

            return View(shopreviewVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ShopReviewViewModel srVM)
        {
            //Add judge record to database
            shopContext.ShopDelete(srVM);
            HttpContext.Session.Remove("Role");
            HttpContext.Session.Remove("LoginID");
            //Redirect user to Judge/Create View
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Edit()
        {
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "Central", Value = "Central" });
            li.Add(new SelectListItem { Text = "East", Value = "East" });
            li.Add(new SelectListItem { Text = "North", Value = "North" });
            li.Add(new SelectListItem { Text = "North-East", Value = "North-East" });
            li.Add(new SelectListItem { Text = "West", Value = "West" });
            li.Add(new SelectListItem { Text = "NA", Value = "NA" });
            ViewData["zoneList"] = li;

            EditShopDetailsViewModel shopDetails = new EditShopDetailsViewModel();

            Shop shop = shopContext.GetDetails((int)HttpContext.Session.GetInt32("LoginID"));

            shopDetails.ShopName = shop.ShopName;
            shopDetails.ShopDescription = shop.ShopDescription;
            shopDetails.Zone = shop.Zone;
            shopDetails.ContactNumber = shop.ContactNumber;
            shopDetails.Address = shop.Address;
            shopDetails.PostalCode = shop.PostalCode;
            shopDetails.SocialMediaLink = shop.SocialMediaLink;
            shopDetails.WebsiteLink = shop.WebsiteLink;
            shopDetails.EmailAddr = shop.EmailAddr;
            shopDetails.Password = shop.Password;

            return View(shopDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditShopDetailsViewModel shopDetail)
        {
            if(shopDetail.PostalCode.ToString().Length != 6)
            {
                List<SelectListItem> Li = new List<SelectListItem>();
                Li.Add(new SelectListItem { Text = "Central", Value = "Central" });
                Li.Add(new SelectListItem { Text = "East", Value = "East" });
                Li.Add(new SelectListItem { Text = "North", Value = "North" });
                Li.Add(new SelectListItem { Text = "North-East", Value = "North-East" });
                Li.Add(new SelectListItem { Text = "West", Value = "West" });
                Li.Add(new SelectListItem { Text = "NA", Value = "NA" });
                ViewData["zoneList"] = Li;
                return View(shopDetail);
            }
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "Central", Value = "Central" });
            li.Add(new SelectListItem { Text = "East", Value = "East" });
            li.Add(new SelectListItem { Text = "North", Value = "North" });
            li.Add(new SelectListItem { Text = "North-East", Value = "North-East" });
            li.Add(new SelectListItem { Text = "West", Value = "West" });
            li.Add(new SelectListItem { Text = "NA", Value = "NA" });
            ViewData["zoneList"] = li;

                Shop shop = new Shop();
                shop.ShopID = (int)HttpContext.Session.GetInt32("LoginID");
                shop.ShopName = shopDetail.ShopName;
                shop.ShopDescription = shopDetail.ShopDescription;
                shop.Zone = shopDetail.Zone;
                shop.ContactNumber = shopDetail.ContactNumber;
                shop.Address = shopDetail.Address;
                shop.PostalCode = shopDetail.PostalCode;
                shop.SocialMediaLink = shopDetail.SocialMediaLink;
                shop.WebsiteLink = shopDetail.WebsiteLink;
                shop.EmailAddr = shopDetail.EmailAddr;
                shop.Password = shopDetail.Password;

                // Update shop record to database
                shopContext.Update(shop);

                return RedirectToAction("ShopProfile", "Shops");
            
        }

        public ActionResult EditPhoto()
        {
            EditPhotoViewModel photoViewModel = new EditPhotoViewModel();

            Shop shop = shopContext.GetDetails((int)HttpContext.Session.GetInt32("LoginID"));
            photoViewModel.ShopPicture = shop.ShopPicture;
            photoViewModel.ShopName = shop.ShopName;

            //If query id not in db, redirect out
            if (shop.ShopName == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(photoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPhoto(EditPhotoViewModel photoViewModel)
        {
            if (ModelState.IsValid)
            {
                //Update shop record to database
                return RedirectToAction("ShopProfile", "Shops");
            }
            else
            {
                //Input validation fails, return to the view to display error message
                return View(photoViewModel);
            }
        }


        // POST: CompetitionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShopDetails(ShopReviewViewModel review, int id)
        {
            //Populate list for drop-down list
            //in case of the need to return to Edit.cshtml view
            //ViewData["InterestList"] = GetAllAreaInterests();
            review.DateTimePosted = DateTime.Now;
            if (review.Rating > 5 || review.Rating<0)
            {
                ViewData["RatingError"] = "Rating cannot be more than 5 or less than 0!";
                /*return RedirectToAction("ShopDetails", "Shops", new { id = HttpContext.Session.GetString("ShopID") },review);*/
                ShopReviewViewModel shopVM = new ShopReviewViewModel();
                shopVM = shopContext.GetDetailsVM(id);
                shopVM.reviewsList = reviewContext.GetAllReviews(id);
                shopVM.shopPostList = shopPostContext.GetLatestShopPost(id);
                return View(shopVM);
            }
            //Update record to database
            ShopReviewViewModel shopreviewVM = new ShopReviewViewModel();
            shopreviewVM = shopContext.GetDetailsVM(id);
            shopreviewVM.ReviewsID = reviewContext.Add(review);
            shopreviewVM.reviewsList = reviewContext.GetAllReviews(id);
            shopreviewVM.shopPostList = shopPostContext.GetLatestShopPost(id);
            return View(shopreviewVM);

        }

        public ActionResult ViewPosts(int id) // customer and public
        {
            ShopReviewViewModel shopPost = new ShopReviewViewModel();
            shopPost.shopPostList = shopPostContext.GetAllShopPost(id);
            shopPost.ShopID = id;
            return View(shopPost);
        }

        public ActionResult ViewReviews(int id) // customer and public
        {
            ShopReviewViewModel reviews = new ShopReviewViewModel();
            reviews.reviewsList = reviewContext.GetAllReviews(id);
            reviews.ShopID = id;
            return View(reviews);
        }
    }
}
