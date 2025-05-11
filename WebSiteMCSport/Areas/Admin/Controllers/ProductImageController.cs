using WebSiteMCSport.Models;
using WebSiteMCSport.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSiteMCSport.Areas.Admin.Controllers
{
    public class ProductImageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/ProductImage
        public ActionResult Index(int Id)
        {
            ViewBag.ProductId = Id;
            var items = db.ProductImages.Where(x => x.ProductId == Id).ToList();
            return View(items);
        }

        public ActionResult AddImage(int productId, string url)
        {
            db.ProductImages.Add(new ProductImage
            {
                ProductId = productId,
                Image = url,
                IsDefault = false
            });
            db.SaveChanges();
            return Json(new { Success = true });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.ProductImages.Find(id);
            db.ProductImages.Remove(item);
            db.SaveChanges();
            return Json(new { success = true });
        }
        [HttpPost]
        public JsonResult UpdateDefault(long id)
        {
            var image = db.ProductImages.Find(id);
            if (image != null)
            {
                // Xóa trạng thái mặc định của tất cả ảnh cùng sản phẩm
                var images = db.ProductImages.Where(x => x.ProductId == image.ProductId).ToList();

                foreach (var item in images)
                {
                    item.IsDefault = false;
                    db.Entry(item).State = System.Data.Entity.EntityState.Modified; // Thêm dòng này
                }

                // Set ảnh này là mặc định
                image.IsDefault = true;
                db.Entry(image).State = System.Data.Entity.EntityState.Modified; // Thêm dòng này

                db.SaveChanges();

                return Json(new { success = true });
            }
            return Json(new { success = false });
        }


    }
}