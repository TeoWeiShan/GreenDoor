using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using greendoor.Models;

namespace greendoor.DAL
{
    public class FavouriteDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public FavouriteDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();

            //CHANGE CONNECTION STRING
            string strConn = Configuration.GetConnectionString(
            "GreenDoorConnectionString");
            //Instantiate a SqlConnection object with the
            //Connection String read.
            conn = new SqlConnection(strConn);


        }
        public int Add(int custID, int shopID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO Favourite (CustomerID, ShopID) VALUES(@custID, @shopID)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@custID", custID);
            cmd.Parameters.AddWithValue("@shopID", shopID);

            //A connection to database must be opened before any operations made.
            conn.Open();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return shopID;
        }

        public bool GetDetails(int? custID, int? shopID)
        {
            bool favbool = false;
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a shop record.

            cmd.CommandText = @"SELECT * FROM Favourite WHERE ShopID = @selectedShopID AND CustomerID = @selectedCustID";

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “judgeId”.
            cmd.Parameters.AddWithValue("@selectedCustID", custID);
            cmd.Parameters.AddWithValue("@selectedShopID", shopID);

            //Open a database connection
            conn.Open();

            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                favbool = true;
            }

            //Close DataReader
            reader.Close();

            //Close the database connection
            conn.Close();

            return favbool;
        }

        public void Delete(Favourite favourite)
        {
            //Instantiate a SqlCommand object, supply it with a DELETE SQL statement

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM Favourite
                                WHERE ShopID = @selectedShopID AND CustID = @selectedCustID";
            cmd.Parameters.AddWithValue("@selectedShopID", favourite.ShopID);
            cmd.Parameters.AddWithValue("@selectedCustID", favourite.CustomerID);

            //Open a database connection
            conn.Open();
            //Execute the DELETE SQL to remove the staff record
            cmd.ExecuteNonQuery();
            //Close database connection
            conn.Close();
        }
    }
}
