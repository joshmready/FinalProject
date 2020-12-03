using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalProject.DATA.EF;
using Microsoft.AspNet.Identity;

namespace FinalProject.UI.MVC.Controllers
{
    public class ReservationsController : Controller
    {
        private ReservationSystemEntities db = new ReservationSystemEntities();

        // GET: Reservations
        [Authorize]
        public ActionResult Index()
        {
            if (User.IsInRole("Customer"))
            {
                string currentUserID = User.Identity.GetUserId();
                var reservations = db.Reservations.Where(r => r.CustomerAsset.CustomerID == currentUserID).Include(r => r.CustomerAsset).Include(r => r.Location);
                return View(reservations.ToList());
            }
            else
            {
                string currentUserID = User.Identity.GetUserId();
                var reservations = db.Reservations.Include(r => r.CustomerAsset).Include(r => r.Location);
                return View(reservations.ToList());
            }
            
        }

        // GET: Reservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: Reservations/Create
        public ActionResult Create()
        {
            string currentUserID = User.Identity.GetUserId();

            ViewBag.CustomerAssetID = new SelectList(db.CustomerAssets.Where(ca => ca.CustomerID == currentUserID), "CustomerAssetID", "AssetName");
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReservationID,CustomerAssetID,LocationID,ReservationDate")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {

                //if the User is NOT an Admin
                if (!User.IsInRole("Admin"))
                {
                    //find the reservation limit for this location
                    int limit = db.Locations.Where(l => l.LocationID == reservation.LocationID).Select(l => l.ReservationLimit).Single();

                    //find the number of resevations at that location ON that date
                    int numberRes = db.Reservations.Where(r => r.LocationID == reservation.LocationID && r.ReservationDate == reservation.ReservationDate).Count();

                    //check the resevation limit < nbr of reservations
                    if (numberRes < limit)
                    {
                        //add the reservation
                        db.Reservations.Add(reservation);
                        db.SaveChanges();
                        return RedirectToAction("Index");

                    }
                    else
                    //viewbag message that reservations are full
                    {
                        ViewBag.CustomerMessage = $"Sorry! It appears that we are either fully booked that day, or closed. Please choose another day we are open.";
                    }

                }

                //IF THE USER IS AN ADMIN (else)
                else
                {
                    db.Reservations.Add(reservation);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
            }

            string currentUserID = User.Identity.GetUserId();

            ViewBag.CustomerAssetID = new SelectList(db.CustomerAssets.Where(ca => ca.CustomerID == currentUserID), "CustomerAssetID", "AssetName", reservation.CustomerAssetID);
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName", reservation.LocationID);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerAssetID = new SelectList(db.CustomerAssets, "CustomerAssetID", "AssetName", reservation.CustomerAssetID);
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName", reservation.LocationID);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReservationID,CustomerAssetID,LocationID,ReservationDate")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerAssetID = new SelectList(db.CustomerAssets, "CustomerAssetID", "AssetName", reservation.CustomerAssetID);
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName", reservation.LocationID);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
