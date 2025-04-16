using BanHangThoiTrangMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Rotativa;
using BanHangThoiTrangMVC.Models.EF;

namespace BanHangThoiTrangMVC.Areas.Admin.Controllers
{
    public class StatisticalController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetStatistical(string fromDate, string toDate)
        {
            var result = GetData(fromDate, toDate);
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportPdf(string fromDate, string toDate)
        {
            var query = from o in db.Orders
                        join od in db.OrderDetails on o.Id equals od.OrderId
                        join p in db.Products on od.ProductId equals p.Id
                        select new
                        {
                            CreatedDate = o.CreateDate,
                            Quantity = od.Quantity,
                            Price = od.Price,
                            OriginalPrice = p.OriginalPrice
                        };

            if (!string.IsNullOrEmpty(fromDate))
            {
                DateTime startDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", null); // dùng đúng format input
                query = query.Where(x => x.CreatedDate >= startDate);
            }

            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", null).AddDays(1);
                query = query.Where(x => x.CreatedDate < endDate);
            }

            var result = query
                .GroupBy(x => DbFunctions.TruncateTime(x.CreatedDate))
                .Select(x => new BCDoanhThu
                {
                    Date = x.Key.Value,
                    DoanhThu = x.Sum(y => y.Quantity * y.Price),
                    LoiNhuan = x.Sum(y => y.Quantity * y.Price) - x.Sum(y => y.Quantity * y.OriginalPrice)
                })
                .OrderBy(x => x.Date)
                .ToList();

            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;

            return new Rotativa.ViewAsPdf("PdfReport", result)
            {
                FileName = "BaoCaoDoanhThu.pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait
            };
        }




        private List<BCDoanhThu> GetData(string fromDate, string toDate)

        {
            var query = from o in db.Orders
                        join od in db.OrderDetails on o.Id equals od.OrderId
                        join p in db.Products on od.ProductId equals p.Id
                        select new
                        {
                            CreatedDate = o.CreateDate,
                            Quantity = od.Quantity,
                            Price = od.Price,
                            OriginalPrice = p.OriginalPrice
                        };

            if (!string.IsNullOrEmpty(fromDate))
            {
                DateTime startDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", null);
                query = query.Where(x => x.CreatedDate >= startDate);
            }

            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", null).AddDays(1); // bao gồm ngày kết thúc
                query = query.Where(x => x.CreatedDate < endDate);
            }

            var result = query
                .GroupBy(x => DbFunctions.TruncateTime(x.CreatedDate))
                .Select(x => new BCDoanhThu
                {
                    Date = x.Key.Value,
                    DoanhThu = x.Sum(y => y.Quantity * y.Price),
                    LoiNhuan = x.Sum(y => y.Quantity * y.Price) - x.Sum(y => y.Quantity * y.OriginalPrice)
                })
                .OrderBy(x => x.Date)
                .ToList();

            return result;
        }
    }
}
