using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bangazon
{
    public class PaymentOption
    {
        public string Name { get; set; }
        public string AccountNumber { get; set; }
        public int IdCustomer { get; set; }
        public int IdPaymentOption { get; set; }
    }
}
