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
    public class AdminDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public AdminDAL()
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
        public List<AdminShopViewModel> GetAllShop()
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
            List<AdminShopViewModel> shopList = new List<AdminShopViewModel>();
            while (reader.Read())
            {
                shopList.Add(
                new AdminShopViewModel
                {
                    ShopID = reader.GetInt32(0),
                    ShopName = reader.GetString(2),
                    EmailAddr = reader.GetString(10)
                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return shopList;
        }

        public AdminShopViewModel GetShopDetails(int shopId)
        {
            AdminShopViewModel shop = new AdminShopViewModel();

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

        public List<AdminEventViewModel> GetAllEvents()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT ec.EventID, ec.ShopID, ec.EventName, ec.EventDescription,ec.DateTimePosted,ec.StartDate,ec.EndDate, s.ShopName
                                FROM EcoEvents ec
                                INNER JOIN Shop s
                                ON ec.ShopID = s.ShopID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a list
            List<AdminEventViewModel> eventList = new List<AdminEventViewModel>();
            while (reader.Read())
            {
                eventList.Add(
                new AdminEventViewModel
                {
                    EventID = reader.GetInt32(0),
                    ShopID = reader.GetInt32(1),
                    EventName = reader.GetString(2),
                    EventDescription = reader.GetString(3),
                    DateTimePosted = reader.GetDateTime(4),
                    StartDate = !reader.IsDBNull(5) ? reader.GetDateTime(5) : (DateTime?)null,
                    EndDate = !reader.IsDBNull(6) ? reader.GetDateTime(6) : (DateTime?)null,
                    ShopName = reader.GetString(7)
                });
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return eventList;
        }

        public AdminEventViewModel GetEventDetails(int eventId)
        {
            AdminEventViewModel ecoevent = new AdminEventViewModel();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a shop record.

            cmd.CommandText = @"SELECT ec.EventID, ec.ShopID, ec.EventName, ec.EventDescription,ec.DateTimePosted,ec.StartDate,ec.EndDate, s.ShopName
                                FROM EcoEvents ec
                                INNER JOIN Shop s
                                ON ec.ShopID = s.ShopID
								WHERE ec.EventID = @selectedEventID";

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “judgeId”.
            cmd.Parameters.AddWithValue("@selectedEventID", eventId);

            //Open a database connection
            conn.Open();

            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    ecoevent.ShopID = reader.GetInt32(1);
                    ecoevent.ShopName = reader.GetString(7);
                    ecoevent.EventName = reader.GetString(2);
                    ecoevent.EventDescription = reader.GetString(3);
                    ecoevent.DateTimePosted = reader.GetDateTime(4);
                    ecoevent.StartDate = !reader.IsDBNull(5) ? reader.GetDateTime(5) : (DateTime?)null;
                    ecoevent.EndDate = !reader.IsDBNull(6) ? reader.GetDateTime(6) : (DateTime?)null;
                }
            }
            //Close DataReader
            reader.Close();

            //Close the database connection
            conn.Close();

            return ecoevent;
        }

        public List<AdminForumPostViewModel> GetAllForumPost()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT fp.ForumPostID, fp.CustomerID,fp.PostName,fp.PostDescription,fp.DateTimePosted, c.CustomerName,c.EmailAddr
                                FROM ForumPost fp
                                INNER JOIN Customer c
                                ON fp.CustomerID = c.CustomerID";

            //Open a database connection
            conn.Open();

            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a list of forum post objects
            List<AdminForumPostViewModel> forumPostList = new List<AdminForumPostViewModel>();

            while (reader.Read())
            {
                forumPostList.Add(
                    new AdminForumPostViewModel
                    {
                        CustomerID = reader.GetInt32(1),
                        ForumPostID = reader.GetInt32(0),
                        PostName = reader.GetString(2),
                        PostDescription = reader.GetString(3),
                        DateTimePosted = reader.GetDateTime(4),
                        CustomerName = reader.GetString(5),
                        EmailAddr = reader.GetString(6)
                    }
                );
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return forumPostList;
        }

        public AdminForumPostViewModel GetForumPostDetails(int ForumPostID)
        {
            AdminForumPostViewModel postDetails = new AdminForumPostViewModel();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a shop record.

            cmd.CommandText = @"SELECT fp.ForumPostID, fp.CustomerID,fp.PostName,fp.PostDescription,fp.DateTimePosted, c.CustomerName,c.EmailAddr
                                FROM ForumPost fp
                                INNER JOIN Customer c
                                ON fp.CustomerID = c.CustomerID
                                WHERE fp.ForumPostID = @selectedFPID";

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “judgeId”.
            cmd.Parameters.AddWithValue("@selectedFPID", ForumPostID);

            //Open a database connection
            conn.Open();

            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    postDetails.CustomerID = reader.GetInt32(1);
                    postDetails.ForumPostID = reader.GetInt32(0);
                    postDetails.PostName = reader.GetString(2);
                    postDetails.PostDescription = reader.GetString(3);
                    postDetails.DateTimePosted = reader.GetDateTime(4);
                    postDetails.CustomerName = reader.GetString(5);
                    postDetails.EmailAddr = reader.GetString(6);
                }
            }
            //Close DataReader
            reader.Close();

            //Close the database connection
            conn.Close();

            return postDetails;
        }
    }
}
