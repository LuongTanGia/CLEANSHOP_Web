using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CLEANSHOP.Models;

namespace CLEANSHOP.Models
{
    public class Giohang
    {

        MydataDataContext data = new MydataDataContext();
        public int ID { get; set; }
                      
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }
        [Display(Name = "Ảnh bìa")]
        public string Image { get; set; }
        [Display(Name = "Đơn Giá")] 
        public Double DisPrice { get; set; }            
        [Display(Name = "Số lượng")]
        public int iSoLuong { get; set; }
        public int soluongton { get; set; }

        [Display(Name = "Thành tiền")]
        public Double TotalPrice
        {
            get { return iSoLuong * DisPrice; }
        }

        public Giohang(int  id)
        {
             ID = id;
             Product product = data.Products.Single(n => n.Id ==id);
            ProductName = product.ProductName;
            Image = product.Image;
            DisPrice = double.Parse(product.DisPrice.ToString());
            iSoLuong = 1;
            soluongton = int.Parse(product.Amount.ToString());
        }

        
     
    }
}