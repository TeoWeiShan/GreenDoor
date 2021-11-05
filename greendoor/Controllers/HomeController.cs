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
        public ActionResult HomeLogin(IFormCollection FormData)
        {
            string email = FormData["txtEmail"].ToString().Trim();
            string password = FormData["txtPassword"].ToString();
            /*Customer customer = customerContext.LoginCustomer(email);
            Owner owner = ownerContext.LoginOwner(email);
            if(email=="admin1@gmail.com" && password == "admin3699") //for admin
            {
                 HttpContext.Session.SetString("Role",  "Admin");
                return RedirectToAction("Index","Admin")
            }
            if(email==customer.EmailAddr && password == customer.Password)
            {
                HttpContext.Session.SetInt32("customerId", customer.CustomerID);
                HttpContext.Session.SetString("Name", customer.CustomerName);
                HttpContext.Session.SetString("Role", "Customer");

                // Redirect user to the "create" view through an action
                return RedirectToAction("Index", "Customer");
            }
            else if((email == owner.EmailAddr && password == owner.Password))
            {
                HttpContext.Session.SetInt32("ownerId", owner.OwnerID);
                HttpContext.Session.SetInt32("shopID", owner.ShopID);
                HttpContext.Session.SetString("ownerName", owner.OwnerName);
                HttpContext.Session.SetString("shopName", owner.ShopName);
                HttpContext.Session.SetString("Role", "Owner");

                return RedirectToAction("Index", "Owner");
            }
            else
            {
                // Store an error message in TempData for display at the index view
                TempData["Message"] = "Invalid Login Credentials!";
                // Redirect user back to the login view through an action
                return RedirectToAction("Login");
            }
            */
            return View(); //test
        }

        // GET: About Us
        public ActionResult About()
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
