using greendoor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using greendoor.DAL;
using Microsoft.AspNetCore.Hosting;

namespace greendoor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private CustomerDAL customerContext = new CustomerDAL();
        private ShopDAL shopContext = new ShopDAL();
        private EventDAL eContext = new EventDAL();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public ActionResult ViewEvents()
        {
            List<EcoEvents> eList = eContext.GetAllEvent();
            return View(eList);
        }

        public ActionResult EventDetails(int EventID)
        {
            EcoEvents e = eContext.GetDetails(EventID);
            if (e.EventName == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(e);
        }

        // GET: RegCust
        public ActionResult RegisterCustomer()
        {
            return View();
        }

        // POST: RegCust
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterCustomer(Customer customer)
        {
            if (ModelState.IsValid)
            {                      
                // Add customer record to database
                customer.CustomerID = customerContext.Add(customer);
                // Store Login ID in session with the key “LoginID”
                HttpContext.Session.SetString("LoginID", customer.CustomerID.ToString());
                // Store user role “Customer” as a string in session with the key “Role” 
                HttpContext.Session.SetString("Role", "Customer");
                //Redirect user to Home/Index view
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the Create view to display error message
                return View(customer);
            }
        }

        // GET: RegShop
        public ActionResult RegisterShop()
        {
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "Central", Value = "Central" });
            li.Add(new SelectListItem { Text = "East", Value = "East" });
            li.Add(new SelectListItem { Text = "North", Value = "North" });
            li.Add(new SelectListItem { Text = "North-East", Value = "North-East" });
            li.Add(new SelectListItem { Text = "West", Value = "West" });
            li.Add(new SelectListItem { Text = "NA", Value = "NA" });
            ViewData["zoneList"] = li;

            return View();
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
                List<SelectListItem> li = new List<SelectListItem>();
                li.Add(new SelectListItem { Text = "Central", Value = "Central" });
                li.Add(new SelectListItem { Text = "East", Value = "East" });
                li.Add(new SelectListItem { Text = "North", Value = "North" });
                li.Add(new SelectListItem { Text = "North-East", Value = "North-East" });
                li.Add(new SelectListItem { Text = "West", Value = "West" });
                li.Add(new SelectListItem { Text = "NA", Value = "NA" });
                ViewData["zoneList"] = li;
            }
            catch (Exception ex) //Other type of error
            {
                List<SelectListItem> li = new List<SelectListItem>();
                li.Add(new SelectListItem { Text = "Central", Value = "Central" });
                li.Add(new SelectListItem { Text = "East", Value = "East" });
                li.Add(new SelectListItem { Text = "North", Value = "North" });
                li.Add(new SelectListItem { Text = "North-East", Value = "North-East" });
                li.Add(new SelectListItem { Text = "West", Value = "West" });
                li.Add(new SelectListItem { Text = "NA", Value = "NA" });
                ViewData["zoneList"] = li;
                ViewData["Message"] = ex.Message;
            }
            return View(shopModel);
        }
    

        public IActionResult Login()
        {
            if ((HttpContext.Session.GetString("Role") == "Admin") || (HttpContext.Session.GetString("Role") == "Shop") || (HttpContext.Session.GetString("Role") == "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            } 
        }
        

        [HttpPost]
        public ActionResult Login(IFormCollection FormData)
        {
            string email = FormData["txtEmail"].ToString().ToLower().Trim();
            string password = FormData["txtPassword"].ToString();

            //Check if login is Customer
            List<Customer> customerList = customerContext.GetAllCustomer();
            foreach (Customer customer in customerList)
            {
                if (customer.EmailAddr.ToLower() == email && customer.Password == password)
                {
                    // Store Login ID in session with the key “LoginID”
                    HttpContext.Session.SetInt32("LoginID", customer.CustomerID);
                    // Store user role “customer” as a string in session with the key “Role” 
                    HttpContext.Session.SetString("Role", "Customer");
                    return RedirectToAction("Index","Customer");
                }
            }
            //Check if login is Shop
            List<Shop> shopList = shopContext.GetAllShop();
            foreach (Shop shop in shopList)
            {
                if (shop.EmailAddr.ToLower() == email && shop.Password == password)
                {
                    // Store Login ID in session with the key “LoginID”
                    HttpContext.Session.SetInt32("LoginID", shop.ShopID);
                    // Store user role “customer” as a string in session with the key “Role” 
                    HttpContext.Session.SetString("Role", "Shop");
                    //Redirect user to Shops/Index view
                    return RedirectToAction("Index", "Shops");
                }
            }
            if (email == "admin@greendoor.sg" && password == "adminpass")
            {
                // Store Login ID in session with the key “LoginID”
                HttpContext.Session.SetString("LoginID", email);
                // Store user role “Staff” as a string in session with the key “Role” 
                HttpContext.Session.SetString("Role", "Admin");
                return RedirectToAction("Index", "Admin");
            }
            //Invalid credentials
            else
            {
                // Store an error message in TempData for display at the index view
                TempData["Message"] = "Invalid Login Credentials!";

                // Redirect user back to the index view through an action
                return RedirectToAction("Login");
            }
        }

        public ActionResult LogOut()
        {
            // Clear all key-values pairs stored in session state
            HttpContext.Session.Clear();
            // Call the Index action of Home controller
            return RedirectToAction("Index");
        }


        // GET: About Us
        public ActionResult About()
        {
            return View();
        }

        // GET: User Profile
        public ActionResult UserProfile()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
    }
}
