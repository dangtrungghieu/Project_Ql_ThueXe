using PagedList;
using Project_OOD;
using Project_Ql_ThueXe.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace DHGo.Areas.User.Controllers
{
    public class UserController : Controller
    {
        QL_ThueXeEntities db = new QL_ThueXeEntities();

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection f)
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
            return RedirectToAction("Index");
        }
        public ActionResult MenuUser()
        {
            return View();
        }
        [HttpGet]
        public ActionResult NavUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NavUser(FormCollection f)
        {
            NGUOIDUNG nGUOIDUNG = (NGUOIDUNG)Session["TaiKhoan"];

            NGUOIDUNG ng = db.NGUOIDUNG.SingleOrDefault(n => n.MaNguoiDung == nGUOIDUNG.MaNguoiDung);

            if(ng.Pass != f["sPass"])
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
            return RedirectToAction("Index");
        }
        public ActionResult foster()
        {
            return View();
        }
        [HttpGet]
        public ActionResult QuanLyThongTin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult QuanLyThongTin(FormCollection f)
        {
            NGUOIDUNG nGUOIDUNG = (NGUOIDUNG)Session["TaiKhoan"];

            NGUOIDUNG ng = db.NGUOIDUNG.SingleOrDefault(n => n.MaNguoiDung == nGUOIDUNG.MaNguoiDung);
    
            ng.TenNguoiDung = f["sTenNguoiDung"];
            ng.GioiTinh = f["sGioiTinh"];
            ng.Email = f["sEmail"];
            ng.DiaChi = f["sDiaChi"];
            db.SaveChanges();

            Session["TaiKhoan"] = ng;
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult DoiMatKhau()
        {

            return View();
        }
        [HttpPost]
        public ActionResult DoiMatKhau(FormCollection f)
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
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult GopY()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GopY(FormCollection f)
        {
            NGUOIDUNG nGUOIDUNG = (NGUOIDUNG)Session["TaiKhoan"];

            GOPY gy = new GOPY();

            gy.TieuDe = f["sTieuDe"];
            gy.NgayGui = DateTime.Now;
            gy.NoiDung = f["sNoiDung"];
            gy.MaND = nGUOIDUNG.MaNguoiDung;

            db.GOPY.Add(gy);
            db.SaveChanges();

            return RedirectToAction("GopY", "User");
        }
        public ActionResult Logout()
        {
            Session["TaiKhoan"] = null;

            return RedirectToAction("Home", "Home", new { area = "" });
        }
        [HttpGet]
        public ActionResult QuanLyViDienTu()
        {
            ViewBag.ThongBao = TempData["ThongBao"];
            return View();
        }
        [HttpPost]
        public ActionResult QuanLyViDienTu(FormCollection f)
        {
            NGUOIDUNG nGUOIDUNG = (NGUOIDUNG)Session["TaiKhoan"];
            NGUOIDUNG ng = db.NGUOIDUNG.SingleOrDefault(n => n.MaNguoiDung == nGUOIDUNG.MaNguoiDung);
           
            var mathemuon = int.Parse(f["sMaTheMuon"]);
            var matkhau = f["sPassThe"];
            int soLuong = db.NGUOIDUNG.Where(n => n.MaTheMuon == mathemuon).Count();
            THEMUON tm = db.THEMUON.Find(mathemuon);

            if(tm.PassTheMuon != matkhau)
            {
                TempData["ThongBao"] = "Sai mật khẩu!";

            }
            else if(tm.MaTheMuon != mathemuon)
            {
                TempData["ThongBao"] = "Sai Mã thẻ!";

            }
            else if (soLuong > 0)
            {
                TempData["ThongBao"] = "Thẻ đã được liên kết với tài khoản khác!";
            }
            else if (ng.MaTheMuon == null && tm.PassTheMuon == matkhau)
            {
                ng.MaTheMuon = int.Parse(f["sMaTheMuon"]);
                db.SaveChanges();
            }
            
            Session["TaiKhoan"] = ng;
            return RedirectToAction("QuanLyViDienTu", "User");
        }
        public ActionResult HoanTraThe()
        {
            NGUOIDUNG nGUOIDUNG = (NGUOIDUNG)Session["TaiKhoan"];
            NGUOIDUNG ng = db.NGUOIDUNG.SingleOrDefault(n => n.MaNguoiDung == nGUOIDUNG.MaNguoiDung);

            if (ng.THEMUON.MaLoaiTheMuon == 1)
            {
                return View();
            }
            else
            {
                TempData["ThongBao"] = "Thẻ của bạn là loại thẻ trả sau. Nên không thể hoàn trả thẻ!";
                Session["TaiKhoan"] = ng;

                return RedirectToAction("QuanLyViDienTu", "User");
            }
        }
        public ActionResult CapNhatSoDu()
        {
            NGUOIDUNG nGUOIDUNG = (NGUOIDUNG)Session["TaiKhoan"];
            NGUOIDUNG ng = db.NGUOIDUNG.SingleOrDefault(n => n.MaNguoiDung == nGUOIDUNG.MaNguoiDung);
            ng.THEMUON.SoDu = 0;
            db.SaveChanges();
            Session["TaiKhoan"] = ng;
            return RedirectToAction("HoanTraThe", "User");
        }
        [HttpGet]
        public ActionResult Payment()
        {
            NGUOIDUNG nGUOIDUNG = (NGUOIDUNG)Session["TaiKhoan"];
            NGUOIDUNG ng = db.NGUOIDUNG.SingleOrDefault(n => n.MaNguoiDung == nGUOIDUNG.MaNguoiDung);
            if (ng.THEMUON.MaLoaiTheMuon == 2)
            {
                TempData["ThongBao"] = "Thẻ của bạn là loại thẻ trả sau. Nên không thể nạp tiền vào thẻ!";
                Session["TaiKhoan"] = ng;
                return RedirectToAction("QuanLyViDienTu", "User");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Payment(int? id, FormCollection f)
        {
            string url = ConfigurationManager.AppSettings["Url"];
            string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
            string hashSecret = ConfigurationManager.AppSettings["HashSecret"];

            PayLib pay = new PayLib();


            pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)

            pay.AddRequestData("vnp_Amount", f["sSoTien"].ToString() + "00"); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000

            pay.AddRequestData("vnp_BankCode", "NCB"); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn

            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

            NGUOIDUNG nGUOIDUNG = (NGUOIDUNG)Session["TaiKhoan"];
            NGUOIDUNG nd = db.NGUOIDUNG.SingleOrDefault(n => n.MaNguoiDung == nGUOIDUNG.MaNguoiDung);
            nd.THEMUON.SoDu = nd.THEMUON.SoDu + int.Parse(f["sSoTien"]);
            db.SaveChanges();
            Session.Clear();
            Session["TaiKhoan"] = nd;
            return Redirect(paymentUrl);
        }
        public ActionResult PaymentConfirm()
        {
            if (Request.QueryString.Count > 0)
            {
                string hashSecret = ConfigurationManager.AppSettings["HashSecret"]; //Chuỗi bí mật
                var vnpayData = Request.QueryString;
                PayLib pay = new PayLib();
                //lấy toàn bộ dữ liệu được trả về
                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s, vnpayData[s]);
                    }
                }
                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"]; //hash của dữ liệu trả về
                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        //Thanh toán thành công
                        ViewBag.Message = "Thanh toán thành công hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId;
                    }
                    else
                    {
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                    }
                }
                else
                {
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }
            return View();
        }
        public ActionResult HuyLienKet()
        {
            NGUOIDUNG nGUOIDUNG = (NGUOIDUNG)Session["TaiKhoan"];
            NGUOIDUNG ng = db.NGUOIDUNG.SingleOrDefault(n => n.MaNguoiDung == nGUOIDUNG.MaNguoiDung);
            ng.MaTheMuon = null;
            db.SaveChanges();
            Session["TaiKhoan"] = ng;
            return RedirectToAction("QuanLyViDienTu", "User");
        }
        [HttpGet]
        public ActionResult QuanLyLichSu(int? page)
        {
            NGUOIDUNG nGUOIDUNG = (NGUOIDUNG)Session["TaiKhoan"];
            if(nGUOIDUNG.MaTheMuon == null)
            {
                return View();
            }
            else
            {
                int iSize = 5;
                int iPageNum = (page ?? 1);
                var DSls = from lsm in db.MUONXE where lsm.MaTheMuon == nGUOIDUNG.THEMUON.MaTheMuon select lsm;
                ViewBag.StartingIndex = (iPageNum - 1) * iSize + 1;
                return View(DSls.OrderBy(s => s.MaMuonXe).ToPagedList(iPageNum, iSize));
            }
        }
    }
}