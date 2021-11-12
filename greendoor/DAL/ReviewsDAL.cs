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
    public class ReviewsDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public ReviewsDAL()
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
        public int Add(Reviews reviews)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO Reviews (Rating, CustomerID, ShopID, Description, DateTimePosted)
                                OUTPUT INSERTED.ReviewsID
                                VALUES(@rating, @custID, @shopID, @desc,@dateTime)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@rating", reviews.Rating);
            cmd.Parameters.AddWithValue("@custID", reviews.CustomerID);
            cmd.Parameters.AddWithValue("@shopID", reviews.ShopID);
            cmd.Parameters.AddWithValue("@desc", reviews.Description);
            cmd.Parameters.AddWithValue("@dateTime", DateTime.Now);

            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            reviews.ReviewsID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return reviews.ReviewsID;
        }
        public List<Reviews> GetAllReviews(int shopID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            //CHANGE SQL QUERY (Name)
            cmd.CommandText = @"SELECT * FROM Reviews WHERE ShopID = @selectedShopID";
            cmd.Parameters.AddWithValue("@selectedShopID", shopID);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a list
            List<Reviews> reviewsList = new List<Reviews>();
            while (reader.Read())
            {
                reviewsList.Add(
                new Reviews
                {
                    //CHANGE RELAVANT DETAILS
                    ReviewsID = reader.GetInt32(0),
                    Rating = !reader.IsDBNull(1) ? reader.GetInt32(1):(int?)null,
                    CustomerID = reader.GetInt32(2),
                    ShopID = reader.GetInt32(3),
                    Description = reader.GetString(4),
                    DateTimePosted = reader.GetDateTime(5)
                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return reviewsList;
        }
    }
}