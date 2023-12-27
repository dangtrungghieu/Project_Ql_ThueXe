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
            NGUOIDUNG nd = db.NGUOIDUNG.SingleOrDefault(n => n.UserName == TenDN && n.Pass == MatKhau);
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
    }
}