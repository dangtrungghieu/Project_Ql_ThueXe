using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Project_Ql_ThueXe.Models;
namespace Project_Ql_ThueXe.Areas.Admin.Controllers
{
    public class QuanLyGopYController : Controller
    {
        QL_ThueXeEntities db = new QL_ThueXeEntities();
        [HttpGet]
        public ActionResult QuanLyGopY(int?page)
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "Admin");
            }
            else
            {
                int iSize = 10;
                int iPageNum = (page ?? 1);
                var dsGopY = from lx in db.GOPY select lx;
                ViewBag.StartingIndex = (iPageNum - 1) * iSize + 1;
                return View(dsGopY.OrderBy(s => s.MaGY).ToPagedList(iPageNum, iSize));
            }
        }
    }
}