using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Project_Ql_ThueXe.Models;
namespace Project_Ql_ThueXe.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        QL_ThueXeEntities db = new QL_ThueXeEntities();
        // GET: Admin/Admin
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            var sTenDN = f["Username"];
            var sMatKhau = f["Password"];

            if (db.NGUOIDUNG.SingleOrDefault(n => n.UserName == sTenDN && n.Pass == sMatKhau) == null)
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không chính xác";
            }
            else
            {
                NGUOIDUNG nguoiDung = db.NGUOIDUNG.SingleOrDefault(n => n.UserName == sTenDN && n.Pass == sMatKhau);
                if (nguoiDung.QUYENTRUYCAP.TenQTC == "Admin")
                {
                    Session["Admin"] = nguoiDung;
                    return RedirectToAction("Home_Admin", "Admin");
                }
                else
                {
                    ViewBag.ThongBao = "Bạn không có quyền truy cập vào trang Admin";
                    return View();
                }
            }
            return View("DangNhap");
        }
        [HttpGet]
        public ActionResult Home_Admin()
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "Admin");
            }
            else
            {
                return View();
            }
        }
        public ActionResult Header_Admin()
        {
            return PartialView("_HeaderAdminPartial");
        }
        public ActionResult Menu_Admin()
        {
            return PartialView("_MenuAdminPartial"); 
        }
        [HttpGet]
        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("DangNhap", "Admin");
        }
        [HttpGet]
        public ActionResult QuanLyNguoiDung(int ?page)
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "Admin");
            }
            else
            {
                ViewBag.ThongBao = TempData["ThongBao"];
                int iSize = 10;
                int iPageNum = (page ?? 1);
                var dsNguoiDung = from lx in db.NGUOIDUNG where lx.MaQTC == 2 select lx;
                ViewBag.StartingIndex = (iPageNum - 1) * iSize + 1;
                return View(dsNguoiDung.OrderBy(s => s.MaNguoiDung).ToPagedList(iPageNum, iSize));
            }
        }
        [HttpGet]
        public ActionResult ThemMoi_NguoiDung()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemMoi_NguoiDung(FormCollection f, NGUOIDUNG ng)
        {
            var sTen = f["sTen"];
            var sEmail = f["sEmail"];
            var sNgaySinh = f["sNgaySinh"];
            var sGioiTinh = f["sGioiTinh"];
            var sDiaChi = f["sDiaChi"];
            var sUserName = f["sUserName"];
            var sPass = f["sPass"];
            
            if(db.NGUOIDUNG.SingleOrDefault(n=> n.Email == sEmail) != null)
            {
                ViewBag.Ten = sTen;
                ViewBag.Email = sEmail;
                ViewBag.NgaySinh = sNgaySinh;
                ViewBag.GioiTinh = sGioiTinh;
                ViewBag.DiaChi = sDiaChi;
                ViewBag.UserName = sUserName;
                ViewBag.ThongBao = "Địa chỉ Email đã được sử dụng!";
                return View();
            }else if(db.NGUOIDUNG.SingleOrDefault(n => n.UserName== sUserName) != null)
            {
                ViewBag.Ten = sTen;
                ViewBag.Email = sEmail;
                ViewBag.NgaySinh = sNgaySinh;
                ViewBag.GioiTinh = sGioiTinh;
                ViewBag.DiaChi = sDiaChi;
                ViewBag.UserName = sUserName;
                ViewBag.ThongBao = "UserName đã được sử dụng!";
                return View();
            }else if(Convert.ToDateTime(sNgaySinh) > DateTime.Now)
            {
                ViewBag.Ten = sTen;
                ViewBag.Email = sEmail;
                ViewBag.NgaySinh = sNgaySinh;
                ViewBag.GioiTinh = sGioiTinh;
                ViewBag.DiaChi = sDiaChi;
                ViewBag.UserName = sUserName;
                ViewBag.ThongBao = "Ngày sinh không hợp lệ!";
                return View();
            }
            else
            {
                ng.TenNguoiDung = sTen;
                ng.GioiTinh = sGioiTinh;
                ng.NgaySinh = Convert.ToDateTime(sNgaySinh);
                ng.Email = sEmail;
                ng.DiaChi = sDiaChi;
                ng.UserName = sUserName;
                ng.Pass = sPass;
                ng.MaQTC = 2;
                db.NGUOIDUNG.Add(ng);
                db.SaveChanges();
                return RedirectToAction("QuanLyNguoiDung", "Admin");
            }
        }
        [HttpGet]
        public ActionResult XemChiTiet_NguoiDung(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var ng = db.NGUOIDUNG.Find(id);

            if (ng == null)
            {
                return HttpNotFound();
            }
            return View(ng);
        }
        [HttpGet]
        public ActionResult Xoa_NguoiDung(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var ng = db.NGUOIDUNG.Find(id);

            if (ng == null)
            {
                return HttpNotFound();
            }
            return View(ng);
        }
        [HttpPost]
        public ActionResult Xoa_NguoiDung(FormCollection f)
        {
            var id = int.Parse(f["sMaNguoiDung"]);

            var ng = db.NGUOIDUNG.Find(id);

            if (ng == null)
            {
                return HttpNotFound();
            }
            db.NGUOIDUNG.Remove(ng);
            TempData["ThongBao"] = "Xóa người dùng thành công!";
            db.SaveChanges();
            return RedirectToAction("QuanLyNguoiDung", "Admin");
        }
    }
}