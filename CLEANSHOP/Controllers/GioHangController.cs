    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CLEANSHOP.Models;
using CLEANSHOP.Controllers;
using System.Net.Mail;
using System.Net;


namespace CLEANSHOP.Controllers
{
    public class GioHangController : Controller
    {
        MydataDataContext data = new MydataDataContext();

        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;

            if (lstGiohang == null)
            {
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }

        public ActionResult ThemGioHang(int id, string strURL)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.Find(n => n.ID == id);
            if (sanpham == null)
            {
                sanpham = new Giohang(id);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoLuong++;
                return Redirect(strURL);
            }
        }
        private int TongSoluong()
        {
            int tsl = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Sum(n => n.iSoLuong);
            }
            return tsl;
        }
        private int TongSoLuongSanPham()
        {
            int tsl = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Count;
            }
            return tsl;
        }
        private double TongTien()
        {
            double tt = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                tt = lstGiohang.Sum(n => n.TotalPrice);
            }
            return tt;
        }
        public ActionResult GioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoluong();
            ViewBag.Tongtien = TongTien();
            ViewBag.TienGiam1 = TongTien() * 0.7;
            ViewBag.TienGiam2 = TongTien() * 0.5;
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstGiohang);
        }
        public ActionResult miniGiohang()
        {
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoluong();
            ViewBag.Tongtien = TongTien();
            return PartialView(lstGiohang);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoluong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return PartialView();

        }
        public ActionResult XoaGiohang(int id)
        {
            List<Giohang> lstGiohang = Laygiohang();

            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.ID == id);
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.ID == id);
                return RedirectToAction("Giohang");
            }
            return RedirectToAction("Giohang");
        }
        public ActionResult CapnhatGiohang(int id, FormCollection collection)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.ID == id);
            if (sanpham != null)
            {
                sanpham.iSoLuong = int.Parse(collection["txtSoLg"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaTatCaGioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("GioHang");
        }



        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "DichVu");
            }
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoluong();
            ViewBag.Tongtien = TongTien();
            ViewBag.TienGiam1 = TongTien() *0.7 ;
            ViewBag.TienGiam2 = TongTien() * 0.5;
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstGiohang);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            ViewBag.thongbao = "Ngay loi";
            Cart dh = new Cart();
            Customer kh = (Customer)Session["Taikhoan"];
            
            List<Giohang> gh = Laygiohang();
            var DeliveryDate = String.Format("{0:MM/dd/yyyy}", collection["BookingDate"]);
            dh.Customer_Id = kh.IdCustomer;
            dh.BookingDate = DateTime.Now;
            dh.DeliveryDate = DateTime.Parse(DeliveryDate);
            dh.Delivery = false;
            dh.TotalPrice = 0;
            data.Carts.InsertOnSubmit(dh);
            data.SubmitChanges();
            Product s = new Product();
            int sl = 0;
            decimal tien = 0;
            foreach (var item in gh)
            {
                if (dh.DeliveryDate < dh.BookingDate)
                {
                    ViewData["checkngay"] = "Ngày giao phải lớn hơn ngày đặt";
                }
                else
                {
                    s = data.Products.Single(n => n.Id == item.ID);
                    CartDetail ctdh = new CartDetail();
                    ctdh.IdCart = dh.IdCart;
                    ctdh.IdProduct = s.Id;
                    ctdh.Amount = item.iSoLuong;
                    sl += item.iSoLuong;
                    ctdh.Price = (decimal)item.DisPrice;
                    dh.TotalPrice = tien += (decimal)item.DisPrice * item.iSoLuong;
                    data.CartDetails.InsertOnSubmit(ctdh);
                    s.Amount -= ctdh.Amount;
                    data.SubmitChanges();
                }
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            MailAddress fromAddress = new MailAddress("binhminh220901@gmail.com", "VỆ SINH CHĂM SÓC GIÀY AK2M");

            MailAddress toAddress = new MailAddress(kh.Email.ToString());

            const string fromPassword = "mnlggkyozblporpa";

            string subject = "Xác nhận đơn hàng";

            string Tien = tien.ToString();

            string Soluong = sl.ToString();

            string Ten = kh.Name.ToString();

            string NgayDat = dh.BookingDate.ToString();

            string madh = dh.IdCart.ToString();


            SmtpClient smtp = new SmtpClient

            {

                Host = "smtp.gmail.com",

                Port = 587,

                EnableSsl = true,

                DeliveryMethod = SmtpDeliveryMethod.Network,

                UseDefaultCredentials = false,

                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)

            };

            using (MailMessage message = new MailMessage(fromAddress, toAddress)

            {

                Subject = subject,

                Body =

                        "<p>Xác nhận đơn hàng gồm : </p>" +
                         "<p>Số lượng sản phẩm :  " + Soluong + " sản phẩm  </p>" +
                         "<p>Tên KH : " + Ten + "   </p>" +
                          "<p>Email : " + kh.Email.ToString() + "   </p>" +
                         "<p color = red>Tổng thành tiền : " + Tien + " VND  </p>"
                        + "<p>Ngày đặt hàng  :" + NgayDat + "</p>"
                        + "<p>Mã đơn hàng  : " + madh + "</p>" +
                         "<p>Về trang mua hàng https://localhost:44364/ </p>",

                IsBodyHtml = true,

            })

            {

                smtp.Send(message);
                return RedirectToAction("XacnhanDonHang", "GioHang");
            }
        }
        //public JsonResult CheckDate(DateTime userdata)
        //{
        //    System.Threading.Thread.Sleep(200);
        //    var SearchData = data.Carts.Where(x => x.DeliveryDate == userdata).SingleOrDefault();
        //    var time = DateTime.Now;
        //    if (SearchData > time)
        //    {
        //        return Json(1);
        //    }
        //    else
        //    {
        //        return Json(0);
        //    }
        //}
        public ActionResult XacNhanDonHang()
        {

            return View();
        }
        public ActionResult XacNhanAdmin()
        {

            return View();
        }
        public ActionResult Ten()
        {

            return PartialView();


        }

        
    }
}
