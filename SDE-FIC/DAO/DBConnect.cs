using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using MySql;

namespace SDE_FIC.DAO
{
    public sealed class DBConnect
    {

        private static DBConnect instance;
        private MySqlConnection mConnection;
        private MySqlTransaction mTransaction;

        public DBConnect()
        {
            try
            {
                mConnection = new MySqlConnection();
                mConnection.ConnectionString = "Server=localhost;Database=bdsde;Uid=root;Pwd='123456';";                
                mConnection.Open();

            }
            catch (Exception Exception)
            {
                throw Exception;

            }
        }

        public static DBConnect GetInstance()
        {
            if (instance == null)
            {
                instance = new DBConnect();

            }

            return instance;
        }

        public MySql.Data.MySqlClient.MySqlConnection Connection
        {
            get
            {
                return mConnection;
            }
            set
            {
                mConnection = value;
            }
        }

        public MySqlTransaction Transaction
        {
            get
            {
                return mTransaction;
            }
            set
            {
                mTransaction = value;
            }
        }

        public void CloseConnection()
        {
            mConnection.Close();

        }

        public void OpenConnection()
        {
            mConnection.Open();
        }

        public void BeginTransaction()
        {
            mTransaction = mConnection.BeginTransaction();

        }

        public void Commit()
        {
            mTransaction.Commit();
            mTransaction = null;

        }

        public void RollBack()
        {
            mTransaction.Rollback();
            mTransaction = null;
        }

        //public DataTable ExecuteQuery(ref MySql.Data.MySqlClient.MySqlCommand myCmd)
        //{
        //    DataTable t1 = new DataTable();
            
        //    myCmd.Transaction = mTransaction;

        //    using (MySql.Data.MySqlClient.MySqlDataAdapter a = new MySql.Data.MySqlClient.MySqlDataAdapter(myCmd))
        //    {
        //        a.Fill(t1);
        //    }

        //    return t1;
        //}

        public DataTable ExecuteQuery(ref MySql.Data.MySqlClient.MySqlCommand myCmd)
        {
            DataTable t1 = new DataTable();

            myCmd.Transaction = mTransaction;

            using (MySql.Data.MySqlClient.MySqlDataAdapter a = new MySql.Data.MySqlClient.MySqlDataAdapter(myCmd))
            {
                a.Fill(t1);
            }

            return t1;
        }

        internal void Dispose()
        {
            throw new NotImplementedException();
        }
    }



}