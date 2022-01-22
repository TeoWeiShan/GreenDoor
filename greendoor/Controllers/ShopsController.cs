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
        private FavouriteDAL favouriteContext = new FavouriteDAL();
        private List<string> searchShopZoneList = new List<string> { "-", "West", "Central", "East", "North", "North-East", "NA" };
        private List<SelectListItem> searchshopZoneDropDownList = new List<SelectListItem>();
        public ShopsController()
        {
            foreach (string postCat in searchShopZoneList)
            {
                searchshopZoneDropDownList.Add(
                    new SelectListItem
                    {
                        Text = postCat,
                    });
            }
        }

        public IActionResult ViewShops() // shop lsit for customer and public
        {
            ViewData["zoneSelected"] = searchshopZoneDropDownList;
            List<Shop> shopList = shopContext.GetAllShop();
            // Calculating top 3 shops in terms of number of reviews
            Dictionary<string, int> shopReviewList = new Dictionary<string, int>();
            foreach (Shop shop in shopList)
            {
                int i = reviewContext.GetNumberOfReviews(shop.ShopID);
                shopReviewList.Add(shop.ShopName, i);
            }

            int j = shopReviewList.Count();
            if (j > 3)
            {
                j = 3;
            }
            var sortedDict = (from pair in shopReviewList
                              orderby pair.Value descending
                              select pair).Take(j).ToDictionary(pair => pair.Key, pair => pair.Value);

            if (j == 3)
            {
                var first = sortedDict.First();
                ViewData["topNumberReviews1a"] = first.Key;
                ViewData["topNumberReviews1b"] = first.Value;
                sortedDict.Remove(first.Key);

                var second = sortedDict.First();
                ViewData["topNumberReviews2a"] = second.Key;
                ViewData["topNumberReviews2b"] = second.Value;
                sortedDict.Remove(second.Key);

                var third = sortedDict.First();
                ViewData["topNumberReviews3a"] = third.Key;
                ViewData["topNumberReviews3b"] = third.Value;
                sortedDict.Remove(third.Key);
            }
            else if (j == 2)
            {
                var first = sortedDict.First();
                ViewData["topNumberReviews1a"] = first.Key;
                ViewData["topNumberReviews1b"] = first.Value;
                sortedDict.Remove(first.Key);

                var second = sortedDict.First();
                ViewData["topNumberReviews2a"] = second.Key;
                ViewData["topNumberReviews2b"] = second.Value;
                sortedDict.Remove(second.Key);
            }
            else if (j == 1)
            {
                var first = sortedDict.First();
                ViewData["topNumberReviews1a"] = first.Key;
                ViewData["topNumberReviews1b"] = first.Value;
                sortedDict.Remove(first.Key);
            }

            // Calculating top 3 shops in terms of number of reviews
            Dictionary<string, int> shop2ReviewList = new Dictionary<string, int>();
            foreach (Shop shop in shopList)
            {
                int i = reviewContext.GetAverageReviewScore(shop.ShopID);
                shop2ReviewList.Add(shop.ShopName, i);
            }

            int l = shop2ReviewList.Count();
            if (l > 3)
            {
                l = 3;
            }
            var sortedDict2 = (from pair in shop2ReviewList
                               orderby pair.Value descending
                               select pair).Take(l).ToDictionary(pair => pair.Key, pair => pair.Value);

            if (l == 3)
            {
                var first = sortedDict2.First();
                ViewData["topAverageReviews1a"] = first.Key;
                ViewData["topAverageReviews1b"] = first.Value;
                sortedDict2.Remove(first.Key);

                var second = sortedDict2.First();
                ViewData["topAverageReviews2a"] = second.Key;
                ViewData["topAverageReviews2b"] = second.Value;
                sortedDict2.Remove(second.Key);

                var third = sortedDict2.First();
                ViewData["topAverageReviews3a"] = third.Key;
                ViewData["topAverageReviews3b"] = third.Value;
                sortedDict2.Remove(third.Key);
            }
            else if (l == 2)
            {
                var first = sortedDict2.First();
                ViewData["topAverageReviews1a"] = first.Key;
                ViewData["topAverageReviews1b"] = first.Value;
                sortedDict2.Remove(first.Key);

                var second = sortedDict2.First();
                ViewData["topAverageReviews2a"] = second.Key;
                ViewData["topAverageReviews2b"] = second.Value;
                sortedDict2.Remove(second.Key);
            }
            else if (l == 1)
            {
                var first = sortedDict2.First();
                ViewData["topAverageReviews1a"] = first.Key;
                ViewData["topAverageReviews1b"] = first.Value;
                sortedDict2.Remove(first.Key);
            }

            // Calculating top 3 shops in terms of number of favourites
            Dictionary<string, int> shopFavouriteList = new Dictionary<string, int>();
            foreach (Shop shop in shopList)
            {
                int i = favouriteContext.GetNumFavourite(shop.ShopID);
                shopFavouriteList.Add(shop.ShopName, i);
            }

            int n = shopFavouriteList.Count();
            if (n > 3)
            {
                n = 3;
            }
            var sortedDict3 = (from pair in shopFavouriteList
                              orderby pair.Value descending
                              select pair).Take(n).ToDictionary(pair => pair.Key, pair => pair.Value);
            if (n == 3)
            {
                var first = sortedDict3.First();
                ViewData["topNumFavourite1a"] = first.Key;
                ViewData["topNumFavourite1b"] = first.Value;
                sortedDict3.Remove(first.Key);

                var second = sortedDict3.First();
                ViewData["topNumFavourite2a"] = second.Key;
                ViewData["topNumFavourite2b"] = second.Value;
                sortedDict3.Remove(second.Key);

                var third = sortedDict3.First();
                ViewData["topNumFavourite3a"] = third.Key;
                ViewData["topNumFavourite3b"] = third.Value;
                sortedDict3.Remove(third.Key);
            }
            else if (n == 2)
            {
                var first = sortedDict3.First();
                ViewData["topNumFavourite1a"] = first.Key;
                ViewData["topNumFavourite1b"] = first.Value;
                sortedDict3.Remove(first.Key);

                var second = sortedDict3.First();
                ViewData["topNumFavourite2a"] = second.Key;
                ViewData["topNumFavourite2b"] = second.Value;
                sortedDict3.Remove(second.Key);
            }
            else if (n == 1)
            {
                var first = sortedDict3.First();
                ViewData["topNumFavourite1a"] = first.Key;
                ViewData["topNumFavourite1b"] = first.Value;
                sortedDict3.Remove(first.Key);
            }


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
            if (customerID != null)
            {
                shopreviewVM.FavBool = favouriteContext.GetDetails(customerID, Convert.ToInt32(shopID));
            }
            else
            {
                shopreviewVM.FavBool = false;
            }
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

            if (count != 0)
            {
                int averageScore = totalScore / count;
                ViewData["avgScore"] = "Average review score: " + averageScore;
            }
            return View(shopreviewVM);
        }

        public ActionResult AddFav(int id) //for shop
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            HttpContext.Session.SetString("ShopID", id.ToString());

            int? customerID = HttpContext.Session.GetInt32("LoginID");
            //string shopID = HttpContext.Session.GetString("ShopID");
            int custID = Convert.ToInt32(customerID);
            ShopFav shopfav = new ShopFav();
            Shop shop = shopContext.GetDetails(id);
            shopfav.CustomerID = custID;
            shopfav.ShopID = id;
            shopfav.ShopPicture = shop.ShopPicture;
            shopfav.ShopName = shop.ShopName;
            shopfav.ShopDescription = shop.ShopDescription;
            shopfav.Zone = shop.Zone;
            shopfav.ContactNumber = shop.ContactNumber;
            shopfav.Address = shop.Address;
            shopfav.PostalCode = shop.PostalCode;
            shopfav.SocialMediaLink = shop.SocialMediaLink;
            shopfav.WebsiteLink = shop.WebsiteLink;
            shopfav.FavBool = false;

            return View(shopfav);
        }
        // POST: CompetitionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFav(ShopReviewViewModel fav, int id)
        {
            int? customerID = HttpContext.Session.GetInt32("LoginID");
            //Populate list for drop-down list
            //in case of the need to return to Edit.cshtml view
            //ViewData["InterestList"] = GetAllAreaInterests();
           
            //Update record to database
            ShopFav shopfav = new ShopFav();
            shopfav.FavBool = favouriteContext.Add(customerID, id);
            
            return RedirectToAction("ShopDetails", "Shops", new { id });

        }

        public ActionResult RemoveFav(int id) //for shop
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            HttpContext.Session.SetString("ShopID", id.ToString());

            int? customerID = HttpContext.Session.GetInt32("LoginID");
            //string shopID = HttpContext.Session.GetString("ShopID");
            int custID = Convert.ToInt32(customerID);
            ShopFav shopfav = new ShopFav();
            Shop shop = shopContext.GetDetails(id);
            shopfav.CustomerID = custID;
            shopfav.ShopID = id;
            shopfav.ShopPicture = shop.ShopPicture;
            shopfav.ShopName = shop.ShopName;
            shopfav.ShopDescription = shop.ShopDescription;
            shopfav.Zone = shop.Zone;
            shopfav.ContactNumber = shop.ContactNumber;
            shopfav.Address = shop.Address;
            shopfav.PostalCode = shop.PostalCode;
            shopfav.SocialMediaLink = shop.SocialMediaLink;
            shopfav.WebsiteLink = shop.WebsiteLink;
            shopfav.FavBool = false;

            return View(shopfav);
        }
        // POST: CompetitionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveFav(ShopReviewViewModel fav, int id)
        {
            int? customerID = HttpContext.Session.GetInt32("LoginID");
            //Populate list for drop-down list
            //in case of the need to return to Edit.cshtml view
            //ViewData["InterestList"] = GetAllAreaInterests();

            //Update record to database
            ShopFav shopfav = new ShopFav();
            shopfav.FavBool = favouriteContext.Delete(customerID, id);

            return RedirectToAction("ShopDetails", "Shops", new { id });

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
            int? customerID = HttpContext.Session.GetInt32("LoginID");
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
            else if(review.Rating == null || review.Description == null)
            {
                ViewData["RatingError"] = "Missing values!";
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
            shopreviewVM.FavBool = favouriteContext.Add(customerID, id);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShopSearchResults(string searchQuery, string zoneSelected)
        {
            ViewData["zoneSelected"] = searchshopZoneDropDownList;
            if (searchQuery != null && zoneSelected != "-")
            {
                Shop shop = new Shop();
                shop.searchResultsList = shopContext.shopSearchList(searchQuery, zoneSelected);
                shop.zoneSelected = zoneSelected;
                shop.searchQuery = searchQuery;
                return View(shop);
            }
            else if (zoneSelected != "-")
            {
                Shop shop = new Shop();
                shop.searchResultsList = shopContext.shopSearchZoneList(zoneSelected);
                shop.zoneSelected = zoneSelected;
                return View(shop);
            }
            else if (searchQuery != null)
            {
                Shop shop = new Shop();
                shop.searchResultsList = shopContext.shopSearchQueryList(searchQuery);
                shop.searchQuery = searchQuery;
                return View(shop);
            }
            else
            {
                return View();
            }
        }
    }
}
