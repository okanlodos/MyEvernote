using MyEvernote.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class RepositoryBase
    {
        protected static DatabaseContext db;
        private static object _LockSync = new object();

        protected RepositoryBase()
        {
            CreateContect();
        }
        private static void CreateContect()
        {
            if (db == null)
            {
                lock (_LockSync)
                {
                    if (db == null)
                    {
                        db = new DatabaseContext();
                    }
                }
            }
        }
    }
}
