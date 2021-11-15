using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using greendoor.DAL;

namespace greendoor.Models
{
    public class ValidateEmailExists : ValidationAttribute
    {
        private CustomerDAL customerContext = new CustomerDAL();
        private ShopDAL shopContext = new ShopDAL();
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Making empty lists for storage
            List<string> emailList = new List<string>();
            List<Customer> customerList = new List<Customer>();
            List<Shop> shopList = new List<Shop>();

            // Filling the lists
            customerList = customerContext.GetAllCustomer();
            shopList = shopContext.GetAllShop();
            foreach (Customer customer in customerList)
            {
                emailList.Add(customer.EmailAddr);
            }
            foreach (Shop shop in shopList)
            {
                emailList.Add(shop.EmailAddr);
            }
            emailList.Add("admin@greendoor.sg");
            emailList.Add("Admin@greendoor.sg");

            // Get the email value to validate
            string email = Convert.ToString(value);

            foreach (string mail in emailList)
            {
                if (mail == email)
                {
                    // validation failed
                    return new ValidationResult
                    ("Email address is already in use!");
                }
            }
            // validation passed
            return ValidationResult.Success;
        }

    }
}
