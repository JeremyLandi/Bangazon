using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bangazon
{
    public class SqlData
    {
        #region Variables

        private SqlConnection _sqlConnection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" +
            "\"C:\\Users\\Jeremy\\Documents\\Visual Studio 2015\\Projects\\ExercisesDotNet\\Invoices\\Invoices\\Invoices.mdf\";Integrated Security=True");

        #endregion

        #region Properties

        #endregion

        #region Constructors

        #endregion

        #region Public Methods

        public void CreateCustomer(Customer customer)
        {
            string command = string.Format("INSERT INTO Customer (FirstName, LastName, StreetAddress, City, State, Zipcode, PhoneNumber) " +
                "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')", customer.FirstName, customer.LastName, customer.StreetAddress, customer.City, customer.State, customer.Zipcode, customer.PhoneNumber);
            UpdateDataBase(command);
        }

        public List<Customer> GetCustomers()
        { //create a list so we can have the data from the customer table
            List<Customer> customerList = new List<Customer>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "SELECT IdCustomer, FirstName, LastName, StreetAddress, City, State, Zipcode, PhoneNumber FROM Customer";
            cmd.Connection = _sqlConnection;

            _sqlConnection.Open();
            //using will clean up everything... open and close connections
            //Read the database. While there is items in database build customer
            using (SqlDataReader dataReader = cmd.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    Customer customer = new Customer();
                    customer.IdCustomer = dataReader.GetInt32(0);
                    customer.FirstName = dataReader.GetString(1);
                    customer.LastName = dataReader.GetString(2);
                    customer.StreetAddress = dataReader.GetString(3);
                    customer.City = dataReader.GetString(4);
                    customer.State = dataReader.GetString(5);
                    customer.Zipcode = dataReader.GetString(6);
                    customer.PhoneNumber = dataReader.GetString(7);

                    // Add the created customer and adds it to the customerList
                    customerList.Add(customer);
                }
            }
            _sqlConnection.Close();

            return customerList;
        }

        public void CreatePaymentOption(PaymentOption paymentOption)
        {
            string command = string.Format("INSERT INTO PaymentOption (IdCustomer, Name, AccountNumber)" +
                "VALUES ('{0}', '{1}', '{2}')", paymentOption.IdCustomer, paymentOption.Name, paymentOption.AccountNumber);
            UpdateDataBase(command);
        }

        public PaymentOption GetPaymentOptionForCustomer(Customer customer)
        {
            PaymentOption paymentOption = null;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            //Only access the payment option that matches that customer we have passed in (only getting the first payment option)
            cmd.CommandText = string.Format("SELECT IdPaymentOption, Name, AccountNumber FROM PaymentOption " +
                "WHERE IdCustomer = '{0}'", customer.IdCustomer);
            cmd.Connection = _sqlConnection;

            try
            {
                _sqlConnection.Open();
                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        paymentOption = new PaymentOption();
                        paymentOption.IdPaymentOption = dataReader.GetInt32(0);
                        paymentOption.IdCustomer = customer.IdCustomer;
                        paymentOption.Name = dataReader.GetString(1);
                        paymentOption.AccountNumber = dataReader.GetString(2);
                    }
                }
            }
            finally
            {
                _sqlConnection.Close();
            }

            return paymentOption;
        }

        //DOES'T WORK
        //public List<MostPopular> GetPopularProducts()
        //{
        //    List<MostPopular> mostPopularList = new List<MostPopular>();
        //    //MostPopular mostPopular = null;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = System.Data.CommandType.Text;
        //    cmd.CommandText =
        //        //Product name
        //        "SELECT p.Name" +
        //        //number of times ordered
        //        "COUNT(op.IdProduct) AS TimesOrdered" +
        //        //number of customers
        //        "COUNT(co.IdCustomer) AS NumberOfCustomers" +
        //        //total revenue
        //        "ROUND(SUM(p.Price), 2) AS Total" +
        //        "FROM OrderProducts AS op" +
        //        "INNER JOIN CustomerOrder co ON op.IdOrder = co.IdOrder " +
        //        "INNER JOIN Product p ON op.IdOrderProducts = p.IdOrderProducts " +
        //        "GROUP BY p.Name" +
        //        "ORDER BY TimesOrdered DESC";
        //    cmd.Connection = _sqlConnection;

        //    _sqlConnection.Open();
        //    using (SqlDataReader dataReader = cmd.ExecuteReader())
        //    {
        //        if (dataReader.Read())
        //        {
        //            MostPopular mostPopular = new MostPopular();
        //            mostPopular.name = dataReader.GetString(0);
        //            mostPopular.timesOrdered = dataReader.GetInt32(1);
        //            mostPopular.numberOfCustomers = dataReader.GetInt32(2);
        //            mostPopular.price = dataReader.GetInt32(3);

        //            mostPopularList.Add(mostPopular);

        //        }
        //    }
        //    _sqlConnection.Close();
        //    return mostPopularList;
        //}

        public List<Product> GetProducts()
        {
            List<Product> productList = new List<Product>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "SELECT IdProduct, Name, Description, Price, IdProductType FROM Product";
            cmd.Connection = _sqlConnection;

            _sqlConnection.Open();

            //using will clean up everything... open and close connections
            using (SqlDataReader dataReader = cmd.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    Product product = new Product();
                    product.IdProduct = dataReader.GetInt32(0);
                    product.Name = dataReader.GetString(1);
                    product.Description = dataReader.GetString(2);
                    product.Price = dataReader.GetDecimal(3);
                    product.IdProductType = dataReader.GetInt32(4);

                    productList.Add(product);
                }
            }

            _sqlConnection.Close();

            return productList;
        }

        public void CreateCustomerOrder(CustomerProducts customerProducts)
        {
            DateTime now = DateTime.Now;
            int orderNumber = (new Random().Next(int.MaxValue));
            //1. Add row to customer order table
            string command = string.Format("INSERT INTO CustomerOrder (OrderNumber, DateCreated, IdCustomer, IdPaymentOption, Shipping)" +
                "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')", orderNumber, now.ToString(), customerProducts.TheCustomer.IdCustomer,
                customerProducts.Payment.IdPaymentOption, "UPS");
            UpdateDataBase(command);

            //2. get IdOrder from CustomerOrder table
            command = string.Format("SELECT IdOrder FROM CustomerOrder WHERE IdCustomer='{0}' AND OrderNumber='{1}'", 
                customerProducts.TheCustomer.IdCustomer, orderNumber);
            int idOrder = GetIdFromTable(command);

            //3. add row to OrderProducts table
            foreach(var product in customerProducts.Products)
            {
                command = string.Format("INSERT INTO OrderProducts (IdProduct, IdOrder) " + 
                    "VALUES ('{0}', '{1}')", product.IdProduct, idOrder);
                UpdateDataBase(command);
            }
        }



        #endregion

        #region Private Methods

        private void UpdateDataBase(string commandString)
        {
            //SQL Command tells the database what you're going to do (and update, an insert, a delete or a select)
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = commandString;
            cmd.Connection = _sqlConnection;

            _sqlConnection.Open();
            cmd.ExecuteNonQuery();
            _sqlConnection.Close();
        }

        private int GetIdFromTable(string commandString)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = commandString;
            cmd.Connection = _sqlConnection;

            _sqlConnection.Open();
            //ExecuteScalar returns back one thing (object)
            object idObj = cmd.ExecuteScalar();
            _sqlConnection.Close();

            return (int)idObj;
        }
        #endregion
    }
}

