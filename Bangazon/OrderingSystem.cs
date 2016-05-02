using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Bangazon
{
    class OrderingSystem
    {
        //create the object to use it in this class
        private SqlData _sqlData = new SqlData();

        //constructor
        public OrderingSystem()
        {
            ShowMenu();
        }

        public void ShowMenu()
        {
            Console.WriteLine("\n*********************************************************" +
                                 "\n* ​*Welcome to Bangazon!Command Line Ordering System *​ *" +
                                 "\n*********************************************************" +
                                 "\n1.Create an account" +
                                 "\n2.Create a payment option" +
                                 "\n3.Order a product" +
                                 "\n4.Complete an order" +
                                 "\n5.See product popularity" +
                                 "\n6.Leave Bangazon!");
            string key = Console.ReadLine();

            //we need the while loop to keep the menu going
            while (key != "6")
            {
                switch (key)
                {
                    case "1":
                        CreateAccount();
                        break;
                    case "2":
                        CreatePaymentOption();
                        break;
                    case "3":
                        OrderProduct();
                        break;
                    case "4":
                        CompleteOrder();
                        break;
                    case "5":
                        SeeProductPopularity();
                        break;
                }

                key = Console.ReadLine();
            }
        }

        //this is a stubbed function
        public void CreateAccount()
        {
            string FirstName = "";
            string LastName = "";
            string StreetAddress = "";
            string City = "";
            string State = "";
            string Zipcode = "";
            string PhoneNumber = "";

            Console.WriteLine("Enter Customer First Name");
            FirstName = Console.ReadLine();
            Console.WriteLine("Enter Customer Last Name");
            LastName = Console.ReadLine();
            Console.WriteLine("Enter Street");
            StreetAddress = Console.ReadLine();
            Console.WriteLine("Enter City");
            City = Console.ReadLine();
            Console.WriteLine("Enter State/Providence");
            State = Console.ReadLine();
            Console.WriteLine("Enter Zip Code");
            Zipcode = Console.ReadLine();
            Console.WriteLine("Enter Phone Number");
            PhoneNumber = Console.ReadLine();

            _sqlData.CreateCustomer(FirstName, LastName, StreetAddress, City, State, Zipcode, PhoneNumber);
        }

        public void CreatePaymentOption() { }
        public void OrderProduct() { }
        public void CompleteOrder() { }
        public void SeeProductPopularity() { }
        public void LeaveBangazon() { }

    }
  }


