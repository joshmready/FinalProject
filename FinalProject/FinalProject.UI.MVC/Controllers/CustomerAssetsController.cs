using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalProject.DATA.EF;
using MVC3.UI.MVC.Utilities;
using Microsoft.AspNet.Identity;

namespace FinalProject.UI.MVC.Controllers
{
    public class CustomerAssetsController : Controller
    {
        private ReservationSystemEntities db = new ReservationSystemEntities();

        // GET: CustomerAssets
        [Authorize]
        public ActionResult Index()
        {
            if (User.IsInRole("Customer"))
            {
                string currentUserID = User.Identity.GetUserId();
                var customerAssets = db.CustomerAssets.Where(ca => ca.CustomerID == currentUserID).Include(c => c.UserDetail);
                return View(customerAssets.ToList());
            }
            else
            {
                string currentUserID = User.Identity.GetUserId();
                var customerAssets = db.CustomerAssets.Include(c => c.UserDetail);
                return View(customerAssets.ToList());
            }
        }

        // GET: CustomerAssets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerAsset customerAsset = db.CustomerAssets.Find(id);
            if (customerAsset == null)
            {
                return HttpNotFound();
            }
            return View(customerAsset);
        }

        // GET: CustomerAssets/Create
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.UserDetails, "UserID", "FirstName");
            return View();
        }

        // POST: CustomerAssets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerAssetID,AssetName,CustomerID,AssetPhoto,SpecialNotes,IsActive,DateAdded")] CustomerAsset customerAsset, HttpPostedFileBase assetImage)
        {
            if (ModelState.IsValid)
            {

                #region File Upload - Using the Image Service

                //use a default image if one is not provided when the record is created - noImage.png
                string imgName = "noImage.png";

                //branch - to make sure the input type file (HttpPostedFileBase) is NOT null (it has a file)
                if (assetImage != null)
                {
                    //retrieve the image from HPFB and assign to our image variable
                    imgName = assetImage.FileName;

                    //declare and assign the extension
                    string ext = imgName.Substring(imgName.LastIndexOf('.'));

                    //create a list of valid extensions
                    string[] goodExts = { ".jpeg", ".jpg", ".gif", ".png" };

                    //check the extension against the list of valid extensions and make sure the file size is 4MB or LESS (ASP.net limit)
                    //if all requirements are met - do the following
                    if (goodExts.Contains(ext.ToLower()) && (assetImage.ContentLength <= 4194304))//4mb in bytes
                    {
                        //rename the file using a GUID (Global Unique IDentifier) and concatonate with the extension.
                        imgName = Guid.NewGuid() + ext.ToLower();//toLower() is not required, does ensure all ext's are lower case

                        //other renaming options - Make sure your data type SIZE can accommodate your unique Naming convention
                        //in the data type in the database - ours is 100,  but our title for books 150 - 
                        //They should be unique (advantage the guid) - if not using a guid the name should be meaningfull

                        //declare the var = if the title more than 10 substring the first 10, other wise use the title property
                        //string shortTitle = book.BookTitle.Length > 10 ? book.BookTitle.Substring(0, 10) : book.BookTitle;
                        //imgName = shortTitle + "_" + DateTime.Now + "_" + User.Identity.Name;
                        //reassign var ShortTitle_DateAdded_UserInfo
                        //for the user to be added, you MUST make sure the person adding a record is LOGGED IN.
                        //regular file saving WITHOUT RESIZE

                        //bookCover.SaveAs(Server.MapPath("~/Content/imgstore/books/" + imgName));
                        //RESIZE IMAGE UTILITY
                        //provide the path for saving the image
                        string savePath = Server.MapPath("~/Content/CustomImages/");

                        //image value for the converted image
                        Image convertedImage = Image.FromStream(assetImage.InputStream);

                        //max image size
                        int maxImageSize = 500;

                        //max thumbnail size
                        int maxThumbSize = 100;

                        //Call the imageService.ResizeImage() - (Utilities Folder)
                        ImageService.ResizeImage(savePath, imgName, convertedImage, maxImageSize, maxThumbSize);
                    }
                    else
                    {
                        //If the ext is not valid of the file size is too large - default back to the default image
                        imgName = "noImage.png";
                    }
                }
                //No matter what - add the imgName value to the bookImage property of the Book Object. 
                customerAsset.AssetPhoto = imgName;

                #endregion
                db.CustomerAssets.Add(customerAsset);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.UserDetails, "UserID", "FirstName", customerAsset.CustomerID);
            return View(customerAsset);
        }

        // GET: CustomerAssets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerAsset customerAsset = db.CustomerAssets.Find(id);
            if (customerAsset == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.UserDetails, "UserID", "FirstName", customerAsset.CustomerID);
            return View(customerAsset);
        }

        // POST: CustomerAssets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerAssetID,AssetName,CustomerID,AssetPhoto,SpecialNotes,IsActive,DateAdded")] CustomerAsset customerAsset, HttpPostedFileBase assetImage)
        {
            if (ModelState.IsValid)
            {

                #region File Upload - Using the Image Service
                //no default image to be concerned with, all records in the database should have a valid file name
                //AND all files in the database shold be represented in the Website Content folder.
                //if there is NO FILE in the input, maintain the existing image (Front End using the HiddenFor() field)



                //branch - to make sure the input type file (HttpPostedFileBase) is NOT null (it has a file)
                if (assetImage != null)
                {

                    //use a default image if one is not provided when the record is created - noImage.png
                    string imgName = assetImage.FileName;

                    //retrieve the image from HPFB and assign to our image variable
                    //imgName = ProductImage.FileName; ----------------I changed this chasing an answer-------------

                    //declare and assign the extension
                    string ext = imgName.Substring(imgName.LastIndexOf('.'));

                    //create a list of valid extensions
                    string[] goodExts = { ".jpeg", ".jpg", ".gif", ".png" };

                    //check the extension against the list of valid extensions and make sure the file size is 4MB or LESS (ASP.net limit)
                    //if all requirements are met - do the following
                    if (goodExts.Contains(ext.ToLower()) && (assetImage.ContentLength <= 4194304))//4mb in bytes
                    {
                        //rename the file using a GUID (Global Unique IDentifier) and concatonate with the extension.
                        imgName = Guid.NewGuid() + ext.ToLower();//toLower() is not required, does ensure all ext's are lower case

                        //other renaming options - Make sure your data type SIZE can accommodate your unique Naming convention
                        //in the data type in the database - ours is 100,  but our title for books 150 - 
                        //They should be unique (advantage the guid) - if not using a guid the name should be meaningfull

                        //declare the var = if the title more than 10 substring the first 10, other wise use the title property
                        //string shortTitle = book.BookTitle.Length > 10 ? book.BookTitle.Substring(0, 10) : book.BookTitle;
                        //imgName = shortTitle + "_" + DateTime.Now + "_" + User.Identity.Name;
                        //reassign var ShortTitle_DateAdded_UserInfo
                        //for the user to be added, you MUST make sure the person adding a record is LOGGED IN.
                        //regular file saving WITHOUT RESIZE

                        //bookCover.SaveAs(Server.MapPath("~/Content/imgstore/books/" + imgName));
                        //RESIZE IMAGE UTILITY
                        //provide the path for saving the image
                        string savePath = Server.MapPath("~/Content/CustomImages/");

                        //image value for the converted image
                        Image convertedImage = Image.FromStream(assetImage.InputStream);

                        //max image size
                        int maxImageSize = 500;

                        //max thumbnail size
                        int maxThumbSize = 100;

                        //Call the imageService.ResizeImage() - (Utilities Folder)
                        ImageService.ResizeImage(savePath, imgName, convertedImage, maxImageSize, maxThumbSize);

                        //DELETE from the Image Service and delete the old image
                        //--Image Service Makes sure the file is NOT noImage.png && that it exists on the server BEFORE deleting
                        //we don't need to do that check
                        ImageService.Delete(savePath, customerAsset.AssetPhoto);

                        customerAsset.AssetPhoto = imgName;
                    }
                }
                //No matter what - add the imgName value 

                #endregion

                db.Entry(customerAsset).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.UserDetails, "UserID", "FirstName", customerAsset.CustomerID);
            return View(customerAsset);
        }

        // GET: CustomerAssets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerAsset customerAsset = db.CustomerAssets.Find(id);
            if (customerAsset == null)
            {
                return HttpNotFound();
            }
            return View(customerAsset);
        }

        // POST: CustomerAssets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerAsset customerAsset = db.CustomerAssets.Find(id);
            db.CustomerAssets.Remove(customerAsset);

            ImageService.Delete(Server.MapPath("~/Content/CustomImages/"), customerAsset.AssetPhoto);

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
