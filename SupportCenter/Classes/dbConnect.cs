using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SupportCenter.Classes
{
    internal class dbConnect
    {
        NpgsqlConnection db_connect = new NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=010203456;Database=supportdb");

        public void openConnection()
        {
            if (db_connect.State == System.Data.ConnectionState.Closed)
            {
      

                
                try
                {
                    db_connect.Open();
  

                }
                catch (Exception ex)
                {

                    MessageBox.Show("123");
                }
            }

        }
        public void closeConnection()
        {
            if (db_connect.State == System.Data.ConnectionState.Open)
            {
                db_connect.Close();
            }

        }
        public NpgsqlConnection GetConnection()
        {
            return db_connect;
        }
    }
}
