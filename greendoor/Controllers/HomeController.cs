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
using greendoor.DAL;

namespace greendoor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private List<string> salutList = new List<string> { "Mr", "Mrs", "Mdm", "Dr" };
        private List<SelectListItem> salutDropDownList = new List<SelectListItem>();
        private CustomerDAL customerContext = new CustomerDAL();
        private ShopDAL shopContext = new ShopDAL();

        public HomeController(ILogger<HomeController> logger)
        {
            int i = 1;
            foreach (string salut in salutList)
            {
                salutDropDownList.Add(
                    new SelectListItem
                    {
                        Text = salut,
                    });
                i++;
            }
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
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
                //Add staff record to database
                customer.CustomerID = customerContext.Add(customer);
                //Redirect user to Staff/Index view
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(customer);
            }
        }
        /*
        // GET: RegShop
        public ActionResult RegisterShop()
        {
            return View();
        }

        // POST: RegShop
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterShop(Shop shop)
        {
            if (ModelState.IsValid)
            {
                //Add staff record to database
                customer.CustomerID = customerContext.Add(customer);
                //Redirect user to Staff/Index view
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(customer);
            }
        }
        */

        public IActionResult Login()
        {
            if ((HttpContext.Session.GetString("Role") == "Admin") || (HttpContext.Session.GetString("Role") == "Owner") || (HttpContext.Session.GetString("Role") == "Customer"))
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
            string email = FormData["txtEmail"].ToString().Trim();
            string password = FormData["txtPassword"].ToString();

            //Check if login is Customer
            List<Customer> customerList = customerContext.GetAllCustomer();
            foreach (Customer customer in customerList)
            {
                if (customer.EmailAddr.ToLower() == email && customer.Password == password)
                {
                    // Store Login ID in session with the key “LoginID”
                    HttpContext.Session.SetString("LoginID", customer.CustomerID.ToString());
                    // Store user role “customer” as a string in session with the key “Role” 
                    HttpContext.Session.SetString("Role", "Customer");
                    return RedirectToAction("Index");
                }
            }
            //Check if login is Shop
            List<Shop> shopList = shopContext.GetAllShop();
            foreach (Shop shop in shopList)
            {
                if (shop.EmailAddr.ToLower() == email && shop.Password == password)
                {
                    // Store Login ID in session with the key “LoginID”
                    HttpContext.Session.SetString("LoginID", shop.ShopID.ToString());
                    // Store user role “customer” as a string in session with the key “Role” 
                    HttpContext.Session.SetString("Role", "Shop");
                    return RedirectToAction("Index");
                }
            }
            if (email == "admin@greendoor.sg" && password == "adminpass")
            {
                // Store Login ID in session with the key “LoginID”
                HttpContext.Session.SetString("LoginID", email);
                // Store user role “Staff” as a string in session with the key “Role” 
                HttpContext.Session.SetString("Role", "Admin");
                return RedirectToAction("Index");
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
