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
    public class TramController : Controller
    {
        QL_ThueXeEntities db = new QL_ThueXeEntities();
        // GET: Admin/Tram
        [HttpGet]
        public ActionResult QuanLyTram(int? page)
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "Admin");
            }
            else
            {
                ViewBag.ThongBao = TempData["ThongBao"];
                int iSize = 5;
                int iPageNum = (page ?? 1);
                var dsTram = from lx in db.TRAM select lx;
                ViewBag.StartingIndex = (iPageNum - 1) * iSize + 1;
                return View(dsTram.OrderBy(s => s.MaTram).ToPagedList(iPageNum, iSize));
            }
        }
        [HttpPost]
        public ActionResult UpdateSessionStatus(int tramId)
        {
            var tram = db.TRAM.Find(tramId);
            if (tram != null)
            {
                if (tram.TinhTrang == "Đang hoạt động")
                {
                    tram.TinhTrang = "Ngừng hoạt động";
                }
                else if (tram.TinhTrang == "Ngừng hoạt động")
                {
                    tram.TinhTrang = "Đang sửa chữa";
                }
                else if (tram.TinhTrang == "Đang sửa chữa")
                {
                    tram.TinhTrang = "Đang hoạt động";
                }

                db.SaveChanges();
                return Content(tram.TinhTrang);
            }

            return Content("Không tìm thấy trạm");
        }
        [HttpGet]
        public ActionResult ThemMoi_Tram()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemMoi_Tram(FormCollection f, TRAM tram)
        {
            var sLong = f["sLong"];
            var sLast = f["sLast"];
            var sDiaChi = f["sDiaChi"];
            var sKiemTra = int.Parse(f["radio1"]);
            var sTinhTrang = f["sTinhTrang"];
            if (sTinhTrang == "")
            {
                ViewBag.ThongBao1 = "Tình trạng không hợp lệ!";
                ViewBag.sLong = sLong;
                ViewBag.sLast = sLast;
                ViewBag.sDiaChi = sDiaChi;
                return View();
            }
            else if (db.TRAM.SingleOrDefault(n => n.DiaChi == sDiaChi) != null)
            {
                ViewBag.ThongBao0 = "Đã có trạm ở vị trí này!";
                ViewBag.sLong = sLong;
                ViewBag.sLast = sLast;
                ViewBag.sDiaChi = sDiaChi;
                return View();
            }
            else if (db.TRAM.SingleOrDefault(n => n.Long == sLong) != null || db.TRAM.SingleOrDefault(n => n.Last == sLast) != null)
            {
                ViewBag.ThongBao2 = "Long với Last đã tồn tại!";
                ViewBag.ThongBao3 = "Long với Last đã tồn tại!";
                ViewBag.sLong = sLong;
                ViewBag.sLast = sLast;
                ViewBag.sDiaChi = sDiaChi;
                return View();
            }
            else
            {
                tram.DiaChi = sDiaChi;
                tram.TinhTrang = sTinhTrang;
                tram.Long = sLong;
                tram.Last = sLast;
                db.TRAM.Add(tram);
                db.SaveChanges();
                TempData["ThongBao"] = "Thêm mới trạm thành công!";
            }
            return RedirectToAction("QuanLyTram", "Tram");
        }
        [HttpGet]
        public ActionResult PhanBo_Tram(int? id, int? page)
        {
            ViewBag.ThongBao = TempData["ThongBao"];
            if (TempData["MaTram"] != null)
            {
                ViewBag.MaTram = TempData["MaTram"];
            }
            else
            {
                ViewBag.MaTram = id;
            }
            int iSize = 7;
            int iPageNum = (page ?? 1);
            var dsXe = from lx in db.XE select lx;
            ViewBag.StartingIndex = (iPageNum - 1) * iSize + 1;
            return View(dsXe.OrderBy(s => s.MaXe).ToPagedList(iPageNum, iSize));
        }
        [HttpPost]
        public ActionResult PhanBo_Tram(PhanBoModel model)
        {
            if (model != null)
            {
                List<string> selectedCars = model.SelectedCars;
                string tramId = model.TramId;
                int tramIDtemp = int.Parse(tramId);
                var tramTemp = db.TRAM.SingleOrDefault(n => n.MaTram == tramIDtemp);
                bool hasBorrowedCar = false;

                foreach (var selectedCar in selectedCars)
                {
                    int carId = int.Parse(selectedCar);
                    var xeTemp = db.XE.SingleOrDefault(n => n.MaXe == carId);

                    if (xeTemp != null && xeTemp.TinhTrang == "Đang được mượn")
                    {
                        hasBorrowedCar = true;
                        TempData["ThongBao"] = "Xe có mã số " + xeTemp.MaXe.ToString() + " không đủ điều kiện!";
                        TempData["MaTram"] = model.TramId;

                        return RedirectToAction("PhanBo_Tram", "Tram", new { id = model.TramId, area = "Admin" });
                    }
                    else if (tramTemp != null && tramTemp.TinhTrang == "Đang sửa chữa")
                    {
                        hasBorrowedCar = true;
                        TempData["ThongBao"] = "Trạm có mã số " + tramTemp.MaTram.ToString() + " không đủ điều kiện!";
                        TempData["MaTram"] = model.TramId;
                        return RedirectToAction("PhanBo_Tram", "Tram", new { id = model.TramId, area = "Admin" });
                    }
                }

                if (!hasBorrowedCar)
                {
                    foreach (var carId in selectedCars)
                    {
                        int xeId = int.Parse(carId);
                        var xeTemp = db.XE.Find(xeId);
                        xeTemp.MaTram = int.Parse(tramId);
                    }
                    TempData["ThongBao"] = "Phân bổ thành công!";
                    db.SaveChanges();
                    return RedirectToAction("QuanLyTram", "Tram", new { area = "Admin" });
                }
            }
            else
            {
                TempData["ThongBao"] = "Không phân bổ thành công!";
            }

            return RedirectToAction("PhanBo_Tram", "Tram", new { id = model.TramId, area = "Admin" });

        }
        [HttpGet]
        public ActionResult XemChiTiet_Tram(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.SoLuongXe = db.XE.Where(n => n.MaTram == id).Count();
            var tram = db.TRAM.Find(id);

            if (tram == null)
            {
                return HttpNotFound();
            }
            return View(tram);
        }
        [HttpGet]
        public ActionResult Xoa_Tram(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tram = db.TRAM.Find(id);

            if (tram == null)
            {
                return HttpNotFound();
            }
            if (tram.TinhTrang == "Đang hoạt động")
            {
                ViewBag.ThongBao = "Trạm đang hoạt động! Vui lòng không xóa trạm!";
            }
            return View(tram);
        }
        [HttpPost]
        public ActionResult Xoa_Tram(FormCollection f)
        {
            var id = int.Parse(f["MaTram"]);
            var sKiemTra = int.Parse(f["radio1"]);
            var tram = db.TRAM.Find(id);
            int soXe = db.XE.Where(n => n.MaTram == id).Count();
            if (sKiemTra == 0)
            {
                ViewBag.ThongBao = "Xóa trạm không thành công! Vui lòng chọn có để xóa trạm!";
                return View(tram);
            }
            else if (tram.TinhTrang == "Đang hoạt động")
            {
                ViewBag.ThongBao = "Trạm đang hoạt động! Vui lòng không xóa trạm!";
                return View(tram);
            }
            else if (soXe > 0)
            {
                ViewBag.ThongBao = "Trong trạm đang có xe! Vui lòng không xóa trạm!";
                return View(tram);
            }else{
                var xe = db.XE.Where(m => m.MaTram == id).ToList();

                foreach (var item in xe)
                {
                    item.MaTram = null;
                }
                db.SaveChanges();
                db.TRAM.Remove(tram);
                db.SaveChanges();
            }
            return RedirectToAction("QuanLyTram");
        }
        [HttpGet]
        public ActionResult SuaThongTin_Tram(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tram = db.TRAM.Find(id);

            if (tram == null)
            {
                return HttpNotFound();
            }
            int soXeDangDuocMuon = db.XE.Where(n => n.MaTram == id && n.TinhTrang == "Đang được mượn").Count();
            if (soXeDangDuocMuon > 0)
            {
                ViewBag.ThongBao = "Trong trạm đang có xe được mượn! Vui lòng không sửa thông tin trạm!";
            }
            return View(tram);
        }
        [HttpPost]
        public ActionResult SuaThongTin_Tram(FormCollection f)
        {
            var id = int.Parse(f["sMaTram"]);
            var sKiemTra = int.Parse(f["radio1"]);
            var tram = db.TRAM.Find(id);
            var sDiaChi = f["sDiaChi"];
            var sLong = f["sLong"];
            var sLast = f["sLast"];
            if (tram == null)
            {
                return HttpNotFound();
            }
            int soXeDangDuocMuon = db.XE.Where(n => n.MaTram == id && n.TinhTrang == "Đang được mượn").Count();
            int soDiaChi = db.TRAM.Where(n => n.DiaChi == sDiaChi).Count();
            int soLong = db.TRAM.Where(n => n.Long == sLong).Count();
            int soLast= db.TRAM.Where(n => n.Last == sLast).Count();
            if (soXeDangDuocMuon > 0)
            {
                ViewBag.ThongBao = "Trong trạm đang có xe được mượn! Vui lòng không sửa thông tin trạm!";
                return View(tram);
            }
            if (sKiemTra == 0)
            {
                ViewBag.ThongBao = "Sửa thông tin trạm không thành công! Vui lòng chọn có để sửa thông tin xe!";
                return View(tram);
            }
            else if (soDiaChi > 1)
            {
                ViewBag.ThongBao = "Sửa thông tin trạm không thành công! Vì địa chỉ đã tồn tại!";
                return View(tram);
            } else if(soLong > 1 || soLast >1)
            {
                ViewBag.ThongBao = "Sửa thông tin trạm không thành công! Vì địa chỉ trên Google Map đã tồn tại!";
                return View(tram);
            }
            else
            {
                tram.TinhTrang = f["sTinhTrang"];
                tram.DiaChi = sDiaChi;
                tram.Long = sLong;
                tram.Last = sLast;
                db.SaveChanges();
            }
            return RedirectToAction("QuanLyTram");
        }
    }
}