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

        public Customer GetDetails(int CustomerID)
        {
            Customer cust = new Customer();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a shop record.

            cmd.CommandText = @"SELECT * FROM Customer WHERE CustomerID = @selectedCustID";

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “judgeId”.
            cmd.Parameters.AddWithValue("@selectedCustID", CustomerID);

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
                    cust.CustomerID = reader.GetInt32(0);
                    cust.CustomerName = reader.GetString(1);
                    cust.EmailAddr = reader.GetString(2);
                    cust.Password = reader.GetString(3);
                }
            }

            //Close DataReader
            reader.Close();

            //Close the database connection
            conn.Close();

            return cust;
        }

        public void Delete(Customer cust)
        {
            //Instantiate a SqlCommand object, supply it with a DELETE SQL statement

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM Favourite
                                WHERE CustomerID = @selectedCustID";
            cmd.Parameters.AddWithValue("@selectedCustID", cust.CustomerID);

            SqlCommand cmd1 = conn.CreateCommand();
            cmd1.CommandText = @"DELETE FROM ForumComment
                                WHERE CustomerID= @selectedCustID";
            cmd1.Parameters.AddWithValue("@selectedCustID", cust.CustomerID);

            SqlCommand cmd2 = conn.CreateCommand();
            cmd2.CommandText = @"DELETE FROM ForumPost
                                WHERE CustomerID= @selectedCustID";
            cmd2.Parameters.AddWithValue("@selectedCustID", cust.CustomerID);

            SqlCommand cmd3 = conn.CreateCommand();
            cmd3.CommandText = @"DELETE FROM Reviews
                                WHERE CustomerID= @selectedCustID";
            cmd3.Parameters.AddWithValue("@selectedCustID", cust.CustomerID);

            SqlCommand cmd4 = conn.CreateCommand();
            cmd4.CommandText = @"DELETE FROM ShopComment
                                WHERE CustomerID= @selectedCustID";
            cmd4.Parameters.AddWithValue("@selectedCustID", cust.CustomerID);

            SqlCommand cmd5 = conn.CreateCommand();
            cmd5.CommandText = @"DELETE FROM Customer
                                WHERE CustomerID= @selectedCustID";
            cmd5.Parameters.AddWithValue("@selectedCustID", cust.CustomerID);

            //Open a database connection
            conn.Open();
            //Execute the DELETE SQL to remove the staff record
            cmd.ExecuteNonQuery();
            cmd1.ExecuteNonQuery();
            cmd2.ExecuteNonQuery();
            cmd3.ExecuteNonQuery();
            cmd4.ExecuteNonQuery();
            cmd5.ExecuteNonQuery();
            //Close database connection
            conn.Close();
        }
        public void Update(Customer cust)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify an INSERT SQL statement which will
            //return the auto-generated CriteriaID after insertion
            cmd.CommandText = @"UPDATE Customer
                                SET CustomerName = @newCustName,
                                Password = @newPassword
                                WHERE CustomerID = @selectedCustID";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@newCustName", cust.CustomerName);
            cmd.Parameters.AddWithValue("@newPassword", cust.Password);
            cmd.Parameters.AddWithValue("@selectedCustID", cust.CustomerID);

            //A connection to database must be opened before any operations made.
            conn.Open();

            //ExecuteScalar is used to retrieve the auto-generated
            cmd.ExecuteNonQuery();
            //A connection should be closed after operations.
            conn.Close();
        }

        public ShopReviewViewModel ShopDetails(int shopId)
        {
            ShopReviewViewModel shop = new ShopReviewViewModel();

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
    }
}
