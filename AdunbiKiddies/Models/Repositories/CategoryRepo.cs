using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdunbiKiddies.Models;

namespace AdunbiKiddies.Models.Repository
{
    public class CategoryRepo: Repository<Category>
    {
        
       public List<Category> GetBySection(string section)
        {
            return DbSet.Where(p => p.StoreSection.SectionName == section).ToList();
        }
        
    }

    public class StoreSectionRepo : Repository<StoreSection>
    {
        
  
    }
    public class ProductRepo : Repository<Product>
    {
       public List<Product> GetProductByCat(int id)
        {
            return DbSet.Where(p => p.CategoryId == id).ToList();
        }

        public List<Product> GetByPrice(decimal price)
        {
            return DbSet.Where(p => p.Price >= price).ToList();
        }
        public List<Product> GetByPriceLowest()
        {
            return DbSet.OrderBy(p => p.Price).ToList();
            
        }
        public List<Product> GetByDateRecent()
        {
            return DbSet.OrderBy(p => p.DateAdded).ToList();

        }


        public List<Product> SearchProduct(string searchTerm)
        {
            return DbSet.Where(p => p.Name.Contains(searchTerm) ||  p.Category.Name.Contains(searchTerm)).ToList();
        }
        public List<Product> GetByDiscount(decimal discount)
        {
            discount = discount / 100;
            return DbSet.Where(p => p.ProductDiscount > discount).ToList();
        }
    }

    public class ShoppingCartRepo: Repository<ShoppingCart>
    {

    }
}