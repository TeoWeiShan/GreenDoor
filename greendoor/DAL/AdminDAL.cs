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
                    ShopPicture = reader.GetString(1),
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
                    ecoevent.EventID = reader.GetInt32(0);
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
        public List<AdminForumViewModel> CustomerPostList()
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
            List<AdminForumViewModel> forumPostList = new List<AdminForumViewModel>();

            while (reader.Read())
            {
                forumPostList.Add(
                    new AdminForumViewModel
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

        public List<AdminForumViewModel> ShopPostList()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT fp.ForumPostID, fp.ShopID, s.ShopName, fp.PostName, fp.PostDescription, fp.DateTimePosted
                                FROM ForumPost fp
                                INNER JOIN Shop s
                                ON fp.ShopID = s.ShopID";

            //Open a database connection
            conn.Open();

            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a list of forum post objects
            List<AdminForumViewModel> forumPostList = new List<AdminForumViewModel>();

            while (reader.Read())
            {
                forumPostList.Add(
                    new AdminForumViewModel
                    {
                        ShopID = reader.GetInt32(1),
                        ForumPostID = reader.GetInt32(0),
                        ShopName = reader.GetString(2),
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

        public List<AdminForumViewModel> CustomerComments(int ForumPostID)
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

            List<AdminForumViewModel> CustomerCommentsList = new List<AdminForumViewModel>();

            while (reader.Read())
            {
                CustomerCommentsList.Add(
                    new AdminForumViewModel
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

        public List<AdminForumViewModel> ShopComments(int ForumPostID)
        {
            AdminForumViewModel fpcVM = new AdminForumViewModel();

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

            List<AdminForumViewModel> ShopCommentsList = new List<AdminForumViewModel>();

            while (reader.Read())
            {
                ShopCommentsList.Add(
                    new AdminForumViewModel
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

        public AdminForumViewModel CustomerPostDetails(int ForumPostID)
        {
            AdminForumViewModel fpcVM = new AdminForumViewModel();

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

        public AdminForumViewModel ShopPostDetails(int ForumPostID)
        {
            AdminForumViewModel fpcVM = new AdminForumViewModel();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a shop record.

            cmd.CommandText = @"SELECT fp.ForumPostID, fp.PostName, fp.PostDescription,fp.DateTimePosted, fp.ShopID, s.ShopName
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

                }
            }
            //Close DataReader
            reader.Close();

            //Close the database connection
            conn.Close();

            return fpcVM;
        }

        public int CustomerAddComment(AdminForumViewModel custComment)
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

        public int ShopAddComment(AdminForumViewModel shopComment)
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

        public List<AdminForumViewModel> ShopPostIDCheckList()
        {
            AdminForumViewModel fpcVM = new AdminForumViewModel();

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

            List<AdminForumViewModel> ShopIDList = new List<AdminForumViewModel>();

            while (reader.Read())
            {
                ShopIDList.Add(
                    new AdminForumViewModel
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

        public List<AdminForumViewModel> CustPostIDCheckList()
        {
            AdminForumViewModel fpcVM = new AdminForumViewModel();

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

            List<AdminForumViewModel> CustIDList = new List<AdminForumViewModel>();

            while (reader.Read())
            {
                CustIDList.Add(
                    new AdminForumViewModel
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
        public void EventUpdate(AdminEventViewModel aeVM)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify an INSERT SQL statement which will
            //return the auto-generated CriteriaID after insertion
            cmd.CommandText = @"UPDATE EcoEvents
                                SET EventName = @newEventName,
                                EventDescription = @newDesc,
                                StartDate = @newStart,
                                EndDate = @newEnd
                                WHERE EventID = @selectedEventID";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@newEventName", aeVM.EventName);
            cmd.Parameters.AddWithValue("@newDesc", aeVM.EventDescription);
            cmd.Parameters.AddWithValue("@newStart", aeVM.StartDate);
            cmd.Parameters.AddWithValue("@newEnd", aeVM.EndDate);
            cmd.Parameters.AddWithValue("@selectedEventID", aeVM.EventID);
            //A connection to database must be opened before any operations made.
            conn.Open();

            //ExecuteScalar is used to retrieve the auto-generated
            cmd.ExecuteNonQuery();
            //A connection should be closed after operations.
            conn.Close();
        }

        public void EventDelete(AdminEventViewModel aeVM)
        {
            //Instantiate a SqlCommand object, supply it with a DELETE SQL statement

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM EcoEvents
                                WHERE EventID = @selectedEventID";
            cmd.Parameters.AddWithValue("@selectedEventID", aeVM.EventID);

            //Open a database connection
            conn.Open();
            //Execute the DELETE SQL to remove the staff record
            cmd.ExecuteNonQuery();
            //Close database connection
            conn.Close();
        }

        public void PostDelete(AdminForumViewModel afVM)
        {
            //Instantiate a SqlCommand object, supply it with a DELETE SQL statement

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM ForumPost
                                WHERE ForumPostID = @selectedPostID";
            cmd.Parameters.AddWithValue("@selectedPostID", afVM.ForumPostID);

            //Open a database connection
            conn.Open();
            //Execute the DELETE SQL to remove the staff record
            cmd.ExecuteNonQuery();
            //Close database connection
            conn.Close();
        }

        public List<AdminForumViewModel> ShopCommentIDCheckList(int ForumCommentID)
        {
            AdminForumViewModel fpcVM = new AdminForumViewModel();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a shop record.

            cmd.CommandText = @"SELECT fc.ForumPostID, fc.ForumCommentsID, fc.ShopID
                                FROM ForumComment fc
                                INNER JOIN Shop s
                                ON s.ShopID = fc.ShopID
								WHERE fc.ForumCommentsID = @selectedCommentID";

            cmd.Parameters.AddWithValue("@selectedCommentID", ForumCommentID);
            //Open a database connection
            conn.Open();

            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            List<AdminForumViewModel> ShopIDList = new List<AdminForumViewModel>();

            while (reader.Read())
            {
                ShopIDList.Add(
                    new AdminForumViewModel
                    {
                        ForumPostID = reader.GetInt32(0),
                        ForumCommentsID = reader.GetInt32(1),
                        ShopID = reader.GetInt32(2)
                    }
                );
            }

            //Close DataReader
            reader.Close();

            //Close the database connection
            conn.Close();

            return ShopIDList;
        }

        public List<AdminForumViewModel> CustCommentIDCheckList(int ForumCommentID)
        {
            AdminForumViewModel fpcVM = new AdminForumViewModel();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a shop record.

            cmd.CommandText = @"SELECT fc.ForumPostID, fc.ForumCommentsID, fc.CustomerID
                                FROM ForumComment fc
                                INNER JOIN Customer c
                                ON c.CustomerID = fc.CustomerID
								WHERE fc.ForumCommentsID = @selectedCommentID";

            cmd.Parameters.AddWithValue("@selectedCommentID", ForumCommentID);
            //Open a database connection
            conn.Open();

            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            List<AdminForumViewModel> CustIDList = new List<AdminForumViewModel>();

            while (reader.Read())
            {
                CustIDList.Add(
                    new AdminForumViewModel
                    {
                        ForumPostID = reader.GetInt32(0),
                        ForumCommentsID = reader.GetInt32(1),
                        CustomerID = reader.GetInt32(2)
                    }
                );
            }

            //Close DataReader
            reader.Close();

            //Close the database connection
            conn.Close();

            return CustIDList;
        }

        public AdminForumViewModel ShopCommentsDetails(int ForumCommentsID)
        {
            AdminForumViewModel fpcVM = new AdminForumViewModel();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a shop record.

            cmd.CommandText = @"SELECT fc.ForumCommentsID, fc.CommentsDescription,fc.ShopID, s.ShopName, fc.DateTimePosted
                                FROM ForumComment fc
                                INNER JOIN Shop s
                                ON fc.ShopID = s.ShopID
                                WHERE fc.ForumCommentsID = @selectedCommentID";

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “judgeId”.
            cmd.Parameters.AddWithValue("@selectedCommentID", ForumCommentsID);

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
                    fpcVM.ForumCommentsID = reader.GetInt32(0);
                    fpcVM.CommentsDescription = reader.GetString(1);
                    fpcVM.ShopID = reader.GetInt32(2);
                    fpcVM.ShopName = reader.GetString(3);
                    fpcVM.DateTimePosted = reader.GetDateTime(4);

                }
            }
            //Close DataReader
            reader.Close();

            //Close the database connection
            conn.Close();

            return fpcVM;
        }

        public AdminForumViewModel CustomerCommentsDetails(int ForumCommentsID)
        {
            AdminForumViewModel fpcVM = new AdminForumViewModel();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a shop record.

            cmd.CommandText = @"SELECT fc.ForumCommentsID, fc.CommentsDescription,fc.CustomerID, c.CustomerName, fc.DateTimePosted
                                FROM ForumComment fc
                                INNER JOIN Customer c
                                ON fc.CustomerID = c.CustomerID
                                WHERE fc.ForumCommentsID = @selectedCommentID";

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “judgeId”.
            cmd.Parameters.AddWithValue("@selectedCommentID", ForumCommentsID);

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
                    fpcVM.ForumCommentsID = reader.GetInt32(0);
                    fpcVM.CommentsDescription = reader.GetString(1);
                    fpcVM.CustomerID = reader.GetInt32(2);
                    fpcVM.CustomerName = reader.GetString(3);
                    fpcVM.DateTimePosted = reader.GetDateTime(4);

                }
            }
            //Close DataReader
            reader.Close();

            //Close the database connection
            conn.Close();

            return fpcVM;
        }

        public void CommentDelete(AdminForumViewModel afVM)
        {
            //Instantiate a SqlCommand object, supply it with a DELETE SQL statement

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM ForumComment
                                WHERE ForumCommentsID = @selectedCommentID";
            cmd.Parameters.AddWithValue("@selectedCommentID", afVM.ForumCommentsID);

            //Open a database connection
            conn.Open();
            //Execute the DELETE SQL to remove the staff record
            cmd.ExecuteNonQuery();
            //Close database connection
            conn.Close();
        }

        public AdminShopViewModel InShopPostDetails(int ShopPostID)
        {
            AdminShopViewModel asVM = new AdminShopViewModel();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a shop record.

            cmd.CommandText = @"SELECT sp.ShopPostID, sp.PostName, sp.PostDescription, sp.DateTimePosted, sp.ShopID, s.ShopName
                                FROM ShopPost sp
                                INNER JOIN Shop s
                                ON sp.ShopID = s.ShopID
                                WHERE sp.ShopPostID = @selectedPostID";

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “judgeId”.
            cmd.Parameters.AddWithValue("@selectedPostID", ShopPostID);

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
                    asVM.ShopPostID = reader.GetInt32(0);
                    asVM.PostName = reader.GetString(1);
                    asVM.PostDescription = reader.GetString(2);
                    asVM.DateTimePosted = reader.GetDateTime(3);
                    asVM.ShopID = reader.GetInt32(4);
                    asVM.ShopName = reader.GetString(5);

                }
            }
            //Close DataReader
            reader.Close();

            //Close the database connection
            conn.Close();

            return asVM;
        }

        public void InShopPostDelete(AdminShopViewModel asVM)
        {
            //Instantiate a SqlCommand object, supply it with a DELETE SQL statement

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM ShopPost
                                WHERE ShopPostID = @selectedPostID";
            cmd.Parameters.AddWithValue("@selectedPostID", asVM.ShopPostID);

            //Open a database connection
            conn.Open();
            //Execute the DELETE SQL to remove the staff record
            cmd.ExecuteNonQuery();
            //Close database connection
            conn.Close();
        }

        public void InShopReviewsDelete(AdminShopViewModel asVM)
        {
            //Instantiate a SqlCommand object, supply it with a DELETE SQL statement

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM Reviews
                                WHERE ReviewsID = @selectedReviewsID";
            cmd.Parameters.AddWithValue("@selectedReviewsID", asVM.ReviewsID);

            //Open a database connection
            conn.Open();
            //Execute the DELETE SQL to remove the staff record
            cmd.ExecuteNonQuery();
            //Close database connection
            conn.Close();
        }

        public AdminShopViewModel InShopReviewDetails(int ReviewsID)
        {
            AdminShopViewModel asVM = new AdminShopViewModel();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a shop record.

            cmd.CommandText = @"SELECT r.ReviewsID, r.Rating, r.Description, r.DateTimePosted, r.CustomerID,c.CustomerName, r.ShopID,s.ShopName
                                FROM Reviews r
                                INNER JOIN Shop s
                                ON r.ShopID = s.ShopID
								INNER JOIN Customer c
								ON c.CustomerID = r.CustomerID
								WHERE r.ReviewsID = @selectedReviewID";

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “judgeId”.
            cmd.Parameters.AddWithValue("@selectedReviewID", ReviewsID);

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
                    asVM.ReviewsID = reader.GetInt32(0);
                    asVM.Rating = !reader.IsDBNull(1) ? reader.GetInt32(1) : (int?)null;
                    asVM.Description = reader.GetString(2);
                    asVM.DateTimePosted = reader.GetDateTime(3);
                    asVM.CustomerID = reader.GetInt32(4);
                    asVM.CustomerName = reader.GetString(5);
                    asVM.ShopID = reader.GetInt32(6);
                    asVM.ShopName = reader.GetString(7);

                }
            }
            //Close DataReader
            reader.Close();

            //Close the database connection
            conn.Close();

            return asVM;
        }

        public void ShopDelete(AdminShopViewModel asVM)
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

    }
}
