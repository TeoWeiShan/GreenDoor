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
    public class ShopDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public ShopDAL()
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

        public List<Shop> GetAllShop()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            //CHANGE SQL QUERY (Name)
            cmd.CommandText = @"SELECT * FROM Shop ORDER BY ShopName";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a list
            List<Shop> shopList = new List<Shop>();
            while (reader.Read())
            {
                shopList.Add(
                new Shop
                {
                    //CHANGE RELAVANT DETAILS
                    ShopID = reader.GetInt32(0),
                    //Name = reader.GetString(1),
                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return shopList;
        }

        public Shop GetDetails(int shopId)
        {
            Shop shop = new Shop();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a shop record.
            
            cmd.CommandText = @"SELECT * FROM Shop WHERE ShopID = @selectedShopID";

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “judgeId”.
            cmd.Parameters.AddWithValue("@selectedShopID", shopId);

            //Open a database connection
            conn.Open();

            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    // Fill judge object with values from the data reader
                    shop.ShopID = shopId;
                    shop.ShopPicture = reader.GetString(1);
                    shop.ShopName = reader.GetString(2);
                    shop.ShopDescription = !reader.IsDBNull(3) ? reader.GetString(3) : null;
                    shop.Zone = reader.GetString(4);
                    shop.ContactNumber = reader.GetInt32(5);
                    shop.Address = reader.GetString(6);
                    shop.Website = reader.GetString(7);
                }
            }

            //Close DataReader
            reader.Close();

            //Close the database connection
            conn.Close();

            return shop;
        }
        public int Add(Customer customer)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO Competitor (CompetitorName, Salutation, EmailAddr, Password)
                                OUTPUT INSERTED.CompetitorID
                                VALUES(@name, @salutation, @email, @password)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@name", customer.CustomerName);
            cmd.Parameters.AddWithValue("@salutation", customer.Salutation);
            cmd.Parameters.AddWithValue("@email", customer.EmailAddr);
            cmd.Parameters.AddWithValue("@password", customer.Password);
            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            customer.CustomerID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return customer.CustomerID;
        }
    }
}
