using Project_Ql_ThueXe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DHGo.Controllers
{
    public class UsersController : Controller
    {
        // GET: User
        QL_ThueXeEntities db = new QL_ThueXeEntities();
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            var TenDN = f["sTenDN"];
            var MatKhau = f["sMatKhau"];
            NGUOIDUNG nd = db.NGUOIDUNG.SingleOrDefault(n => n.UserName == TenDN && n.Pass == MatKhau && n.QUYENTRUYCAP.MaQTC == 2);
            if (nd != null)
            {
                ViewBag.ThongBao = "Chúc mừng bạn đăng nhập thành công!";
                Session["Taikhoan"] = nd;
                return RedirectToAction("Home", "Home", new { area = "" });
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng!";
            }
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(FormCollection f)
        {
            var sTen = f["sTen"];
            var sEmail = f["sEmail"];
            var sNgaySinh = Convert.ToDateTime(f["sNgaySinh"]);
            var sDiaChi = f["sDiaChi"];
            var sGioiTinh = f["sGioiTinh"];
            var sTenDN = f["sTenDN"];
            var sMatKhau = f["sMatKhau"];
            var sMatKhauNL = f["sXacNhanMatKhau"];
            if(sNgaySinh > DateTime.Now)
            {
                ViewBag.sTen = sTen;
                ViewBag.sEmail = sEmail;
                ViewBag.sNgaySinh = sNgaySinh;
                ViewBag.sDiaChi = sDiaChi;
                ViewBag.sTenDN = sTenDN;
                ViewBag.MatKhau = sMatKhau;
                ViewBag.MatKhauNL = sMatKhauNL;
                ViewBag.ThongBao = "Ngày sinh không hợp lệ!";
                return View("Register");
            }
            else if (db.NGUOIDUNG.SingleOrDefault(n => n.Email == sEmail)!= null)
            {
                ViewBag.sTen = sTen;
                ViewBag.sEmail = sEmail;
                ViewBag.sNgaySinh = sNgaySinh;
                ViewBag.sDiaChi = sDiaChi;
                ViewBag.sTenDN = sTenDN;
                ViewBag.MatKhau = sMatKhau;
                ViewBag.MatKhauNL = sMatKhauNL;
                ViewBag.ThongBao = "Email đã được sử dụng!";
                return View("Register");
            }
            else if(db.NGUOIDUNG.SingleOrDefault(n => n.UserName == sTenDN) != null)
            {
                ViewBag.sTen = sTen;
                ViewBag.sEmail = sEmail;
                ViewBag.sNgaySinh = sNgaySinh;
                ViewBag.sDiaChi = sDiaChi;
                ViewBag.sTenDN = sTenDN;
                ViewBag.MatKhau = sMatKhau;
                ViewBag.MatKhauNL = sMatKhauNL;
                ViewBag.ThongBao = "Tên đăng nhập đã được sử dụng!";
                return View("Register");
            }
            else if(sMatKhau != sMatKhauNL)
            {
                ViewBag.sTen = sTen;
                ViewBag.sEmail = sEmail;
                ViewBag.sNgaySinh = sNgaySinh;
                ViewBag.sDiaChi = sDiaChi;
                ViewBag.sTenDN = sTenDN;
                ViewBag.MatKhau = sMatKhau;
                ViewBag.MatKhauNL = sMatKhauNL;
                ViewBag.ThongBao = "Mật khẩu nhập lại không trùng khớp!";
                return View("Register");
            }
            else
            {
                NGUOIDUNG nGUOIDUNG = new NGUOIDUNG();
                ViewBag.ThongBao = "Đăng ký người dùng thành công!";
                nGUOIDUNG.TenNguoiDung = sTen;
                nGUOIDUNG.GioiTinh = sGioiTinh;
                nGUOIDUNG.NgaySinh = sNgaySinh;
                nGUOIDUNG.Email = sEmail;
                nGUOIDUNG.DiaChi = sDiaChi;
                nGUOIDUNG.UserName = sTenDN;
                nGUOIDUNG.Pass = sMatKhau;
                nGUOIDUNG.MaQTC = 2;
                db.NGUOIDUNG.Add(nGUOIDUNG);
                db.SaveChanges();
                return View("Register");
            }
           
        }
    }
}