﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Web;

namespace AdunbiKiddies.Models
{

    public class Product
    {
        public int ProductId { get; set; }

        public string MerchantId { get; set; }

        [DisplayName("Category Name")]
        [Required(ErrorMessage = "Please select a related category")]
        public int CategoryId { get; set; }

        // public int SubCategoryId { get; set; }

        [Required(ErrorMessage = "An Product Name is required")]
        [StringLength(300)]
        public string Name { get; set; }

        [Display(Name = "Alternative/General Name")]
        [StringLength(200)]
        public string AlternativeName { get; set; }


        [Required(ErrorMessage = "Price Per Quantity")]
        public decimal Price { get; set; }

        [Range(1, 1000)]
        [DisplayName("Quantity")]
        [Required(ErrorMessage = "Quantity")]
        public int Quantity { get; set; }

        [DisplayName("Unit Name")]
        public string Unit { get; set; }

        [DisplayName("Other Unit Name")]
        public string OtherUnitName { get; set; }

        //[Required(ErrorMessage = "A Product Description is required")]
        [Display(Name = "Product Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public decimal? ProductDiscount { get; set; }

        public decimal DiscountPrice
        {
            get
            {
                if (ProductDiscount != null)
                {
                    decimal calculatedPrice = ((decimal)ProductDiscount / 100) * Price;
                    decimal discountPrice = Price - calculatedPrice;
                    return Math.Round(discountPrice, 2);
                }
                return 0;
            }
            set { }
        }

        public bool IsApproved { get; set; }

        public DateTime DateAdded
        {

            get
            {
                return DateTime.Now;
            }
            set { }
        }
        public int? StockQuantity { get; set; }

        public byte[] InternalImage { get; set; }

        [Display(Name = "Local file")]
        [NotMapped]
        public HttpPostedFileBase File
        {
            get
            {
                return null;
            }

            set
            {
                try
                {
                    MemoryStream target = new MemoryStream();

                    if (value.InputStream == null)
                        return;

                    value.InputStream.CopyTo(target);
                    InternalImage = target.ToArray();
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
            }
        }

        [DisplayName("Product Picture URL")]
        [StringLength(1024)]
        public string ItemPictureUrl { get; set; }
        public virtual Merchant Merchant { get; set; }
        public virtual Category Category { get; set; }
        // public virtual SubCategory SubCategory { get; set; }
        public virtual List<SaleDetail> SaleDetails { get; set; }
        public virtual ICollection<Stock> Stocks { get; set; }
        public virtual ICollection<ProductReview> ProductReviews { get; set; }

    }
}