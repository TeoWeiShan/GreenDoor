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
    public class EventDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public EventDAL()
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

        public List<EcoEvents> GetAllEvent()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM EcoEvents ORDER BY EventID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a list
            List<EcoEvents> eList = new List<EcoEvents>();
            while (reader.Read())
            {
                eList.Add(
                new EcoEvents
                {
                    EventID = reader.GetInt32(0),
                    ShopID = reader.GetInt32(1),
                    EventName = reader.GetString(2),
                    
                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return eList;
        }

        public EcoEvents GetDetails(int eventId)
        {
            EcoEvents e = new EcoEvents();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a shop record.

            cmd.CommandText = @"SELECT * FROM EcoEvents WHERE EventID = @selectedEventID";

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
                    // Fill judge object with values from the data reader
                    e.EventID = eventId;
                    e.ShopID = reader.GetInt32(1);
                    e.EventName = reader.GetString(2);
                    e.EventDescription = !reader.IsDBNull(3) ? reader.GetString(3) : null;
                    e.DateTimePosted = reader.GetDateTime(4);
                    e.StartDate = reader.GetDateTime(5);
                    e.EndDate = reader.GetDateTime(6);
                    
                }
            }

            //Close DataReader
            reader.Close();

            //Close the database connection
            conn.Close();

            return e;
        }

        public int Add(EcoEvents e)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated competitionID after insertion
            cmd.CommandText = @"INSERT INTO EcoEvents (ShopID, EventName,EventDescription,DateTimePosted, StartDate, EndDate)
                                OUTPUT INSERTED.EventID
                                VALUES(@shopID, @eventName, @eventDescription,@dateTimePosted,@startDate,@endDate)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.

            cmd.Parameters.AddWithValue("@shopID", e.ShopID);
            cmd.Parameters.AddWithValue("@eventName", e.EventName);
            cmd.Parameters.AddWithValue("@eventDescription", e.EventDescription);
            cmd.Parameters.AddWithValue("@dateTimePosted", e.DateTimePosted);
            cmd.Parameters.AddWithValue("@startDate", e.StartDate);
            cmd.Parameters.AddWithValue("@endDate", e.EndDate);
            

            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //CompetitionID after executing the INSERT SQL statement
            e.EventID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return e.EventID;
        }

    }
}

