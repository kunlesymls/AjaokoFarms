using System;
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
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "An Product Name is required")]
        [StringLength(300)]
        public string Name { get; set; }

        [Display(Name = "Alternative/General Name")]
        [StringLength(200)]
        public string AlternativeName { get; set; }


        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }

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
                    return discountPrice;
                }
                return 0;
            }
            set { }
        }

        public string IsApproved { get; set; }
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
        public virtual List<SaleDetail> SaleDetails { get; set; }
        public virtual ICollection<Stock> Stocks { get; set; }

    }
}