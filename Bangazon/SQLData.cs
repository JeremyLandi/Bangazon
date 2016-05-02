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

        public void CreateCustomer(string FirstName, string LastName, string StreetAddress, string City, string State, string Zipcode, string PhoneNumber)
        {
            string command = string.Format("INSERT INTO Customer (FirstName, LastName, StreetAddress, City, State, Zipcode, PhoneNumber) " +
                "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')", FirstName, LastName, StreetAddress, City, State, Zipcode, PhoneNumber);
            UpdateDataBase(command);
        }
        public void CreatePaymentOption()
        {
            string command = "";
            UpdateDataBase(command);
        }

        #endregion

        #region Private Methods

        private void UpdateDataBase(string commandString)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = commandString;
            cmd.Connection = _sqlConnection;

            _sqlConnection.Open();
            cmd.ExecuteNonQuery();
            _sqlConnection.Close();
        }

        #endregion
    }
}
namespace Bangazon
{
    public class SQLData
    {
        
    }
}
