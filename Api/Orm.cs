using Newtonsoft.Json.Linq;
using System.Data;
using ZionApi;

namespace ZionOrm
{
    public class Orm : IOrm
    {
        protected IOrm driver;
        protected string connectionNumber;

        public Orm(string connectionNumber = "1")
        {
            this.connectionNumber = connectionNumber;
            string className = ZionEnv.GetValue("ZION_ORM_DRIVER_NAME_" + connectionNumber);
            Type type = null;

            type = Type.GetType($"ZionOrm." + className);

            if (type != null)
            {
                var constructor = type.GetConstructor(new[] { typeof(string) });
                object instance = constructor.Invoke(new object[] { connectionNumber });

                if (instance is IOrm)
                {
                    driver = (IOrm)instance;
                }
                else
                {
                    throw new ZionException(4, "ZionOrm", ZionOrm.GetOrmVersion(), "ZionOrm LoadDrive(): key ZION_ORM_DRIVE_NAME_" + connectionNumber + "=" + className + " : The content of the ZION_ORM_DRIVE_NAME key in the API's .env file, which indicates the name of a drive (Postgres, Mysql, SqlServer, etc.), was not found to be an implementation of the IOrm interface. Check the capital and lowercase of the class name. Example valid key: ZION_ORM_DRIVE_NAME_" + connectionNumber + "=Postgres", 412);
                }
            }
            else
            {
                throw new ZionException(5, "ZionOrm", ZionOrm.GetOrmVersion(), "ZionOrm.Orm(): key ZION_ORM_DRIVE_NAME_" + connectionNumber + "=" + className + " not found in .env file. Check the capital and lowercase of the class name. Example valid key: ZION_ORM_DRIVE_NAME_" + connectionNumber + "=Postgres");
            }
        }

        public DataTable Select(string tableName, string fields, string filter, string order = "")
        {
            return driver.Select(tableName, fields, filter, order);
        }
        public DataTable Query(string sqlSentence)
        {
            return driver.Query(sqlSentence);
        }

        public void Update(string tableName, string fields, string values, string condition)
        {
            driver.Update(tableName, fields, values, condition);
        }
        public void Insert(string tableName, string fields, string values)
        {
            driver.Insert(tableName, fields, values);
        }

        public void Delete(string tableName, string condition)
        {
            driver.Delete(tableName, condition);
        }

        public void Exec(string sqlSentence, string action)
        {
            driver.Exec(sqlSentence, action);
        }

        public void AutomaticFields(string tableName)
        {
            driver.AutomaticFields(tableName);
        }

        public dynamic GetTableIndex(string tableName)
        {
            return driver.GetTableIndex(tableName);
        }

        public string GetViolationUK()
        {
            return driver.GetViolationUK();
        }

        public string GetTableNotFound()
        {
            return driver.GetTableNotFound();
        }

        public string GetColumnNotFound()
        {
            return driver.GetColumnNotFound();
        }

        public string GetViolationNotNull()
        {
            return driver.GetViolationNotNull();
        }

        public string GetStatus()
        {
            return driver.GetStatus();  
        }
        public string GetDbError()
        {
            return driver.GetDbError();
        }

        public DataTable GetDataTable()
        {
            return driver.GetDataTable();
        }

        public long GetRowsAffected()
        {
            return driver.GetRowsAffected();
        }

        public long GetGeneratedId()
        {
            return driver.GetGeneratedId();
        }

        public string GetLastSqlSentence()
        {
            return driver.GetLastSqlSentence();
        }
    }
}
