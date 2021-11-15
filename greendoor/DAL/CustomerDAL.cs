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
    public class CustomerDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public CustomerDAL()
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

        public List<Customer> GetAllCustomer()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Customer ORDER BY CustomerID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a judge list
            List<Customer> customerList = new List<Customer>();
            while (reader.Read())
            {
                customerList.Add(
                new Customer
                {
                    CustomerID = reader.GetInt32(0), //0: 1st column
                    CustomerName = reader.GetString(1), //1: 2nd column
                    EmailAddr = reader.GetString(2),
                    Password = reader.GetString(3)
                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return customerList;
        }

        public int Add(Customer customer)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO Customer (CustomerName, EmailAddr, Password)
                                OUTPUT INSERTED.CustomerID
                                VALUES(@name, @email, @password)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@name", customer.CustomerName);
            cmd.Parameters.AddWithValue("@email", customer.EmailAddr);
            cmd.Parameters.AddWithValue("@password", customer.Password);
            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            customer.CustomerID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return customer.CustomerID;
        }

        public bool IsEmailExist(string email, int customerID)
        {
            bool emailFound = false;
            //Create a SqlCommand object and specify the SQL statement
            //to get a customer record with the email address to be validated
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT CustomerID FROM Customer
                                WHERE EmailAddr=@selectedEmail";
            cmd.Parameters.AddWithValue("@selectedEmail", email);
            //Open a database connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            { //Records found
                while (reader.Read())
                {
                    if (reader.GetInt32(0) != customerID)
                        //The email address is used by another customer
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
    }
}
