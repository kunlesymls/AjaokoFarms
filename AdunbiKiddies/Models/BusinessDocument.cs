using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Web;

namespace AdunbiKiddies.Models
{
    public class BusinessDocument
    {
        public int BusinessDocumentId { get; set; }
        public string DocumentName { get; set; }
        public int BusinessRegistrationId { get; set; }
        public byte[] Document { get; set; }

        [Display(Name = "Document file")]
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
                    Document = target.ToArray();
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
            }
        }
        public virtual BusinessRegistration BusinessRegistration { get; set; }
    }
}