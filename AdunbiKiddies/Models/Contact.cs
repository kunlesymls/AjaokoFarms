using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdunbiKiddies.Models
{
    public class Contact
    {
        public int Id { get; set; }

        public string Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress,ErrorMessage ="Please enter a valid email address")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage ="Please enter a valid email address")]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        public string Comment { get; set; }

        [DisplayName("Subribe for our periodic Newsletter ? ")]
        public bool isSubcribed { get; set; }
    }
}