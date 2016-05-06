using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Bangazon
{
    class OrderingSystem
    {
        //create the object to use it in this class
        private SqlData _sqlData = new SqlData();
        private List<CustomerProducts> _customerProducts = new List<CustomerProducts>();

        //constructor
        public OrderingSystem()
        {
            ShowMenu();
        }

        public void ShowMenu()
        {

            bool run = true;
            //we need the while loop to keep the menu going
            while (run)
            {
                //Console.Clear();
                Console.WriteLine("\n*********************************************************" +
                      "\n*​*Welcome to Bangazon!Command Line Ordering System *​*" +
                      "\n*********************************************************" +
                      "\n1.Create an account" +
                      "\n2.Create a payment option" +
                      "\n3.Order a product" +
                      "\n4.Complete an order" +
                      "\n5.See product popularity" +
                      "\n6.Leave Bangazon!");
                Console.Write(">");
                string key = Console.ReadLine();
                switch (key)
                {
                    case "1":
                        CreateAccount();
                        continue;
                    case "2":
                        CreatePaymentOption();
                        continue;
                    case "3":
                        OrderProduct();
                        continue;
                    case "4":
                        CompleteOrder();
                        continue;
                    case "5":
                        SeeProductPopularity();
                        continue;
                    case "6":
                        LeaveBangazon();
                        continue;
                }

                key = Console.ReadLine();
            }
        }

        //this is a stubbed function
        public void CreateAccount()
        {
            Customer customer = new Customer();

            Console.WriteLine("Enter Customer First Name");
            customer.FirstName = Console.ReadLine();
            Console.WriteLine("Enter Customer Last Name");
            customer.LastName = Console.ReadLine();
            Console.WriteLine("Enter Street");
            customer.StreetAddress = Console.ReadLine();
            Console.WriteLine("Enter City");
            customer.City = Console.ReadLine();
            Console.WriteLine("Enter State/Providence");
            customer.State = Console.ReadLine();
            Console.WriteLine("Enter Zip Code");
            customer.Zipcode = Console.ReadLine();
            Console.WriteLine("Enter Phone Number");
            customer.PhoneNumber = Console.ReadLine();

            _sqlData.CreateCustomer(customer);

            Console.Clear();
        }

        // method that gets list of customers from Customer table
        public Customer ChooseCustomer()
        {
            Customer customer = null;
            List<Customer> customerList = _sqlData.GetCustomers();
            for (int i = 0; i < customerList.Count; i++)
            {
                Console.WriteLine(
                    (i + 1) + ". " +
                    customerList[i].FirstName + " " +
                    customerList[i].LastName);
            }

            string chosenCustomer = Console.ReadLine();
            int chosenCustomerId = int.Parse(chosenCustomer);
            //ensure that somebody can only pick a number inside list
            if (chosenCustomerId >= 0 && chosenCustomerId <= customerList.Count)
            {
                customer = customerList[chosenCustomerId - 1];
            }

            return customer;
        }

        public void CreatePaymentOption()
        {
            Console.WriteLine("Which customer would you like to create a payment option for?");
            Customer customer = ChooseCustomer();
            CustomerProducts customerProducts = GetCustomerProducts(customer);
            PaymentOption paymentOption = new PaymentOption();

            //get the IdCustomer from the Customer table
            paymentOption.IdCustomer = customer.IdCustomer;

            Console.WriteLine("Enter payment type (e.g. AmEx, Visa, Checking");
            paymentOption.Name = Console.ReadLine();

            Console.WriteLine("Enter account/C.C number");
            paymentOption.AccountNumber = Console.ReadLine();

            customerProducts.Payment = paymentOption;

            //call to update database
            _sqlData.CreatePaymentOption(paymentOption);

            Console.Clear();
        }


        public Product ChooseProduct()
        {
            Product product = null;
            List<Product> productList = _sqlData.GetProducts();
            for (int i = 0; i < productList.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + productList[i].Name);
            }
            int lastItem = (productList.Count + 1);
            Console.WriteLine((productList.Count + 1) + ". Back to main menu");

            string chosenProduct = Console.ReadLine();
            int chosenProductId = int.Parse(chosenProduct);
            //ensure that somebody can only pick a number inside list

            if (chosenProductId >= 0 && chosenProductId <= productList.Count)
            {
                product = productList[chosenProductId - 1];
            }

            return product;
        }

        public void OrderProduct()
        {
            Console.WriteLine("Which customer?");
            Customer customer = ChooseCustomer();
            CustomerProducts customerProducts = GetCustomerProducts(customer);

            Product nextProduct = null;
            do
            {
                nextProduct = ChooseProduct();
                if (nextProduct != null)
                {
                    customerProducts.Products.Add(nextProduct);
                }
            }
            while (nextProduct != null);

        }

        public void CompleteOrder()
        {

            Console.Write("Which customer?");
            Customer customer = ChooseCustomer();
            CustomerProducts customerProducts = GetCustomerProducts(customer);

            if (customerProducts == null)
            {
                Console.WriteLine("Please add some products to your order first." +
                    "\nPress any key to return to main menu.");
                return;
            }
            else
            {


                //foreach (CustomerProducts cProds in _customerProducts)
                //{
                //    if (cProds.TheCustomer.FirstName == customer.FirstName &&
                //        cProds.TheCustomer.LastName == customer.LastName)
                //{
                //    Console.WriteLine((i + 1) + ". " + productsToOrder[i].Name);
                //}
                //}
                Console.Clear();
                Console.WriteLine(string.Format("The total is ${0}.", customerProducts.Products.Sum(x => x.Price)));
                //decimal finalPrice = 0;
                //foreach (Product product in customerProducts.Products)
                //{
                //    finalPrice += product.Price;
                //}
                Console.WriteLine("Is this total correct?");
                Console.Write("Y/N>");
                string answer = Console.ReadLine();

                if (answer == "Y" || answer == "y")
                {
                    _sqlData.CreateCustomerOrder(customerProducts);
                    Console.WriteLine("Your order is complete");
                }
                else if (answer != "Y" || answer != "y")
                {
                    return;
                }
            }
        }

        public void SeeProductPopularity()
        {
            Console.Clear();
            string query = @"
            SELECT
            p.Name,
            COUNT(op.IdProduct) AS TimesOrdered, 
            COUNT(DISTINCT co.IdCustomer) AS Customers ,
            ROUND(SUM(p.Price), 2) AS total
            FROM Product p 
            INNER JOIN OrderProducts op 
              ON p.IdProduct = op.IdProduct 
            INNER JOIN CustomerOrder co
              ON op.IdOrder = co.IdOrder 
            GROUP BY p.Name 
            ORDER BY TimesOrdered DESC";

            using (SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" +
            "\"C:\\Users\\Jeremy\\Documents\\Visual Studio 2015\\Projects\\ExercisesDotNet\\Invoices\\Invoices\\Invoices.mdf\";Integrated Security=True"))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Check is the reader has any rows at all before starting to read.
                    if (reader.HasRows)
                    {
                        // Read advances to the next row.
                        while (reader.Read())
                        {
                            Console.WriteLine("{0} ordered {1} times by {2} customers for a total revenue of ${3}",
                                reader[0], reader[1], reader[2], reader[3]);
                        }

                    }
                }
                Console.WriteLine("");
                Console.WriteLine("Press any key to return to main menu.");
                Console.ReadKey();
                Console.Clear();

            }
        }





        //DOESN'T WORK
        //public void SeeProductPopularity()
        //{
        //    Console.Clear();
        //    List<MostPopular> mostpopularList = _sqlData.GetPopularProducts();
        //    for (int i = 0; i < mostpopularList.Count; i++)
        //    {
        //        Console.WriteLine(string.Format("{0} ordered {1} times by {2} customers for a total revenue of ${3}.",
        //        mostpopularList[i].name, mostpopularList[i].timesOrdered, mostpopularList[i].numberOfCustomers, mostpopularList[i].price));
        //    }
        //    int lastItem = (mostpopularList.Count + 1);
        //    Console.WriteLine((mostpopularList.Count + 1) + ". Back to main menu");
        //}

        public void LeaveBangazon()
        {
            System.Environment.Exit(0);
        }

        private CustomerProducts GetCustomerProducts(Customer customer)
        {
            CustomerProducts customerProducts = null;
            foreach (CustomerProducts cProds in _customerProducts)
            {
                if (cProds.TheCustomer.FirstName == customer.FirstName &&
                    cProds.TheCustomer.LastName == customer.LastName)
                {
                    customerProducts = cProds;
                }
                if (customerProducts.Payment == null)
                {
                    customerProducts.Payment = _sqlData.GetPaymentOptionForCustomer(customer);
                }
            }
            if (customerProducts == null)
            {
                customerProducts = new CustomerProducts();
                customerProducts.TheCustomer = customer;
                _customerProducts.Add(customerProducts);
            }

            return customerProducts;
        }
    }
}


