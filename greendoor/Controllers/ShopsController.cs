﻿using Microsoft.AspNetCore.Http;
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
        public IActionResult Index()
        {
            List<Shop> shopList = shopContext.GetAllShop();
            return View(shopList);
        }

        public IActionResult HomePage()
        {
            return View();
        }

        // GET: ShopController/Details/5
        public ActionResult Details(int id)
        {
            ShopReviewViewModel shopreviewVM = new ShopReviewViewModel();
            shopreviewVM.reviewsList = reviewContext.GetAllReviews(id);
            //Get details of competition
            Shop shop = shopContext.GetDetails(id);
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

        // GET
        public ActionResult ShopDetails()
        {
            ShopReviewViewModel shopreviewVM = new ShopReviewViewModel();
            shopreviewVM.reviewsList = reviewContext.GetAllReviews((int)HttpContext.Session.GetInt32("LoginID"));
            //Get details of competition
            Shop shop = shopContext.GetDetails((int)HttpContext.Session.GetInt32("LoginID"));
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

        // POST: RegShop
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterShop(RegisterShopViewModel shopModel)
        {
            try
            {
                // Find the filename extension of the file to be uploaded.
                string fileExt = Path.GetExtension(shopModel.PhotoFile.FileName);
                // Rename the uploaded file with the shop’s name.
                string uploadedFile = shopModel.ShopName + fileExt;
                // Get the complete path to the images folder in server
                string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", uploadedFile);
                // Upload the file to server
                using (var fileSteam = new FileStream(savePath, FileMode.Create))
                {
                    await shopModel.PhotoFile.CopyToAsync(fileSteam);
                }

                Shop shop = new Shop();
                shop.ShopDescription = shopModel.ShopDescription;
                shop.Password = shopModel.Password;
                shop.PostalCode = shopModel.PostalCode;
                shop.ShopName = shopModel.ShopName;
                shop.WebsiteLink = shopModel.WebsiteLink;
                shop.SocialMediaLink = shopModel.SocialMediaLink;
                shop.Zone = shopModel.Zone;
                shop.ContactNumber = shopModel.ContactNumber;
                shop.Address = shopModel.Address;
                shop.EmailAddr = shopModel.EmailAddr;
                shop.ShopPicture = uploadedFile;

                // Add shop record to database
                shop.ShopID = shopContext.Add(shop);

                // Store Login ID in session with the key “LoginID”
                HttpContext.Session.SetString("LoginID", shop.ShopID.ToString());
                // Store user role “Shop” as a string in session with the key “Role” 
                HttpContext.Session.SetString("Role", "Shop");

                //Redirect user to Shops/Index view
                return RedirectToAction("Index", "Shops");
            }
            catch (IOException)
            {
                //File IO error, could be due to access rights denied
                ViewData["Message"] = "File uploading fail!";
            }
            catch (Exception ex) //Other type of error
            {
                ViewData["Message"] = ex.Message;
            }
            return View(shopModel);
        }

    }
}
