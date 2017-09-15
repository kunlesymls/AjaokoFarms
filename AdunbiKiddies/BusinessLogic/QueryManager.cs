using System;
using System.Web;
using AdunbiKiddies.Models;
using Microsoft.AspNet.Identity;

namespace AdunbiKiddies.BusinessLogic
{
    public class QueryManager : IDisposable
    {
        private readonly AjaoOkoDb _db;

        public QueryManager()
        {
            _db = new AjaoOkoDb();
        }




        public string GetId()
        {
            var user = HttpContext.Current.User.Identity.GetUserId();
            return user;
        }

        public void Dispose()
        {
            _db?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}