using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AdunbiKiddies.Models.Repository
{
    public class Repository<T> where T :class
    {
        public AjaoOkoDb _db;

        protected IDbSet<T> DbSet { get; set; }

        public Repository()
        {
            _db = new AjaoOkoDb();
            DbSet = _db.Set<T>();
        }

        public virtual List<T> GetAll()
        {
            return DbSet.ToList(); 
        }

        public virtual T GetById (int id)
        {
            return DbSet.Find(id);
        }

        public virtual  void Add(T entity)
        {
            DbSet.Add(entity);
        }
        public virtual void SaveChanges()
        {
            _db.SaveChanges();
        }


        
            
    }
}