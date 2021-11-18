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
    public class ShopPostDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public ShopPostDAL()
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

        public List<ShopPost> GetAllShopPost(int shopID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"select * from ShopPost  Where ShopID = @selectedShopID order by DateTimePosted desc";
            cmd.Parameters.AddWithValue("@selectedShopID", shopID);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a list
            List<ShopPost> shopPostList = new List<ShopPost>();
            while (reader.Read())
            {
                shopPostList.Add(
                new ShopPost
                {
                    ShopPostID = reader.GetInt32(0),
                    ShopID = reader.GetInt32(1),
                    PostName = reader.GetString(2),
                    PostDescription = reader.GetString(3),
                    DateTimePosted = reader.GetDateTime(4)
                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return shopPostList;
        }

        public List<ShopPost> GetLatestShopPost(int shopID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"select top 1 * from ShopPost  Where ShopID = @selectedShopID order by DateTimePosted desc";
            cmd.Parameters.AddWithValue("@selectedShopID", shopID);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a list
            List<ShopPost> shopPostList = new List<ShopPost>();
            while (reader.Read())
            {
                shopPostList.Add(
                new ShopPost
                {
                    ShopPostID = reader.GetInt32(0),
                    ShopID = reader.GetInt32(1),
                    PostName = reader.GetString(2),
                    PostDescription = reader.GetString(3),
                    DateTimePosted = reader.GetDateTime(4)
                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return shopPostList;
        }


    }
}
