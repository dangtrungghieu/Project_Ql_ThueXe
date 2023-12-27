using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project_Ql_ThueXe.Models;
namespace Project_Ql_ThueXe.Models
{
    public class ThongTinXe_DanhSachLoaiXe_DanhSachTram_HienCo
    {
        public XE ThongTinXe { get; set; }
        public List<LOAIXE> DanhSachLoaiXe { get; set; }
        public List<TRAM> DanhSachTram { get; set; }
    }
}