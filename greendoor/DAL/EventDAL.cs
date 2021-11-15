﻿using System;
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
       
    }
}
