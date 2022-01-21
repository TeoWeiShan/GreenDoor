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
        public bool Add(int? custID, int shopID)
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
            cmd.ExecuteNonQuery();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            bool favbool = true;
            return favbool;
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

        public bool Delete(int? custID, int shopID)
        {
            //Instantiate a SqlCommand object, supply it with a DELETE SQL statement

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM Favourite
                                WHERE ShopID = @selectedShopID AND CustomerID = @selectedCustID";
            cmd.Parameters.AddWithValue("@selectedShopID", shopID);
            cmd.Parameters.AddWithValue("@selectedCustID", custID);

            //Open a database connection
            conn.Open();
            //Execute the DELETE SQL to remove the staff record
            cmd.ExecuteNonQuery();
            //Close database connection
            conn.Close();
            bool favbool = false;
            return favbool;
        }

        public List<Shop> GetAllFavShop(int? custID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a shop record.
            cmd.CommandText = @"SELECT * FROM Shop 
                INNER JOIN Favourite ON Favourite.ShopID = Shop.ShopID 
                    WHERE Favourite.CustomerID = @selectedCustID";
            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “judgeId”.
            cmd.Parameters.AddWithValue("@selectedCustID", custID);

            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a list
            List<Shop> favShopList = new List<Shop>();
            while (reader.Read())
            {
                favShopList.Add(
                new Shop
                {
                    ShopID = reader.GetInt32(0),
                    ShopPicture = reader.GetString(1),
                    ShopName = reader.GetString(2),
                    ShopDescription = reader.GetString(3),
                    Zone = reader.GetString(4),
                    ContactNumber = reader.GetInt32(5),
                    Address = reader.GetString(6),
                    PostalCode = reader.GetInt32(7),
                    SocialMediaLink = reader.GetString(8),
                    WebsiteLink = reader.GetString(9),
                    EmailAddr = reader.GetString(10),
                    Password = reader.GetString(11)
                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return favShopList;
        }

        public int GetNumFavourite(int shopID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            //CHANGE SQL QUERY (Name)
            cmd.CommandText = @"SELECT * FROM Favourite WHERE ShopID = @selectedShopID";
            cmd.Parameters.AddWithValue("@selectedShopID", shopID);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a list
            List<Favourite> favouriteList = new List<Favourite>();
            while (reader.Read())
            {
                favouriteList.Add(
                new Favourite
                {
                    //CHANGE RELAVANT DETAILS
                    CustomerID = reader.GetInt32(0),
                    ShopID = reader.GetInt32(1)
                });
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return favouriteList.Count;
        }

    }
}
