﻿using PagedList;
using Project_Ql_ThueXe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;

namespace DHGo.Controllers
{
    public class HomeController : Controller
    {
        QL_ThueXeEntities db = new QL_ThueXeEntities();
        public ActionResult Home()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Home(FormCollection f)
        {
            NGUOIDUNG nGUOIDUNG = (NGUOIDUNG)Session["TaiKhoan"];

            NGUOIDUNG ng = db.NGUOIDUNG.SingleOrDefault(n => n.MaNguoiDung == nGUOIDUNG.MaNguoiDung);

            if (ng.Pass != f["sPass"])
            {

            }
            else if (f["sConfirmNewPass"] != f["sNewPass"])
            {

            }
            else
            {
                ng.Pass = f["sNewPass"];
                db.SaveChanges();
            }
            Session["TaiKhoan"] = ng;
            return RedirectToAction("Home");
        }

        public ActionResult BangGia()
        {
            List<LOAIXE> danhSachLoaiXe = db.LOAIXE.ToList();
            return View(danhSachLoaiXe);
        }
        [HttpGet]
        public ActionResult DiaChiTram(int ?page)
        {
                int iSize = 5;
                int iPageNum = (page ?? 1);
                var dsTram = from lx in db.TRAM select lx;
                ViewBag.StartingIndex = (iPageNum - 1) * iSize + 1;
                return View(dsTram.OrderBy(s => s.MaTram).ToPagedList(iPageNum, iSize));
        }
        public ActionResult HuongDan()
        { 
            return View(); 
        }
        public ActionResult ChiTietXe(int? id)
        {
            LOAIXE lx = db.LOAIXE.Find(id);
            return View(lx);
        }
        [HttpGet]
        public ActionResult NavHome()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NavHome(FormCollection f)
        {
            NGUOIDUNG nGUOIDUNG = (NGUOIDUNG)Session["TaiKhoan"];

            NGUOIDUNG ng = db.NGUOIDUNG.SingleOrDefault(n => n.MaNguoiDung == nGUOIDUNG.MaNguoiDung);

            if (ng.Pass != f["sPass"])
            {

            }
            else if (f["sConfirmNewPass"] != f["sNewPass"])
            {

            }
            else
            {
                ng.Pass = f["sNewPass"];
                db.SaveChanges();
            }
            Session["TaiKhoan"] = ng;
            return RedirectToAction("Home");
        }
    }
}