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
    public class ForumPostDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        //constructor
        public ForumPostDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "NPBookConnectionString");

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
    }
}
