﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Web;

namespace AdunbiKiddies.Models
{
    public abstract class Person
    {
        //[Required]
        [StringLength(100, ErrorMessage = "First name cannot be longer than 100 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        // [Required]
        [StringLength(100, ErrorMessage = "Middle name cannot be longer than 100 characters.")]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        //[Required]
        [StringLength(100, ErrorMessage = "Last name cannot be longer than 100 characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Gender { get; set; }

        // [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        //[Required(ErrorMessage = "Your Phone Number is required")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Full Name")]
        public string UserName => LastName + " " + FirstName;

        [Display(Name = "Full Name")]
        public string FullName => LastName + " " + FirstName + " " + MiddleName;



        [Display(Name = "Town Of Birth")]
        public string TownOfBirth { get; set; }

        [Display(Name = "State of Origin")]
        public string StateOfOrigin { get; set; }

        [Display(Name = "Nationality")]
        public string Nationality { get; set; }



        public byte[] Passport { get; set; }

        [Display(Name = "Upload A Passport/Picture")]
        [ValidateFile(ErrorMessage = "Please select a PNG/JPEG image smaller than 20kb")]
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
                    var target = new MemoryStream();

                    if (value.InputStream == null)
                        return;

                    value.InputStream.CopyTo(target);
                    Passport = target.ToArray();
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
            }
        }
    }
}