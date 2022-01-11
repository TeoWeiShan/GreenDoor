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

        //add area of forum post (C)
        public int CustomerAddPost(ForumPostCommentViewModel forumPost)
        {
            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will 
            //return the auto-generated ForumPostID after insertion 
            cmd.CommandText = @"INSERT INTO ForumPost (CustomerID, PostName, PostDescription, PostCategory, DateTimePosted)
                                OUTPUT INSERTED.ForumPostID 
                                VALUES(@cust, @name, @desc, @cat, @date)";
            //Define the parameters used in SQL statement, value for each parameter 
            //is retrieved from respective class's property. 
            cmd.Parameters.AddWithValue("@cust", forumPost.CustomerID);
            cmd.Parameters.AddWithValue("@name", forumPost.PostName);
            cmd.Parameters.AddWithValue("@desc", forumPost.PostDescription);
            cmd.Parameters.AddWithValue("@cat", forumPost.PostCategory);
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
            cmd.CommandText = @"INSERT INTO ForumPost (ShopID, PostName, PostDescription, PostCategory, DateTimePosted)
                                OUTPUT INSERTED.ForumPostID 
                                VALUES(@shop, @name, @desc, @cat, @date)";
            //Define the parameters used in SQL statement, value for each parameter 
            //is retrieved from respective class's property. 
            cmd.Parameters.AddWithValue("@shop", forumPost.ShopID);
            cmd.Parameters.AddWithValue("@name", forumPost.PostName);
            cmd.Parameters.AddWithValue("@desc", forumPost.PostDescription);
            cmd.Parameters.AddWithValue("@cat", forumPost.PostCategory);
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

        public List<ForumPostCommentViewModel> CustomerPostList()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT fp.ForumPostID, fp.CustomerID, c.CustomerName, fp.PostName, fp.PostDescription, fp.DateTimePosted, fp.PostCategory
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
                        DateTimePosted = reader.GetDateTime(5),
                        PostCategory = reader.GetString(6)
                    }
                );
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return forumPostList;
        }

        public List<ForumPostCommentViewModel> ShopPostList()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT fp.ForumPostID, fp.ShopID, s.ShopName, fp.PostName, fp.PostDescription, fp.DateTimePosted, fp.PostCategory
                                FROM ForumPost fp
                                INNER JOIN Shop s
                                ON fp.ShopID = s.ShopID";

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
                        ShopID = reader.GetInt32(1),
                        ForumPostID = reader.GetInt32(0),
                        ShopName = reader.GetString(2),
                        PostName = reader.GetString(3),
                        PostDescription = reader.GetString(4),
                        DateTimePosted = reader.GetDateTime(5),
                        PostCategory = reader.GetString(6)
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

            cmd.CommandText = @"SELECT fc.ForumCommentsID, fc.CustomerID, c.CustomerName, fc.ForumPostID, fp.PostName, fc.DateTimePosted, fc.CommentsDescription
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

            cmd.CommandText = @"SELECT fp.ForumPostID, fp.PostName, fp.PostDescription,fp.DateTimePosted, fp.CustomerID, c.CustomerName, fp.PostCategory
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
                    fpcVM.PostCategory = reader.GetString(6);

                }
            }
            //Close DataReader
            reader.Close();

            //Close the database connection
            conn.Close();

            return fpcVM;
        }

        public ForumPostCommentViewModel ShopPostDetails(int ForumPostID)
        {
            ForumPostCommentViewModel fpcVM = new ForumPostCommentViewModel();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a shop record.

            cmd.CommandText = @"SELECT fp.ForumPostID, fp.PostName, fp.PostDescription,fp.DateTimePosted, fp.ShopID, s.ShopName, fp.PostCategory
                                FROM ForumPost fp
                                INNER JOIN Shop s
                                ON fp.ShopID = s.ShopID
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
                    fpcVM.ShopID = reader.GetInt32(4);
                    fpcVM.ShopName = reader.GetString(5);
                    fpcVM.PostCategory = reader.GetString(6);

                }
            }
            //Close DataReader
            reader.Close();

            //Close the database connection
            conn.Close();

            return fpcVM;
        }

        public int CustomerAddComment(ForumPostCommentViewModel custComment)
        {
            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will 
            //return the auto-generated ForumPostID after insertion 
            cmd.CommandText = @"INSERT INTO ForumComment (CustomerID, ForumPostID, CommentsDescription, DateTimePosted)
                                OUTPUT INSERTED.ForumCommentsID 
                                VALUES(@custID, @forumID, @desc, @date)";
            //Define the parameters used in SQL statement, value for each parameter 
            //is retrieved from respective class's property. 
            cmd.Parameters.AddWithValue("@custID", custComment.CustomerID);
            cmd.Parameters.AddWithValue("@forumID", custComment.ForumPostID);
            cmd.Parameters.AddWithValue("@desc", custComment.CommentsDescription);
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            //A connection to database must be opened before any operations made. 
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated 
            //ForumPostID after executing the INSERT SQL statement 
            custComment.ForumCommentsID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations. 
            conn.Close();
            return custComment.ForumCommentsID;
        }

        public int ShopAddComment(ForumPostCommentViewModel shopComment)
        {
            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will 
            //return the auto-generated ForumPostID after insertion 
            cmd.CommandText = @"INSERT INTO ForumComment (ShopID, ForumPostID, CommentsDescription, DateTimePosted)
                                OUTPUT INSERTED.ForumCommentsID 
                                VALUES(@shopID, @forumID, @desc, @date)";
            //Define the parameters used in SQL statement, value for each parameter 
            //is retrieved from respective class's property. 
            cmd.Parameters.AddWithValue("@shopID", shopComment.ShopID);
            cmd.Parameters.AddWithValue("@forumID", shopComment.ForumPostID);
            cmd.Parameters.AddWithValue("@desc", shopComment.CommentsDescription);
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            //A connection to database must be opened before any operations made. 
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated 
            //ForumPostID after executing the INSERT SQL statement 
            shopComment.ForumCommentsID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations. 
            conn.Close();
            return shopComment.ForumCommentsID;
        }

        public List<ForumPostCommentViewModel> ShopPostIDCheckList()
        {
            ForumPostCommentViewModel fpcVM = new ForumPostCommentViewModel();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a shop record.

            cmd.CommandText = @"SELECT fc.ForumPostID, fc.ShopID
                                FROM ForumPost fc
                                INNER JOIN Shop s
                                ON s.ShopID = fc.ShopID";

            //Open a database connection
            conn.Open();

            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            List<ForumPostCommentViewModel> ShopIDList = new List<ForumPostCommentViewModel>();

            while (reader.Read())
            {
                ShopIDList.Add(
                    new ForumPostCommentViewModel
                    {
                        ForumPostID = reader.GetInt32(0)
                    }
                );
            }

            //Close DataReader
            reader.Close();

            //Close the database connection
            conn.Close();

            return ShopIDList;
        }

        public List<ForumPostCommentViewModel> CustPostIDCheckList()
        {
            ForumPostCommentViewModel fpcVM = new ForumPostCommentViewModel();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a shop record.

            cmd.CommandText = @"SELECT fc.ForumPostID, fc.CustomerID
                                FROM ForumPost fc
                                INNER JOIN Customer c
                                ON fc.CustomerID = c.CustomerID";

            //Open a database connection
            conn.Open();

            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            List<ForumPostCommentViewModel> CustIDList = new List<ForumPostCommentViewModel>();

            while (reader.Read())
            {
                CustIDList.Add(
                    new ForumPostCommentViewModel
                    {
                        ForumPostID = reader.GetInt32(0)
                    }
                );
            }

            //Close DataReader
            reader.Close();

            //Close the database connection
            conn.Close();

            return CustIDList;
        }

        public List<ForumPostCommentViewModel> searchCustPostList(ForumPostCommentViewModel searchQuery)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT fp.ForumPostID, fp.CustomerID, c.CustomerName, fp.PostName, fp.PostDescription, fp.DateTimePosted, fp.PostCategory
                                FROM ForumPost fp
                                INNER JOIN Customer c
                                ON fp.CustomerID = c.CustomerID
                                WHERE PostDescription like '%@search%' or PostName like '%@search%'";

            cmd.Parameters.AddWithValue("@search", searchQuery.searchQuery);
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
                        DateTimePosted = reader.GetDateTime(5),
                        PostCategory = reader.GetString(6)
                    }
                );
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return forumPostList;
        }

        public List<ForumPostCommentViewModel> searchShopPostList(ForumPostCommentViewModel searchQuery)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT fp.ForumPostID, fp.ShopID, s.ShopName, fp.PostName, fp.PostDescription, fp.DateTimePosted, fp.PostCategory
                                FROM ForumPost fp
                                INNER JOIN Shop s
                                ON fp.ShopID = s.ShopID
                                WHERE PostDescription like '%@search%' or PostName like '%@search%'";

            cmd.Parameters.AddWithValue("@search", searchQuery.searchQuery);
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
                        ShopID = reader.GetInt32(1),
                        ForumPostID = reader.GetInt32(0),
                        ShopName = reader.GetString(2),
                        PostName = reader.GetString(3),
                        PostDescription = reader.GetString(4),
                        DateTimePosted = reader.GetDateTime(5),
                        PostCategory = reader.GetString(6)
                    }
                );
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return forumPostList;
        }
    }
}

