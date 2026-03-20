using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoachReservation
{
    public class Database
    {
        private string connStr = "server=localhost;user=root;password=123456;database=coachreservationdb;";
        private MySqlConnection sqlConn;

        public Database()
        {
            sqlConn = new MySqlConnection(connStr);
        }

        public MySqlConnection SqlConn { get => sqlConn; set => sqlConn = value; }
        public void OpenDatabase()
        {
            if (SqlConn != null && SqlConn.State == ConnectionState.Closed)
            {
                SqlConn.Open();
            }
        }

        public void CloseDatabase()
        {
            if (SqlConn != null && SqlConn.State == ConnectionState.Open)
            {
                SqlConn.Close();
            }
        }
    }
}
