﻿using WebSiteMCSport.Models;
using WebSiteMCSport.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanHangThoiTrangMVC.Models.EF;

namespace WebSiteMCSport.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Products
        public ActionResult Index(string Searchtext, int? id)
        {
            /*var items = db.Products.Where(x => x.IsActive).Take(12).ToList();*/
            IEnumerable<Product> items = db.Products.OrderByDescending(x => x.Id);
            items = items.Where(x => x.IsActive).Take(12).ToList();
            /*if (id != null)
            {
                items = items.Where(x => x.ProductCategoryId == id).ToList();
            }*/
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Alias.Contains(Searchtext) || x.Title.Contains(Searchtext));
            }
            return View(items);
        }

        public ActionResult ProductCategory(string alias, int id)
        {
            var items = db.Products.Where(x => x.IsActive).Take(12).ToList();
            if (id > 0)
            {
                items = items.Where(x => x.ProductCategoryId == id).ToList();
            }
            var cate = db.ProductCategories.Find(id);
            if (cate != null)
            {
                ViewBag.CateName = cate.Title;
            }
            ViewBag.CateId = id;
            return View(items);
        }

        public ActionResult Partial_ItemsByCateId()
        {
            var items = db.Products.Where(x => x.IsHome && x.IsActive).Take(12).ToList();
            return PartialView(items);
        }

        public ActionResult Partial_ProductSale()
        {
            var items = db.Products.Where(x => x.IsSale && x.IsActive).Take(12).ToList();
            return PartialView(items);
        }

        public ActionResult Detail(string alias, int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                db.Products.Attach(item);
                item.ViewCount = item.ViewCount + 1;
                if (item.ViewCount == 1000)
                {
                    item.ViewCount = 0;
                }
                db.Entry(item).Property(x => x.ViewCount).IsModified = true;
                db.SaveChanges();
            }
            ViewBag.Reviews = db.ProductReviews
                        .Where(r => r.ProductId == id)
                        .OrderByDescending(r => r.CreatedAt)
                        .ToList();
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitReview(ProductReview model)
        {
            if (ModelState.IsValid)
            {
                var review = new ProductReview
                {
                    ProductId = model.ProductId,
                    Name = model.Name,
                    Email = model.Email,
                    Message = model.Message,
                    Rating = model.Rating,
                    CreatedAt = DateTime.Now
                };

                db.ProductReviews.Add(review);
                db.SaveChanges();

                // ✅ Lấy alias của sản phẩm để redirect
                var product = db.Products.Find(model.ProductId);
                if (product != null)
                {
                    TempData["ReviewSuccess"] = "Cảm ơn bạn đã gửi đánh giá!";
                    return RedirectToAction("Detail", "Products", new { alias = product.Alias, id = product.Id });
                }
            }

            TempData["ReviewError"] = "Vui lòng điền đầy đủ thông tin.";
            return RedirectToAction("Detail", "Products", new { alias = "san-pham", id = model.ProductId }); // fallback nếu lỗi
        }
    }
    }