using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bangazon
{
    // This is a linking class. It links 2 things together - the Customer and the Customer's Products
    public class CustomerProducts
    {
        public List<Product> ProductsList = new List<Product>();
        public Customer TheCustomer { get; set; }
        public PaymentOption Payment { get; set; }

    }
}
