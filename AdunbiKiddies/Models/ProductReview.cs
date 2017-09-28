using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AdunbiKiddies.Models
{
    public class ProductReview
    {
        public int ProductReviewId { get; set; }
        [DisplayName("Product Name")]
        public int ProductId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [DisplayName("User Name")]
        public string UserName { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Subject")]
        public string Subject { get; set; }
        [DisplayName("Review")]
        public string Review { get; set; }
        public virtual Product Product { get; set; }
    }

    public class ProductReviewVm
    {
        public int ProductId { get; set; }
        public int rating { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string subject { get; set; }
        public string review { get; set; }
    }
}