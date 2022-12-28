using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CLEANSHOP.Models;
using PagedList;

namespace CLEANSHOP.Controllers
{
    public class DichVuController : Controller
    {
        // GET: DichVu
        MydataDataContext data = new MydataDataContext();

        public ActionResult Index(int? page, string searchString)
        {
            if (page == null) page = 1;

            var all_product = (from tt in data.Products select tt).OrderBy(m => m.Id);
            if (!string.IsNullOrEmpty(searchString)) all_product = (IOrderedQueryable<Product>)all_product.Where(a => a.ProductName.Contains(searchString));
            int pageSize = 6;
            int pageNum = page ?? 1;

            return View(all_product.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Index2(int? page, string searchString)
        {
            if (page == null) page = 1;

            var all_product = (from tt in data.Products select tt).OrderBy(m => m.Id);
            if (!string.IsNullOrEmpty(searchString)) all_product = (IOrderedQueryable<Product>)all_product.Where(a => a.ProductName.Contains(searchString));
            int pageSize = 6;
            int pageNum = page ?? 1;

            return View(all_product.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Sanpham(int? page)
        {
            if (page == null) page = 1;

            var all_product = (from tt in data.Products select tt).OrderBy(m => m.Id);
            int pageSize = 100;
            int pageNum = page ?? 1;

            return View(all_product.ToPagedList(pageNum, pageSize));
        }

        public ActionResult Detail(int id)
        {
            var D_Products = data.Products.Where(m => m.Id == id).First(); return View(D_Products);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection, Product s)
        {

            var E_ProductName = collection["ProductName"];
            var E_Image = collection["Image"];
            var E_Detail = collection["Detail"];
            var E_Time = collection["Time"];
            var E_Text = collection["Text"];

            var E_Price = Convert.ToDecimal(collection["Price"]);
            var E_Amount = Convert.ToInt32(collection["Amount"]);

            // var E_ngaycapnhat = Convert.ToDateTime(collection["ngaycapnhat"]);
            // var E_soluongton Convert.ToInt32(collection["soluongton"]);
            if (string.IsNullOrEmpty(E_ProductName))

            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                s.ProductName = E_ProductName.ToString();
                s.Image = E_Image.ToString();
                s.Detail = E_Detail.ToString();

                s.Text = E_Text.ToString();
                s.Price = E_Price;
                s.Amount = E_Amount;
                s.DisPrice = E_Price;
                s.DisCount = 1.0;
                //   s.ngaycapnhat = E_ngaycapnhat;
                //  s.soluongton = E_soluongton;
                data.Products.InsertOnSubmit(s);
                data.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Create();
        }


        public ActionResult Edit(int id)
        {
            var E_Products = data.Products.First(m => m.Id == id);
            /*    List<SelectListItem> discountList = new List<SelectListItem>();// set discount list
                discountList.Add(new SelectListItem { Text = "No Discount", Value = "1" });
                discountList.Add(new SelectListItem { Text = "10%", Value = "0,9" });
                discountList.Add(new SelectListItem { Text = "20%", Value = "0,8" });
                discountList.Add(new SelectListItem { Text = "50%", Value = "0,5" });
                ViewBag.Discount = discountList;*/
            return View(E_Products);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var E_Products = data.Products.First(m => m.Id == id);
            var E_ProductName = collection["ProductName"];
            var E_Image = collection["Image"];
            var E_Detail = collection["Detail"];

            var E_Text = collection["Text"];
            var E_Amount = Convert.ToInt32(collection["Amount"]);
            var E_Price = Convert.ToDecimal(collection["Price"]);
            var E_Discount = Convert.ToDouble(collection["DisCount"]);
            var E_DisPrice = Convert.ToDecimal(collection["Price"]) * Convert.ToDecimal(collection["DisCount"]);
            //var E_ngaycapnhat = Convert.ToDateTime(collection["ngaycatnhat"]);
            //  var E_soluongton = Convert.ToInt32(collection["soluongton"]);
            E_Products.Id = id;
            if (string.IsNullOrEmpty(E_ProductName))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else

            {
                E_Products.ProductName = E_ProductName;
                E_Products.Image = E_Image;
                E_Products.Detail = E_Detail;

                E_Products.Text = E_Text;
                E_Products.Amount = E_Amount;
                E_Products.Price = E_Price;
                E_Products.DisCount = E_Discount;
                E_Products.DisPrice = E_DisPrice;
                // E_Products.ngaycapnhat = E_ngaycapnhat;
                // E_Products.soluongton = E_soluongton;
                UpdateModel(E_Products);
                data.SubmitChanges();
                return RedirectToAction("ListSach");
            }
            return this.Edit(id);
        }

        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/Images/" + file.FileName));
            return "/Content/Images/" + file.FileName;
        }

        public ActionResult Delete(int id)
        {
            var E_Products = data.Products.First(m => m.Id == id);

            return View(E_Products);
        }
        [HttpPost]

        public ActionResult Delete(int id, FormCollection collection)
        {


            var E_Products = data.Products.First(m => m.Id == id);
            var E_ProductName = collection["ProductName"];
            var E_Image = collection["Image"];
            var E_Detail = collection["Detail"];

            var E_Text = collection["Text"];
            var E_Amount = Convert.ToInt32(collection["Amount"]);
            var E_Price = Convert.ToDecimal(collection["Price"]);
            var E_Discount = Convert.ToDouble(collection["DisCount"]);
            var E_DisPrice = Convert.ToDecimal(collection["Price"]) * Convert.ToDecimal(collection["DisCount"]);
            //var E_ngaycapnhat = Convert.ToDateTime(collection["ngaycatnhat"]);
            //var E_soluongton = Convert.ToInt32(collection["soluongton"]);
            E_Products.Id = id;
            if (string.IsNullOrEmpty(E_ProductName))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else

            {
                E_Products.ProductName = E_ProductName;
                E_Products.Image = E_Image;
                E_Products.Detail = E_Detail;

                E_Products.Text = E_Text;
                E_Products.Amount = E_Amount;
                E_Products.Price = E_Price;
                E_Products.DisCount = E_Discount;
                E_Products.DisPrice = E_DisPrice;
                //E_Products.ngaycapnhat = E_ngaycapnhat;
                //E_Products.soluongton = E_soluongton;
                UpdateModel(E_Products);
                data.SubmitChanges();
                return RedirectToAction("ListSach");
            }
            return this.Delete(id);


        }
        public ActionResult ListSach()
        {


            Customer kh = (Customer)Session["Taikhoan"];
            if (Session["Taikhoan"] == null)
            {
                return RedirectToAction("XacNhanAdmin", "GioHang");

            }
            else
            {
                if (kh.limit == 1)
                {
                    var all_product = from ss in data.Products select ss;
                    return View(all_product);
                }
                else
                    return RedirectToAction("XacNhanAdmin", "GioHang");
            }

            return View();

        }

        public ActionResult Nhom()
        {
            return PartialView();
        }


        public ActionResult Category()
        {
            return PartialView();
        }
        public ActionResult LoaiSanPham()
        {
            var loaisanpham = from s in data.Types select s;
            return PartialView(loaisanpham);
        }
        public ActionResult LoaiSanPham2()
        {
            var loaisanpham2 = from s in data.TypeShoes select s;
            return PartialView(loaisanpham2);
        }
        public ActionResult DanhMuc()
        {
            var loaisanpham = from s in data.Types select s;
            return PartialView(loaisanpham);
        }

        public ActionResult SPTheoLoai(int id)
        {
            var sptl = from ss in data.Products where ss.ID_Type == id select ss;
            return PartialView(sptl);

        }
        public ActionResult SPTheoLoai2(int id)
        {
            var sptl = from ss in data.Products where ss.ID_Shoes == id select ss;
            return PartialView(sptl);

        }
        public ActionResult DonHang()
        {

            var dh = from ss in data.Carts select ss;
            var tt = data.Carts.Sum(n => Convert.ToInt32(n.TotalPrice));
            ViewBag.tongtien = tt;
            return View(dh);
                
        }

        public ActionResult Chitiet(int id)
        {
            List<CartDetail> chitiet = data.CartDetails.Where(m => m.IdCart == id).ToList();
            return View(chitiet);
        }
        public ActionResult DeleteDH(int id)
        {
            var E_Cart = data.Carts.First(m => m.IdCart == id);

            return View(E_Cart);
        }
       
            [HttpPost]
            public ActionResult DeleteDH(int id, FormCollection collection)
            {
                var D_giay = data.Carts.Where(m => m.IdCart== id).First();          
                data.Carts.DeleteOnSubmit(D_giay);
            var D_Dtail = data.CartDetails.Where(m => m.IdCart == id).First();
                 data.CartDetails.DeleteOnSubmit(D_Dtail);
            data.SubmitChanges();
                return RedirectToAction("ListSach");
            }

        public ActionResult ListNhanVien()
        {
            var all_product = from ss in data.Customers select ss;
            return View(all_product);

        }
    }
    }
   




    
