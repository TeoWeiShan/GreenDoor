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
            // Get the email value to validate
            string email = Convert.ToString(value);
            // Casting the validation context to the "Customer" model class
          
            if (email == "admin@greendoor.sg")
            {
                // validation failed
                return new ValidationResult
                ("Email address is already in use!");
            }
            else
            {
                if ((Customer)validationContext.ObjectInstance != null)
                {
                    Customer customer = (Customer)validationContext.ObjectInstance;
                    // Get the customerID from the customer instance
                    int customerID = customer.CustomerID;

                    if (customerContext.IsEmailExist(email, customerID))
                        // validation failed
                        return new ValidationResult
                        ("Email address is already in use!");
                    else
                        // validation passed
                        return ValidationResult.Success;
                }
                else
                {
                    if ((Shop)validationContext.ObjectInstance != null)
                    {
                        // Get the shopID from the shop instance
                        Shop shop = (Shop)validationContext.ObjectInstance;
                        int shopID = shop.ShopID;

                        if (shopContext.IsEmailExist(email, shopID))
                            // validation failed
                            return new ValidationResult
                            ("Email address is already in use!");
                        else
                            // validation passed
                            return ValidationResult.Success;
                    }
                    else
                    {
                        // validation passed
                        return ValidationResult.Success;
                    }
                }
                /*
            if ((Customer)validationContext.ObjectInstance != null)
            {
                Customer customer = (Customer)validationContext.ObjectInstance;
                // Get the customerID from the customer instance
                int customerID = customer.CustomerID;

                if (customerContext.IsEmailExist(email, customerID))
                    // validation failed
                    return new ValidationResult
                    ("Email address is already in use!");
                else
                    // validation passed
                    return ValidationResult.Success;
            }
            else
            {
                if ((Shop)validationContext.ObjectInstance != null)
                {
                    // Get the shopID from the shop instance
                    Shop shop = (Shop)validationContext.ObjectInstance;
                    int shopID = shop.ShopID;

                    if (shopContext.IsEmailExist(email, shopID))
                        // validation failed
                        return new ValidationResult
                        ("Email address is already in use!");
                    else
                        // validation passed
                        return ValidationResult.Success;
                }
                else
                {
                    
                    if (email == "admin@greendoor.sg")
                    {
                        // validation failed
                        return new ValidationResult
                        ("Email address is already in use!");
                    }
                    else
                    {
                        // validation passed
                        return ValidationResult.Success;
                    }
                } */
            }

        }

    }
}
