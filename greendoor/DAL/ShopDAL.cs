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

        public ShopReviewViewModel GetDetailsVM(int shopId)
        {
            ShopReviewViewModel shop = new ShopReviewViewModel();

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
                    shop.PostalCode = reader.GetInt32(7);
                    shop.SocialMediaLink = !reader.IsDBNull(8) ? reader.GetString(8) : null;
                    shop.WebsiteLink = !reader.IsDBNull(9) ? reader.GetString(9) : null;
                    shop.EmailAddr = reader.GetString(10);
                }
            }

            //Close DataReader
            reader.Close();

            //Close the database connection
            conn.Close();

            return shop;
        }
        public List<Shop> GetAllShop()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Shop ORDER BY ShopID";
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
                    shop.PostalCode = reader.GetInt32(7);
                    shop.SocialMediaLink = !reader.IsDBNull(8) ? reader.GetString(8) : null;
                    shop.WebsiteLink = !reader.IsDBNull(9) ? reader.GetString(9) : null;
                    shop.EmailAddr = reader.GetString(10);
                }
            }

            //Close DataReader
            reader.Close();

            //Close the database connection
            conn.Close();

            return shop;
        }
        public int Add(Shop shop)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO Shop (ShopPicture, ShopName, ShopDescription, Zone, ContactNum, Address, PostalCode, SocialMediaLink, WebsiteLink, EmailAddr, Password)
                                OUTPUT INSERTED.ShopID
                                VALUES(@shopPic, @shopName, @shopDesc, @zone,@contactNum,@addr,@postalCode,@socialMedia,@webLink,@emailAddr,@password)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            string web = " ";
            string social = " ";
            if (shop.SocialMediaLink != null)
            {
                web = shop.WebsiteLink;
            }
            if (shop.WebsiteLink != null)
            {
                social = shop.SocialMediaLink;
            }
            cmd.Parameters.AddWithValue("@shopPic", shop.ShopPicture);
            cmd.Parameters.AddWithValue("@shopName", shop.ShopName);
            cmd.Parameters.AddWithValue("@shopDesc", shop.ShopDescription);
            cmd.Parameters.AddWithValue("@zone", shop.Zone);
            cmd.Parameters.AddWithValue("@contactNum", shop.ContactNumber);
            cmd.Parameters.AddWithValue("@addr", shop.Address);
            cmd.Parameters.AddWithValue("@postalCode", shop.PostalCode);
            cmd.Parameters.AddWithValue("@socialMedia", social);
            cmd.Parameters.AddWithValue("@webLink", web);
            cmd.Parameters.AddWithValue("@emailAddr", shop.EmailAddr);
            cmd.Parameters.AddWithValue("@password", shop.Password);

            //A connection to database must be opened before any operations made.
            conn.Open();
            shop.ShopID = (int)cmd.ExecuteScalar();
            conn.Close();
            //Return id when no error occurs.
            return shop.ShopID;
        }

        public bool IsEmailExist(string email, int shopID)
        {
            bool emailFound = false;
            //Create a SqlCommand object and specify the SQL statement
            //to get a shop record with the email address to be validated
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT ShopID FROM Shop
                                WHERE EmailAddr=@selectedEmail";
            cmd.Parameters.AddWithValue("@selectedEmail", email);
            //Open a database connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            { //Records found
                while (reader.Read())
                {
                    if (reader.GetInt32(0) != shopID)
                        //The email address is used by another shop
                        emailFound = true;
                }
            }
            else
            { //No record
                emailFound = false; // The email address given does not exist
            }
            reader.Close();
            conn.Close();

            return emailFound;
        }

        public void Update(Shop shop)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify an INSERT SQL statement which will
            //return the auto-generated CriteriaID after insertion
            cmd.CommandText = @"UPDATE Shop
                                SET ShopName = @shopName,
                                ShopDescription = @shopDescription,
                                Zone = @zone,
                                ContactNum = @contact,
                                Address = @address,
                                PostalCode = @postalCode,
                                SocialMediaLink = @socialMedia,
                                WebsiteLink = @website,
                                EmailAddr = @emailAddr,
                                Password = @password
                                WHERE ShopID = @selectedShopID";

            string web = " ";
            string social = " ";
            if (shop.SocialMediaLink != null)
            {
                web = shop.WebsiteLink;
            }
            if (shop.WebsiteLink != null)
            {
                social = shop.SocialMediaLink;
            }
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@shopName", shop.ShopName);
            cmd.Parameters.AddWithValue("@shopDescription", shop.ShopDescription);
            cmd.Parameters.AddWithValue("@zone", shop.Zone);
            cmd.Parameters.AddWithValue("@contact", shop.ContactNumber);
            cmd.Parameters.AddWithValue("@address", shop.Address);
            cmd.Parameters.AddWithValue("@postalCode", shop.PostalCode);
            cmd.Parameters.AddWithValue("@socialMedia", social);
            cmd.Parameters.AddWithValue("@website", web);
            cmd.Parameters.AddWithValue("@emailAddr", shop.EmailAddr);
            cmd.Parameters.AddWithValue("@password", shop.Password);
            cmd.Parameters.AddWithValue("@selectedShopID", shop.ShopID);

            //A connection to database must be opened before any operations made.
            conn.Open();

            //ExecuteScalar is used to retrieve the auto-generated
            cmd.ExecuteNonQuery();
            //A connection should be closed after operations.
            conn.Close();
        }

        public void ShopDelete(ShopReviewViewModel asVM)
        {
            //Instantiate a SqlCommand object, supply it with a DELETE SQL statement

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM ShopPost
                                WHERE ShopID = @selectedShopID";
            cmd.Parameters.AddWithValue("@selectedShopID", asVM.ShopID);

            SqlCommand cmd2 = conn.CreateCommand();
            cmd2.CommandText = @"DELETE FROM ShopComment
                                WHERE ShopID = @selectedShopID";
            cmd2.Parameters.AddWithValue("@selectedShopID", asVM.ShopID);

            SqlCommand cmd3 = conn.CreateCommand();
            cmd3.CommandText = @"DELETE FROM Reviews
                                WHERE ShopID = @selectedShopID";
            cmd3.Parameters.AddWithValue("@selectedShopID", asVM.ShopID);

            SqlCommand cmd4 = conn.CreateCommand();
            cmd4.CommandText = @"DELETE FROM ForumPost
                                WHERE ShopID = @selectedShopID";
            cmd4.Parameters.AddWithValue("@selectedShopID", asVM.ShopID);

            SqlCommand cmd5 = conn.CreateCommand();
            cmd5.CommandText = @"DELETE FROM ForumComment
                                WHERE ShopID = @selectedShopID";
            cmd5.Parameters.AddWithValue("@selectedShopID", asVM.ShopID);

            SqlCommand cmd6 = conn.CreateCommand();
            cmd6.CommandText = @"DELETE FROM Favourite
                                WHERE ShopID = @selectedShopID";
            cmd6.Parameters.AddWithValue("@selectedShopID", asVM.ShopID);

            SqlCommand cmd7 = conn.CreateCommand();
            cmd7.CommandText = @"DELETE FROM EcoEvents
                                WHERE ShopID = @selectedShopID";
            cmd7.Parameters.AddWithValue("@selectedShopID", asVM.ShopID);

            SqlCommand cmd8 = conn.CreateCommand();
            cmd8.CommandText = @"DELETE FROM Shop
                                WHERE ShopID = @selectedShopID";
            cmd8.Parameters.AddWithValue("@selectedShopID", asVM.ShopID);

            //Open a database connection
            conn.Open();
            //Execute the DELETE SQL to remove the staff record
            cmd.ExecuteNonQuery();
            cmd2.ExecuteNonQuery();
            cmd3.ExecuteNonQuery();
            cmd4.ExecuteNonQuery();
            cmd5.ExecuteNonQuery();
            cmd6.ExecuteNonQuery();
            cmd7.ExecuteNonQuery();
            cmd8.ExecuteNonQuery();
            //Close database connection
            conn.Close();
        }
        public List<Shop> shopSearchQueryList(string searchQuery)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * 
                                FROM Shop
                                WHERE ShopName like '%"+searchQuery+"%'";

            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a list of forum post objects
            List<Shop> shopList = new List<Shop>();

            while (reader.Read())
            {
                shopList.Add(
                    new Shop
                    {
                        ShopID = reader.GetInt32(0),
                        ShopPicture = !reader.IsDBNull(1) ? reader.GetString(1) : null,
                        ShopName = reader.GetString(2),
                    }
                );
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return shopList;
        }
        public List<Shop> shopSearchZoneList(string zoneSelected)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * 
                                FROM Shop
                                WHERE Zone = '" + zoneSelected + "'";

            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a list of forum post objects
            List<Shop> shopList = new List<Shop>();

            while (reader.Read())
            {
                shopList.Add(
                    new Shop
                    {
                        ShopID = reader.GetInt32(0),
                        ShopPicture = !reader.IsDBNull(1) ? reader.GetString(1) : null,
                        ShopName = reader.GetString(2),
                    }
                );
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return shopList;
        }
        public List<Shop> shopSearchList(string searchQuery, string zoneSelected)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * 
                                FROM Shop
                                WHERE Zone = '" + zoneSelected + "' and ShopName like '%" + searchQuery + "%'";

            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a list of forum post objects
            List<Shop> shopList = new List<Shop>();

            while (reader.Read())
            {
                shopList.Add(
                    new Shop
                    {
                        ShopID = reader.GetInt32(0),
                        ShopPicture = !reader.IsDBNull(1) ? reader.GetString(1) : null,
                        ShopName = reader.GetString(2),
                    }
                );
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return shopList;
        }

    }
}
