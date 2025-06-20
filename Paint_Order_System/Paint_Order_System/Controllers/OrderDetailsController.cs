﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Paint_Order_System.Models;

namespace Paint_Order_System.Controllers
{
    public class OrderDetailsController : Controller
    {
        private PaintIndustryDBEntities db = new PaintIndustryDBEntities();

        // GET: OrderDetails
        public ActionResult Index()
        {
            var orderDetails = db.OrderDetails.Include(o => o.Order).Include(o => o.Product);
            return View(orderDetails.ToList());
        }

        // GET: OrderDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            OrderDetail orderDetail = db.OrderDetails.Find(id);
            if (orderDetail == null)
                return HttpNotFound();

            return View(orderDetail);
        }

        // GET: OrderDetails/Create
        public ActionResult Create()
        {
            ViewBag.OrderID = db.Orders.Include(o => o.Customer).ToList().Select(o => new SelectListItem
            {
                Value = o.OrderID.ToString(),
                Text = "Order #" + o.OrderID + " - " + o.Customer.Name
            });

            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName");
            return View();
        }

        // POST: OrderDetails/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderDetailID,OrderID,ProductID,Quantity,UnitPrice")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                db.OrderDetails.Add(orderDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderID = db.Orders.Include(o => o.Customer).ToList().Select(o => new SelectListItem
            {
                Value = o.OrderID.ToString(),
                Text = "Order #" + o.OrderID + " - " + o.Customer.Name,
                Selected = (o.OrderID == orderDetail.OrderID)
            });

            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", orderDetail.ProductID);
            return View(orderDetail);
        }

        // GET: OrderDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            OrderDetail orderDetail = db.OrderDetails.Find(id);
            if (orderDetail == null)
                return HttpNotFound();

            ViewBag.OrderID = db.Orders.Include(o => o.Customer).ToList().Select(o => new SelectListItem
            {
                Value = o.OrderID.ToString(),
                Text = "Order #" + o.OrderID + " - " + o.Customer.Name,
                Selected = (o.OrderID == orderDetail.OrderID)
            });

            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", orderDetail.ProductID);
            return View(orderDetail);
        }

        // POST: OrderDetails/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderDetailID,OrderID,ProductID,Quantity,UnitPrice")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderID = db.Orders.Include(o => o.Customer).ToList().Select(o => new SelectListItem
            {
                Value = o.OrderID.ToString(),
                Text = "Order #" + o.OrderID + " - " + o.Customer.Name,
                Selected = (o.OrderID == orderDetail.OrderID)
            });

            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", orderDetail.ProductID);
            return View(orderDetail);
        }

        // GET: OrderDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            OrderDetail orderDetail = db.OrderDetails.Find(id);
            if (orderDetail == null)
                return HttpNotFound();

            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            db.OrderDetails.Remove(orderDetail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}
