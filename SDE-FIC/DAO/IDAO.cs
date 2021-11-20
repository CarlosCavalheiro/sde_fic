using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SDE_FIC.DAO
{
    public interface IDAO<T>:IDisposable
            where T: class, new()
        {
            T Insert(T model);
            void Update(T model);
            bool Delete(T model);
            IEnumerable<T> All();
            T FindOrDefault(params Object[] Keys);   
        }
}
