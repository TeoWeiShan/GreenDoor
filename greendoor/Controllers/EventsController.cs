using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using greendoor.DAL;
using greendoor.Models;

namespace greendoor.Controllers
{
    public class EventsController : Controller
    {
        // GET: EventsController
        private EventDAL eContext = new EventDAL();

        public ActionResult Index()
        {
            List<EcoEvents> eList = eContext.GetAllEvent();
            return View(eList);
        }
        
        // GET: EventsController/Details/5
        public ActionResult Details(int id)
        {
            
            EcoEvents e = eContext.GetDetails(id);
            if (e.EventName == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(e);

        }

        // GET: EventsController/Create
       
    
        public ActionResult Create()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Owner"))
            {
                return RedirectToAction("Index", "Home");
            }

           // ViewData["AreaInterestList"] = GetAreaInterest();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EcoEvents e)
        {
            //Get country list for drop-down list
            //in case of the need to return to Create.cshtml view
           // ViewData["AreaInterestList"] = GetAreaInterest();
            if (ModelState.IsValid)
            {
                //Add staff record to database
                e.EventID = eContext.Add(e);
                //Redirect user to Competition/Index view
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(e);
            }
        }

        // GET: EventsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EventsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EventsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EventsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        // GET: EventController/Details/5
        
    }
}
