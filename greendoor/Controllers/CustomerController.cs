﻿using greendoor.Models;
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
    public class CustomerController : Controller
    {
        private CustomerDAL custCtx = new CustomerDAL();

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Profile()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            Customer cust = new Customer();
            cust = custCtx.GetDetails((int)HttpContext.Session.GetInt32("LoginID"));
            return View(cust);
        }

        public ActionResult UpdateProfile()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            Customer cust = new Customer();
            cust = custCtx.GetDetails((int)HttpContext.Session.GetInt32("LoginID"));
            return View(cust);
        }

        public ActionResult DeleteProfile()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            Customer cust = new Customer();
            cust = custCtx.GetDetails((int)HttpContext.Session.GetInt32("LoginID"));
            return View(cust);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Customer cust)
        {

            //Add judge record to database
            custCtx.Update(cust);
            //Redirect user to Judge/Create View
            return RedirectToAction("Profile", "Customer");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Customer cust)
        {
            custCtx.Delete(cust);
            HttpContext.Session.Remove("Role");
            //Redirect user to Judge/ Create View
            return RedirectToAction("Index", "Home");
        }
    }
}
