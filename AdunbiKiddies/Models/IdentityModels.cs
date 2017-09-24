using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OpenOrderFramework.Models;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdunbiKiddies.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class AjaoOkoDb : IdentityDbContext<ApplicationUser>
    {
        public AjaoOkoDb()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static AjaoOkoDb Create()
        {
            return new AjaoOkoDb();
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StoreSection> StoreSections { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<ProfessionalWorker> ProfessionalWorkers { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<PartnerShipAgreement> PartnerShipAgreements { get; set; }
        public DbSet<ProfessionalPayment> ProfessionalPayments { get; set; }

        public DbSet<BankDetail> BankDetails { get; set; }

        public DbSet<BusinessRegistration> BusinessRegistrations { get; set; }

        public DbSet<BusinessAddress> BusinessAddresses { get; set; }

        public System.Data.Entity.DbSet<AdunbiKiddies.Models.BusinessDocument> BusinessDocuments { get; set; }
    }
}