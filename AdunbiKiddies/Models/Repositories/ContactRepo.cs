using AdunbiKiddies.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdunbiKiddies.Models.Repositories
{
    public class ContactRepo:Repository<Contact>
    {
        public IList<Contact> GetAllSubcribed()
        {
            return DbSet.Where(c => c.isSubcribed == true).ToList();
        }
    }
}