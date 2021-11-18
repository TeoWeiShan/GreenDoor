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
    public class ForumDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        //constructor
        public ForumDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "GreenDoorConnectionString");

            //Instantiate a SqlConnection object with the
            //Connection String read.
            conn = new SqlConnection(strConn);
        }

        //retrieve a list of forum post objects from database
        public List<ForumPost> GetAllForumPost()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM ForumPost ORDER BY ForumPostID";

            //Open a database connection
            conn.Open();

            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a list of forum post objects
            List<ForumPost> forumPostList = new List<ForumPost>();

            while (reader.Read())
            {
                forumPostList.Add(
                    new ForumPost
                    {
                        CustomerID = reader.GetInt32(0),
                        ForumPostID = reader.GetInt32(1),
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
            return forumPostList;
        }

        //add area of forum post (C)
        public int CustomerAddPost(ForumPostCommentViewModel forumPost)
        {
            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will 
            //return the auto-generated ForumPostID after insertion 
            cmd.CommandText = @"INSERT INTO ForumPost (CustomerID, PostName, PostDescription, DateTimePosted)
                                OUTPUT INSERTED.ForumPostID 
                                VALUES(@cust, @name, @desc, @date)";
            //Define the parameters used in SQL statement, value for each parameter 
            //is retrieved from respective class's property. 
            cmd.Parameters.AddWithValue("@cust", forumPost.CustomerID);
            cmd.Parameters.AddWithValue("@name", forumPost.PostName);
            cmd.Parameters.AddWithValue("@desc", forumPost.PostDescription);
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            //A connection to database must be opened before any operations made. 
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated 
            //ForumPostID after executing the INSERT SQL statement 
            forumPost.ForumPostID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations. 
            conn.Close();
            return forumPost.ForumPostID;
        }

        public int ShopAddPost(ForumPostCommentViewModel forumPost)
        {
            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will 
            //return the auto-generated ForumPostID after insertion 
            cmd.CommandText = @"INSERT INTO ForumPost (ShopID, PostName, PostDescription, DateTimePosted)
                                OUTPUT INSERTED.ForumPostID 
                                VALUES(@shop, @name, @desc, @date)";
            //Define the parameters used in SQL statement, value for each parameter 
            //is retrieved from respective class's property. 
            cmd.Parameters.AddWithValue("@shop", forumPost.ShopID);
            cmd.Parameters.AddWithValue("@name", forumPost.PostName);
            cmd.Parameters.AddWithValue("@desc", forumPost.PostDescription);
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            //A connection to database must be opened before any operations made. 
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated 
            //ForumPostID after executing the INSERT SQL statement 
            forumPost.ForumPostID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations. 
            conn.Close();
            return forumPost.ForumPostID;
        }

        public List<ForumPostCommentViewModel> GetAllForumPostVM()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT fp.ForumPostID, fp.CustomerID, c.CustomerName, fp.PostName, fp.PostDescription, fp.DateTimePosted
                                FROM ForumPost fp
                                INNER JOIN Customer c
                                ON fp.CustomerID = c.CustomerID";

            //Open a database connection
            conn.Open();

            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a list of forum post objects
            List<ForumPostCommentViewModel> forumPostList = new List<ForumPostCommentViewModel>();

            while (reader.Read())
            {
                forumPostList.Add(
                    new ForumPostCommentViewModel
                    {
                        CustomerID = reader.GetInt32(1),
                        ForumPostID = reader.GetInt32(0),
                        CustomerName = reader.GetString(2),
                        PostName = reader.GetString(3),
                        PostDescription = reader.GetString(4),
                        DateTimePosted = reader.GetDateTime(5)
                    }
                );
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return forumPostList;
        }

        public List<ForumPostCommentViewModel> CustomerComments(int ForumPostID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a shop record.

            cmd.CommandText = @"SELECT fc.ForumCommentsID, fc.CustomerID, c.CustomerName, fc.ForumPostID, fp.PostName, fc.DateTimePosted, fc.ForumCommentsID, fc.CommentsDescription
                                FROM ForumComment fc
                                INNER JOIN Customer c
                                ON c.CustomerID = fc.CustomerID
                                INNER JOIN ForumPost fp
                                ON fc.ForumPostID = fp.ForumPostID
                                WHERE fc.ForumPostID = @selectedPostID";

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “judgeId”.
            cmd.Parameters.AddWithValue("@selectedPostID", ForumPostID);

            //Open a database connection
            conn.Open();

            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            List<ForumPostCommentViewModel> CustomerCommentsList = new List<ForumPostCommentViewModel>();

            while (reader.Read())
            {
                CustomerCommentsList.Add(
                    new ForumPostCommentViewModel
                    {
                        ForumCommentsID = reader.GetInt32(0),
                        CustomerID = reader.GetInt32(1),
                        CustomerName = reader.GetString(2),
                        ForumPostID = reader.GetInt32(3),
                        PostName = reader.GetString(4),
                        DateTimePosted = reader.GetDateTime(5),
                        CommentsDescription = reader.GetString(6)
                    }
                );
            }

            //Close DataReader
            reader.Close();

            //Close the database connection
            conn.Close();

            return CustomerCommentsList;
        }

        public List<ForumPostCommentViewModel> ShopComments(int ForumPostID)
        {
            ForumPostCommentViewModel fpcVM = new ForumPostCommentViewModel();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a shop record.

            cmd.CommandText = @"SELECT fc.ForumCommentsID, fc.ShopID, s.ShopName, fc.ForumPostID, fp.PostName, fc.DateTimePosted, fc.CommentsDescription
                                FROM ForumComment fc
                                INNER JOIN Shop s
                                ON s.ShopID = fc.ShopID
                                INNER JOIN ForumPost fp
                                ON fc.ForumPostID = fp.ForumPostID
                                WHERE fc.ForumPostID = @selectedPostID";

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “judgeId”.
            cmd.Parameters.AddWithValue("@selectedPostID", ForumPostID);

            //Open a database connection
            conn.Open();

            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            List<ForumPostCommentViewModel> ShopCommentsList = new List<ForumPostCommentViewModel>();

            while (reader.Read())
            {
                ShopCommentsList.Add(
                    new ForumPostCommentViewModel
                    {
                        ForumCommentsID = reader.GetInt32(0),
                        ShopID = reader.GetInt32(1),
                        ShopName = reader.GetString(2),
                        ForumPostID = reader.GetInt32(3),
                        PostName = reader.GetString(4),
                        DateTimePosted = reader.GetDateTime(5),
                        CommentsDescription = reader.GetString(6)
                    }
                );
            }

            //Close DataReader
            reader.Close();

            //Close the database connection
            conn.Close();

            return ShopCommentsList;
        }

        public ForumPostCommentViewModel CustomerPostDetails(int ForumPostID)
        {
            ForumPostCommentViewModel fpcVM = new ForumPostCommentViewModel();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a shop record.

            cmd.CommandText = @"SELECT fp.ForumPostID, fp.PostName, fp.PostDescription,fp.DateTimePosted, fp.CustomerID, c.CustomerName
                                FROM ForumPost fp
                                INNER JOIN Customer c
                                ON fp.CustomerID = c.CustomerID
                                WHERE ForumPostID = @selectedPostID";

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “judgeId”.
            cmd.Parameters.AddWithValue("@selectedPostID", ForumPostID);

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
                    fpcVM.ForumPostID = reader.GetInt32(0);
                    fpcVM.PostName = reader.GetString(1);
                    fpcVM.PostDescription = reader.GetString(2);
                    fpcVM.DateTimePosted = reader.GetDateTime(3);
                    fpcVM.CustomerID = reader.GetInt32(4);
                    fpcVM.CustomerName = reader.GetString(5);

                }
            }
            //Close DataReader
            reader.Close();

            //Close the database connection
            conn.Close();

            return fpcVM;
        }
    }
}

